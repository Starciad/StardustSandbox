using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Collections;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Elements;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.Simulation;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.World;
using StardustSandbox.Explosions;
using StardustSandbox.Extensions;
using StardustSandbox.Inputs.Game;
using StardustSandbox.Interfaces.Collections;
using StardustSandbox.Managers;
using StardustSandbox.Mathematics;
using StardustSandbox.Serialization.Saving;
using StardustSandbox.Serialization.Saving.Data;
using StardustSandbox.WorldSystem.Chunking;
using StardustSandbox.WorldSystem.Components;
using StardustSandbox.WorldSystem.Status;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StardustSandbox.WorldSystem
{
    internal sealed class World
    {
        internal Information Information => this.information;
        internal Simulation Simulation => this.simulation;
        internal Time Time => this.time;

        internal bool CanUpdate { get; set; }
        internal bool CanDraw { get; set; }

        private SaveFile currentlySelectedSaveFile;

        private Slot[,] slots;

        private readonly Information information;
        private readonly Simulation simulation;
        private readonly Time time;

        private readonly WorldChunking chunking;
        private readonly WorldRendering rendering;
        private readonly WorldUpdating updating;

        private readonly ElementContext worldElementContext;

        private readonly ObjectPool worldSlotsPool;
        private readonly ObjectPool explosionPool = new();

        private readonly Queue<Explosion> instantiatedExplosions = new(ExplosionConstants.ACTIVE_EXPLOSIONS_LIMIT);

        private readonly GameManager gameManager;
        private readonly ElementNeighbors elementNeighbors;

        private Slot this[int x, int y]
        {
            get => this.slots[x, y];
            set => this.slots[x, y] = value;
        }

        internal World(InputController inputController, GameManager gameManager)
        {
            this.gameManager = gameManager;

            this.information = new();
            this.time = new();
            this.simulation = new();

            this.worldSlotsPool = new();

            this.chunking = new(this);
            this.rendering = new(inputController, this);
            this.updating = new(this);

            this.worldElementContext = new(this);
            this.elementNeighbors = new();
        }

        public void Reset()
        {
            this.currentlySelectedSaveFile = null;

            this.information.Reset();
            this.chunking.Reset();
            this.updating.Reset();

            Clear();
        }

        #region ELEMENTS

        internal bool TryInstantiateElement(Point position, Layer layer, Element value)
        {
            if (!InsideTheWorldDimensions(position) || !IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            NotifyChunk(position);

            Slot slot = this[position.X, position.Y];
            slot.Instantiate(position, layer, value);

            this.worldElementContext.UpdateInformation(position, layer, slot);
            value.SetContext(this.worldElementContext);
            value.Instantiate();

            return true;
        }

        internal bool TryInstantiateElement(Point position, Layer layer, ElementIndex index)
        {
            return TryInstantiateElement(position, layer, ElementDatabase.GetElement(index));
        }

        internal bool TryUpdateElementPosition(Point oldPosition, Point newPosition, Layer layer)
        {
            if (!InsideTheWorldDimensions(oldPosition) ||
                !InsideTheWorldDimensions(newPosition) ||
                 IsEmptySlotLayer(oldPosition, layer) ||
                !IsEmptySlotLayer(newPosition, layer) ||
                oldPosition == newPosition)
            {
                return false;
            }

            NotifyChunk(oldPosition);
            NotifyChunk(newPosition);

            this[newPosition.X, newPosition.Y].Copy(layer, this[oldPosition.X, oldPosition.Y].GetLayer(layer));
            this[newPosition.X, newPosition.Y].SetPosition(newPosition);
            this[oldPosition.X, oldPosition.Y].Destroy(layer);

            return true;
        }

        internal bool TrySwappingElements(Point element1Position, Point element2Position, Layer layer)
        {
            if (!InsideTheWorldDimensions(element1Position) ||
                !InsideTheWorldDimensions(element2Position) ||
                IsEmptySlotLayer(element1Position, layer) ||
                IsEmptySlotLayer(element2Position, layer) ||
                element1Position == element2Position)
            {
                return false;
            }

            NotifyChunk(element1Position);
            NotifyChunk(element2Position);

            Slot tempSlot = this.worldSlotsPool.TryDequeue(out IPoolableObject value) ? (Slot)value : new();

            tempSlot.Copy(layer, this[element1Position.X, element1Position.Y].GetLayer(layer));

            this[element1Position.X, element1Position.Y].Copy(layer, this[element2Position.X, element2Position.Y].GetLayer(layer));
            this[element2Position.X, element2Position.Y].Copy(layer, tempSlot.GetLayer(layer));

            this[element1Position.X, element1Position.Y].SetPosition(element1Position);
            this[element2Position.X, element2Position.Y].SetPosition(element2Position);

            this.worldSlotsPool.Enqueue(tempSlot);

            return true;
        }

        internal bool TryDestroyElement(Point position, Layer layer)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            NotifyChunk(position);

            Slot slot = this[position.X, position.Y];
            SlotLayer slotLayer = slot.GetLayer(layer);

            this.worldElementContext.UpdateInformation(position, layer, slot);
            slotLayer.Element.SetContext(this.worldElementContext);
            slotLayer.Element.Destroy();
            slotLayer.Destroy();

            return true;
        }

        internal bool TryRemoveElement(Point position, Layer layer)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            NotifyChunk(position);

            this[position.X, position.Y].Destroy(layer);

            return true;
        }

        internal bool TryReplaceElement(Point position, Layer layer, ElementIndex index)
        {
            return TryRemoveElement(position, layer) && TryInstantiateElement(position, layer, index);
        }

        internal bool TryReplaceElement(Point position, Layer layer, Element value)
        {
            return TryRemoveElement(position, layer) && TryInstantiateElement(position, layer, value);
        }

        internal bool TryGetElement(Point position, Layer layer, out Element value)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptySlotLayer(position, layer))
            {
                value = null;
                return false;
            }

            SlotLayer slotLayer = this[position.X, position.Y].GetLayer(layer);

            if (slotLayer.HasState(ElementStates.IsEmpty))
            {
                value = null;
                return false;
            }

            value = slotLayer.Element;
            return true;
        }

        internal bool TryGetSlot(Point position, out Slot value)
        {
            value = default;

            if (!InsideTheWorldDimensions(position) || IsEmptySlot(position))
            {
                return false;
            }

            value = this[position.X, position.Y];
            return true;
        }

        internal bool TrySetElementTemperature(Point position, Layer layerType, float value)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptySlotLayer(position, layerType))
            {
                return false;
            }

            SlotLayer slotLayer = this[position.X, position.Y].GetLayer(layerType);

            if (slotLayer.Temperature != value)
            {
                NotifyChunk(position);
                slotLayer.SetTemperatureValue(value);
            }

            return true;
        }

        internal bool TrySetElementColorModifier(Point position, Layer layer, Color value)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            this[position.X, position.Y].GetLayer(layer).SetColorModifier(value);

            return true;
        }

        internal bool TrySetStoredElement(Point position, Layer layer, ElementIndex index)
        {
            return TrySetStoredElement(position, layer, ElementDatabase.GetElement(index));
        }

        internal bool TrySetStoredElement(Point position, Layer layer, Element element)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            this[position.X, position.Y].GetLayer(layer).SetStoredElement(element);

            return true;
        }

        internal bool TryHasElementState(Point position, Layer layer, ElementStates state, out bool value)
        {
            value = false;
            if (!InsideTheWorldDimensions(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            value = this[position.X, position.Y].HasState(layer, state);
            return true;
        }

        internal bool TrySetElementState(Point position, Layer layer, ElementStates state)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            this[position.X, position.Y].SetState(layer, state);
            return true;
        }

        internal bool TryRemoveElementState(Point position, Layer layer, ElementStates state)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            this[position.X, position.Y].RemoveState(layer, state);
            return true;
        }

        internal bool TryClearElementStates(Point position, Layer layer)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            this[position.X, position.Y].ClearStates(layer);
            return true;
        }

        internal bool TryToggleElementState(Point position, Layer layer, ElementStates state)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            this[position.X, position.Y].ToggleState(layer, state);
            return true;
        }

        internal void InstantiateElement(Point position, Layer layer, ElementIndex index)
        {
            InstantiateElement(position, layer, ElementDatabase.GetElement(index));
        }

        internal void InstantiateElement(Point position, Layer layer, Element value)
        {
            _ = TryInstantiateElement(position, layer, value);
        }

        internal void UpdateElementPosition(Point oldPosition, Point newPosition, Layer layer)
        {
            _ = TryUpdateElementPosition(oldPosition, newPosition, layer);
        }

        internal void SwappingElements(Point element1Position, Point element2Position, Layer layer)
        {
            _ = TrySwappingElements(element1Position, element2Position, layer);
        }

        internal void DestroyElement(Point position, Layer layer)
        {
            _ = TryDestroyElement(position, layer);
        }

        internal void RemoveElement(Point position, Layer layer)
        {
            _ = TryRemoveElement(position, layer);
        }

        internal void ReplaceElement(Point position, Layer layer, ElementIndex index)
        {
            _ = TryReplaceElement(position, layer, index);
        }

        internal void ReplaceElement(Point position, Layer layer, Element value)
        {
            _ = TryReplaceElement(position, layer, value);
        }

        internal Element GetElement(Point position, Layer layer)
        {
            _ = TryGetElement(position, layer, out Element value);
            return value;
        }

        internal Slot GetSlot(Point position)
        {
            _ = TryGetSlot(position, out Slot value);
            return value;
        }

        internal ElementNeighbors GetNeighboringSlots(Point position)
        {
            this.elementNeighbors.Reset();

            int index = 0;

            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    if (dx == 0 && dy == 0)
                    {
                        continue;
                    }

                    Point neighborPosition = position + new Point(dx, dy);

                    if (!InsideTheWorldDimensions(neighborPosition))
                    {
                        continue;
                    }

                    this.elementNeighbors.SetNeighbor(index, GetSlot(neighborPosition));
                    index++;
                }
            }

            return this.elementNeighbors;
        }

        internal void SetElementTemperature(Point position, Layer layer, float value)
        {
            _ = TrySetElementTemperature(position, layer, value);
        }

        internal void SetElementColorModifier(Point position, Layer layer, Color value)
        {
            _ = TrySetElementColorModifier(position, layer, value);
        }

        internal void SetStoredElement(Point position, Layer layer, ElementIndex index)
        {
            SetStoredElement(position, layer, ElementDatabase.GetElement(index));
        }

        internal void SetStoredElement(Point position, Layer layer, Element element)
        {
            _ = TrySetStoredElement(position, layer, element);
        }

        internal bool HasElementState(Point position, Layer layer, ElementStates state)
        {
            _ = TryHasElementState(position, layer, state, out bool value);
            return value;
        }

        internal void SetElementState(Point position, Layer layer, ElementStates state)
        {
            _ = TrySetElementState(position, layer, state);
        }

        internal void RemoveElementState(Point position, Layer layer, ElementStates state)
        {
            _ = TryRemoveElementState(position, layer, state);
        }

        internal void ClearElementStates(Point position, Layer layer)
        {
            _ = TryClearElementStates(position, layer);
        }

        internal void ToggleElementState(Point position, Layer layer, ElementStates state)
        {
            _ = TryToggleElementState(position, layer, state);
        }

        internal bool IsEmptySlot(Point position)
        {
            return !InsideTheWorldDimensions(position) || this[position.X, position.Y].IsEmpty;
        }

        internal bool IsEmptySlotLayer(Point position, Layer layer)
        {
            return !InsideTheWorldDimensions(position) || this[position.X, position.Y].GetLayer(layer).HasState(ElementStates.IsEmpty);
        }

        internal uint GetTotalElementCount()
        {
            return GetTotalForegroundElementCount() + GetTotalBackgroundElementCount();
        }

        internal uint GetTotalForegroundElementCount()
        {
            return GetTotalElementCountForLayer(slot => !slot.Foreground.HasState(ElementStates.IsEmpty));
        }

        internal uint GetTotalBackgroundElementCount()
        {
            return GetTotalElementCountForLayer(slot => !slot.Background.HasState(ElementStates.IsEmpty));
        }

        private uint GetTotalElementCountForLayer(Func<Slot, bool> predicate)
        {
            uint count = 0;
            object lockObj = new();

            _ = Parallel.For(0, this.information.Size.Y, y =>
            {
                uint localCount = 0;

                for (int x = 0; x < this.information.Size.X; x++)
                {
                    if (TryGetSlot(new(x, y), out Slot value) && predicate(value))
                    {
                        localCount++;
                    }
                }

                lock (lockObj)
                {
                    count += localCount;
                }
            });

            return count;
        }

        #endregion

        #region CHUNKING

        internal bool TryNotifyChunk(Point position)
        {
            return this.chunking.TryNotifyChunk(position);
        }

        internal void NotifyChunk(Point position)
        {
            _ = TryNotifyChunk(position);
        }

        internal bool TryGetChunkUpdateState(Point position, out bool result)
        {
            return this.chunking.TryGetChunkUpdateState(position, out result);
        }

        internal bool GetChunkUpdateState(Point position)
        {
            _ = TryGetChunkUpdateState(position, out bool result);
            return result;
        }

        internal int GetActiveChunksCount()
        {
            return this.chunking.GetActiveChunksCount();
        }

        internal IEnumerable<Chunk> GetActiveChunks()
        {
            return this.chunking.GetActiveChunks();
        }

        #endregion

        #region EXPLOSIONS

        internal bool TryInstantiateExplosion(Point position, Layer layer, ExplosionBuilder explosionBuilder)
        {
            if (!InsideTheWorldDimensions(position) && this.instantiatedExplosions.Count >= ExplosionConstants.ACTIVE_EXPLOSIONS_LIMIT)
            {
                return false;
            }

            Explosion explosion = this.explosionPool.TryDequeue(out IPoolableObject pooledObject)
                ? (Explosion)pooledObject
                : new();

            explosion.Build(position, layer, explosionBuilder);
            this.instantiatedExplosions.Enqueue(explosion);

            return true;
        }

        internal void InstantiateExplosion(Point position, Layer layer, ExplosionBuilder explosionBuilder)
        {
            _ = TryInstantiateExplosion(position, layer, explosionBuilder);
        }

        private void HandleExplosions()
        {
            while (this.instantiatedExplosions.TryDequeue(out Explosion value))
            {
                HandleExplosion(value);
                this.explosionPool.Enqueue(value);
            }
        }

        private void HandleExplosion(Explosion explosion)
        {
            foreach (Point point in ShapePointGenerator.GenerateCirclePoints(explosion.Position, Convert.ToInt32(explosion.Radius)))
            {
                if (!InsideTheWorldDimensions(point))
                {
                    continue;
                }

                if (TryGetSlot(point, out Slot slot))
                {
                    TryAffectPoint(slot, point, explosion);
                }

                InstantiateElement(point, explosion.Layer, explosion.ExplosionResidues.GetRandomItem());
            }
        }

        private void TryAffectSlotLayer(SlotLayer slotLayer, Layer layer, Point targetPosition, Explosion explosion)
        {
            if (slotLayer.HasState(ElementStates.IsEmpty))
            {
                return;
            }

            Element element = slotLayer.Element;

            if (element.Characteristics.HasFlag(ElementCharacteristics.IsExplosionImmune))
            {
                return;
            }

            if (element.DefaultExplosionResistance >= explosion.Power)
            {
                slotLayer.SetTemperatureValue(slotLayer.Temperature + explosion.Heat);
            }
            else
            {
                DestroyElement(targetPosition, layer);
            }
        }

        private void TryAffectPoint(Slot slot, Point targetPosition, Explosion explosion)
        {
            TryAffectSlotLayer(slot.GetLayer(Layer.Foreground), Layer.Foreground, targetPosition, explosion);
            TryAffectSlotLayer(slot.GetLayer(Layer.Background), Layer.Background, targetPosition, explosion);
        }

        #endregion

        #region ROUTINE

        #region Update

        internal void Update(in GameTime gameTime)
        {
            if (!this.CanUpdate)
            {
                return;
            }

            this.time.Update(gameTime);
            this.simulation.Update(gameTime);

            if (this.simulation.CanContinueExecution())
            {
                this.chunking.Update();
                this.updating.Update(gameTime);
            }

            HandleExplosions();
        }

        #endregion

        #region Draw

        internal void Draw(SpriteBatch spriteBatch)
        {
            if (!this.CanDraw)
            {
                return;
            }

            if (Parameters.ShowChunks)
            {
                this.chunking.Draw(spriteBatch);
            }

            this.rendering.Draw(spriteBatch);
        }

        #endregion

        #endregion

        #region UTILITIES

        internal void StartNew()
        {
            StartNew(this.information.Size);
        }

        internal void StartNew(Point size)
        {
            this.CanUpdate = true;
            this.CanDraw = true;

            if (this.information.Size != size)
            {
                Resize(size);
            }

            Reset();
        }

        internal void SetSaveFile(SaveFile saveFile)
        {
            this.currentlySelectedSaveFile = saveFile;
        }

        internal void LoadFromSaveFile(SaveFile saveFile)
        {
            this.gameManager.SetState(GameStates.IsSimulationPaused);

            // World
            StartNew(saveFile.Properties.Size);

            // Cache
            SetSaveFile(saveFile);

            // Metadata
            this.information.Identifier = saveFile.Metadata.Identifier;
            this.information.Name = saveFile.Metadata.Name;
            this.information.Description = saveFile.Metadata.Description;

            // Time
            this.time.SetTime(saveFile.Environment.Time.CurrentTime);
            this.time.IsFrozen = saveFile.Environment.Time.IsFrozen;

            // Allocate Slots
            foreach (SlotData slotData in saveFile.Content.Slots)
            {
                if (slotData.ForegroundLayer != null)
                {
                    LoadSlotLayerData(Layer.Foreground, slotData.Position, slotData.ForegroundLayer);
                }

                if (slotData.BackgroundLayer != null)
                {
                    LoadSlotLayerData(Layer.Background, slotData.Position, slotData.BackgroundLayer);
                }
            }
        }

        private void LoadSlotLayerData(Layer layer, Point position, SlotLayerData slotLayerData)
        {
            InstantiateElement(position, layer, slotLayerData.ElementIndex);

            Slot slot = GetSlot(position);

            slot.SetTemperatureValue(layer, slotLayerData.Temperature);
            slot.SetState(layer, ElementStates.IsFalling);
            slot.SetColorModifier(layer, slotLayerData.ColorModifier);
            slot.SetStoredElement(layer, ElementDatabase.GetElement(slotLayerData.StoredElementIndex));
        }

        internal void Resize(Point size)
        {
            DestroySlots();

            this.information.Size = size;
            this.slots = new Slot[size.X, size.Y];

            InstantiateSlots();
        }

        internal void Reload()
        {
            if (this.currentlySelectedSaveFile != null)
            {
                LoadFromSaveFile(this.currentlySelectedSaveFile);
            }
            else
            {
                Clear();
            }
        }

        internal bool InsideTheWorldDimensions(Point position)
        {
            return position.X >= 0 && position.X < this.information.Size.X &&
                   position.Y >= 0 && position.Y < this.information.Size.Y;
        }

        internal void SetSpeed(SimulationSpeed speed)
        {
            switch (speed)
            {
                case SimulationSpeed.Normal:
                    this.time.InGameSecondsPerRealSecond = TimeConstants.DEFAULT_SECONDS_PER_FRAMES;
                    this.simulation.SetSpeed(SimulationSpeed.Normal);
                    break;

                case SimulationSpeed.Fast:
                    this.time.InGameSecondsPerRealSecond = TimeConstants.DEFAULT_FAST_SECONDS_PER_FRAMES;
                    this.simulation.SetSpeed(SimulationSpeed.Fast);
                    break;

                case SimulationSpeed.VeryFast:
                    this.time.InGameSecondsPerRealSecond = TimeConstants.DEFAULT_VERY_FAST_SECONDS_PER_FRAMES;
                    this.simulation.SetSpeed(SimulationSpeed.VeryFast);
                    break;

                default:
                    this.time.InGameSecondsPerRealSecond = TimeConstants.DEFAULT_SECONDS_PER_FRAMES;
                    this.simulation.SetSpeed(SimulationSpeed.Normal);
                    break;
            }
        }

        #region Clear

        internal void Clear()
        {
            ClearSlots();
        }

        private void ClearSlots()
        {
            if (this == null)
            {
                return;
            }

            for (int y = 0; y < this.information.Size.Y; y++)
            {
                for (int x = 0; x < this.information.Size.X; x++)
                {
                    if (IsEmptySlot(new(x, y)))
                    {
                        continue;
                    }

                    RemoveElement(new(x, y), Layer.Foreground);
                    RemoveElement(new(x, y), Layer.Background);
                }
            }
        }

        #endregion

        #region Build and Destroy World

        private void InstantiateSlots()
        {
            if (this.slots == null || this.slots.Length == 0)
            {
                return;
            }

            for (int y = 0; y < this.information.Size.Y; y++)
            {
                for (int x = 0; x < this.information.Size.X; x++)
                {
                    this[x, y] = this.worldSlotsPool.TryDequeue(out IPoolableObject value) ? (Slot)value : new();
                }
            }
        }

        private void DestroySlots()
        {
            if (this.slots == null || this.slots.Length == 0)
            {
                return;
            }

            for (int y = 0; y < this.information.Size.Y; y++)
            {
                for (int x = 0; x < this.information.Size.X; x++)
                {
                    if (this[x, y] == null)
                    {
                        continue;
                    }

                    this.worldSlotsPool.Enqueue(this[x, y]);
                    this[x, y] = null;
                }
            }
        }

        #endregion

        #endregion
    }
}