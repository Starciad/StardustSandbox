using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Collections;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Elements;
using StardustSandbox.Enums.Indexers;
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

        internal void Initialize()
        {
            this.rendering.Initialize();
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

        internal bool TryInstantiateElement(Point position, LayerType worldLayer, Element value)
        {
            if (!InsideTheWorldDimensions(position) || !IsEmptySlotLayer(position, worldLayer))
            {
                return false;
            }

            NotifyChunk(position);

            Slot worldSlot = this[position.X, position.Y];
            worldSlot.Instantiate(position, worldLayer, value);

            this.worldElementContext.UpdateInformation(position, worldLayer, worldSlot);
            value.Context = this.worldElementContext;
            value.Instantiate();

            return true;
        }

        internal bool TryInstantiateElement(Point position, LayerType worldLayer, ElementIndex index)
        {
            return TryInstantiateElement(position, worldLayer, ElementDatabase.GetElementByIndex(index));
        }

        internal bool TryUpdateElementPosition(Point oldPosition, Point newPosition, LayerType worldLayer)
        {
            if (!InsideTheWorldDimensions(oldPosition) ||
                !InsideTheWorldDimensions(newPosition) ||
                 IsEmptySlotLayer(oldPosition, worldLayer) ||
                !IsEmptySlotLayer(newPosition, worldLayer))
            {
                return false;
            }

            NotifyChunk(oldPosition);
            NotifyChunk(newPosition);

            this[newPosition.X, newPosition.Y].Copy(worldLayer, this[oldPosition.X, oldPosition.Y].GetLayer(worldLayer));
            this[newPosition.X, newPosition.Y].SetPosition(newPosition);
            this[oldPosition.X, oldPosition.Y].Destroy(worldLayer);

            return true;
        }

        internal bool TrySwappingElements(Point element1Position, Point element2Position, LayerType worldLayer)
        {
            if (!InsideTheWorldDimensions(element1Position) ||
                !InsideTheWorldDimensions(element2Position) ||
                IsEmptySlotLayer(element1Position, worldLayer) ||
                IsEmptySlotLayer(element2Position, worldLayer))
            {
                return false;
            }

            NotifyChunk(element1Position);
            NotifyChunk(element2Position);

            Slot tempSlot = this.worldSlotsPool.TryDequeue(out IPoolableObject value) ? (Slot)value : new();

            tempSlot.Copy(worldLayer, this[element1Position.X, element1Position.Y].GetLayer(worldLayer));

            this[element1Position.X, element1Position.Y].Copy(worldLayer, this[element2Position.X, element2Position.Y].GetLayer(worldLayer));
            this[element2Position.X, element2Position.Y].Copy(worldLayer, tempSlot.GetLayer(worldLayer));

            this[element1Position.X, element1Position.Y].SetPosition(element1Position);
            this[element2Position.X, element2Position.Y].SetPosition(element2Position);

            this.worldSlotsPool.Enqueue(tempSlot);

            return true;
        }

        internal bool TryDestroyElement(Point position, LayerType worldLayer)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptySlotLayer(position, worldLayer))
            {
                return false;
            }

            NotifyChunk(position);

            Slot worldSlot = this[position.X, position.Y];
            SlotLayer worldSlotLayer = worldSlot.GetLayer(worldLayer);

            this.worldElementContext.UpdateInformation(position, worldLayer, worldSlot);
            worldSlotLayer.Element.Context = this.worldElementContext;
            worldSlotLayer.Element.Destroy();
            worldSlotLayer.Destroy();

            return true;
        }

        internal bool TryRemoveElement(Point position, LayerType worldLayer)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptySlotLayer(position, worldLayer))
            {
                return false;
            }

            NotifyChunk(position);

            this[position.X, position.Y].Destroy(worldLayer);

            return true;
        }

        internal bool TryReplaceElement(Point position, LayerType worldLayer, ElementIndex index)
        {
            return TryRemoveElement(position, worldLayer) && TryInstantiateElement(position, worldLayer, index);
        }

        internal bool TryReplaceElement(Point position, LayerType worldLayer, Element value)
        {
            return TryRemoveElement(position, worldLayer) && TryInstantiateElement(position, worldLayer, value);
        }

        internal bool TryGetElement(Point position, LayerType worldLayer, out Element value)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptySlotLayer(position, worldLayer))
            {
                value = null;
                return false;
            }

            SlotLayer worldSlotLayer = this[position.X, position.Y].GetLayer(worldLayer);

            if (worldSlotLayer.IsEmpty)
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

        internal bool TrySetElementTemperature(Point position, LayerType worldLayer, short value)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptySlotLayer(position, worldLayer))
            {
                return false;
            }

            SlotLayer layer = this[position.X, position.Y].GetLayer(worldLayer);

            if (layer.Temperature != value)
            {
                NotifyChunk(position);
                layer.SetTemperatureValue(value);
            }

            return true;
        }

        internal bool TrySetElementFreeFalling(Point position, LayerType worldLayer, bool value)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptySlotLayer(position, worldLayer))
            {
                return false;
            }

            this[position.X, position.Y].GetLayer(worldLayer).SetFreeFalling(value);

            return true;
        }

        internal bool TrySetElementColorModifier(Point position, LayerType worldLayer, Color value)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptySlotLayer(position, worldLayer))
            {
                return false;
            }

            this[position.X, position.Y].GetLayer(worldLayer).SetColorModifier(value);

            return true;
        }

        internal bool TrySetStoredElement(Point position, LayerType worldLayer, ElementIndex index)
        {
            return TrySetStoredElement(position, worldLayer, ElementDatabase.GetElementByIndex(index));
        }

        internal bool TrySetStoredElement(Point position, LayerType worldLayer, Element element)
        {
            if (!InsideTheWorldDimensions(position) || IsEmptySlotLayer(position, worldLayer))
            {
                return false;
            }

            this[position.X, position.Y].GetLayer(worldLayer).SetStoredElement(element);

            return true;
        }

        internal void InstantiateElement(Point position, LayerType worldLayer, ElementIndex index)
        {
            InstantiateElement(position, worldLayer, ElementDatabase.GetElementByIndex(index));
        }

        internal void InstantiateElement(Point position, LayerType worldLayer, Element value)
        {
            _ = TryInstantiateElement(position, worldLayer, value);
        }

        internal void UpdateElementPosition(Point oldPosition, Point newPosition, LayerType worldLayer)
        {
            _ = TryUpdateElementPosition(oldPosition, newPosition, worldLayer);
        }

        internal void SwappingElements(Point element1Position, Point element2Position, LayerType worldLayer)
        {
            _ = TrySwappingElements(element1Position, element2Position, worldLayer);
        }

        internal void DestroyElement(Point position, LayerType worldLayer)
        {
            _ = TryDestroyElement(position, worldLayer);
        }

        internal void RemoveElement(Point position, LayerType worldLayer)
        {
            _ = TryRemoveElement(position, worldLayer);
        }

        internal void ReplaceElement(Point position, LayerType worldLayer, ElementIndex index)
        {
            _ = TryReplaceElement(position, worldLayer, index);
        }

        internal void ReplaceElement(Point position, LayerType worldLayer, Element value)
        {
            _ = TryReplaceElement(position, worldLayer, value);
        }

        internal Element GetElement(Point position, LayerType worldLayer)
        {
            _ = TryGetElement(position, worldLayer, out Element value);
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

        internal void SetElementTemperature(Point position, LayerType worldLayer, short value)
        {
            _ = TrySetElementTemperature(position, worldLayer, value);
        }

        internal void SetElementFreeFalling(Point position, LayerType worldLayer, bool value)
        {
            _ = TrySetElementFreeFalling(position, worldLayer, value);
        }

        internal void SetElementColorModifier(Point position, LayerType worldLayer, Color value)
        {
            _ = TrySetElementColorModifier(position, worldLayer, value);
        }

        internal void SetStoredElement(Point position, LayerType worldLayer, ElementIndex index)
        {
            SetStoredElement(position, worldLayer, ElementDatabase.GetElementByIndex(index));
        }

        internal void SetStoredElement(Point position, LayerType worldLayer, Element element)
        {
            _ = TrySetStoredElement(position, worldLayer, element);
        }

        internal bool IsEmptySlot(Point position)
        {
            return !InsideTheWorldDimensions(position) || this[position.X, position.Y].IsEmpty;
        }

        internal bool IsEmptySlotLayer(Point position, LayerType worldLayer)
        {
            return !InsideTheWorldDimensions(position) || this[position.X, position.Y].GetLayer(worldLayer).IsEmpty;
        }

        internal uint GetTotalElementCount()
        {
            return GetTotalForegroundElementCount() + GetTotalBackgroundElementCount();
        }

        internal uint GetTotalForegroundElementCount()
        {
            return GetTotalElementCountForLayer(slot => !slot.ForegroundLayer.IsEmpty);
        }

        internal uint GetTotalBackgroundElementCount()
        {
            return GetTotalElementCountForLayer(slot => !slot.BackgroundLayer.IsEmpty);
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

        private void TryAffectSlotLayer(SlotLayer worldSlotLayer, LayerType worldLayer, Point targetPosition, Explosion explosion)
        {
            if (worldSlotLayer.IsEmpty)
            {
                return;
            }

            Element element = worldSlotLayer.Element;

            if (element.IsExplosionImmune)
            {
                return;
            }

            if (element.DefaultExplosionResistance >= explosion.Power)
            {
                worldSlotLayer.SetTemperatureValue((short)(worldSlotLayer.Temperature + explosion.Heat));
            }
            else
            {
                DestroyElement(targetPosition, worldLayer);
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

            this.time.Update();

            UpdateWorld(gameTime);
            UpdateExplosions();
        }

        private void UpdateWorld(GameTime gameTime)
        {
            this.simulation.Update();

            if (this.simulation.CanContinueExecution())
            {
                this.chunking.Update();
                this.updating.Update(gameTime);
            }
        }

        private void UpdateExplosions()
        {
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

        private void LoadSlotLayerData(SaveFileWorldResources resources, LayerType worldLayer, Point position, SaveFileSlotLayer worldSlotLayerData)
        {
            InstantiateElement(position, worldLayer, resources.Elements.FindValueByIndex(worldSlotLayerData.ElementIndex));

            Slot worldSlot = GetSlot(position);

            worldSlot.SetTemperatureValue(worldLayer, worldSlotLayerData.Temperature);
            worldSlot.SetFreeFalling(worldLayer, worldSlotLayerData.FreeFalling);
            worldSlot.SetColorModifier(worldLayer, worldSlotLayerData.ColorModifier);
            worldSlot.SetStoredElement(worldLayer, ElementDatabase.GetElementByIndex(resources.Elements.FindValueByIndex(worldSlotLayerData.StoredElementIndex)));
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
                    this.time.SecondsPerFrames = TimeConstants.DEFAULT_NORMAL_SECONDS_PER_FRAMES;
                    this.simulation.SetSpeed(SimulationSpeed.Normal);
                    break;

                case SimulationSpeed.Fast:
                    this.time.SecondsPerFrames = TimeConstants.DEFAULT_FAST_SECONDS_PER_FRAMES;
                    this.simulation.SetSpeed(SimulationSpeed.Fast);
                    break;

                case SimulationSpeed.VeryFast:
                    this.time.SecondsPerFrames = TimeConstants.DEFAULT_VERY_FAST_SECONDS_PER_FRAMES;
                    this.simulation.SetSpeed(SimulationSpeed.VeryFast);
                    break;

                default:
                    this.time.SecondsPerFrames = TimeConstants.DEFAULT_NORMAL_SECONDS_PER_FRAMES;
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