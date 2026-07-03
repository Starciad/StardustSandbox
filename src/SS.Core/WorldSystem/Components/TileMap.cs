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

using StardustSandbox.Core.Collections;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.Indexers;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Events.Elements;
using StardustSandbox.Core.Interfaces.Collections;
using StardustSandbox.Core.WorldSystem.Slots;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.WorldSystem.Components
{
    internal sealed class TileMap
    {
        internal Point Size => new(this.width, this.height);
        internal int Width => this.width;
        internal int Height => this.height;

        internal int ActiveForegroundElementCount => this.activeForegroundElementCount;
        internal int ActiveBackgroundElementCount => this.activeBackgroundElementCount;
        internal int UniqueActiveElementCount => this.uniqueActiveElements.Count;
        internal int ActiveCorruptedElementCount => this.activeCorruptedElementCount;
        internal int TotalActiveElementCount => this.activeForegroundElementCount + this.activeBackgroundElementCount;

        internal int MaxForegroundElementCapacity => this.width * this.height;
        internal int MaxBackgroundElementCapacity => this.width * this.height;
        internal int MaxTotalElementCapacity => this.width * this.height * 2;

        private int totalForegroundElementCount;
        private int totalBackgroundElementCount;
        private int activeCorruptedElementCount;

        private int activeForegroundElementCount;
        private int activeBackgroundElementCount;

        private int width;
        private int height;

        private Slot[,] slots;

        private readonly ElementDatabase elementDatabase;
        private readonly GameEvents gameEvents;
        private readonly ElementNeighbors elementNeighbors = new();
        private readonly ObjectPool slotObjectPool = new();

        private readonly HashSet<Element> uniqueActiveElements = [];

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

        internal TileMap(ElementDatabase elementDatabase, GameEvents gameEvents)
        {
            this.elementDatabase = elementDatabase;
            this.gameEvents = gameEvents;
        }

        #region Utilities

        internal void Clear()
        {
            for (int y = 0; y < this.height; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    if (IsEmpty(new(x, y)))
                    {
                        continue;
                    }

                    Remove(new(x, y), Layer.Foreground);
                    Remove(new(x, y), Layer.Background);
                }
            }

            ResetCounts();
        }

        private void InstantiateSlots()
        {
            if (this.slots == null || this.slots.Length == 0)
            {
                return;
            }

            for (int y = 0; y < this.height; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    this[x, y] = this.slotObjectPool.TryDequeue(out IPoolableObject value) ? (Slot)value : new(this.elementDatabase);
                }
            }
        }

        private void DestroySlots()
        {
            if (this.slots == null || this.slots.Length == 0)
            {
                return;
            }

            for (int y = 0; y < this.height; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    if (this[x, y] == null)
                    {
                        continue;
                    }

                    this.slotObjectPool.Enqueue(this[x, y]);
                    this[x, y] = null;
                }
            }
        }

        internal void Resize(int width, int height)
        {
            DestroySlots();

            this.width = width;
            this.height = height;

            this.slots = new Slot[width, height];

            InstantiateSlots();
        }

        internal void Resize(Point value)
        {
            Resize(value.X, value.Y);
        }

        internal bool IsWithinHorizontalBounds(int x)
        {
            return x >= 0 && x < this.width;
        }

        internal bool IsWithinVerticalBounds(int y)
        {
            return y >= 0 && y < this.height;
        }

        internal bool IsWithinBounds(int x, int y)
        {
            return IsWithinHorizontalBounds(x) && IsWithinVerticalBounds(y);
        }

        internal bool IsWithinBounds(Point position)
        {
            return IsWithinBounds(position.X, position.Y);
        }

        #endregion

        #region Counters

        private void ResetCounts()
        {
            this.totalForegroundElementCount = 0;
            this.totalBackgroundElementCount = 0;
            this.activeCorruptedElementCount = 0;
        }

        private void IncrementSpecificElementCount(Element element)
        {
            if (element is null)
            {
                return;
            }

            if (element.IsCorruption)
            {
                this.activeCorruptedElementCount++;
            }
        }

        private void IncrementLayerElementCount(Layer layer)
        {
            switch (layer)
            {
                case Layer.Foreground:
                    this.totalForegroundElementCount++;
                    this.activeForegroundElementCount++;
                    break;
                case Layer.Background:
                    this.totalBackgroundElementCount++;
                    this.activeBackgroundElementCount++;
                    break;
                default:
                    break;
            }
        }

        private void IncrementElementCount(Element element, Layer layer)
        {
            IncrementSpecificElementCount(element);
            IncrementLayerElementCount(layer);
        }

        private void DecrementSpecificElementCount(Element element)
        {
            if (element is null)
            {
                return;
            }

            if (element.IsCorruption)
            {
                this.activeCorruptedElementCount = Math.Max(0, this.activeCorruptedElementCount - 1);
            }
        }

        private void DecrementLayerElementCount(Layer layer)
        {
            switch (layer)
            {
                case Layer.Foreground:
                    this.totalForegroundElementCount = Math.Max(0, this.totalForegroundElementCount - 1);
                    this.activeForegroundElementCount = Math.Max(0, this.activeForegroundElementCount - 1);
                    break;
                case Layer.Background:
                    this.totalBackgroundElementCount = Math.Max(0, this.totalBackgroundElementCount - 1);
                    this.activeBackgroundElementCount = Math.Max(0, this.activeBackgroundElementCount - 1);
                    break;
                default:
                    break;
            }
        }

        private void DecrementElementCount(Element element, Layer layer)
        {
            DecrementSpecificElementCount(element);
            DecrementLayerElementCount(layer);
        }

        #endregion

        #region Elements

        #region Try Methods

        internal bool TryDestroy(Point position, Layer layer)
        {
            if (!IsWithinBounds(position) || IsEmpty(position, layer))
            {
                return false;
            }

            ElementIndex index = this[position].GetElementIndex(layer);
            this[position].Destroy(layer);

            DecrementElementCount(this.elementDatabase.GetElement(index), layer);
            this.gameEvents.Publish(new ElementDestroyedEvent(position, layer, index));

            return true;
        }
        internal bool TryGetColorModifier(Point position, Layer layer, out Color value)
        {
            value = Color.White;

            if (!IsWithinBounds(position) || IsEmpty(position, layer))
            {
                return false;
            }

            value = this[position].GetColorModifier(layer);
            return true;
        }
        internal bool TryGetDissipatingState(Point position, Layer layer, out bool value)
        {
            value = false;

            if (!IsWithinBounds(position) || IsEmpty(position, layer))
            {
                return false;
            }

            value = this[position].GetDissipatingState(layer);
            return true;
        }
        internal bool TryGetElement(Point position, Layer layer, out Element value)
        {
            value = null;

            if (!IsWithinBounds(position) || IsEmpty(position, layer) || !this[position].HasElement(layer))
            {
                return false;
            }

            value = this[position].GetElement(layer);
            return true;
        }
        internal bool TryGetElementIndex(Point position, Layer layer, out ElementIndex index)
        {
            index = ElementIndex.None;

            if (!IsWithinBounds(position) || IsEmpty(position, layer) || !this[position].HasElement(layer))
            {
                return false;
            }

            index = this[position].GetElementIndex(layer);
            return true;
        }
        internal bool TryGetFallingState(Point position, Layer layer, out bool value)
        {
            value = false;

            if (!IsWithinBounds(position) || IsEmpty(position, layer))
            {
                return false;
            }

            value = this[position].GetFallingState(layer);
            return true;
        }
        internal bool TryGetPushedState(Point position, Layer layer, out bool value)
        {
            value = false;

            if (!IsWithinBounds(position) || IsEmpty(position, layer))
            {
                return false;
            }

            value = this[position].GetPushedState(layer);
            return true;
        }
        internal bool TryGetSlot(Point position, out Slot value)
        {
            value = null;

            if (!IsWithinBounds(position) || IsEmpty(position))
            {
                return false;
            }

            value = this[position];
            return true;
        }
        internal bool TryGetStepCycleFlag(Point position, Layer layer, out UpdateCycleFlag value)
        {
            value = UpdateCycleFlag.None;

            if (!IsWithinBounds(position) || IsEmpty(position, layer))
            {
                return false;
            }

            value = this[position].GetStepCycleFlag(layer);
            return true;
        }
        internal bool TryGetStoredElementIndex(Point position, Layer layer, out ElementIndex index)
        {
            index = ElementIndex.None;

            if (!IsWithinBounds(position) || IsEmpty(position, layer))
            {
                return false;
            }

            index = this[position].GetStoredElementIndex(layer);

            return index is not ElementIndex.None;
        }
        internal bool TryGetStoredElement(Point position, Layer layer, out Element value)
        {
            value = null;

            if (!IsWithinBounds(position) || IsEmpty(position, layer) || !this[position].HasStoredElement(layer))
            {
                return false;
            }

            value = this[position].GetStoredElement(layer);
            return true;
        }
        internal bool TryGetTemperature(Point position, Layer layer, out float value)
        {
            value = 0f;
            if (!IsWithinBounds(position) || IsEmpty(position, layer))
            {
                return false;
            }
            value = this[position].GetTemperature(layer);
            return true;
        }
        internal bool TryHasElement(Point position, Layer layer, out bool value)
        {
            value = false;

            if (!IsWithinBounds(position) || IsEmpty(position, layer))
            {
                return false;
            }

            value = this[position].HasElement(layer);
            return true;
        }
        internal bool TryHasStoredElement(Point position, Layer layer, out bool value)
        {
            value = false;

            if (!IsWithinBounds(position) || IsEmpty(position, layer))
            {
                return false;
            }

            value = this[position].HasStoredElement(layer);
            return true;
        }
        internal bool TryInstantiate(Point position, Layer layer, ElementIndex index)
        {
            if (!IsWithinBounds(position) || !IsEmpty(position, layer))
            {
                return false;
            }

            Slot slot = this[position];
            Element element = this.elementDatabase.GetElement(index);

            slot.Position = position;
            slot.Instantiate(layer, index);

            IncrementElementCount(element, layer);
            this.gameEvents.Publish(new ElementInstantiatedEvent(position, layer, index));

            _ = this.uniqueActiveElements.Add(element);

            return true;
        }
        internal bool TryIsEmpty(Point position, Layer layer, out bool value)
        {
            value = false;

            if (!IsWithinBounds(position))
            {
                return false;
            }

            value = this[position].IsEmpty(layer);
            return true;
        }
        internal bool TryIsEmpty(Point position, out bool value)
        {
            value = false;

            if (!IsWithinBounds(position))
            {
                return false;
            }

            value = this[position].IsEmpty();
            return true;
        }
        internal bool TryRemove(Point position, Layer layer)
        {
            if (!IsWithinBounds(position) || IsEmpty(position, layer))
            {
                return false;
            }

            this[position].Destroy(layer);

            DecrementElementCount(this[position].GetElement(layer), layer);
            this.gameEvents.Publish(new ElementRemovedEvent(position, layer));

            return true;
        }
        internal bool TryReplace(Point position, Layer layer, ElementIndex newIndex)
        {
            if (!IsWithinBounds(position) || IsEmpty(position, layer))
            {
                return false;
            }

            ElementIndex oldIndex = this[position].GetElementIndex(layer);

            this[position].Destroy(layer);
            this[position].Instantiate(layer, newIndex);

            this.gameEvents.Publish(new ElementReplacedEvent(position, layer, oldIndex, newIndex));

            return true;
        }
        internal bool TrySetDissipatingState(Point position, Layer layer, bool value)
        {
            if (!IsWithinBounds(position) || IsEmpty(position, layer))
            {
                return false;
            }

            this[position].SetDissipatingState(layer, value);
            return true;
        }
        internal bool TrySetElementColorModifier(Point position, Layer layer, Color value)
        {
            if (!IsWithinBounds(position) || IsEmpty(position, layer))
            {
                return false;
            }

            this[position].SetColorModifier(layer, value);
            return true;
        }
        internal bool TrySetElementIndex(Point position, Layer layer, ElementIndex value)
        {
            if (!IsWithinBounds(position) || IsEmpty(position, layer))
            {
                return false;
            }

            this[position].SetElementIndex(layer, value);
            return true;
        }
        internal bool TrySetTemperature(Point position, Layer layer, float value)
        {
            if (!IsWithinBounds(position) || IsEmpty(position, layer))
            {
                return false;
            }

            if (this[position].GetTemperature(layer) != value)
            {
                this[position].SetTemperature(layer, value);
                this.gameEvents.Publish(new ElementTemperatureChangedEvent(position, layer, value));
            }

            return true;
        }
        internal bool TrySetFallingState(Point position, Layer layer, bool value)
        {
            if (!IsWithinBounds(position) || IsEmpty(position, layer))
            {
                return false;
            }

            this[position].SetFallingState(layer, value);
            return true;
        }
        internal bool TrySetPushedState(Point position, Layer layer, bool value)
        {
            if (!IsWithinBounds(position) || IsEmpty(position, layer))
            {
                return false;
            }

            this[position].SetPushedState(layer, value);
            return true;
        }
        internal bool TrySetStepCycleFlag(Point position, Layer layer, UpdateCycleFlag value)
        {
            if (!IsWithinBounds(position) || IsEmpty(position, layer))
            {
                return false;
            }

            this[position].SetStepCycleFlag(layer, value);
            return true;
        }
        internal bool TrySetStoredElementIndex(Point position, Layer layer, ElementIndex index)
        {
            if (!IsWithinBounds(position) || IsEmpty(position, layer))
            {
                return false;
            }

            this[position].SetStoredElementIndex(layer, index);
            return true;
        }
        internal bool TrySwapping(Point element1Position, Point element2Position, Layer layer)
        {
            if (!IsWithinBounds(element1Position) ||
                !IsWithinBounds(element2Position) ||
                IsEmpty(element1Position, layer) ||
                IsEmpty(element2Position, layer) ||
                element1Position == element2Position)
            {
                return false;
            }

            Slot tempSlot = this.slotObjectPool.TryDequeue(out IPoolableObject value) ? (Slot)value : new(this.elementDatabase);

            tempSlot.Instantiate(layer, this[element1Position]);

            this[element1Position].Instantiate(layer, this[element2Position]);
            this[element2Position].Instantiate(layer, tempSlot);

            this[element1Position].Position = element1Position;
            this[element2Position].Position = element2Position;

            this.slotObjectPool.Enqueue(tempSlot);

            this.gameEvents.Publish(new ElementSwappedEvent(element1Position, element2Position, layer));

            return true;
        }
        internal bool TryUpdatePosition(Point oldPosition, Point newPosition, Layer layer)
        {
            if (!IsWithinBounds(oldPosition) ||
                !IsWithinBounds(newPosition) ||
                 IsEmpty(oldPosition, layer) ||
                !IsEmpty(newPosition, layer) ||
                oldPosition == newPosition)
            {
                return false;
            }

            this[newPosition].Instantiate(layer, this[oldPosition]);
            this[newPosition].Position = newPosition;
            this[oldPosition].Destroy(layer);

            this.gameEvents.Publish(new ElementPositionUpdatedEvent(oldPosition, newPosition, layer));

            return true;
        }

        #endregion

        #region Internal Methods

        internal void Destroy(Point position, Layer layer)
        {
            _ = TryDestroy(position, layer);
        }
        internal Color GetColorModifier(Point position, Layer layer)
        {
            _ = TryGetColorModifier(position, layer, out Color value);
            return value;
        }
        internal bool GetDissipatingState(Point position, Layer layer)
        {
            _ = TryGetDissipatingState(position, layer, out bool value);
            return value;
        }
        internal Element GetElement(Point position, Layer layer)
        {
            _ = TryGetElement(position, layer, out Element value);
            return value;
        }
        internal ElementIndex GetElementIndex(Point position, Layer layer)
        {
            _ = TryGetElementIndex(position, layer, out ElementIndex index);
            return index;
        }
        internal bool GetFallingState(Point position, Layer layer)
        {
            _ = TryGetFallingState(position, layer, out bool value);
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
        internal bool GetPushedState(Point position, Layer layer)
        {
            _ = TryGetPushedState(position, layer, out bool value);
            return value;
        }
        internal Slot GetSlot(Point position)
        {
            _ = TryGetSlot(position, out Slot value);
            return value;
        }
        internal UpdateCycleFlag GetStepCycleFlag(Point position, Layer layer)
        {
            _ = TryGetStepCycleFlag(position, layer, out UpdateCycleFlag value);
            return value;
        }
        internal Element GetStoredElement(Point position, Layer layer)
        {
            _ = TryGetStoredElement(position, layer, out Element value);
            return value;
        }
        internal ElementIndex GetStoredElementIndex(Point position, Layer layer)
        {
            _ = TryGetStoredElementIndex(position, layer, out ElementIndex index);
            return index;
        }
        internal float GetTemperature(Point position, Layer layer)
        {
            _ = TryGetTemperature(position, layer, out float value);
            return value;
        }
        internal bool HasElement(Point position, Layer layer)
        {
            _ = TryHasElement(position, layer, out bool value);
            return value;
        }
        internal bool HasStoredElement(Point position, Layer layer)
        {
            _ = TryHasStoredElement(position, layer, out bool value);
            return value;
        }
        internal void Instantiate(Point position, Layer layer, ElementIndex index)
        {
            _ = TryInstantiate(position, layer, index);
        }
        internal bool IsEmpty(Point position, Layer layer)
        {
            _ = TryIsEmpty(position, layer, out bool value);
            return value;
        }
        internal bool IsEmpty(Point position)
        {
            _ = TryIsEmpty(position, out bool value);
            return value;
        }
        internal void Remove(Point position, Layer layer)
        {
            _ = TryRemove(position, layer);
        }
        internal void Replace(Point position, Layer layer, ElementIndex index)
        {
            _ = TryReplace(position, layer, index);
        }
        internal void SetDissipatingState(Point position, Layer layer, bool value)
        {
            _ = TrySetDissipatingState(position, layer, value);
        }
        internal void SetColorModifier(Point position, Layer layer, Color value)
        {
            _ = TrySetElementColorModifier(position, layer, value);
        }
        internal void SetElementIndex(Point position, Layer layer, ElementIndex value)
        {
            _ = TrySetElementIndex(position, layer, value);
        }
        internal void SetTemperature(Point position, Layer layer, float value)
        {
            _ = TrySetTemperature(position, layer, value);
        }
        internal void SetFallingState(Point position, Layer layer, bool value)
        {
            _ = TrySetFallingState(position, layer, value);
        }
        internal void SetPushedState(Point position, Layer layer, bool value)
        {
            _ = TrySetPushedState(position, layer, value);
        }
        internal void SetStepCycleFlag(Point position, Layer layer, UpdateCycleFlag value)
        {
            _ = TrySetStepCycleFlag(position, layer, value);
        }
        internal void SetStoredElementIndex(Point position, Layer layer, ElementIndex index)
        {
            _ = TrySetStoredElementIndex(position, layer, index);
        }
        internal void Swapping(Point element1Position, Point element2Position, Layer layer)
        {
            _ = TrySwapping(element1Position, element2Position, layer);
        }
        internal void UpdatePosition(Point oldPosition, Point newPosition, Layer layer)
        {
            _ = TryUpdatePosition(oldPosition, newPosition, layer);
        }

        #endregion

        #endregion
    }
}
