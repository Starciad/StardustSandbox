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

        internal void Initialize(in Point position, in Layer layer)
        {
            this.CurrentLayer = layer;
            this.CurrentPosition = position;

            if (world.IsWithinBounds(position))
            {
                this.CurrentSlot = world[position.X, position.Y];
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

        internal bool TrySetPosition(in Point newPosition, in Layer layer)
        {
            if (world.TryUpdateElementPosition(this.CurrentPosition, newPosition, layer))
            {
                this.CurrentSlot = GetSlot(newPosition);
                return true;
            }

            return false;
        }
        internal bool TrySetPosition(in Point newPosition)
        {
            return TrySetPosition(newPosition, this.CurrentLayer);
        }

        internal bool TryInstantiateElement(in Point position, in Layer layer, in ElementIndex index)
        {
            return world.TryInstantiateElement(position, layer, index);
        }
        internal bool TryInstantiateElement(in Point position, in ElementIndex index)
        {
            return TryInstantiateElement(position, this.CurrentLayer, index);
        }
        internal bool TryInstantiateElement(in ElementIndex index)
        {
            return TryInstantiateElement(this.CurrentPosition, index);
        }

        internal bool TryUpdateElementPosition(in Point oldPosition, in Point newPosition, in Layer layer)
        {
            return world.TryUpdateElementPosition(oldPosition, newPosition, layer);
        }
        internal bool TryUpdateElementPosition(in Point oldPosition, in Point newPosition)
        {
            return TryUpdateElementPosition(oldPosition, newPosition, this.CurrentLayer);
        }
        internal bool TryUpdateElementPosition(in Point newPosition)
        {
            return TryUpdateElementPosition(this.CurrentPosition, newPosition);
        }

        internal bool TrySwappingElements(in Point element1Position, in Point element2Position, in Layer layer)
        {
            return world.TrySwappingElements(element1Position, element2Position, layer);
        }
        internal bool TrySwappingElements(in Point element1Position, in Point element2Position)
        {
            return TrySwappingElements(element1Position, element2Position, this.CurrentLayer);
        }
        internal bool TrySwappingElements(in Point targetPosition)
        {
            return TrySwappingElements(this.CurrentPosition, targetPosition);
        }

        internal bool TryDestroyElement(in Point position, in Layer layer)
        {
            return world.TryDestroyElement(position, layer);
        }
        internal bool TryDestroyElement(in Point position)
        {
            return TryDestroyElement(position, this.CurrentLayer);
        }
        internal bool TryDestroyElement()
        {
            return TryDestroyElement(this.CurrentPosition);
        }

        internal bool TryRemoveElement(in Point position, in Layer layer)
        {
            return world.TryRemoveElement(position, layer);
        }
        internal bool TryRemoveElement(in Point position)
        {
            return TryRemoveElement(position, this.CurrentLayer);
        }
        internal bool TryRemoveElement()
        {
            return TryRemoveElement(this.CurrentPosition);
        }

        internal bool TryReplaceElement(in Point position, in Layer layer, in ElementIndex index)
        {
            return world.TryReplaceElement(position, layer, index);
        }
        internal bool TryReplaceElement(in Point position, in ElementIndex index)
        {
            return TryReplaceElement(position, this.CurrentLayer, index);
        }
        internal bool TryReplaceElement(in ElementIndex index)
        {
            return TryReplaceElement(this.CurrentPosition, index);
        }

        internal bool TryGetElement(in Point position, in Layer layer, out ElementIndex index)
        {
            return world.TryGetElement(position, layer, out index);
        }
        internal bool TryGetElement(in Point position, out ElementIndex index)
        {
            return TryGetElement(position, this.CurrentLayer, out index);
        }
        internal bool TryGetElement(out ElementIndex index)
        {
            return TryGetElement(this.CurrentPosition, out index);
        }

        internal bool TryGetSlot(in Point position, out Slot value)
        {
            return world.TryGetSlot(position, out value);
        }
        internal bool TryGetSlot(out Slot value)
        {
            return TryGetSlot(this.CurrentPosition, out value);
        }

        internal bool TryGetSlotLayer(in Point position, in Layer layer, out SlotLayer value)
        {
            return world.TryGetSlotLayer(position, layer, out value);
        }
        internal bool TryGetSlotLayer(in Point position, out SlotLayer value)
        {
            return TryGetSlotLayer(position, this.CurrentLayer, out value);
        }
        internal bool TryGetSlotLayer(out SlotLayer value)
        {
            return TryGetSlotLayer(this.CurrentPosition, out value);
        }

        internal bool TrySetElementTemperature(in Point position, in Layer layer, in float value)
        {
            return world.TrySetElementTemperature(position, layer, value);
        }
        internal bool TrySetElementTemperature(in Point position, in float value)
        {
            return TrySetElementTemperature(position, this.CurrentLayer, value);
        }
        internal bool TrySetElementTemperature(in float value)
        {
            return TrySetElementTemperature(this.CurrentPosition, value);
        }

        internal bool TrySetElementColorModifier(in Point position, in Layer layer, in Color value)
        {
            return world.TrySetElementColorModifier(position, layer, value);
        }
        internal bool TrySetElementColorModifier(in Point position, in Color value)
        {
            return TrySetElementColorModifier(position, this.CurrentLayer, value);
        }
        internal bool TrySetElementColorModifier(in Color value)
        {
            return TrySetElementColorModifier(this.CurrentPosition, value);
        }

        internal bool TryHasStoredElement(in Point position, in Layer layer, out bool value)
        {
            return world.TryHasStoredElement(position, layer, out value);
        }
        internal bool TryHasStoredElement(in Point position, out bool value)
        {
            return TryHasStoredElement(position, this.CurrentLayer, out value);
        }
        internal bool TryHasStoredElement(out bool value)
        {
            return TryHasStoredElement(this.CurrentPosition, out value);
        }

        internal bool TrySetStoredElement(in Point position, in Layer layer, in ElementIndex index)
        {
            return world.TrySetStoredElement(position, layer, index);
        }
        internal bool TrySetStoredElement(in Point position, in ElementIndex index)
        {
            return TrySetStoredElement(position, this.CurrentLayer, index);
        }
        internal bool TrySetStoredElement(in ElementIndex index)
        {
            return TrySetStoredElement(this.CurrentPosition, index);
        }

        internal bool TryGetStoredElement(in Point position, in Layer layer, out ElementIndex index)
        {
            return world.TryGetStoredElement(position, layer, out index);
        }
        internal bool TryGetStoredElement(in Point position, out ElementIndex index)
        {
            return TryGetStoredElement(position, this.CurrentLayer, out index);
        }
        internal bool TryGetStoredElement(out ElementIndex index)
        {
            return TryGetStoredElement(this.CurrentPosition, out index);
        }

        internal bool TryHasElementState(in Point position, in Layer layer, in ElementStates state, out bool value)
        {
            return world.TryHasElementState(position, layer, state, out value);
        }
        internal bool TryHasElementState(in Point position, in ElementStates state, out bool value)
        {
            return TryHasElementState(position, this.CurrentLayer, state, out value);
        }
        internal bool TryHasElementState(in ElementStates state, out bool value)
        {
            return TryHasElementState(this.CurrentPosition, state, out value);
        }
        internal bool TrySetElementState(in Point position, in Layer layer, ElementStates state)
        {
            return world.TrySetElementState(position, layer, state);
        }
        internal bool TrySetElementState(in Point position, in ElementStates state)
        {
            return TrySetElementState(position, this.CurrentLayer, state);
        }
        internal bool TrySetElementState(in ElementStates state)
        {
            return TrySetElementState(this.CurrentPosition, state);
        }
        internal bool TryRemoveElementState(in Point position, in Layer layer, in ElementStates state)
        {
            return world.TryRemoveElementState(position, layer, state);
        }
        internal bool TryRemoveElementState(in Point position, in ElementStates state)
        {
            return TryRemoveElementState(position, this.CurrentLayer, state);
        }
        internal bool TryRemoveElementState(in ElementStates state)
        {
            return TryRemoveElementState(this.CurrentPosition, state);
        }
        internal bool TryClearElementStates(in Point position, in Layer layer)
        {
            return world.TryClearElementStates(position, layer);
        }
        internal bool TryClearElementStates(in Point position)
        {
            return TryClearElementStates(position, this.CurrentLayer);
        }
        internal bool TryClearElementStates()
        {
            return TryClearElementStates(this.CurrentPosition, this.CurrentLayer);
        }
        internal bool TryToggleElementState(in Point position, in Layer layer, in ElementStates state)
        {
            return world.TryToggleElementState(position, layer, state);
        }
        internal bool TryToggleElementState(in Point position, in ElementStates state)
        {
            return TryToggleElementState(position, this.CurrentLayer, state);
        }
        internal bool TryToggleElementState(in ElementStates state)
        {
            return TryToggleElementState(this.CurrentPosition, state);
        }

        internal void SetPosition(in Point newPosition, in Layer layer)
        {
            _ = TrySetPosition(newPosition, layer);
        }
        internal void SetPosition(in Point newPosition)
        {
            SetPosition(newPosition, this.CurrentLayer);
        }

        internal void InstantiateElement(in Point position, in Layer layer, in ElementIndex index)
        {
            world.InstantiateElement(position, layer, index);
        }
        internal void InstantiateElement(in Point position, in ElementIndex index)
        {
            InstantiateElement(position, this.CurrentLayer, index);
        }
        internal void InstantiateElement(in ElementIndex index)
        {
            InstantiateElement(this.CurrentPosition, index);
        }

        internal void UpdateElementPosition(in Point oldPosition, in Point newPosition, in Layer layer)
        {
            world.UpdateElementPosition(oldPosition, newPosition, layer);
        }
        internal void UpdateElementPosition(in Point oldPosition, in Point newPosition)
        {
            UpdateElementPosition(oldPosition, newPosition, this.CurrentLayer);
        }
        internal void UpdateElementPosition(in Point newPosition)
        {
            UpdateElementPosition(this.CurrentPosition, newPosition);
        }

        internal void SwappingElements(in Point element1Position, in Point element2Position, in Layer layer)
        {
            _ = TrySwappingElements(element1Position, element2Position, layer);
        }
        internal void SwappingElements(in Point element1Position, in Point element2Position)
        {
            SwappingElements(element1Position, element2Position, this.CurrentLayer);
        }
        internal void SwappingElements(in Point targetPosition)
        {
            SwappingElements(this.CurrentPosition, targetPosition);
        }

        internal void DestroyElement(in Point position, in Layer layer)
        {
            world.DestroyElement(position, layer);
        }
        internal void DestroyElement(in Point position)
        {
            DestroyElement(position, this.CurrentLayer);
        }
        internal void DestroyElement()
        {
            DestroyElement(this.CurrentPosition);
        }

        internal void RemoveElement(in Point position, in Layer layer)
        {
            _ = TryRemoveElement(position, layer);
        }
        internal void RemoveElement(in Point position)
        {
            RemoveElement(position, this.CurrentLayer);
        }
        internal void RemoveElement()
        {
            RemoveElement(this.CurrentPosition, this.CurrentLayer);
        }

        internal void ReplaceElement(in Point position, in Layer layer, in ElementIndex index)
        {
            world.ReplaceElement(position, layer, index);
        }
        internal void ReplaceElement(in Point position, in ElementIndex index)
        {
            ReplaceElement(position, this.CurrentLayer, index);
        }
        internal void ReplaceElement(in ElementIndex index)
        {
            ReplaceElement(this.CurrentPosition, index);
        }

        internal void SetElementTemperature(in Point position, in Layer layer, in float value)
        {
            world.SetElementTemperature(position, layer, value);
        }
        internal void SetElementTemperature(in Point position, in float value)
        {
            SetElementTemperature(position, this.CurrentLayer, value);
        }
        internal void SetElementTemperature(in float value)
        {
            SetElementTemperature(this.CurrentPosition, value);
        }

        internal void SetElementColorModifier(in Point position, in Layer layer, in Color value)
        {
            world.SetElementColorModifier(position, layer, value);
        }
        internal void SetElementColorModifier(in Point position, in Color value)
        {
            SetElementColorModifier(position, this.CurrentLayer, value);
        }
        internal void SetElementColorModifier(in Color value)
        {
            SetElementColorModifier(this.CurrentPosition, value);
        }

        internal void SetStoredElement(in Point position, in Layer layer, in ElementIndex index)
        {
            world.SetStoredElement(position, layer, index);
        }
        internal void SetStoredElement(in Point position, in ElementIndex index)
        {
            SetStoredElement(position, this.CurrentLayer, index);
        }
        internal void SetStoredElement(in ElementIndex index)
        {
            SetStoredElement(this.CurrentPosition, index);
        }

        internal ElementIndex GetStoredElement(in Point position, in Layer layer)
        {
            return world.GetStoredElement(position, layer);
        }
        internal ElementIndex GetStoredElement(in Point position)
        {
            return GetStoredElement(position, this.CurrentLayer);
        }
        internal ElementIndex GetStoredElement()
        {
            return GetStoredElement(this.CurrentPosition);
        }

        internal bool HasStoredElement(in Point position, in Layer layer)
        {
            return world.HasStoredElement(position, layer);
        }
        internal bool HasStoredElement(in Point position)
        {
            return HasStoredElement(position, this.CurrentLayer);
        }
        internal bool HasStoredElement()
        {
            return HasStoredElement(this.CurrentPosition);
        }

        internal bool HasElementState(in Point position, in Layer layer, in ElementStates state)
        {
            return world.HasElementState(position, layer, state);
        }
        internal bool HasElementState(in Point position, in ElementStates state)
        {
            return HasElementState(position, this.CurrentLayer, state);
        }
        internal bool HasElementState(in ElementStates state)
        {
            return HasElementState(this.CurrentPosition, state);
        }
        internal void SetElementState(in Point position, in Layer layer, in ElementStates state)
        {
            world.SetElementState(position, layer, state);
        }
        internal void SetElementState(in Point position, in ElementStates state)
        {
            SetElementState(position, this.CurrentLayer, state);
        }
        internal void SetElementState(in ElementStates state)
        {
            SetElementState(this.CurrentPosition, state);
        }
        internal void RemoveElementState(in Point position, in Layer layer, in ElementStates state)
        {
            world.RemoveElementState(position, layer, state);
        }
        internal void RemoveElementState(in Point position, in ElementStates state)
        {
            RemoveElementState(position, this.CurrentLayer, state);
        }
        internal void RemoveElementState(in ElementStates state)
        {
            RemoveElementState(this.CurrentPosition, state);
        }
        internal void ClearElementStates(in Point position, in Layer layer)
        {
            world.ClearElementStates(position, layer);
        }
        internal void ClearElementStates(in Point position)
        {
            ClearElementStates(position, this.CurrentLayer);
        }
        internal void ClearElementStates()
        {
            ClearElementStates(this.CurrentPosition, this.CurrentLayer);
        }
        internal void ToggleElementState(in Point position, in Layer layer, in ElementStates state)
        {
            world.ToggleElementState(position, layer, state);
        }
        internal void ToggleElementState(in Point position, in ElementStates state)
        {
            ToggleElementState(position, this.CurrentLayer, state);
        }
        internal void ToggleElementState(in ElementStates state)
        {
            ToggleElementState(this.CurrentPosition, state);
        }

        internal ElementIndex GetElement(in Point position, in Layer layer)
        {
            return world.GetElement(position, layer);
        }
        internal ElementIndex GetElement(in Point position)
        {
            return GetElement(position, this.CurrentLayer);
        }
        internal ElementIndex GetElement()
        {
            return GetElement(this.CurrentPosition);
        }

        internal Slot GetSlot(in Point position)
        {
            return world.GetSlot(position);
        }
        internal Slot GetSlot()
        {
            return GetSlot(this.CurrentPosition);
        }

        internal SlotLayer GetSlotLayer(in Point position, in Layer layer)
        {
            return world.GetSlotLayer(position, layer);
        }
        internal SlotLayer GetSlotLayer(in Point position)
        {
            return GetSlotLayer(position, this.CurrentLayer);
        }
        internal SlotLayer GetSlotLayer()
        {
            return GetSlotLayer(this.CurrentPosition, this.CurrentLayer);
        }

        internal ElementNeighbors GetNeighboringSlots(in Point position)
        {
            return world.GetNeighboringSlots(position);
        }
        internal ElementNeighbors GetNeighboringSlots()
        {
            return GetNeighboringSlots(this.CurrentPosition);
        }

        internal bool IsEmptySlot(in Point position)
        {
            return world.IsEmptySlot(position);
        }
        internal bool IsEmptySlot()
        {
            return IsEmptySlot(this.CurrentPosition);
        }

        internal bool IsEmptySlotLayer(in Point position, in Layer layer)
        {
            return world.IsEmptySlotLayer(position, layer);
        }
        internal bool IsEmptySlotLayer(in Point position)
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

        internal bool TryNotifyChunk(in Point position)
        {
            return world.TryNotifyChunk(position);
        }
        internal bool TryNotifyChunk()
        {
            return TryNotifyChunk(this.CurrentPosition);
        }

        internal void NotifyChunk(in Point position)
        {
            world.NotifyChunk(position);
        }
        internal void NotifyChunk()
        {
            NotifyChunk(this.CurrentPosition);
        }

        internal bool TryGetChunkUpdateState(in Point position, out bool result)
        {
            return world.TryGetChunkUpdateState(position, out result);
        }
        internal bool GetChunkUpdateState(in Point position)
        {
            return world.GetChunkUpdateState(position);
        }

        #endregion

        #region EXPLOSIONS

        internal bool TryInstantiateExplosion(in Point position, in Layer layer, in ExplosionBuilder explosionBuilder)
        {
            return world.TryInstantiateExplosion(position, layer, explosionBuilder);
        }
        internal bool TryInstantiateExplosion(in Point position, in ExplosionBuilder explosionBuilder)
        {
            return TryInstantiateExplosion(position, this.CurrentLayer, explosionBuilder);
        }
        internal bool TryInstantiateExplosion(in ExplosionBuilder explosionBuilder)
        {
            return TryInstantiateExplosion(this.CurrentPosition, explosionBuilder);
        }

        internal void InstantiateExplosion(in Point position, in Layer layer, in ExplosionBuilder explosionBuilder)
        {
            world.InstantiateExplosion(position, layer, explosionBuilder);
        }
        internal void InstantiateExplosion(in Point position, in ExplosionBuilder explosionBuilder)
        {
            InstantiateExplosion(position, this.CurrentLayer, explosionBuilder);
        }
        internal void InstantiateExplosion(in ExplosionBuilder explosionBuilder)
        {
            InstantiateExplosion(this.CurrentPosition, explosionBuilder);
        }

        #endregion
    }
}
