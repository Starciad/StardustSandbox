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

        internal void Clear()
        {
            for (int y = 0; y < this.height; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    if (IsEmptySlot(new(x, y)))
                    {
                        continue;
                    }

                    RemoveElement(new(x, y), Layer.Foreground);
                    RemoveElement(new(x, y), Layer.Background);
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

        #region ELEMENTS

        internal bool TryInstantiateElementIndex(Point position, Layer layer, ElementIndex index)
        {
            if (!IsWithinBounds(position) || !IsEmptySlotLayer(position, layer))
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

            this[newPosition].Instantiate(layer, this[oldPosition]);
            this[newPosition].Position = newPosition;
            this[oldPosition].Destroy(layer);

            this.gameEvents.Publish(new ElementPositionUpdatedEvent(oldPosition, newPosition, layer));

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

        internal bool TryDestroyElement(Point position, Layer layer)
        {
            if (!IsWithinBounds(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            ElementIndex index = this[position].GetElementIndex(layer);
            this[position].Destroy(layer);

            DecrementElementCount(this.elementDatabase.GetElement(index), layer);
            this.gameEvents.Publish(new ElementDestroyedEvent(position, layer, index));

            return true;
        }

        internal bool TryRemoveElement(Point position, Layer layer)
        {
            if (!IsWithinBounds(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            this[position].Destroy(layer);

            DecrementElementCount(this[position].GetElement(layer), layer);
            this.gameEvents.Publish(new ElementRemovedEvent(position, layer));

            return true;
        }

        internal bool TryReplaceElementIndex(Point position, Layer layer, ElementIndex newIndex)
        {
            if (!IsWithinBounds(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            ElementIndex oldIndex = this[position].GetElementIndex(layer);

            this[position].Destroy(layer);
            this[position].Instantiate(layer, newIndex);

            this.gameEvents.Publish(new ElementReplacedEvent(position, layer, oldIndex, newIndex));

            return true;
        }

        internal bool TryGetElementIndex(Point position, Layer layer, out ElementIndex index)
        {
            index = ElementIndex.None;

            if (!IsWithinBounds(position) || IsEmptySlotLayer(position, layer) || !this[position].HasElement(layer))
            {
                return false;
            }

            index = this[position].GetElementIndex(layer);
            return true;
        }

        internal bool TryGetElement(Point position, Layer layer, out Element value)
        {
            value = null;

            if (!IsWithinBounds(position) || IsEmptySlotLayer(position, layer) || !this[position].HasElement(layer))
            {
                return false;
            }

            value = this[position].GetElement(layer);
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

        internal bool TrySetElementTemperature(Point position, Layer layer, float value)
        {
            if (!IsWithinBounds(position) || IsEmptySlotLayer(position, layer))
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

        internal bool TrySetElementColorModifier(Point position, Layer layer, Color value)
        {
            if (!IsWithinBounds(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            this[position].SetColorModifier(layer, value);
            return true;
        }

        internal bool TryHasStoredElement(Point position, Layer layer, out bool value)
        {
            value = false;

            if (!IsWithinBounds(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            value = this[position].HasStoredElement(layer);
            return true;
        }

        internal bool TrySetStoredElementIndex(Point position, Layer layer, ElementIndex index)
        {
            if (!IsWithinBounds(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            this[position].SetStoredElementIndex(layer, index);
            return true;
        }

        internal bool TryGetStoredElementIndex(Point position, Layer layer, out ElementIndex index)
        {
            index = ElementIndex.None;

            if (!IsWithinBounds(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            index = this[position].GetStoredElementIndex(layer);

            return index is not ElementIndex.None;
        }

        internal bool TryGetStoredElement(Point position, Layer layer, out Element value)
        {
            value = null;

            if (!IsWithinBounds(position) || IsEmptySlotLayer(position, layer) || !this[position].HasStoredElement(layer))
            {
                return false;
            }

            value = this[position].GetStoredElement(layer);
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

        internal bool IsEmptySlot(Point position)
        {
            return !IsWithinBounds(position) || this[position].IsEmpty();
        }

        internal bool IsEmptySlotLayer(Point position, Layer layer)
        {
            return !IsWithinBounds(position) || this[position].HasElement(layer);
        }

        #endregion
    }
}
