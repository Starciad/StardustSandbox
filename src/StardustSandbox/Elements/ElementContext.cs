using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.World;
using StardustSandbox.ExplosionSystem;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements
{
    internal sealed class ElementContext(World world)
    {
        internal Slot Slot => this.worldSlot;
        internal SlotLayer SlotLayer => this.worldSlot.GetLayer(this.Layer);
        internal Point Position => this.worldSlot.Position;
        internal LayerType Layer { get; private set; }

        private Slot worldSlot;

        private readonly World world = world;

        internal void UpdateInformation(Point position, LayerType layer, Slot worldSlot)
        {
            worldSlot.SetPosition(position);

            this.Layer = layer;
            this.worldSlot = worldSlot;
        }

        #region ELEMENTS

        internal bool TrySetPosition(Point newPosition, LayerType layer)
        {
            if (this.world.TryUpdateElementPosition(this.Position, newPosition, layer))
            {
                this.worldSlot = GetSlot(newPosition);
                return true;
            }

            return false;
        }
        internal bool TrySetPosition(Point newPosition)
        {
            return TrySetPosition(newPosition, this.Layer);
        }

        internal bool TryInstantiateElement(Point position, LayerType layer, ElementIndex index)
        {
            return this.world.TryInstantiateElement(position, layer, index);
        }
        internal bool TryInstantiateElement(Point position, LayerType layer, Element value)
        {
            return this.world.TryInstantiateElement(position, layer, value);
        }
        internal bool TryInstantiateElement(Point position, ElementIndex index)
        {
            return TryInstantiateElement(position, this.Layer, index);
        }
        internal bool TryInstantiateElement(Point position, Element value)
        {
            return TryInstantiateElement(position, this.Layer, value);
        }
        internal bool TryInstantiateElement(ElementIndex index)
        {
            return TryInstantiateElement(this.Position, index);
        }
        internal bool TryInstantiateElement(Element value)
        {
            return TryInstantiateElement(this.Position, value);
        }

        internal bool TryUpdateElementPosition(Point oldPosition, Point newPosition, LayerType layer)
        {
            return this.world.TryUpdateElementPosition(oldPosition, newPosition, layer);
        }
        internal bool TryUpdateElementPosition(Point oldPosition, Point newPosition)
        {
            return TryUpdateElementPosition(oldPosition, newPosition, this.Layer);
        }
        internal bool TryUpdateElementPosition(Point newPosition)
        {
            return TryUpdateElementPosition(this.Position, newPosition);
        }

        internal bool TrySwappingElements(Point element1Position, Point element2Position, LayerType layer)
        {
            return this.world.TrySwappingElements(element1Position, element2Position, layer);
        }
        internal bool TrySwappingElements(Point element1Position, Point element2Position)
        {
            return TrySwappingElements(element1Position, element2Position, this.Layer);
        }
        internal bool TrySwappingElements(Point targetPosition)
        {
            return TrySwappingElements(this.Position, targetPosition);
        }

        internal bool TryDestroyElement(Point position, LayerType layer)
        {
            return this.world.TryDestroyElement(position, layer);
        }
        internal bool TryDestroyElement(Point position)
        {
            return TryDestroyElement(position, this.Layer);
        }
        internal bool TryDestroyElement()
        {
            return TryDestroyElement(this.Position);
        }

        internal bool TryRemoveElement(Point position, LayerType layer)
        {
            return this.world.TryRemoveElement(position, layer);
        }
        internal bool TryRemoveElement(Point position)
        {
            return TryRemoveElement(position, this.Layer);
        }
        internal bool TryRemoveElement()
        {
            return TryRemoveElement(this.Position);
        }

        internal bool TryReplaceElement(Point position, LayerType layer, ElementIndex index)
        {
            return this.world.TryReplaceElement(position, layer, index);
        }
        internal bool TryReplaceElement(Point position, LayerType layer, Element value)
        {
            return this.world.TryReplaceElement(position, layer, value);
        }
        internal bool TryReplaceElement(Point position, ElementIndex index)
        {
            return TryReplaceElement(position, this.Layer, index);
        }
        internal bool TryReplaceElement(Point position, Element value)
        {
            return TryReplaceElement(position, this.Layer, value);
        }
        internal bool TryReplaceElement(ElementIndex index)
        {
            return TryReplaceElement(this.Position, index);
        }
        internal bool TryReplaceElement(Element value)
        {
            return TryReplaceElement(this.Position, value);
        }

        internal bool TryGetElement(Point position, LayerType layer, out Element value)
        {
            return this.world.TryGetElement(position, layer, out value);
        }
        internal bool TryGetElement(Point position, out Element value)
        {
            return TryGetElement(position, this.Layer, out value);
        }
        internal bool TryGetElement(out Element value)
        {
            return TryGetElement(this.Position, out value);
        }

        internal bool TryGetSlot(Point position, out Slot value)
        {
            return this.world.TryGetSlot(position, out value);
        }
        internal bool TryGetSlot(out Slot value)
        {
            return TryGetSlot(this.Position, out value);
        }

        internal bool TrySetElementTemperature(Point position, LayerType layer, short value)
        {
            return this.world.TrySetElementTemperature(position, layer, value);
        }
        internal bool TrySetElementTemperature(Point position, short value)
        {
            return TrySetElementTemperature(position, this.Layer, value);
        }
        internal bool TrySetElementTemperature(short value)
        {
            return TrySetElementTemperature(this.Position, value);
        }

        internal bool TrySetElementColorModifier(Point position, LayerType layer, Color value)
        {
            return this.world.TrySetElementColorModifier(position, layer, value);
        }
        internal bool TrySetElementColorModifier(Point position, Color value)
        {
            return TrySetElementColorModifier(position, this.Layer, value);
        }
        internal bool TrySetElementColorModifier(Color value)
        {
            return TrySetElementColorModifier(this.Position, value);
        }

        internal bool TrySetStoredElement(Point position, LayerType layer, ElementIndex index)
        {
            return this.world.TrySetStoredElement(position, layer, index);
        }
        internal bool TrySetStoredElement(Point position, LayerType layer, Element element)
        {
            return this.world.TrySetStoredElement(position, layer, element);
        }
        internal bool TrySetStoredElement(Point position, ElementIndex index)
        {
            return TrySetStoredElement(position, this.Layer, index);
        }
        internal bool TrySetStoredElement(Point position, Element element)
        {
            return TrySetStoredElement(position, this.Layer, element);
        }
        internal bool TrySetStoredElement(ElementIndex index)
        {
            return TrySetStoredElement(this.Position, index);
        }
        internal bool TrySetStoredElement(Element element)
        {
            return TrySetStoredElement(this.Position, element);
        }

        internal bool TryHasElementState(Point position, LayerType layer, ElementStates state, out bool value)
        {
            return this.world.TryHasElementState(position, layer, state, out value);
        }
        internal bool TryHasElementState(Point position, ElementStates state, out bool value)
        {
            return TryHasElementState(position, this.Layer, state, out value);
        }
        internal bool TryHasElementState(ElementStates state, out bool value)
        {
            return TryHasElementState(this.Position, state, out value);
        }
        internal bool TrySetElementState(Point position, LayerType layer, ElementStates state)
        {
            return this.world.TrySetElementState(position, layer, state);
        }
        internal bool TrySetElementState(Point position, ElementStates state)
        {
            return TrySetElementState(position, this.Layer, state);
        }
        internal bool TrySetElementState(ElementStates state)
        {
            return TrySetElementState(this.Position, state);
        }
        internal bool TryRemoveElementState(Point position, LayerType layer, ElementStates state)
        {
            return this.world.TryRemoveElementState(position, layer, state);
        }
        internal bool TryRemoveElementState(Point position, ElementStates state)
        {
            return TryRemoveElementState(position, this.Layer, state);
        }
        internal bool TryRemoveElementState(ElementStates state)
        {
            return TryRemoveElementState(this.Position, state);
        }
        internal bool TryClearElementStates(Point position, LayerType layer)
        {
            return this.world.TryClearElementStates(position, layer);
        }
        internal bool TryClearElementStates(Point position)
        {
            return TryClearElementStates(position, this.Layer);
        }
        internal bool TryClearElementStates()
        {
            return TryClearElementStates(this.Position, this.Layer);
        }
        internal bool TryToggleElementState(Point position, LayerType layer, ElementStates state)
        {
            return this.world.TryToggleElementState(position, layer, state);
        }
        internal bool TryToggleElementState(Point position, ElementStates state)
        {
            return TryToggleElementState(position, this.Layer, state);
        }
        internal bool TryToggleElementState(ElementStates state)
        {
            return TryToggleElementState(this.Position, state);
        }

        internal void SetPosition(Point newPosition, LayerType layer)
        {
            _ = TrySetPosition(newPosition, layer);
        }
        internal void SetPosition(Point newPosition)
        {
            SetPosition(newPosition, this.Layer);
        }

        internal void InstantiateElement(Point position, LayerType layer, Element value)
        {
            this.world.InstantiateElement(position, layer, value);
        }
        internal void InstantiateElement(Point position, LayerType layer, ElementIndex index)
        {
            this.world.InstantiateElement(position, layer, index);
        }
        internal void InstantiateElement(Point position, ElementIndex index)
        {
            InstantiateElement(position, this.Layer, index);
        }
        internal void InstantiateElement(Point position, Element value)
        {
            InstantiateElement(position, this.Layer, value);
        }
        internal void InstantiateElement(ElementIndex index)
        {
            InstantiateElement(this.Position, index);
        }
        internal void InstantiateElement(Element value)
        {
            InstantiateElement(this.Position, value);
        }

        internal void UpdateElementPosition(Point oldPosition, Point newPosition, LayerType layer)
        {
            this.world.UpdateElementPosition(oldPosition, newPosition, layer);
        }
        internal void UpdateElementPosition(Point oldPosition, Point newPosition)
        {
            UpdateElementPosition(oldPosition, newPosition, this.Layer);
        }
        internal void UpdateElementPosition(Point newPosition)
        {
            UpdateElementPosition(this.Position, newPosition);
        }

        internal void SwappingElements(Point element1Position, Point element2Position, LayerType layer)
        {
            _ = TrySwappingElements(element1Position, element2Position, layer);
        }
        internal void SwappingElements(Point element1Position, Point element2Position)
        {
            SwappingElements(element1Position, element2Position, this.Layer);
        }
        internal void SwappingElements(Point targetPosition)
        {
            SwappingElements(this.Position, targetPosition);
        }

        internal void DestroyElement(Point position, LayerType layer)
        {
            this.world.DestroyElement(position, layer);
        }
        internal void DestroyElement(Point position)
        {
            DestroyElement(position, this.Layer);
        }
        internal void DestroyElement()
        {
            DestroyElement(this.Position);
        }

        internal void RemoveElement(Point position, LayerType layer)
        {
            _ = TryRemoveElement(position, layer);
        }
        internal void RemoveElement(Point position)
        {
            RemoveElement(position, this.Layer);
        }
        internal void RemoveElement()
        {
            RemoveElement(this.Position, this.Layer);
        }

        internal void ReplaceElement(Point position, LayerType layer, ElementIndex index)
        {
            this.world.ReplaceElement(position, layer, index);
        }
        internal void ReplaceElement(Point position, LayerType layer, Element value)
        {
            this.world.ReplaceElement(position, layer, value);
        }
        internal void ReplaceElement(Point position, ElementIndex index)
        {
            ReplaceElement(position, this.Layer, index);
        }
        internal void ReplaceElement(Point position, Element value)
        {
            ReplaceElement(position, this.Layer, value);
        }
        internal void ReplaceElement(ElementIndex index)
        {
            ReplaceElement(this.Position, index);
        }
        internal void ReplaceElement(Element value)
        {
            ReplaceElement(this.Position, value);
        }

        internal void SetElementTemperature(Point position, LayerType layer, short value)
        {
            this.world.SetElementTemperature(position, layer, value);
        }
        internal void SetElementTemperature(Point position, short value)
        {
            SetElementTemperature(position, this.Layer, value);
        }
        internal void SetElementTemperature(short value)
        {
            SetElementTemperature(this.Position, value);
        }

        internal void SetElementColorModifier(Point position, LayerType layer, Color value)
        {
            this.world.SetElementColorModifier(position, layer, value);
        }
        internal void SetElementColorModifier(Point position, Color value)
        {
            SetElementColorModifier(position, this.Layer, value);
        }
        internal void SetElementColorModifier(Color value)
        {
            SetElementColorModifier(this.Position, value);
        }

        internal void SetStoredElement(Point position, LayerType layer, ElementIndex index)
        {
            this.world.SetStoredElement(position, layer, index);
        }
        internal void SetStoredElement(Point position, LayerType layer, Element element)
        {
            this.world.SetStoredElement(position, layer, element);
        }
        internal void SetStoredElement(Point position, ElementIndex index)
        {
            SetStoredElement(position, this.Layer, index);
        }
        internal void SetStoredElement(Point position, Element element)
        {
            SetStoredElement(position, this.Layer, element);
        }
        internal void SetStoredElement(ElementIndex index)
        {
            SetStoredElement(this.Position, index);
        }
        internal void SetStoredElement(Element element)
        {
            SetStoredElement(this.Position, element);
        }

        internal bool HasElementState(Point position, LayerType layer, ElementStates state)
        {
            return this.world.HasElementState(position, layer, state);
        }
        internal bool HasElementState(Point position, ElementStates state)
        {
            return HasElementState(position, this.Layer, state);
        }
        internal bool HasElementState(ElementStates state)
        {
            return HasElementState(this.Position, state);
        }
        internal void SetElementState(Point position, LayerType layer, ElementStates state)
        {
            this.world.SetElementState(position, layer, state);
        }
        internal void SetElementState(Point position, ElementStates state)
        {
            SetElementState(position, this.Layer, state);
        }
        internal void SetElementState(ElementStates state)
        {
            SetElementState(this.Position, state);
        }
        internal void RemoveElementState(Point position, LayerType layer, ElementStates state)
        {
            this.world.RemoveElementState(position, layer, state);
        }
        internal void RemoveElementState(Point position, ElementStates state)
        {
            RemoveElementState(position, this.Layer, state);
        }
        internal void RemoveElementState(ElementStates state)
        {
            RemoveElementState(this.Position, state);
        }
        internal void ClearElementStates(Point position, LayerType layer)
        {
            this.world.ClearElementStates(position, layer);
        }
        internal void ClearElementStates(Point position)
        {
            ClearElementStates(position, this.Layer);
        }
        internal void ClearElementStates()
        {
            ClearElementStates(this.Position, this.Layer);
        }
        internal void ToggleElementState(Point position, LayerType layer, ElementStates state)
        {
            this.world.ToggleElementState(position, layer, state);
        }
        internal void ToggleElementState(Point position, ElementStates state)
        {
            ToggleElementState(position, this.Layer, state);
        }
        internal void ToggleElementState(ElementStates state)
        {
            ToggleElementState(this.Position, state);
        }

        internal Element GetElement(Point position, LayerType layer)
        {
            return this.world.GetElement(position, layer);
        }
        internal Element GetElement(Point position)
        {
            return GetElement(position, this.Layer);
        }
        internal Element GetElement()
        {
            return GetElement(this.Position);
        }

        internal Slot GetSlot(Point position)
        {
            return this.world.GetSlot(position);
        }
        internal Slot GetSlot()
        {
            return GetSlot(this.Position);
        }

        internal IEnumerable<Slot> GetNeighboringSlots(Point position)
        {
            return this.world.GetNeighboringSlots(position);
        }
        internal IEnumerable<Slot> GetNeighboringSlots()
        {
            return GetNeighboringSlots(this.Position);
        }

        internal bool IsEmptySlot(Point position)
        {
            return this.world.IsEmptySlot(position);
        }
        internal bool IsEmptySlot()
        {
            return IsEmptySlot(this.Position);
        }

        internal bool IsEmptySlotLayer(Point position, LayerType layer)
        {
            return this.world.IsEmptySlotLayer(position, layer);
        }
        internal bool IsEmptySlotLayer(Point position)
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

        internal bool TryNotifyChunk(Point position)
        {
            return this.world.TryNotifyChunk(position);
        }
        internal bool TryNotifyChunk()
        {
            return TryNotifyChunk(this.Position);
        }

        internal void NotifyChunk(Point position)
        {
            this.world.NotifyChunk(position);
        }
        internal void NotifyChunk()
        {
            NotifyChunk(this.Position);
        }

        internal bool TryGetChunkUpdateState(Point position, out bool result)
        {
            return this.world.TryGetChunkUpdateState(position, out result);
        }
        internal bool GetChunkUpdateState(Point position)
        {
            return this.world.GetChunkUpdateState(position);
        }

        #endregion

        #region EXPLOSIONS

        internal bool TryInstantiateExplosion(Point position, ExplosionBuilder explosionBuilder)
        {
            return this.world.TryInstantiateExplosion(position, explosionBuilder);
        }
        internal bool TryInstantiateExplosion(ExplosionBuilder explosionBuilder)
        {
            return TryInstantiateExplosion(this.Position, explosionBuilder);
        }

        internal void InstantiateExplosion(Point position, ExplosionBuilder explosionBuilder)
        {
            this.world.InstantiateExplosion(position, explosionBuilder);
        }
        internal void InstantiateExplosion(ExplosionBuilder explosionBuilder)
        {
            InstantiateExplosion(this.Position, explosionBuilder);
        }

        #endregion
    }
}