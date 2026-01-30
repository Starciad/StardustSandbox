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
        internal Information WorldInformation => world.Information;
        internal Temperature WorldTemperature => world.Temperature;

        internal Layer Layer { get; private set; }
        internal Point Position => this.Slot.Position;
        internal Slot Slot { get; private set; }
        internal SlotLayer SlotLayer => this.Slot.GetLayer(this.Layer);

        internal void Initialize(in Point position, in Layer layer, Slot slot)
        {
            slot.SetPosition(position);

            this.Layer = layer;
            this.Slot = slot;
        }

        #region ELEMENTS

        internal bool TrySetPosition(in Point newPosition, in Layer layer)
        {
            if (world.TryUpdateElementPosition(this.Position, newPosition, layer))
            {
                this.Slot = GetSlot(newPosition);
                return true;
            }

            return false;
        }
        internal bool TrySetPosition(in Point newPosition)
        {
            return TrySetPosition(newPosition, this.Layer);
        }

        internal bool TryInstantiateElement(in Point position, in Layer layer, in ElementIndex index)
        {
            return world.TryInstantiateElement(position, layer, index);
        }
        internal bool TryInstantiateElement(in Point position, in ElementIndex index)
        {
            return TryInstantiateElement(position, this.Layer, index);
        }
        internal bool TryInstantiateElement(in ElementIndex index)
        {
            return TryInstantiateElement(this.Position, index);
        }

        internal bool TryUpdateElementPosition(in Point oldPosition, in Point newPosition, in Layer layer)
        {
            return world.TryUpdateElementPosition(oldPosition, newPosition, layer);
        }
        internal bool TryUpdateElementPosition(in Point oldPosition, in Point newPosition)
        {
            return TryUpdateElementPosition(oldPosition, newPosition, this.Layer);
        }
        internal bool TryUpdateElementPosition(in Point newPosition)
        {
            return TryUpdateElementPosition(this.Position, newPosition);
        }

        internal bool TrySwappingElements(in Point element1Position, in Point element2Position, in Layer layer)
        {
            return world.TrySwappingElements(element1Position, element2Position, layer);
        }
        internal bool TrySwappingElements(in Point element1Position, in Point element2Position)
        {
            return TrySwappingElements(element1Position, element2Position, this.Layer);
        }
        internal bool TrySwappingElements(in Point targetPosition)
        {
            return TrySwappingElements(this.Position, targetPosition);
        }

        internal bool TryDestroyElement(in Point position, in Layer layer)
        {
            return world.TryDestroyElement(position, layer);
        }
        internal bool TryDestroyElement(in Point position)
        {
            return TryDestroyElement(position, this.Layer);
        }
        internal bool TryDestroyElement()
        {
            return TryDestroyElement(this.Position);
        }

        internal bool TryRemoveElement(in Point position, in Layer layer)
        {
            return world.TryRemoveElement(position, layer);
        }
        internal bool TryRemoveElement(in Point position)
        {
            return TryRemoveElement(position, this.Layer);
        }
        internal bool TryRemoveElement()
        {
            return TryRemoveElement(this.Position);
        }

        internal bool TryReplaceElement(in Point position, in Layer layer, in ElementIndex index)
        {
            return world.TryReplaceElement(position, layer, index);
        }
        internal bool TryReplaceElement(in Point position, in ElementIndex index)
        {
            return TryReplaceElement(position, this.Layer, index);
        }
        internal bool TryReplaceElement(in ElementIndex index)
        {
            return TryReplaceElement(this.Position, index);
        }

        internal bool TryGetElement(in Point position, in Layer layer, out ElementIndex index)
        {
            return world.TryGetElement(position, layer, out index);
        }
        internal bool TryGetElement(in Point position, out ElementIndex index)
        {
            return TryGetElement(position, this.Layer, out index);
        }
        internal bool TryGetElement(out ElementIndex index)
        {
            return TryGetElement(this.Position, out index);
        }

        internal bool TryGetSlot(in Point position, out Slot value)
        {
            return world.TryGetSlot(position, out value);
        }
        internal bool TryGetSlot(out Slot value)
        {
            return TryGetSlot(this.Position, out value);
        }

        internal bool TryGetSlotLayer(in Point position, in Layer layer, out SlotLayer value)
        {
            return world.TryGetSlotLayer(position, layer, out value);
        }
        internal bool TryGetSlotLayer(in Point position, out SlotLayer value)
        {
            return TryGetSlotLayer(position, this.Layer, out value);
        }
        internal bool TryGetSlotLayer(out SlotLayer value)
        {
            return TryGetSlotLayer(this.Position, out value);
        }

        internal bool TrySetElementTemperature(in Point position, in Layer layer, in float value)
        {
            return world.TrySetElementTemperature(position, layer, value);
        }
        internal bool TrySetElementTemperature(in Point position, in float value)
        {
            return TrySetElementTemperature(position, this.Layer, value);
        }
        internal bool TrySetElementTemperature(in float value)
        {
            return TrySetElementTemperature(this.Position, value);
        }

        internal bool TrySetElementColorModifier(in Point position, in Layer layer, in Color value)
        {
            return world.TrySetElementColorModifier(position, layer, value);
        }
        internal bool TrySetElementColorModifier(in Point position, in Color value)
        {
            return TrySetElementColorModifier(position, this.Layer, value);
        }
        internal bool TrySetElementColorModifier(in Color value)
        {
            return TrySetElementColorModifier(this.Position, value);
        }

        internal bool TryHasStoredElement(in Point position, in Layer layer, out bool value)
        {
            return world.TryHasStoredElement(position, layer, out value);
        }
        internal bool TryHasStoredElement(in Point position, out bool value)
        {
            return TryHasStoredElement(position, this.Layer, out value);
        }
        internal bool TryHasStoredElement(out bool value)
        {
            return TryHasStoredElement(this.Position, out value);
        }

        internal bool TrySetStoredElement(in Point position, in Layer layer, in ElementIndex index)
        {
            return world.TrySetStoredElement(position, layer, index);
        }
        internal bool TrySetStoredElement(in Point position, in ElementIndex index)
        {
            return TrySetStoredElement(position, this.Layer, index);
        }
        internal bool TrySetStoredElement(in ElementIndex index)
        {
            return TrySetStoredElement(this.Position, index);
        }

        internal bool TryGetStoredElement(in Point position, in Layer layer, out ElementIndex index)
        {
            return world.TryGetStoredElement(position, layer, out index);
        }
        internal bool TryGetStoredElement(in Point position, out ElementIndex index)
        {
            return TryGetStoredElement(position, this.Layer, out index);
        }
        internal bool TryGetStoredElement(out ElementIndex index)
        {
            return TryGetStoredElement(this.Position, out index);
        }

        internal bool TryHasElementState(in Point position, in Layer layer, in ElementStates state, out bool value)
        {
            return world.TryHasElementState(position, layer, state, out value);
        }
        internal bool TryHasElementState(in Point position, in ElementStates state, out bool value)
        {
            return TryHasElementState(position, this.Layer, state, out value);
        }
        internal bool TryHasElementState(in ElementStates state, out bool value)
        {
            return TryHasElementState(this.Position, state, out value);
        }
        internal bool TrySetElementState(in Point position, in Layer layer, ElementStates state)
        {
            return world.TrySetElementState(position, layer, state);
        }
        internal bool TrySetElementState(in Point position, in ElementStates state)
        {
            return TrySetElementState(position, this.Layer, state);
        }
        internal bool TrySetElementState(in ElementStates state)
        {
            return TrySetElementState(this.Position, state);
        }
        internal bool TryRemoveElementState(in Point position, in Layer layer, in ElementStates state)
        {
            return world.TryRemoveElementState(position, layer, state);
        }
        internal bool TryRemoveElementState(in Point position, in ElementStates state)
        {
            return TryRemoveElementState(position, this.Layer, state);
        }
        internal bool TryRemoveElementState(in ElementStates state)
        {
            return TryRemoveElementState(this.Position, state);
        }
        internal bool TryClearElementStates(in Point position, in Layer layer)
        {
            return world.TryClearElementStates(position, layer);
        }
        internal bool TryClearElementStates(in Point position)
        {
            return TryClearElementStates(position, this.Layer);
        }
        internal bool TryClearElementStates()
        {
            return TryClearElementStates(this.Position, this.Layer);
        }
        internal bool TryToggleElementState(in Point position, in Layer layer, in ElementStates state)
        {
            return world.TryToggleElementState(position, layer, state);
        }
        internal bool TryToggleElementState(in Point position, in ElementStates state)
        {
            return TryToggleElementState(position, this.Layer, state);
        }
        internal bool TryToggleElementState(in ElementStates state)
        {
            return TryToggleElementState(this.Position, state);
        }

        internal bool TrySetElementTicksRemaining(in Point position, in Layer layer, in int ticks)
        {
            return world.TrySetElementTicksRemaining(position, layer, ticks);
        }
        internal bool TrySetElementTicksRemaining(in Point position, in int ticks)
        {
            return TrySetElementTicksRemaining(position, this.Layer, ticks);
        }
        internal bool TrySetElementTicksRemaining(in int ticks)
        {
            return TrySetElementTicksRemaining(this.Position, ticks);
        }

        internal void SetPosition(in Point newPosition, in Layer layer)
        {
            _ = TrySetPosition(newPosition, layer);
        }
        internal void SetPosition(in Point newPosition)
        {
            SetPosition(newPosition, this.Layer);
        }

        internal void InstantiateElement(in Point position, in Layer layer, in ElementIndex index)
        {
            world.InstantiateElement(position, layer, index);
        }
        internal void InstantiateElement(in Point position, in ElementIndex index)
        {
            InstantiateElement(position, this.Layer, index);
        }
        internal void InstantiateElement(in ElementIndex index)
        {
            InstantiateElement(this.Position, index);
        }

        internal void UpdateElementPosition(in Point oldPosition, in Point newPosition, in Layer layer)
        {
            world.UpdateElementPosition(oldPosition, newPosition, layer);
        }
        internal void UpdateElementPosition(in Point oldPosition, in Point newPosition)
        {
            UpdateElementPosition(oldPosition, newPosition, this.Layer);
        }
        internal void UpdateElementPosition(in Point newPosition)
        {
            UpdateElementPosition(this.Position, newPosition);
        }

        internal void SwappingElements(in Point element1Position, in Point element2Position, in Layer layer)
        {
            _ = TrySwappingElements(element1Position, element2Position, layer);
        }
        internal void SwappingElements(in Point element1Position, in Point element2Position)
        {
            SwappingElements(element1Position, element2Position, this.Layer);
        }
        internal void SwappingElements(in Point targetPosition)
        {
            SwappingElements(this.Position, targetPosition);
        }

        internal void DestroyElement(in Point position, in Layer layer)
        {
            world.DestroyElement(position, layer);
        }
        internal void DestroyElement(in Point position)
        {
            DestroyElement(position, this.Layer);
        }
        internal void DestroyElement()
        {
            DestroyElement(this.Position);
        }

        internal void RemoveElement(in Point position, in Layer layer)
        {
            _ = TryRemoveElement(position, layer);
        }
        internal void RemoveElement(in Point position)
        {
            RemoveElement(position, this.Layer);
        }
        internal void RemoveElement()
        {
            RemoveElement(this.Position, this.Layer);
        }

        internal void ReplaceElement(in Point position, in Layer layer, in ElementIndex index)
        {
            world.ReplaceElement(position, layer, index);
        }
        internal void ReplaceElement(in Point position, in ElementIndex index)
        {
            ReplaceElement(position, this.Layer, index);
        }
        internal void ReplaceElement(in ElementIndex index)
        {
            ReplaceElement(this.Position, index);
        }

        internal void SetElementTemperature(in Point position, in Layer layer, in float value)
        {
            world.SetElementTemperature(position, layer, value);
        }
        internal void SetElementTemperature(in Point position, in float value)
        {
            SetElementTemperature(position, this.Layer, value);
        }
        internal void SetElementTemperature(in float value)
        {
            SetElementTemperature(this.Position, value);
        }

        internal void SetElementColorModifier(in Point position, in Layer layer, in Color value)
        {
            world.SetElementColorModifier(position, layer, value);
        }
        internal void SetElementColorModifier(in Point position, in Color value)
        {
            SetElementColorModifier(position, this.Layer, value);
        }
        internal void SetElementColorModifier(in Color value)
        {
            SetElementColorModifier(this.Position, value);
        }

        internal void SetStoredElement(in Point position, in Layer layer, in ElementIndex index)
        {
            world.SetStoredElement(position, layer, index);
        }
        internal void SetStoredElement(in Point position, in ElementIndex index)
        {
            SetStoredElement(position, this.Layer, index);
        }
        internal void SetStoredElement(in ElementIndex index)
        {
            SetStoredElement(this.Position, index);
        }

        internal ElementIndex GetStoredElement(in Point position, in Layer layer)
        {
            return world.GetStoredElement(position, layer);
        }
        internal ElementIndex GetStoredElement(in Point position)
        {
            return GetStoredElement(position, this.Layer);
        }
        internal ElementIndex GetStoredElement()
        {
            return GetStoredElement(this.Position);
        }

        internal bool HasStoredElement(in Point position, in Layer layer)
        {
            return world.HasStoredElement(position, layer);
        }
        internal bool HasStoredElement(in Point position)
        {
            return HasStoredElement(position, this.Layer);
        }
        internal bool HasStoredElement()
        {
            return HasStoredElement(this.Position);
        }

        internal bool HasElementState(in Point position, in Layer layer, in ElementStates state)
        {
            return world.HasElementState(position, layer, state);
        }
        internal bool HasElementState(in Point position, in ElementStates state)
        {
            return HasElementState(position, this.Layer, state);
        }
        internal bool HasElementState(in ElementStates state)
        {
            return HasElementState(this.Position, state);
        }
        internal void SetElementState(in Point position, in Layer layer, in ElementStates state)
        {
            world.SetElementState(position, layer, state);
        }
        internal void SetElementState(in Point position, in ElementStates state)
        {
            SetElementState(position, this.Layer, state);
        }
        internal void SetElementState(in ElementStates state)
        {
            SetElementState(this.Position, state);
        }
        internal void RemoveElementState(in Point position, in Layer layer, in ElementStates state)
        {
            world.RemoveElementState(position, layer, state);
        }
        internal void RemoveElementState(in Point position, in ElementStates state)
        {
            RemoveElementState(position, this.Layer, state);
        }
        internal void RemoveElementState(in ElementStates state)
        {
            RemoveElementState(this.Position, state);
        }
        internal void ClearElementStates(in Point position, in Layer layer)
        {
            world.ClearElementStates(position, layer);
        }
        internal void ClearElementStates(in Point position)
        {
            ClearElementStates(position, this.Layer);
        }
        internal void ClearElementStates()
        {
            ClearElementStates(this.Position, this.Layer);
        }
        internal void ToggleElementState(in Point position, in Layer layer, in ElementStates state)
        {
            world.ToggleElementState(position, layer, state);
        }
        internal void ToggleElementState(in Point position, in ElementStates state)
        {
            ToggleElementState(position, this.Layer, state);
        }
        internal void ToggleElementState(in ElementStates state)
        {
            ToggleElementState(this.Position, state);
        }

        internal void SetElementTicksRemaining(in Point position, in Layer layer, in int ticks)
        {
            world.SetElementTicksRemaining(position, layer, ticks);
        }
        internal void SetElementTicksRemaining(in Point position, in int ticks)
        {
            SetElementTicksRemaining(position, this.Layer, ticks);
        }
        internal void SetElementTicksRemaining(in int ticks)
        {
            SetElementTicksRemaining(this.Position, ticks);
        }

        internal ElementIndex GetElement(in Point position, in Layer layer)
        {
            return world.GetElement(position, layer);
        }
        internal ElementIndex GetElement(in Point position)
        {
            return GetElement(position, this.Layer);
        }
        internal ElementIndex GetElement()
        {
            return GetElement(this.Position);
        }

        internal Slot GetSlot(in Point position)
        {
            return world.GetSlot(position);
        }
        internal Slot GetSlot()
        {
            return GetSlot(this.Position);
        }

        internal SlotLayer GetSlotLayer(in Point position, in Layer layer)
        {
            return world.GetSlotLayer(position, layer);
        }
        internal SlotLayer GetSlotLayer(in Point position)
        {
            return GetSlotLayer(position, this.Layer);
        }
        internal SlotLayer GetSlotLayer()
        {
            return GetSlotLayer(this.Position, this.Layer);
        }

        internal ElementNeighbors GetNeighboringSlots(in Point position)
        {
            return world.GetNeighboringSlots(position);
        }
        internal ElementNeighbors GetNeighboringSlots()
        {
            return GetNeighboringSlots(this.Position);
        }

        internal bool IsEmptySlot(in Point position)
        {
            return world.IsEmptySlot(position);
        }
        internal bool IsEmptySlot()
        {
            return IsEmptySlot(this.Position);
        }

        internal bool IsEmptySlotLayer(in Point position, in Layer layer)
        {
            return world.IsEmptySlotLayer(position, layer);
        }
        internal bool IsEmptySlotLayer(in Point position)
        {
            return IsEmptySlotLayer(position, this.Layer);
        }
        internal bool IsEmptySlotLayer()
        {
            return IsEmptySlotLayer(this.Position, this.Layer);
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
            return TryNotifyChunk(this.Position);
        }

        internal void NotifyChunk(in Point position)
        {
            world.NotifyChunk(position);
        }
        internal void NotifyChunk()
        {
            NotifyChunk(this.Position);
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
            return TryInstantiateExplosion(position, this.Layer, explosionBuilder);
        }
        internal bool TryInstantiateExplosion(in ExplosionBuilder explosionBuilder)
        {
            return TryInstantiateExplosion(this.Position, explosionBuilder);
        }

        internal void InstantiateExplosion(in Point position, in Layer layer, in ExplosionBuilder explosionBuilder)
        {
            world.InstantiateExplosion(position, layer, explosionBuilder);
        }
        internal void InstantiateExplosion(in Point position, in ExplosionBuilder explosionBuilder)
        {
            InstantiateExplosion(position, this.Layer, explosionBuilder);
        }
        internal void InstantiateExplosion(in ExplosionBuilder explosionBuilder)
        {
            InstantiateExplosion(this.Position, explosionBuilder);
        }

        #endregion
    }
}
