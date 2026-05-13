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

using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Explosions;
using StardustSandbox.Core.WorldSystem;

namespace StardustSandbox.Core.Elements
{
    internal sealed class ElementContext(World world)
    {
        internal Layer CurrentLayer { get; private set; }
        internal Point CurrentPosition { get; private set; }
        internal Slot CurrentSlot { get; private set; }
        internal SlotLayer CurrentSlotLayer => this.CurrentSlot.GetLayer(this.CurrentLayer);

        internal void Initialize(Point position, Layer layer)
        {
            this.CurrentLayer = layer;
            this.CurrentPosition = position;

            if (world.IsWithinBounds(position))
            {
                this.CurrentSlot = world[position];
                this.CurrentSlot.Position = position;
            }
        }

        #region WORLD

        internal Point GetWorldSize()
        {
            return world.Size;
        }

        internal Temperature GetWorldTemperature()
        {
            return world.Temperature;
        }

        #endregion

        #region ELEMENTS

        internal bool TrySetPosition(Point newPosition, Layer layer)
        {
            if (world.TryUpdateElementPosition(this.CurrentPosition, newPosition, layer))
            {
                this.CurrentSlot = GetSlot(newPosition);
                return true;
            }

            return false;
        }
        internal bool TrySetPosition(Point newPosition)
        {
            return TrySetPosition(newPosition, this.CurrentLayer);
        }

        internal bool TryInstantiateElementIndex(Point position, Layer layer, ElementIndex index)
        {
            return world.TryInstantiateElementIndex(position, layer, index);
        }
        internal bool TryInstantiateElementIndex(Point position, ElementIndex index)
        {
            return TryInstantiateElementIndex(position, this.CurrentLayer, index);
        }
        internal bool TryInstantiateElementIndex(ElementIndex index)
        {
            return TryInstantiateElementIndex(this.CurrentPosition, index);
        }

        internal bool TryUpdateElementPosition(Point oldPosition, Point newPosition, Layer layer)
        {
            return world.TryUpdateElementPosition(oldPosition, newPosition, layer);
        }
        internal bool TryUpdateElementPosition(Point oldPosition, Point newPosition)
        {
            return TryUpdateElementPosition(oldPosition, newPosition, this.CurrentLayer);
        }
        internal bool TryUpdateElementPosition(Point newPosition)
        {
            return TryUpdateElementPosition(this.CurrentPosition, newPosition);
        }

        internal bool TrySwappingElements(Point element1Position, Point element2Position, Layer layer)
        {
            return world.TrySwappingElements(element1Position, element2Position, layer);
        }
        internal bool TrySwappingElements(Point element1Position, Point element2Position)
        {
            return TrySwappingElements(element1Position, element2Position, this.CurrentLayer);
        }
        internal bool TrySwappingElements(Point targetPosition)
        {
            return TrySwappingElements(this.CurrentPosition, targetPosition);
        }

        internal bool TryDestroyElement(Point position, Layer layer)
        {
            return world.TryDestroyElement(position, layer);
        }
        internal bool TryDestroyElement(Point position)
        {
            return TryDestroyElement(position, this.CurrentLayer);
        }
        internal bool TryDestroyElement()
        {
            return TryDestroyElement(this.CurrentPosition);
        }

        internal bool TryRemoveElement(Point position, Layer layer)
        {
            return world.TryRemoveElement(position, layer);
        }
        internal bool TryRemoveElement(Point position)
        {
            return TryRemoveElement(position, this.CurrentLayer);
        }
        internal bool TryRemoveElement()
        {
            return TryRemoveElement(this.CurrentPosition);
        }

        internal bool TryReplaceElementIndex(Point position, Layer layer, ElementIndex index)
        {
            return world.TryReplaceElementIndex(position, layer, index);
        }
        internal bool TryReplaceElementIndex(Point position, ElementIndex index)
        {
            return TryReplaceElementIndex(position, this.CurrentLayer, index);
        }
        internal bool TryReplaceElementIndex(ElementIndex index)
        {
            return TryReplaceElementIndex(this.CurrentPosition, index);
        }

        internal bool TryGetElementIndex(Point position, Layer layer, out ElementIndex index)
        {
            return world.TryGetElementIndex(position, layer, out index);
        }
        internal bool TryGetElementIndex(Point position, out ElementIndex index)
        {
            return TryGetElementIndex(position, this.CurrentLayer, out index);
        }
        internal bool TryGetElementIndex(out ElementIndex index)
        {
            return TryGetElementIndex(this.CurrentPosition, out index);
        }

        internal bool TryGetElement(Point position, Layer layer, out Element element)
        {
            return world.TryGetElement(position, layer, out element);
        }
        internal bool TryGetElement(Point position, out Element element)
        {
            return TryGetElement(position, this.CurrentLayer, out element);
        }
        internal bool TryGetElement(out Element element)
        {
            return TryGetElement(this.CurrentPosition, out element);
        }

        internal bool TryGetSlot(Point position, out Slot value)
        {
            return world.TryGetSlot(position, out value);
        }
        internal bool TryGetSlot(out Slot value)
        {
            return TryGetSlot(this.CurrentPosition, out value);
        }

        internal bool TryGetSlotLayer(Point position, Layer layer, out SlotLayer value)
        {
            return world.TryGetSlotLayer(position, layer, out value);
        }
        internal bool TryGetSlotLayer(Point position, out SlotLayer value)
        {
            return TryGetSlotLayer(position, this.CurrentLayer, out value);
        }
        internal bool TryGetSlotLayer(out SlotLayer value)
        {
            return TryGetSlotLayer(this.CurrentPosition, out value);
        }

        internal bool TrySetElementTemperature(Point position, Layer layer, float value)
        {
            return world.TrySetElementTemperature(position, layer, value);
        }
        internal bool TrySetElementTemperature(Point position, float value)
        {
            return TrySetElementTemperature(position, this.CurrentLayer, value);
        }
        internal bool TrySetElementTemperature(float value)
        {
            return TrySetElementTemperature(this.CurrentPosition, value);
        }

        internal bool TrySetElementColorModifier(Point position, Layer layer, Color value)
        {
            return world.TrySetElementColorModifier(position, layer, value);
        }
        internal bool TrySetElementColorModifier(Point position, Color value)
        {
            return TrySetElementColorModifier(position, this.CurrentLayer, value);
        }
        internal bool TrySetElementColorModifier(Color value)
        {
            return TrySetElementColorModifier(this.CurrentPosition, value);
        }

        internal bool TryHasStoredElement(Point position, Layer layer, out bool value)
        {
            return world.TryHasStoredElement(position, layer, out value);
        }
        internal bool TryHasStoredElement(Point position, out bool value)
        {
            return TryHasStoredElement(position, this.CurrentLayer, out value);
        }
        internal bool TryHasStoredElement(out bool value)
        {
            return TryHasStoredElement(this.CurrentPosition, out value);
        }

        internal bool TrySetStoredElementIndex(Point position, Layer layer, ElementIndex index)
        {
            return world.TrySetStoredElementIndex(position, layer, index);
        }
        internal bool TrySetStoredElementIndex(Point position, ElementIndex index)
        {
            return TrySetStoredElementIndex(position, this.CurrentLayer, index);
        }
        internal bool TrySetStoredElementIndex(ElementIndex index)
        {
            return TrySetStoredElementIndex(this.CurrentPosition, index);
        }

        internal bool TryGetStoredElementIndex(Point position, Layer layer, out ElementIndex index)
        {
            return world.TryGetStoredElementIndex(position, layer, out index);
        }
        internal bool TryGetStoredElementIndex(Point position, out ElementIndex index)
        {
            return TryGetStoredElementIndex(position, this.CurrentLayer, out index);
        }
        internal bool TryGetStoredElementIndex(out ElementIndex index)
        {
            return TryGetStoredElementIndex(this.CurrentPosition, out index);
        }

        internal bool TryGetStoredElement(Point position, Layer layer, out Element element)
        {
            return world.TryGetStoredElement(position, layer, out element);
        }
        internal bool TryGetStoredElement(Point position, out Element element)
        {
            return TryGetStoredElement(position, this.CurrentLayer, out element);
        }
        internal bool TryGetStoredElement(out Element element)
        {
            return TryGetStoredElement(this.CurrentPosition, out element);
        }

        internal bool TryHasElementState(Point position, Layer layer, ElementStates state, out bool value)
        {
            return world.TryHasElementState(position, layer, state, out value);
        }
        internal bool TryHasElementState(Point position, ElementStates state, out bool value)
        {
            return TryHasElementState(position, this.CurrentLayer, state, out value);
        }
        internal bool TryHasElementState(ElementStates state, out bool value)
        {
            return TryHasElementState(this.CurrentPosition, state, out value);
        }
        internal bool TrySetElementState(Point position, Layer layer, ElementStates state)
        {
            return world.TrySetElementState(position, layer, state);
        }
        internal bool TrySetElementState(Point position, ElementStates state)
        {
            return TrySetElementState(position, this.CurrentLayer, state);
        }
        internal bool TrySetElementState(ElementStates state)
        {
            return TrySetElementState(this.CurrentPosition, state);
        }
        internal bool TryRemoveElementState(Point position, Layer layer, ElementStates state)
        {
            return world.TryRemoveElementState(position, layer, state);
        }
        internal bool TryRemoveElementState(Point position, ElementStates state)
        {
            return TryRemoveElementState(position, this.CurrentLayer, state);
        }
        internal bool TryRemoveElementState(ElementStates state)
        {
            return TryRemoveElementState(this.CurrentPosition, state);
        }
        internal bool TryClearElementStates(Point position, Layer layer)
        {
            return world.TryClearElementStates(position, layer);
        }
        internal bool TryClearElementStates(Point position)
        {
            return TryClearElementStates(position, this.CurrentLayer);
        }
        internal bool TryClearElementStates()
        {
            return TryClearElementStates(this.CurrentPosition, this.CurrentLayer);
        }
        internal bool TryToggleElementState(Point position, Layer layer, ElementStates state)
        {
            return world.TryToggleElementState(position, layer, state);
        }
        internal bool TryToggleElementState(Point position, ElementStates state)
        {
            return TryToggleElementState(position, this.CurrentLayer, state);
        }
        internal bool TryToggleElementState(ElementStates state)
        {
            return TryToggleElementState(this.CurrentPosition, state);
        }

        internal void SetPosition(Point newPosition, Layer layer)
        {
            _ = TrySetPosition(newPosition, layer);
        }
        internal void SetPosition(Point newPosition)
        {
            SetPosition(newPosition, this.CurrentLayer);
        }

        internal void InstantiateElementIndex(Point position, Layer layer, ElementIndex index)
        {
            world.InstantiateElementIndex(position, layer, index);
        }
        internal void InstantiateElementIndex(Point position, ElementIndex index)
        {
            InstantiateElementIndex(position, this.CurrentLayer, index);
        }
        internal void InstantiateElementIndex(ElementIndex index)
        {
            InstantiateElementIndex(this.CurrentPosition, index);
        }

        internal void UpdateElementPosition(Point oldPosition, Point newPosition, Layer layer)
        {
            world.UpdateElementPosition(oldPosition, newPosition, layer);
        }
        internal void UpdateElementPosition(Point oldPosition, Point newPosition)
        {
            UpdateElementPosition(oldPosition, newPosition, this.CurrentLayer);
        }
        internal void UpdateElementPosition(Point newPosition)
        {
            UpdateElementPosition(this.CurrentPosition, newPosition);
        }

        internal void SwappingElements(Point element1Position, Point element2Position, Layer layer)
        {
            _ = TrySwappingElements(element1Position, element2Position, layer);
        }
        internal void SwappingElements(Point element1Position, Point element2Position)
        {
            SwappingElements(element1Position, element2Position, this.CurrentLayer);
        }
        internal void SwappingElements(Point targetPosition)
        {
            SwappingElements(this.CurrentPosition, targetPosition);
        }

        internal void DestroyElement(Point position, Layer layer)
        {
            world.DestroyElement(position, layer);
        }
        internal void DestroyElement(Point position)
        {
            DestroyElement(position, this.CurrentLayer);
        }
        internal void DestroyElement()
        {
            DestroyElement(this.CurrentPosition);
        }

        internal void RemoveElement(Point position, Layer layer)
        {
            _ = TryRemoveElement(position, layer);
        }
        internal void RemoveElement(Point position)
        {
            RemoveElement(position, this.CurrentLayer);
        }
        internal void RemoveElement()
        {
            RemoveElement(this.CurrentPosition, this.CurrentLayer);
        }

        internal void ReplaceElementIndex(Point position, Layer layer, ElementIndex index)
        {
            world.ReplaceElementIndex(position, layer, index);
        }
        internal void ReplaceElementIndex(Point position, ElementIndex index)
        {
            ReplaceElementIndex(position, this.CurrentLayer, index);
        }
        internal void ReplaceElementIndex(ElementIndex index)
        {
            ReplaceElementIndex(this.CurrentPosition, index);
        }

        internal void SetElementTemperature(Point position, Layer layer, float value)
        {
            world.SetElementTemperature(position, layer, value);
        }
        internal void SetElementTemperature(Point position, float value)
        {
            SetElementTemperature(position, this.CurrentLayer, value);
        }
        internal void SetElementTemperature(float value)
        {
            SetElementTemperature(this.CurrentPosition, value);
        }

        internal void SetElementColorModifier(Point position, Layer layer, Color value)
        {
            world.SetElementColorModifier(position, layer, value);
        }
        internal void SetElementColorModifier(Point position, Color value)
        {
            SetElementColorModifier(position, this.CurrentLayer, value);
        }
        internal void SetElementColorModifier(Color value)
        {
            SetElementColorModifier(this.CurrentPosition, value);
        }

        internal void SetStoredElementIndex(Point position, Layer layer, ElementIndex index)
        {
            world.SetStoredElementIndex(position, layer, index);
        }
        internal void SetStoredElementIndex(Point position, ElementIndex index)
        {
            SetStoredElementIndex(position, this.CurrentLayer, index);
        }
        internal void SetStoredElementIndex(ElementIndex index)
        {
            SetStoredElementIndex(this.CurrentPosition, index);
        }

        internal ElementIndex GetStoredElementIndex(Point position, Layer layer)
        {
            return world.GetStoredElementIndex(position, layer);
        }
        internal ElementIndex GetStoredElementIndex(Point position)
        {
            return GetStoredElementIndex(position, this.CurrentLayer);
        }
        internal ElementIndex GetStoredElementIndex()
        {
            return GetStoredElementIndex(this.CurrentPosition);
        }

        internal Element GetStoredElement(Point position, Layer layer)
        {
            return world.GetStoredElement(position, layer);
        }
        internal Element GetStoredElement(Point position)
        {
            return GetStoredElement(position, this.CurrentLayer);
        }
        internal Element GetStoredElement()
        {
            return GetStoredElement(this.CurrentPosition);
        }

        internal bool HasStoredElement(Point position, Layer layer)
        {
            return world.HasStoredElement(position, layer);
        }
        internal bool HasStoredElement(Point position)
        {
            return HasStoredElement(position, this.CurrentLayer);
        }
        internal bool HasStoredElement()
        {
            return HasStoredElement(this.CurrentPosition);
        }

        internal bool HasElementState(Point position, Layer layer, ElementStates state)
        {
            return world.HasElementState(position, layer, state);
        }
        internal bool HasElementState(Point position, ElementStates state)
        {
            return HasElementState(position, this.CurrentLayer, state);
        }
        internal bool HasElementState(ElementStates state)
        {
            return HasElementState(this.CurrentPosition, state);
        }
        internal void SetElementState(Point position, Layer layer, ElementStates state)
        {
            world.SetElementState(position, layer, state);
        }
        internal void SetElementState(Point position, ElementStates state)
        {
            SetElementState(position, this.CurrentLayer, state);
        }
        internal void SetElementState(ElementStates state)
        {
            SetElementState(this.CurrentPosition, state);
        }
        internal void RemoveElementState(Point position, Layer layer, ElementStates state)
        {
            world.RemoveElementState(position, layer, state);
        }
        internal void RemoveElementState(Point position, ElementStates state)
        {
            RemoveElementState(position, this.CurrentLayer, state);
        }
        internal void RemoveElementState(ElementStates state)
        {
            RemoveElementState(this.CurrentPosition, state);
        }
        internal void ClearElementStates(Point position, Layer layer)
        {
            world.ClearElementStates(position, layer);
        }
        internal void ClearElementStates(Point position)
        {
            ClearElementStates(position, this.CurrentLayer);
        }
        internal void ClearElementStates()
        {
            ClearElementStates(this.CurrentPosition, this.CurrentLayer);
        }
        internal void ToggleElementState(Point position, Layer layer, ElementStates state)
        {
            world.ToggleElementState(position, layer, state);
        }
        internal void ToggleElementState(Point position, ElementStates state)
        {
            ToggleElementState(position, this.CurrentLayer, state);
        }
        internal void ToggleElementState(ElementStates state)
        {
            ToggleElementState(this.CurrentPosition, state);
        }

        internal ElementIndex GetElementIndex(Point position, Layer layer)
        {
            return world.GetElementIndex(position, layer);
        }
        internal ElementIndex GetElementIndex(Point position)
        {
            return GetElementIndex(position, this.CurrentLayer);
        }
        internal ElementIndex GetElementIndex()
        {
            return GetElementIndex(this.CurrentPosition);
        }

        internal Element GetElement(Point position, Layer layer)
        {
            return world.GetElement(position, layer);
        }
        internal Element GetElement(Point position)
        {
            return GetElement(position, this.CurrentLayer);
        }
        internal Element GetElement()
        {
            return GetElement(this.CurrentPosition);
        }

        internal Slot GetSlot(Point position)
        {
            return world.GetSlot(position);
        }
        internal Slot GetSlot()
        {
            return GetSlot(this.CurrentPosition);
        }

        internal SlotLayer GetSlotLayer(Point position, Layer layer)
        {
            return world.GetSlotLayer(position, layer);
        }
        internal SlotLayer GetSlotLayer(Point position)
        {
            return GetSlotLayer(position, this.CurrentLayer);
        }
        internal SlotLayer GetSlotLayer()
        {
            return GetSlotLayer(this.CurrentPosition, this.CurrentLayer);
        }

        internal ElementNeighbors GetNeighboringSlots(Point position)
        {
            return world.GetNeighboringSlots(position);
        }
        internal ElementNeighbors GetNeighboringSlots()
        {
            return GetNeighboringSlots(this.CurrentPosition);
        }

        internal bool IsEmptySlot(Point position)
        {
            return world.IsEmptySlot(position);
        }
        internal bool IsEmptySlot()
        {
            return IsEmptySlot(this.CurrentPosition);
        }

        internal bool IsEmptySlotLayer(Point position, Layer layer)
        {
            return world.IsEmptySlotLayer(position, layer);
        }
        internal bool IsEmptySlotLayer(Point position)
        {
            return IsEmptySlotLayer(position, this.CurrentLayer);
        }
        internal bool IsEmptySlotLayer()
        {
            return IsEmptySlotLayer(this.CurrentPosition, this.CurrentLayer);
        }

        internal uint GetTotalElementCount()
        {
            return world.GetTotalElementCount();
        }
        internal uint GetTotalForegroundElementCount()
        {
            return world.GetTotalForegroundElementCount();
        }
        internal uint GetTotalBackgroundElementCount()
        {
            return world.GetTotalBackgroundElementCount();
        }

        #endregion

        #region CHUNKING

        internal bool TryNotifyChunk(Point position)
        {
            return world.TryNotifyChunk(position);
        }
        internal bool TryNotifyChunk()
        {
            return TryNotifyChunk(this.CurrentPosition);
        }

        internal void NotifyChunk(Point position)
        {
            world.NotifyChunk(position);
        }
        internal void NotifyChunk()
        {
            NotifyChunk(this.CurrentPosition);
        }

        internal bool TryGetChunkUpdateState(Point position, out bool result)
        {
            return world.TryGetChunkUpdateState(position, out result);
        }
        internal bool GetChunkUpdateState(Point position)
        {
            return world.GetChunkUpdateState(position);
        }

        #endregion

        #region EXPLOSIONS

        internal bool TryInstantiateExplosion(Point position, Layer layer, ExplosionBuilder explosionBuilder)
        {
            return world.TryInstantiateExplosion(position, layer, explosionBuilder);
        }
        internal bool TryInstantiateExplosion(Point position, ExplosionBuilder explosionBuilder)
        {
            return TryInstantiateExplosion(position, this.CurrentLayer, explosionBuilder);
        }
        internal bool TryInstantiateExplosion(ExplosionBuilder explosionBuilder)
        {
            return TryInstantiateExplosion(this.CurrentPosition, explosionBuilder);
        }

        internal void InstantiateExplosion(Point position, Layer layer, ExplosionBuilder explosionBuilder)
        {
            world.InstantiateExplosion(position, layer, explosionBuilder);
        }
        internal void InstantiateExplosion(Point position, ExplosionBuilder explosionBuilder)
        {
            InstantiateExplosion(position, this.CurrentLayer, explosionBuilder);
        }
        internal void InstantiateExplosion(ExplosionBuilder explosionBuilder)
        {
            InstantiateExplosion(this.CurrentPosition, explosionBuilder);
        }

        #endregion
    }
}
