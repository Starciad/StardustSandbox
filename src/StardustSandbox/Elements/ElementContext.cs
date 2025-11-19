using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Indexers;
using StardustSandbox.Enums.World;
using StardustSandbox.ExplosionSystem;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements
{
    internal sealed class ElementContext(World world)
    {
        internal Slot Slot => this.worldSlot;
        internal SlotLayer SlotLayer => this.worldSlot.GetLayer(this.worldLayer);
        internal Point Position => this.worldSlot.Position;
        internal LayerType Layer => this.worldLayer;

        private LayerType worldLayer;
        private Slot worldSlot;

        private readonly World world = world;

        internal void UpdateInformation(Point position, LayerType worldLayer, Slot worldSlot)
        {
            worldSlot.SetPosition(position);

            this.worldLayer = worldLayer;
            this.worldSlot = worldSlot;
        }

        #region ELEMENTS

        internal bool TrySetPosition(Point newPosition, LayerType worldLayer)
        {
            if (this.world.TryUpdateElementPosition(this.Position, newPosition, worldLayer))
            {
                this.worldSlot = GetSlot(newPosition);
                return true;
            }

            return false;
        }
        internal bool TrySetPosition(Point newPosition)
        {
            return TrySetPosition(newPosition, this.worldLayer);
        }

        internal bool TryInstantiateElement(Point position, LayerType worldLayer, ElementIndex index)
        {
            return this.world.TryInstantiateElement(position, worldLayer, index);
        }
        internal bool TryInstantiateElement(Point position, LayerType worldLayer, Element value)
        {
            return this.world.TryInstantiateElement(position, worldLayer, value);
        }
        internal bool TryInstantiateElement(LayerType worldLayer, ElementIndex index)
        {
            return TryInstantiateElement(this.Position, worldLayer, index);
        }
        internal bool TryInstantiateElement(LayerType worldLayer, Element value)
        {
            return TryInstantiateElement(this.Position, worldLayer, value);
        }
        internal bool TryInstantiateElement(ElementIndex index)
        {
            return TryInstantiateElement(this.worldLayer, index);
        }
        internal bool TryInstantiateElement(Element value)
        {
            return TryInstantiateElement(this.worldLayer, value);
        }

        internal bool TryUpdateElementPosition(Point oldPosition, Point newPosition, LayerType worldLayer)
        {
            return this.world.TryUpdateElementPosition(oldPosition, newPosition, worldLayer);
        }
        internal bool TryUpdateElementPosition(Point newPosition, LayerType worldLayer)
        {
            return TryUpdateElementPosition(this.Position, newPosition, worldLayer);
        }
        internal bool TryUpdateElementPosition(Point newPosition)
        {
            return TryUpdateElementPosition(newPosition, this.worldLayer);
        }

        internal bool TrySwappingElements(Point element1Position, Point element2Position, LayerType worldLayer)
        {
            return this.world.TrySwappingElements(element1Position, element2Position, worldLayer);
        }
        internal bool TrySwappingElements(Point targetPosition, LayerType worldLayer)
        {
            return TrySwappingElements(this.Position, targetPosition, worldLayer);
        }
        internal bool TrySwappingElements(Point targetPosition)
        {
            return TrySwappingElements(targetPosition, this.worldLayer);
        }

        internal bool TryDestroyElement(Point position, LayerType worldLayer)
        {
            return this.world.TryDestroyElement(position, worldLayer);
        }
        internal bool TryDestroyElement(LayerType worldLayer)
        {
            return TryDestroyElement(this.Position, worldLayer);
        }
        internal bool TryDestroyElement()
        {
            return TryDestroyElement(this.worldLayer);
        }

        internal bool TryRemoveElement(Point position, LayerType worldLayer)
        {
            return this.world.TryRemoveElement(position, worldLayer);
        }
        internal bool TryRemoveElement(Point position)
        {
            return TryRemoveElement(position, this.worldLayer);
        }
        internal bool TryRemoveElement()
        {
            return TryRemoveElement(this.Position);
        }

        internal bool TryReplaceElement(Point position, LayerType worldLayer, ElementIndex index)
        {
            return this.world.TryReplaceElement(position, worldLayer, index);
        }
        internal bool TryReplaceElement(Point position, LayerType worldLayer, Element value)
        {
            return this.world.TryReplaceElement(position, worldLayer, value);
        }
        internal bool TryReplaceElement(LayerType worldLayer, ElementIndex index)
        {
            return TryReplaceElement(this.Position, worldLayer, index);
        }
        internal bool TryReplaceElement(LayerType worldLayer, Element value)
        {
            return TryReplaceElement(this.Position, worldLayer, value);
        }
        internal bool TryReplaceElement(ElementIndex index)
        {
            return TryReplaceElement(this.worldLayer, index);
        }
        internal bool TryReplaceElement(Element value)
        {
            return TryReplaceElement(this.worldLayer, value);
        }

        internal bool TryGetElement(Point position, LayerType worldLayer, out Element value)
        {
            return this.world.TryGetElement(position, worldLayer, out value);
        }
        internal bool TryGetElement(LayerType worldLayer, out Element value)
        {
            return TryGetElement(this.Position, worldLayer, out value);
        }
        internal bool TryGetElement(out Element value)
        {
            return TryGetElement(this.worldLayer, out value);
        }

        internal bool TryGetSlot(Point position, out Slot value)
        {
            return this.world.TryGetSlot(position, out value);
        }
        internal bool TryGetSlot(out Slot value)
        {
            return TryGetSlot(this.Position, out value);
        }

        internal bool TrySetElementTemperature(Point position, LayerType worldLayer, short value)
        {
            return this.world.TrySetElementTemperature(position, worldLayer, value);
        }
        internal bool TrySetElementTemperature(LayerType worldLayer, short value)
        {
            return TrySetElementTemperature(this.Position, worldLayer, value);
        }
        internal bool TrySetElementTemperature(short value)
        {
            return TrySetElementTemperature(this.worldLayer, value);
        }

        internal bool TrySetElementFreeFalling(Point position, LayerType worldLayer, bool value)
        {
            return this.world.TrySetElementFreeFalling(position, worldLayer, value);
        }
        internal bool TrySetElementFreeFalling(LayerType worldLayer, bool value)
        {
            return TrySetElementFreeFalling(this.Position, worldLayer, value);
        }
        internal bool TrySetElementFreeFalling(bool value)
        {
            return TrySetElementFreeFalling(this.worldLayer, value);
        }

        internal bool TrySetElementColorModifier(Point position, LayerType worldLayer, Color value)
        {
            return this.world.TrySetElementColorModifier(position, worldLayer, value);
        }
        internal bool TrySetElementColorModifier(LayerType worldLayer, Color value)
        {
            return TrySetElementColorModifier(this.Position, worldLayer, value);
        }
        internal bool TrySetElementColorModifier(Color value)
        {
            return TrySetElementColorModifier(this.worldLayer, value);
        }

        internal bool TrySetStoredElement(Point position, LayerType worldLayer, ElementIndex index)
        {
            return this.world.TrySetStoredElement(position, worldLayer, index);
        }
        internal bool TrySetStoredElement(Point position, LayerType worldLayer, Element element)
        {
            return this.world.TrySetStoredElement(position, worldLayer, element);
        }
        internal bool TrySetStoredElement(LayerType worldLayer, ElementIndex index)
        {
            return TrySetStoredElement(this.Position, worldLayer, index);
        }
        internal bool TrySetStoredElement(LayerType worldLayer, Element element)
        {
            return TrySetStoredElement(this.Position, worldLayer, element);
        }
        internal bool TrySetStoredElement(ElementIndex index)
        {
            return TrySetStoredElement(this.worldLayer, index);
        }
        internal bool TrySetStoredElement(Element element)
        {
            return TrySetStoredElement(this.worldLayer, element);
        }

        internal void SetPosition(Point newPosition)
        {
            SetPosition(newPosition, this.worldLayer);
        }
        internal void SetPosition(Point newPosition, LayerType worldLayer)
        {
            _ = TrySetPosition(newPosition, worldLayer);
        }

        internal void InstantiateElement(Point position, LayerType worldLayer, Element value)
        {
            this.world.InstantiateElement(position, worldLayer, value);
        }
        internal void InstantiateElement(Point position, LayerType worldLayer, ElementIndex index)
        {
            this.world.InstantiateElement(position, worldLayer, index);
        }
        internal void InstantiateElement(LayerType worldLayer, ElementIndex index)
        {
            InstantiateElement(this.Position, worldLayer, index);
        }
        internal void InstantiateElement(LayerType worldLayer, Element value)
        {
            InstantiateElement(this.Position, worldLayer, value);
        }
        internal void InstantiateElement(ElementIndex index)
        {
            InstantiateElement(this.worldLayer, index);
        }
        internal void InstantiateElement(Element value)
        {
            InstantiateElement(this.worldLayer, value);
        }

        internal void UpdateElementPosition(Point oldPosition, Point newPosition, LayerType worldLayer)
        {
            this.world.UpdateElementPosition(oldPosition, newPosition, worldLayer);
        }
        internal void UpdateElementPosition(Point newPosition, LayerType worldLayer)
        {
            UpdateElementPosition(this.Position, newPosition, worldLayer);
        }
        internal void UpdateElementPosition(Point newPosition)
        {
            UpdateElementPosition(newPosition, this.worldLayer);
        }

        internal void SwappingElements(Point element1Position, Point element2Position, LayerType worldLayer)
        {
            _ = TrySwappingElements(element1Position, element2Position, worldLayer);
        }
        internal void SwappingElements(Point targetPosition, LayerType worldLayer)
        {
            SwappingElements(this.Position, targetPosition, worldLayer);
        }
        internal void SwappingElements(Point targetPosition)
        {
            SwappingElements(targetPosition, this.worldLayer);
        }

        internal void DestroyElement(Point position, LayerType worldLayer)
        {
            this.world.DestroyElement(position, worldLayer);
        }
        internal void DestroyElement(LayerType worldLayer)
        {
            this.world.DestroyElement(this.Position, worldLayer);
        }
        internal void DestroyElement()
        {
            DestroyElement(this.worldLayer);
        }

        internal void RemoveElement(Point position, LayerType worldLayer)
        {
            _ = TryRemoveElement(position, worldLayer);
        }
        internal void RemoveElement(Point position)
        {
            RemoveElement(position, this.Layer);
        }
        internal void RemoveElement()
        {
            RemoveElement(this.Position, this.Layer);
        }

        internal void ReplaceElement(Point position, LayerType worldLayer, ElementIndex index)
        {
            this.world.ReplaceElement(position, worldLayer, index);
        }
        internal void ReplaceElement(Point position, LayerType worldLayer, Element value)
        {
            this.world.ReplaceElement(position, worldLayer, value);
        }
        internal void ReplaceElement(LayerType worldLayer, ElementIndex index)
        {
            ReplaceElement(this.Position, worldLayer, index);
        }
        internal void ReplaceElement(LayerType worldLayer, Element value)
        {
            ReplaceElement(this.Position, worldLayer, value);
        }
        internal void ReplaceElement(ElementIndex index)
        {
            ReplaceElement(this.worldLayer, index);
        }
        internal void ReplaceElement(Element value)
        {
            ReplaceElement(this.worldLayer, value);
        }

        internal void SetElementTemperature(Point position, LayerType worldLayer, short value)
        {
            this.world.SetElementTemperature(position, worldLayer, value);
        }
        internal void SetElementTemperature(LayerType worldLayer, short value)
        {
            SetElementTemperature(this.Position, worldLayer, value);
        }
        internal void SetElementTemperature(short value)
        {
            SetElementTemperature(this.worldLayer, value);
        }

        internal void SetElementFreeFalling(Point position, LayerType worldLayer, bool value)
        {
            this.world.SetElementFreeFalling(position, worldLayer, value);
        }
        internal void SetElementFreeFalling(LayerType worldLayer, bool value)
        {
            SetElementFreeFalling(this.Position, worldLayer, value);
        }
        internal void SetElementFreeFalling(bool value)
        {
            SetElementFreeFalling(this.worldLayer, value);
        }

        internal void SetElementColorModifier(Point position, LayerType worldLayer, Color value)
        {
            this.world.SetElementColorModifier(position, worldLayer, value);
        }
        internal void SetElementColorModifier(LayerType worldLayer, Color value)
        {
            SetElementColorModifier(this.Position, worldLayer, value);
        }
        internal void SetElementColorModifier(Color value)
        {
            SetElementColorModifier(this.worldLayer, value);
        }

        internal void SetStoredElement(Point position, LayerType worldLayer, ElementIndex index)
        {
            this.world.SetStoredElement(position, worldLayer, index);
        }
        internal void SetStoredElement(Point position, LayerType worldLayer, Element element)
        {
            this.world.SetStoredElement(position, worldLayer, element);
        }
        internal void SetStoredElement(LayerType worldLayer, ElementIndex index)
        {
            SetStoredElement(this.Position, worldLayer, index);
        }
        internal void SetStoredElement(LayerType worldLayer, Element element)
        {
            SetStoredElement(this.Position, worldLayer, element);
        }
        internal void SetStoredElement(ElementIndex index)
        {
            SetStoredElement(this.worldLayer, index);
        }
        internal void SetStoredElement(Element element)
        {
            SetStoredElement(this.worldLayer, element);
        }

        internal Element GetElement(Point position, LayerType worldLayer)
        {
            return this.world.GetElement(position, worldLayer);
        }
        internal Element GetElement(LayerType worldLayer)
        {
            return GetElement(this.Position, worldLayer);
        }
        internal Element GetElement()
        {
            return GetElement(this.worldLayer);
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

        internal bool IsEmptySlotLayer(Point position, LayerType worldLayer)
        {
            return this.world.IsEmptySlotLayer(position, worldLayer);
        }
        internal bool IsEmptySlotLayer(LayerType worldLayer)
        {
            return IsEmptySlotLayer(this.Position, worldLayer);
        }
        internal bool IsEmptySlotLayer()
        {
            return IsEmptySlotLayer(this.Position, this.worldLayer);
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