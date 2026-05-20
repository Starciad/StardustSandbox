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
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces.Collections;
using StardustSandbox.Core.WorldSystem.Slots;

using System;
using System.Threading.Tasks;

namespace StardustSandbox.Core.WorldSystem.Components
{
    internal sealed class TileMap
    {
        internal int Width => this.width;
        internal int Height => this.height;

        internal delegate void ElementInstantiatedHandler(Point position, Layer layer, ElementIndex index);
        internal delegate void ElementPositionUpdatedHandler(Point oldPosition, Point newPosition, Layer layer);
        internal delegate void ElementSwappedHandler(Point element1Position, Point element2Position, Layer layer);
        internal delegate void ElementDestroyedHandler(Point position, Layer layer);
        internal delegate void ElementRemovedHandler(Point position, Layer layer);
        internal delegate void ElementReplacedHandler(Point position, Layer layer, ElementIndex oldIndex, ElementIndex newIndex);
        internal delegate void ElementTemperatureChangedHandler(Point position, Layer layer, float newTemperature);

        internal event ElementInstantiatedHandler OnElementInstantiatedHandler;
        internal event ElementPositionUpdatedHandler OnElementPositionUpdatedHandler;
        internal event ElementSwappedHandler OnElementSwappedHandler;
        internal event ElementDestroyedHandler OnElementDestroyedHandler;
        internal event ElementRemovedHandler OnElementRemovedHandler;
        internal event ElementReplacedHandler OnElementReplacedHandler;
        internal event ElementTemperatureChangedHandler OnElementTemperatureChangedHandler;

        private int width;
        private int height;

        private Slot[,] slots;

        private readonly ElementDatabase elementDatabase;
        private readonly ElementNeighbors elementNeighbors = new();
        private readonly ObjectPool slotObjectPool = new();

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

        internal TileMap(ElementDatabase elementDatabase)
        {
            this.elementDatabase = elementDatabase;
        }

        internal void Clear()
        {
            if (this == null)
            {
                return;
            }

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

        #region ELEMENTS

        internal bool TryInstantiateElementIndex(Point position, Layer layer, ElementIndex index)
        {
            if (!IsWithinBounds(position) || !IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            Slot slot = this[position];

            slot.Position = position;
            slot.Instantiate(layer, index);

            // this.worldElementContext.Initialize(position, layer);
            // 
            // Element element = this.elementDatabase.GetElement(index);
            // 
            // element.SetContext(this.worldElementContext);
            // element.Instantiate();

            // this.statisticsManager.RegisterInstantiatedElement(index);

            this.OnElementInstantiatedHandler?.Invoke(position, layer, index);

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

            this[newPosition].Copy(layer, this[oldPosition].GetLayer(layer));
            this[newPosition].Position = newPosition;
            this[oldPosition].Destroy(layer);

            this.OnElementPositionUpdatedHandler?.Invoke(oldPosition, newPosition, layer);

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

            tempSlot.Copy(layer, this[element1Position].GetLayer(layer));

            this[element1Position].Copy(layer, this[element2Position].GetLayer(layer));
            this[element2Position].Copy(layer, tempSlot.GetLayer(layer));

            this[element1Position].Position = element1Position;
            this[element2Position].Position = element2Position;

            this.slotObjectPool.Enqueue(tempSlot);

            this.OnElementSwappedHandler?.Invoke(element1Position, element2Position, layer);

            return true;
        }

        internal bool TryDestroyElement(Point position, Layer layer)
        {
            if (!IsWithinBounds(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            SlotLayer slotLayer = this[position].GetLayer(layer);

            // this.worldElementContext.Initialize(position, layer);
            // slotLayer.Element.SetContext(this.worldElementContext);
            slotLayer.Element.Destroy();
            slotLayer.Destroy();

            this.OnElementDestroyedHandler?.Invoke(position, layer);

            return true;
        }

        internal bool TryRemoveElement(Point position, Layer layer)
        {
            if (!IsWithinBounds(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            this[position].Destroy(layer);
            this.OnElementRemovedHandler?.Invoke(position, layer);

            return true;
        }

        internal bool TryReplaceElementIndex(Point position, Layer layer, ElementIndex index)
        {
            if (!IsWithinBounds(position) || IsEmptySlotLayer(position, layer))
            {
                return false;
            }

            SlotLayer slotLayer = this[position].GetLayer(layer);
            ElementIndex oldIndex = slotLayer.ElementIndex;

            // this.worldElementContext.Initialize(position, layer);
            // 
            // Element newElement = this.elementDatabase.GetElement(index);
            // newElement.SetContext(this.worldElementContext);
            // newElement.Instantiate();
            // 
            // slotLayer.Element.Destroy();
            // slotLayer.Replace(index, newElement);

            slotLayer.Destroy();
            slotLayer.Instantiate(index);

            this.OnElementReplacedHandler?.Invoke(position, layer, oldIndex, index);

            return true;
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
                slotLayer.Temperature = value;
                this.OnElementTemperatureChangedHandler?.Invoke(position, layer, value);
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

            _ = Parallel.For(0, this.height, y =>
            {
                uint localCount = 0;

                for (int x = 0; x < this.width; x++)
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
    }
}
