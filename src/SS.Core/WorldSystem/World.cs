/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Achievements;
using StardustSandbox.Core.Cameras;
using StardustSandbox.Core.Collections;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Enums.Achievements;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.Serialization;
using StardustSandbox.Core.Enums.Simulation;
using StardustSandbox.Core.Enums.States;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Explosions;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.InputSystem;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Collections;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.Serialization.Saving;
using StardustSandbox.Core.Serialization.Saving.Data;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StardustSandbox.Core.WorldSystem
{
    internal sealed class World : IResettable
    {
        internal string Name { get; set; }
        internal string Description { get; set; }
        internal Point Size { get; set; }

        internal bool CanUpdate { get; set; }
        internal bool CanDraw { get; set; }

        internal Temperature Temperature => this.temperature;
        internal Time Time => this.time;
        internal WorldRendering Rendering => this.rendering;

        private Slot[,] slots;

        private readonly Simulation simulation;
        private readonly Temperature temperature;
        private readonly Time time;

        private readonly WorldChunking chunking;
        private readonly WorldRendering rendering;
        private readonly WorldUpdating updating;

        private readonly ElementContext worldElementContext;

        private readonly ObjectPool worldSlotsPool;
        private readonly ObjectPool explosionPool;

        private readonly Queue<Explosion> instantiatedExplosions;

        private readonly AchievementSystem achievementSystem;
        private readonly AssetDatabase assetDatabase;
        private readonly ElementNeighbors elementNeighbors;
        private readonly ElementDatabase elementDatabase;

        internal Slot this[int x, int y]
        {
            get => this.slots[x, y];
            set => this.slots[x, y] = value;
        }

        internal Slot this[Point point]
        {
            get => this.slots[point.X, point.Y];
            set => this.slots[point.X, point.Y] = value;
        }

        internal World(AchievementSystem achievementSystem, AssetDatabase assetDatabase, ElementDatabase elementDatabase, PlayerInputController playerInputController)
        {
            this.assetDatabase = assetDatabase;
            this.elementDatabase = elementDatabase;

            this.simulation = new();
            this.time = new();
            this.temperature = new(this.time);

            this.worldSlotsPool = new();
            this.explosionPool = new();

            this.chunking = new(this);
            this.rendering = new(assetDatabase, playerInputController, this);
            this.updating = new(this);

            this.instantiatedExplosions = new(ExplosionConstants.MAX_SIMULTANEOUS_EXPLOSIONS);

            this.worldElementContext = new(this);
            this.elementNeighbors = new();
        }

        public void Reset()
        {
            this.Name = string.Empty;
            this.Description = string.Empty;

            GameStatistics.ResetWorldStatistics();

            this.chunking.Reset();
            this.temperature.Reset();
            this.updating.Reset();

            Clear();
        }

        #region UTILITIES

        internal void StartNew(Point size)
        {
            this.CanUpdate = true;
            this.CanDraw = true;

            if (this.Size != size)
            {
                Resize(size);
            }

            Reset();
        }

        internal void StartNew()
        {
            StartNew(this.Size);
        }

        internal SlotData[] Serialize()
        {
            List<SlotData> slots = [];

            for (int y = 0; y < this.Size.Y; y++)
            {
                for (int x = 0; x < this.Size.X; x++)
                {
                    Point point = new(x, y);

                    if (IsEmptySlot(point))
                    {
                        continue;
                    }

                    slots.Add(new(GetSlot(point)));
                }
            }

            return [.. slots];
        }

        internal void Deserialize(string saveFileName)
        {
            SaveFile saveFile = SavingSerializer.Load(saveFileName, LoadFlags.Metadata | LoadFlags.Properties | LoadFlags.Environment | LoadFlags.Content);

            GameHandler.SetState(GameStates.IsSimulationPaused);

            // World
            StartNew(saveFile.Properties.Size);

            // Metadata
            this.Name = saveFile.Metadata.Name;
            this.Description = saveFile.Metadata.Description;

            // Time
            this.time.SetTime(saveFile.Environment.CurrentTime);
            this.time.IsFrozen = saveFile.Environment.IsFrozen;

            // Temperature
            this.temperature.Deserialize(saveFile.Environment.Temperatures);

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
            InstantiateElementIndex(position, layer, slotLayerData.ElementIndex);

            Slot slot = GetSlot(position);

            slot.SetTemperatureValue(layer, slotLayerData.Temperature);
            slot.SetState(layer, slotLayerData.States);
            slot.SetColorModifier(layer, slotLayerData.ColorModifier);
            slot.SetStoredElement(layer, slotLayerData.StoredElementIndex);
        }

        internal void Resize(Point size)
        {
            DestroySlots();

            this.Size = size;
            this.slots = new Slot[size.X, size.Y];

            InstantiateSlots();
        }

        internal void Reload()
        {
            if (GameHandler.HasSaveFileLoaded)
            {
                Deserialize(GameHandler.LoadedSaveFileName);
                return;
            }

            Clear();
        }

        internal bool IsWithinHorizontalBounds(int x)
        {
            return x >= 0 && x < this.Size.X;
        }

        internal bool IsWithinVerticalBounds(int y)
        {
            return y >= 0 && y < this.Size.Y;
        }

        internal bool IsWithinBounds(int x, int y)
        {
            return IsWithinHorizontalBounds(x) && IsWithinVerticalBounds(y);
        }

        internal bool IsWithinBounds(Point position)
        {
            return IsWithinBounds(position.X, position.Y);
        }

        internal void SetSpeed(SimulationSpeed speed)
        {
            this.time.SetSpeed(speed);
            this.simulation.SetSpeed(speed);
        }

        internal void Clear()
        {
            if (this == null)
            {
                return;
            }

            for (int y = 0; y < this.Size.Y; y++)
            {
                for (int x = 0; x < this.Size.X; x++)
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

        private void InstantiateSlots()
        {
            if (this.slots == null || this.slots.Length == 0)
            {
                return;
            }

            for (int y = 0; y < this.Size.Y; y++)
            {
                for (int x = 0; x < this.Size.X; x++)
                {
                    this[x, y] = this.worldSlotsPool.TryDequeue(out IPoolableObject value) ? (Slot)value : new(this.elementDatabase);
                }
            }
        }

        private void DestroySlots()
        {
            if (this.slots == null || this.slots.Length == 0)
            {
                return;
            }

            for (int y = 0; y < this.Size.Y; y++)
            {
                for (int x = 0; x < this.Size.X; x++)
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

        #region ELEMENTS

        internal bool TryInstantiateElementIndex(Point position, Layer layer, ElementIndex index)
        {
            if (!IsWithinBounds(position) || !IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            NotifyChunk(position);

            Slot slot = this[position];

            slot.Position = position;
            slot.Instantiate(layer, index);

            this.worldElementContext.Initialize(position, layer);

            Element element = this.elementDatabase.GetElement(index);

            element.SetContext(this.worldElementContext);
            element.Instantiate();

            GameStatistics.RegisterInstantiatedElement(index);

            return true;
        }

        internal bool TryUpdateElementPosition(Point oldPosition, Point newPosition, Layer layer)
        {
            if (!IsWithinBounds(oldPosition) ||
                !IsWithinBounds(newPosition) ||
                 IsEmptySlotLayer(oldPosition, layer) ||
                !IsEmptySlotLayer(newPosition, layer) ||
                oldPosition == newPosition)
            {
                return false;
            }

            NotifyChunk(oldPosition);
            NotifyChunk(newPosition);

            this[newPosition].Copy(layer, this[oldPosition].GetLayer(layer));
            this[newPosition].Position = newPosition;
            this[oldPosition].Destroy(layer);

            return true;
        }

        internal bool TrySwappingElements(Point element1Position, Point element2Position, Layer layer)
        {
            if (!IsWithinBounds(element1Position) ||
                !IsWithinBounds(element2Position) ||
                IsEmptySlotLayer(element1Position, layer) ||
                IsEmptySlotLayer(element2Position, layer) ||
                element1Position == element2Position)
            {
                return false;
            }

            NotifyChunk(element1Position);
            NotifyChunk(element2Position);

            Slot tempSlot = this.worldSlotsPool.TryDequeue(out IPoolableObject value) ? (Slot)value : new(this.elementDatabase);

            tempSlot.Copy(layer, this[element1Position].GetLayer(layer));

            this[element1Position].Copy(layer, this[element2Position].GetLayer(layer));
            this[element2Position].Copy(layer, tempSlot.GetLayer(layer));

            this[element1Position].Position = element1Position;
            this[element2Position].Position = element2Position;

            this.worldSlotsPool.Enqueue(tempSlot);

            return true;
        }

        internal bool TryDestroyElement(Point position, Layer layer)
        {
            if (!IsWithinBounds(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            NotifyChunk(position);

            Slot slot = this[position];
            SlotLayer slotLayer = slot.GetLayer(layer);

            this.worldElementContext.Initialize(position, layer);
            slotLayer.Element.SetContext(this.worldElementContext);
            slotLayer.Element.Destroy();
            slotLayer.Destroy();

            return true;
        }

        internal bool TryRemoveElement(Point position, Layer layer)
        {
            if (!IsWithinBounds(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            NotifyChunk(position);

            this[position].Destroy(layer);

            return true;
        }

        internal bool TryReplaceElementIndex(Point position, Layer layer, ElementIndex index)
        {
            return TryRemoveElement(position, layer) && TryInstantiateElementIndex(position, layer, index);
        }

        internal bool TryGetElementIndex(Point position, Layer layer, out ElementIndex index)
        {
            index = ElementIndex.None;

            if (!IsWithinBounds(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            SlotLayer slotLayer = this[position].GetLayer(layer);

            if (slotLayer.IsEmpty)
            {
                return false;
            }

            index = slotLayer.ElementIndex;
            return true;
        }

        internal bool TryGetElement(Point position, Layer layer, out Element value)
        {
            value = null;

            if (!IsWithinBounds(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            SlotLayer slotLayer = this[position].GetLayer(layer);
            if (slotLayer.IsEmpty)
            {
                return false;
            }

            value = slotLayer.Element;
            return true;
        }

        internal bool TryGetSlot(Point position, out Slot value)
        {
            value = null;

            if (!IsWithinBounds(position) || IsEmptySlot(position))
            {
                return false;
            }

            value = this[position];
            return true;
        }

        internal bool TryGetSlotLayer(Point position, Layer layer, out SlotLayer slotLayer)
        {
            slotLayer = null;

            if (!IsWithinBounds(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            slotLayer = this[position].GetLayer(layer);
            return true;
        }

        internal bool TrySetElementTemperature(Point position, Layer layer, float value)
        {
            if (!IsWithinBounds(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            SlotLayer slotLayer = this[position].GetLayer(layer);

            if (slotLayer.Temperature != value)
            {
                NotifyChunk(position);
                slotLayer.Temperature = value;
            }

            return true;
        }

        internal bool TrySetElementColorModifier(Point position, Layer layer, Color value)
        {
            if (!IsWithinBounds(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            this[position].GetLayer(layer).ColorModifier = value;

            return true;
        }

        internal bool TryHasStoredElement(Point position, Layer layer, out bool value)
        {
            value = false;

            if (!IsWithinBounds(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            value = this[position].GetLayer(layer).HasStoredElement;
            return true;
        }

        internal bool TrySetStoredElementIndex(Point position, Layer layer, ElementIndex index)
        {
            if (!IsWithinBounds(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            this[position].GetLayer(layer).StoredElementIndex = index;
            return true;
        }

        internal bool TryGetStoredElementIndex(Point position, Layer layer, out ElementIndex index)
        {
            index = ElementIndex.None;

            if (!IsWithinBounds(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            index = this[position].GetLayer(layer).StoredElementIndex;

            return index is not ElementIndex.None;
        }

        internal bool TryGetStoredElement(Point position, Layer layer, out Element value)
        {
            value = null;
            if (!IsWithinBounds(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            SlotLayer slotLayer = this[position].GetLayer(layer);
            if (!slotLayer.HasStoredElement)
            {
                return false;
            }

            value = slotLayer.StoredElement;
            return true;
        }

        internal bool TryHasElementState(Point position, Layer layer, ElementStates state, out bool value)
        {
            value = false;

            if (!IsWithinBounds(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            value = this[position].HasState(layer, state);
            return true;
        }

        internal bool TrySetElementState(Point position, Layer layer, ElementStates state)
        {
            if (!IsWithinBounds(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            this[position].SetState(layer, state);
            return true;
        }

        internal bool TryRemoveElementState(Point position, Layer layer, ElementStates state)
        {
            if (!IsWithinBounds(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            this[position].RemoveState(layer, state);
            return true;
        }

        internal bool TryClearElementStates(Point position, Layer layer)
        {
            if (!IsWithinBounds(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            this[position].ClearStates(layer);
            return true;
        }

        internal bool TryToggleElementState(Point position, Layer layer, ElementStates state)
        {
            if (!IsWithinBounds(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            this[position].ToggleState(layer, state);
            return true;
        }

        internal void InstantiateElementIndex(Point position, Layer layer, ElementIndex index)
        {
            _ = TryInstantiateElementIndex(position, layer, index);
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

        internal void ReplaceElementIndex(Point position, Layer layer, ElementIndex index)
        {
            _ = TryReplaceElementIndex(position, layer, index);
        }

        internal ElementIndex GetElementIndex(Point position, Layer layer)
        {
            _ = TryGetElementIndex(position, layer, out ElementIndex index);
            return index;
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

        internal SlotLayer GetSlotLayer(Point position, Layer layer)
        {
            _ = TryGetSlotLayer(position, layer, out SlotLayer slotLayer);
            return slotLayer;
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

                    Point neighborPosition = new(position.X + dx, position.Y + dy);

                    if (!IsWithinBounds(neighborPosition))
                    {
                        continue;
                    }

                    this.elementNeighbors.SetNeighbor(index, GetSlot(neighborPosition));
                    index++;
                }
            }

            this.elementNeighbors.SetNeighborCountOccupied(index);

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

        internal void SetStoredElementIndex(Point position, Layer layer, ElementIndex index)
        {
            _ = TrySetStoredElementIndex(position, layer, index);
        }

        internal ElementIndex GetStoredElementIndex(Point position, Layer layer)
        {
            _ = TryGetStoredElementIndex(position, layer, out ElementIndex index);
            return index;
        }

        internal Element GetStoredElement(Point position, Layer layer)
        {
            _ = TryGetStoredElement(position, layer, out Element value);
            return value;
        }

        internal bool HasStoredElement(Point position, Layer layer)
        {
            _ = TryHasStoredElement(position, layer, out bool value);
            return value;
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
            return !IsWithinBounds(position) || this[position].IsEmpty;
        }

        internal bool IsEmptySlotLayer(Point position, Layer layer)
        {
            return !IsWithinBounds(position) || this[position].GetLayer(layer).IsEmpty;
        }

        internal uint GetTotalElementCount()
        {
            return GetTotalForegroundElementCount() + GetTotalBackgroundElementCount();
        }

        internal uint GetTotalForegroundElementCount()
        {
            return GetTotalElementCountForLayer(slot => !slot.Foreground.IsEmpty);
        }

        internal uint GetTotalBackgroundElementCount()
        {
            return GetTotalElementCountForLayer(slot => !slot.Background.IsEmpty);
        }

        private uint GetTotalElementCountForLayer(Func<Slot, bool> predicate)
        {
            uint count = 0;
            object lockObj = new();

            _ = Parallel.For(0, this.Size.Y, y =>
            {
                uint localCount = 0;

                for (int x = 0; x < this.Size.X; x++)
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
            if (!IsWithinBounds(position) && this.instantiatedExplosions.Count >= ExplosionConstants.MAX_SIMULTANEOUS_EXPLOSIONS)
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
            foreach (Point point in ShapePointGenerator.EnumerateCirclePoints(explosion.Position, Convert.ToInt32(explosion.Radius)))
            {
                if (!IsWithinBounds(point))
                {
                    continue;
                }

                if (TryGetSlot(point, out Slot slot))
                {
                    TryAffectPoint(slot, point, explosion);
                }

                InstantiateElementIndex(point, explosion.Layer, explosion.ExplosionResidues.GetRandomItem());
            }

            this.achievementSystem.Unlock(AchievementIndex.ACH_016);
        }

        private void TryAffectSlotLayer(SlotLayer slotLayer, Layer layer, Point targetPosition, Explosion explosion)
        {
            if (slotLayer.IsEmpty)
            {
                return;
            }

            if (slotLayer.Element.HasCharacteristic(ElementCharacteristics.IsExplosionImmune))
            {
                return;
            }

            if (slotLayer.Element.DefaultExplosionResistance >= explosion.Power)
            {
                slotLayer.Temperature += explosion.Heat;
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

        internal void Update(GameTime gameTime)
        {
            if (!this.CanUpdate)
            {
                return;
            }

            this.time.Update(gameTime);
            this.simulation.Update(gameTime);
            this.temperature.Update();

            if (this.simulation.CanContinueExecution())
            {
                this.chunking.Update();
                this.updating.Update(gameTime);
            }

            HandleExplosions();
        }

        private void DrawWorldBorder(SpriteBatch spriteBatch)
        {
            int left = -1;
            int top = -1;
            int right = this.Size.X;
            int bottom = this.Size.Y;

            Texture2D texture = this.assetDatabase.GetTexture(TextureIndex.Frames);
            int gridSize = WorldConstants.TILE_SIZE;

            // Top line
            for (int x = left; x <= right; x++)
            {
                FrameSlice slice =
                    x == left ? FrameSlice.Northwest :
                    x == right ? FrameSlice.Northeast :
                    FrameSlice.North;

                spriteBatch.Draw(
                    texture,
                    new Rectangle(x * gridSize, top * gridSize, gridSize, gridSize),
                    WorldConstants.FRAME_SLICES[(byte)slice],
                    Color.White
                );
            }

            // Bottom line
            if (bottom != top)
            {
                for (int x = left; x <= right; x++)
                {
                    FrameSlice slice =
                        x == left ? FrameSlice.Southwest :
                        x == right ? FrameSlice.Southeast :
                        FrameSlice.South;

                    spriteBatch.Draw(
                        texture,
                        new Rectangle(x * gridSize, bottom * gridSize, gridSize, gridSize),
                        WorldConstants.FRAME_SLICES[(byte)slice],
                        Color.White
                    );
                }
            }

            // Sides (excluding corners)
            for (int y = top + 1; y <= bottom - 1; y++)
            {
                spriteBatch.Draw(
                    texture,
                    new Rectangle(left * gridSize, y * gridSize, gridSize, gridSize),
                    WorldConstants.FRAME_SLICES[(byte)FrameSlice.West],
                    Color.White
                );

                if (right != left)
                {
                    spriteBatch.Draw(
                        texture,
                        new Rectangle(right * gridSize, y * gridSize, gridSize, gridSize),
                        WorldConstants.FRAME_SLICES[(byte)FrameSlice.East],
                        Color.White
                    );
                }
            }
        }

        internal void Draw(SpriteBatch spriteBatch, Camera2D camera, GameLaunchOptions options)
        {
            if (!this.CanDraw)
            {
                return;
            }

            DrawWorldBorder(spriteBatch);

            if (options.ShowChunks)
            {
                this.chunking.Draw(spriteBatch, this.assetDatabase);
            }

            this.rendering.Draw(spriteBatch, this.assetDatabase, camera);
        }

        #endregion
    }
}
