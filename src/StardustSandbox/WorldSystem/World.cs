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
using StardustSandbox.ExplosionSystem;
using StardustSandbox.Extensions;
using StardustSandbox.InputSystem.GameInput;
using StardustSandbox.Interfaces.Collections;
using StardustSandbox.IO.Saving;
using StardustSandbox.IO.Saving.World.Content;
using StardustSandbox.IO.Saving.World.Information;
using StardustSandbox.Managers;
using StardustSandbox.Mathematics;
using StardustSandbox.Randomness;
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

        internal bool IsActive { get => this.isActive; set => this.isActive = value; }
        internal bool IsVisible { get => this.isVisible; set => this.isVisible = value; }

        private SaveFile currentlySelectedWorldSaveFile;

        private Slot[,] slots;

        private bool isActive;
        private bool isVisible;

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

        internal Slot this[int x, int y]
        {
            get => this.slots[x, y];
            set => this.slots[x, y] = value;
        }

        internal World(CameraManager cameraManager, InputController inputController, GameManager gameManager)
        {
            this.gameManager = gameManager;

            this.information = new();
            this.time = new();
            this.simulation = new();

            this.worldSlotsPool = new();

            this.chunking = new(this);
            this.rendering = new(cameraManager, inputController, this);
            this.updating = new(this);

            this.worldElementContext = new(this);
        }

        public void Reset()
        {
            this.currentlySelectedWorldSaveFile = null;

            this.information.Reset();
            this.chunking.Reset();
            this.updating.Reset();

            Clear();
        }

        #region ELEMENTS

        internal bool TryInstantiateElement(Point position, LayerType layer, Element value)
        {
            if (!InsideTheWorldDimensions(position) || !IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            NotifyChunk(position);

            Slot worldSlot = this[position.X, position.Y];
            worldSlot.Instantiate(position, layer, value);

            this.worldElementContext.UpdateInformation(position, layer, worldSlot);
            value.SetContext(this.worldElementContext);
            value.Instantiate();

            return true;
        }

        internal bool TryInstantiateElement(Point position, LayerType layer, ElementIndex index)
        {
            return TryInstantiateElement(position, layer, ElementDatabase.GetElement(index));
        }

        internal bool TryUpdateElementPosition(Point oldPosition, Point newPosition, LayerType layer)
        {
            if (!InsideTheWorldDimensions(oldPosition) ||
                !InsideTheWorldDimensions(newPosition) ||
                 IsEmptySlotLayer(oldPosition, layer) ||
                !IsEmptySlotLayer(newPosition, layer))
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

        internal bool TrySwappingElements(Point element1Position, Point element2Position, LayerType layer)
        {
            if (!InsideTheWorldDimensions(element1Position) ||
                !InsideTheWorldDimensions(element2Position) ||
                IsEmptySlotLayer(element1Position, layer) ||
                IsEmptySlotLayer(element2Position, layer))
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

        internal bool TryDestroyElement(Point position, LayerType layer)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            NotifyChunk(position);

            Slot worldSlot = this[position.X, position.Y];
            SlotLayer worldSlotLayer = worldSlot.GetLayer(layer);

            this.worldElementContext.UpdateInformation(position, layer, worldSlot);
            worldSlotLayer.Element.SetContext(this.worldElementContext);
            worldSlotLayer.Element.Destroy();
            worldSlotLayer.Destroy();

            return true;
        }

        internal bool TryRemoveElement(Point position, LayerType layer)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            NotifyChunk(position);

            this[position.X, position.Y].Destroy(layer);

            return true;
        }

        internal bool TryReplaceElement(Point position, LayerType layer, ElementIndex index)
        {
            return TryRemoveElement(position, layer) && TryInstantiateElement(position, layer, index);
        }

        internal bool TryReplaceElement(Point position, LayerType layer, Element value)
        {
            return TryRemoveElement(position, layer) && TryInstantiateElement(position, layer, value);
        }

        internal bool TryGetElement(Point position, LayerType layer, out Element value)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptySlotLayer(position, layer))
            {
                value = null;
                return false;
            }

            SlotLayer worldSlotLayer = this[position.X, position.Y].GetLayer(layer);

            if (worldSlotLayer.HasState(ElementStates.IsEmpty))
            {
                value = null;
                return false;
            }

            value = worldSlotLayer.Element;
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

        internal bool TrySetElementTemperature(Point position, LayerType layerType, double value)
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

        internal bool TrySetElementColorModifier(Point position, LayerType layer, Color value)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            this[position.X, position.Y].GetLayer(layer).SetColorModifier(value);

            return true;
        }

        internal bool TrySetStoredElement(Point position, LayerType layer, ElementIndex index)
        {
            return TrySetStoredElement(position, layer, ElementDatabase.GetElement(index));
        }

        internal bool TrySetStoredElement(Point position, LayerType layer, Element element)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            this[position.X, position.Y].GetLayer(layer).SetStoredElement(element);

            return true;
        }

        internal bool TryHasElementState(Point position, LayerType layer, ElementStates state, out bool value)
        {
            value = false;
            if (!InsideTheWorldDimensions(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            value = this[position.X, position.Y].HasState(layer, state);
            return true;
        }

        internal bool TrySetElementState(Point position, LayerType layer, ElementStates state)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            this[position.X, position.Y].SetState(layer, state);
            return true;
        }

        internal bool TryRemoveElementState(Point position, LayerType layer, ElementStates state)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            this[position.X, position.Y].RemoveState(layer, state);
            return true;
        }

        internal bool TryClearElementStates(Point position, LayerType layer)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            this[position.X, position.Y].ClearStates(layer);
            return true;
        }

        internal bool TryToggleElementState(Point position, LayerType layer, ElementStates state)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            this[position.X, position.Y].ToggleState(layer, state);
            return true;
        }

        internal void InstantiateElement(Point position, LayerType layer, ElementIndex index)
        {
            InstantiateElement(position, layer, ElementDatabase.GetElement(index));
        }

        internal void InstantiateElement(Point position, LayerType layer, Element value)
        {
            _ = TryInstantiateElement(position, layer, value);
        }

        internal void UpdateElementPosition(Point oldPosition, Point newPosition, LayerType layer)
        {
            _ = TryUpdateElementPosition(oldPosition, newPosition, layer);
        }

        internal void SwappingElements(Point element1Position, Point element2Position, LayerType layer)
        {
            _ = TrySwappingElements(element1Position, element2Position, layer);
        }

        internal void DestroyElement(Point position, LayerType layer)
        {
            _ = TryDestroyElement(position, layer);
        }

        internal void RemoveElement(Point position, LayerType layer)
        {
            _ = TryRemoveElement(position, layer);
        }

        internal void ReplaceElement(Point position, LayerType layer, ElementIndex index)
        {
            _ = TryReplaceElement(position, layer, index);
        }

        internal void ReplaceElement(Point position, LayerType layer, Element value)
        {
            _ = TryReplaceElement(position, layer, value);
        }

        internal Element GetElement(Point position, LayerType layer)
        {
            _ = TryGetElement(position, layer, out Element value);
            return value;
        }

        internal Slot GetSlot(Point position)
        {
            _ = TryGetSlot(position, out Slot value);
            return value;
        }

        internal IEnumerable<Slot> GetNeighboringSlots(Point position)
        {
            foreach (Point neighborPosition in position.GetNeighboringCardinalPoints())
            {
                if (TryGetSlot(neighborPosition, out Slot value))
                {
                    yield return value;
                }
            }
        }

        internal void SetElementTemperature(Point position, LayerType layer, double value)
        {
            _ = TrySetElementTemperature(position, layer, value);
        }

        internal void SetElementColorModifier(Point position, LayerType layer, Color value)
        {
            _ = TrySetElementColorModifier(position, layer, value);
        }

        internal void SetStoredElement(Point position, LayerType layer, ElementIndex index)
        {
            SetStoredElement(position, layer, ElementDatabase.GetElement(index));
        }

        internal void SetStoredElement(Point position, LayerType layer, Element element)
        {
            _ = TrySetStoredElement(position, layer, element);
        }

        internal bool HasElementState(Point position, LayerType layer, ElementStates state)
        {
            _ = TryHasElementState(position, layer, state, out bool value);
            return value;
        }

        internal void SetElementState(Point position, LayerType layer, ElementStates state)
        {
            _ = TrySetElementState(position, layer, state);
        }

        internal void RemoveElementState(Point position, LayerType layer, ElementStates state)
        {
            _ = TryRemoveElementState(position, layer, state);
        }

        internal void ClearElementStates(Point position, LayerType layer)
        {
            _ = TryClearElementStates(position, layer);
        }

        internal void ToggleElementState(Point position, LayerType layer, ElementStates state)
        {
            _ = TryToggleElementState(position, layer, state);
        }

        internal bool IsEmptySlot(Point position)
        {
            return !InsideTheWorldDimensions(position) || this[position.X, position.Y].IsEmpty;
        }

        internal bool IsEmptySlotLayer(Point position, LayerType layer)
        {
            return !InsideTheWorldDimensions(position) || this[position.X, position.Y].GetLayer(layer).HasState(ElementStates.IsEmpty);
        }

        internal uint GetTotalElementCount()
        {
            return GetTotalForegroundElementCount() + GetTotalBackgroundElementCount();
        }

        internal uint GetTotalForegroundElementCount()
        {
            return GetTotalElementCountForLayer(slot => !slot.ForegroundLayer.HasState(ElementStates.IsEmpty));
        }

        internal uint GetTotalBackgroundElementCount()
        {
            return GetTotalElementCountForLayer(slot => !slot.BackgroundLayer.HasState(ElementStates.IsEmpty));
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

        internal bool TryInstantiateExplosion(Point position, ExplosionBuilder explosionBuilder)
        {
            if (!InsideTheWorldDimensions(position) && this.instantiatedExplosions.Count >= ExplosionConstants.ACTIVE_EXPLOSIONS_LIMIT)
            {
                return false;
            }

            Explosion explosion = this.explosionPool.TryDequeue(out IPoolableObject pooledObject)
                ? (Explosion)pooledObject
                : new();

            explosion.Build(position, explosionBuilder);
            this.instantiatedExplosions.Enqueue(explosion);

            return true;
        }

        internal void InstantiateExplosion(Point position, ExplosionBuilder explosionBuilder)
        {
            _ = TryInstantiateExplosion(position, explosionBuilder);
        }

        private void HandleExplosions()
        {
            while (this.instantiatedExplosions.TryDequeue(out Explosion value))
            {
                HandleExplosion(value);
                this.explosionPool.Enqueue(value);
            }
        }

        private void InstantiateExplosionResidue(Point position, IEnumerable<ExplosionResidue> explosionResidues)
        {
            foreach (ExplosionResidue residue in explosionResidues)
            {
                LayerType targetLayer = SSRandom.Chance(50) ? LayerType.Foreground : LayerType.Background;

                if (SSRandom.Chance(residue.CreationChance))
                {
                    InstantiateElement(position, targetLayer, residue.Index);
                }
            }
        }

        private void HandleExplosion(Explosion explosion)
        {
            foreach (Point point in ShapePointGenerator.GenerateCirclePoints(explosion.Position, explosion.Radius))
            {
                if (!TryGetSlot(point, out Slot worldSlot))
                {
                    continue;
                }

                TryAffectPoint(worldSlot, point, explosion);
                InstantiateExplosionResidue(point, explosion.ExplosionResidues);
            }
        }

        private void TryAffectSlotLayer(SlotLayer worldSlotLayer, LayerType layer, Point targetPosition, Explosion explosion)
        {
            if (worldSlotLayer.HasState(ElementStates.IsEmpty))
            {
                return;
            }

            Element element = worldSlotLayer.Element;

            if (element.Characteristics.HasFlag(ElementCharacteristics.IsExplosionImmune))
            {
                return;
            }

            if (element.DefaultExplosionResistance >= explosion.Power)
            {
                worldSlotLayer.SetTemperatureValue(worldSlotLayer.Temperature + explosion.Heat);
            }
            else
            {
                DestroyElement(targetPosition, layer);
            }
        }

        private void TryAffectPoint(Slot worldSlot, Point targetPosition, Explosion explosion)
        {
            TryAffectSlotLayer(worldSlot.GetLayer(LayerType.Foreground), LayerType.Foreground, targetPosition, explosion);
            TryAffectSlotLayer(worldSlot.GetLayer(LayerType.Background), LayerType.Background, targetPosition, explosion);
        }

        #endregion

        #region ROUTINE

        #region Update

        internal void Update(GameTime gameTime)
        {
            if (!this.IsActive)
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
            if (!this.IsVisible)
            {
                return;
            }

            this.rendering.Draw(spriteBatch);
        }

        #endregion

        #endregion

        #region UTILITIES

        #region Start New

        internal void StartNew()
        {
            StartNew(this.information.Size);
        }

        internal void StartNew(Point size)
        {
            this.IsActive = true;
            this.IsVisible = true;

            if (this.information.Size != size)
            {
                Resize(size);
            }

            Reset();
        }

        #endregion

        #region Load From World File

        internal void LoadFromWorldSaveFile(SaveFile worldSaveFile)
        {
            this.gameManager.SetState(GameStates.IsSimulationPaused);

            // World
            StartNew(worldSaveFile.World.Information.Size);

            // Cache
            this.currentlySelectedWorldSaveFile = worldSaveFile;

            // Metadata
            this.information.Identifier = worldSaveFile.Header.Metadata.Identifier;
            this.information.Name = worldSaveFile.Header.Metadata.Name;
            this.information.Description = worldSaveFile.Header.Metadata.Description;

            // Time
            this.time.SetTime(worldSaveFile.World.Environment.Time.CurrentTime);
            this.time.IsFrozen = worldSaveFile.World.Environment.Time.IsFrozen;

            // Allocate Slots
            foreach (SaveFileSlot worldSlotData in worldSaveFile.World.Content.Slots)
            {
                if (worldSlotData.ForegroundLayer != null)
                {
                    LoadSlotLayerData(worldSaveFile.World.Resources, LayerType.Foreground, worldSlotData.Position, worldSlotData.ForegroundLayer);
                }

                if (worldSlotData.BackgroundLayer != null)
                {
                    LoadSlotLayerData(worldSaveFile.World.Resources, LayerType.Background, worldSlotData.Position, worldSlotData.BackgroundLayer);
                }
            }
        }

        private void LoadSlotLayerData(SaveFileWorldResources resources, LayerType layer, Point position, SaveFileSlotLayer worldSlotLayerData)
        {
            InstantiateElement(position, layer, resources.Elements.FindValueByIndex(worldSlotLayerData.ElementIndex));

            Slot worldSlot = GetSlot(position);

            worldSlot.SetTemperatureValue(layer, worldSlotLayerData.Temperature);
            worldSlot.SetState(layer, ElementStates.FreeFalling);
            worldSlot.SetColorModifier(layer, worldSlotLayerData.ColorModifier);
            worldSlot.SetStoredElement(layer, ElementDatabase.GetElement(resources.Elements.FindValueByIndex(worldSlotLayerData.StoredElementIndex)));
        }

        #endregion

        internal void Resize(Point size)
        {
            DestroySlots();

            this.information.Size = size;
            this.slots = new Slot[size.X, size.Y];

            InstantiateSlots();
        }

        internal void Reload()
        {
            if (this.currentlySelectedWorldSaveFile != null)
            {
                LoadFromWorldSaveFile(this.currentlySelectedWorldSaveFile);
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

                    RemoveElement(new(x, y), LayerType.Foreground);
                    RemoveElement(new(x, y), LayerType.Background);
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