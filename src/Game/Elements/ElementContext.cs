using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.World;
using StardustSandbox.Explosions;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.Elements
{
    internal sealed class ElementContext(World world)
    {
        internal Slot Slot => this.slot;
        internal SlotLayer SlotLayer => this.slot.GetLayer(this.Layer);
        internal Point Position => this.slot.Position;
        internal Layer Layer { get; private set; }

        private Slot slot;

        private readonly World world = world;

        internal void UpdateInformation(in Point position, in Layer layer, Slot slot)
        {
            slot.SetPosition(position);

            this.Layer = layer;
            this.slot = slot;
        }

        #region WORLD

        internal Point GetWorldSize()
        {
            return this.world.Information.Size;
        }

        internal float GetWorldTemperature()
        {
            return this.world.Temperature.CurrentTemperature;
        }

        internal bool CanApplyWorldTemperature()
        {
            return this.world.Temperature.CanApplyTemperature;
        }

        #endregion

        #region ELEMENTS

        internal bool TrySetPosition(in Point newPosition, in Layer layer)
        {
            if (this.world.TryUpdateElementPosition(this.Position, newPosition, layer))
            {
                this.slot = GetSlot(newPosition);
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
            return this.world.TryInstantiateElement(position, layer, index);
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
            return this.world.TryUpdateElementPosition(oldPosition, newPosition, layer);
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
            return this.world.TrySwappingElements(element1Position, element2Position, layer);
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
            return this.world.TryDestroyElement(position, layer);
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
            return this.world.TryRemoveElement(position, layer);
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
            return this.world.TryReplaceElement(position, layer, index);
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
            return this.world.TryGetElement(position, layer, out index);
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
            return this.world.TryGetSlot(position, out value);
        }
        internal bool TryGetSlot(out Slot value)
        {
            return TryGetSlot(this.Position, out value);
        }

        internal bool TryGetSlotLayer(in Point position, in Layer layer, out SlotLayer value)
        {
            return this.world.TryGetSlotLayer(position, layer, out value);
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
            return this.world.TrySetElementTemperature(position, layer, value);
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
            return this.world.TrySetElementColorModifier(position, layer, value);
        }
        internal bool TrySetElementColorModifier(in Point position, in Color value)
        {
            return TrySetElementColorModifier(position, this.Layer, value);
        }
        internal bool TrySetElementColorModifier(in Color value)
        {
            return TrySetElementColorModifier(this.Position, value);
        }

        internal bool TrySetStoredElement(in Point position, in Layer layer, in ElementIndex index)
        {
            return this.world.TrySetStoredElement(position, layer, index);
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
            return this.world.TryGetStoredElement(position, layer, out index);
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
            return this.world.TryHasElementState(position, layer, state, out value);
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
            return this.world.TrySetElementState(position, layer, state);
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
            return this.world.TryRemoveElementState(position, layer, state);
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
            return this.world.TryClearElementStates(position, layer);
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
            return this.world.TryToggleElementState(position, layer, state);
        }
        internal bool TryToggleElementState(in Point position, in ElementStates state)
        {
            return TryToggleElementState(position, this.Layer, state);
        }
        internal bool TryToggleElementState(in ElementStates state)
        {
            return TryToggleElementState(this.Position, state);
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
            this.world.InstantiateElement(position, layer, index);
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
            this.world.UpdateElementPosition(oldPosition, newPosition, layer);
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
            this.world.DestroyElement(position, layer);
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
            this.world.ReplaceElement(position, layer, index);
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
            this.world.SetElementTemperature(position, layer, value);
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
            this.world.SetElementColorModifier(position, layer, value);
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
            this.world.SetStoredElement(position, layer, index);
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
            return this.world.GetStoredElement(position, layer);
        }
        internal ElementIndex GetStoredElement(in Point position)
        {
            return GetStoredElement(position, this.Layer);
        }
        internal ElementIndex GetStoredElement()
        {
            return GetStoredElement(this.Position);
        }

        internal bool HasElementState(in Point position, in Layer layer, in ElementStates state)
        {
            return this.world.HasElementState(position, layer, state);
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
            this.world.SetElementState(position, layer, state);
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
            this.world.RemoveElementState(position, layer, state);
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
            this.world.ClearElementStates(position, layer);
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
            this.world.ToggleElementState(position, layer, state);
        }
        internal void ToggleElementState(in Point position, in ElementStates state)
        {
            ToggleElementState(position, this.Layer, state);
        }
        internal void ToggleElementState(in ElementStates state)
        {
            ToggleElementState(this.Position, state);
        }

        internal ElementIndex GetElement(in Point position, in Layer layer)
        {
            return this.world.GetElement(position, layer);
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
            return this.world.GetSlot(position);
        }
        internal Slot GetSlot()
        {
            return GetSlot(this.Position);
        }

        internal SlotLayer GetSlotLayer(in Point position, in Layer layer)
        {
            return this.world.GetSlotLayer(position, layer);
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
            return this.world.GetNeighboringSlots(position);
        }
        internal ElementNeighbors GetNeighboringSlots()
        {
            return GetNeighboringSlots(this.Position);
        }

        internal bool IsEmptySlot(in Point position)
        {
            return this.world.IsEmptySlot(position);
        }
        internal bool IsEmptySlot()
        {
            return IsEmptySlot(this.Position);
        }

        internal bool IsEmptySlotLayer(in Point position, in Layer layer)
        {
            return this.world.IsEmptySlotLayer(position, layer);
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
            return this.world.GetTotalElementCount();
        }
        internal uint GetTotalForegroundElementCount()
        {
            return this.world.GetTotalForegroundElementCount();
        }
        internal uint GetTotalBackgroundElementCount()
        {
            return this.world.GetTotalBackgroundElementCount();
        }

        #endregion

        #region CHUNKING

        internal bool TryNotifyChunk(in Point position)
        {
            return this.world.TryNotifyChunk(position);
        }
        internal bool TryNotifyChunk()
        {
            return TryNotifyChunk(this.Position);
        }

        internal void NotifyChunk(in Point position)
        {
            this.world.NotifyChunk(position);
        }
        internal void NotifyChunk()
        {
            NotifyChunk(this.Position);
        }

        internal bool TryGetChunkUpdateState(in Point position, out bool result)
        {
            return this.world.TryGetChunkUpdateState(position, out result);
        }
        internal bool GetChunkUpdateState(in Point position)
        {
            return this.world.GetChunkUpdateState(position);
        }

        #endregion

        #region EXPLOSIONS

        internal bool TryInstantiateExplosion(in Point position, in Layer layer, in ExplosionBuilder explosionBuilder)
        {
            return this.world.TryInstantiateExplosion(position, layer, explosionBuilder);
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
            this.world.InstantiateExplosion(position, layer, explosionBuilder);
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