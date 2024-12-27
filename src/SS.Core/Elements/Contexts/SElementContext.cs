using Microsoft.Xna.Framework;

using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.Elements.Contexts;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.World.Data;

using System.Collections.Generic;

namespace StardustSandbox.Core.Elements.Contexts
{
    internal sealed class SElementContext(ISWorld world) : ISElementContext
    {
        public SWorldSlot Slot => this.worldSlot;
        public SWorldSlotLayer SlotLayer => this.worldSlot.GetLayer(this.worldLayer);
        public Point Position => this.worldSlot.Position;
        public SWorldLayer Layer => this.worldLayer;

        private SWorldLayer worldLayer;
        private SWorldSlot worldSlot;

        private readonly ISWorld world = world;

        public void UpdateInformation(Point position, SWorldLayer worldLayer, SWorldSlot worldSlot)
        {
            worldSlot.SetPosition(position);

            this.worldLayer = worldLayer;
            this.worldSlot = worldSlot;
        }

        #region World
        public void SetPosition(Point newPosition)
        {
            SetPosition(newPosition, this.Layer);
        }
        public void SetPosition(Point newPosition, SWorldLayer worldLayer)
        {
            _ = TrySetPosition(newPosition, worldLayer);
        }
        public bool TrySetPosition(Point newPosition)
        {
            return TrySetPosition(newPosition, this.Layer);
        }
        public bool TrySetPosition(Point newPosition, SWorldLayer worldLayer)
        {
            if (this.world.TryUpdateElementPosition(this.Position, newPosition, worldLayer))
            {
                this.worldSlot = GetWorldSlot(newPosition);
                return true;
            }

            return false;
        }

        public void InstantiateElement(string identifier)
        {
            InstantiateElement(this.Layer, identifier);
        }
        public void InstantiateElement(ISElement value)
        {
            InstantiateElement(this.Layer, value);
        }
        public void InstantiateElement(SWorldLayer worldLayer, string identifier)
        {
            InstantiateElement(this.Position, worldLayer, identifier);
        }
        public void InstantiateElement(SWorldLayer worldLayer, ISElement value)
        {
            InstantiateElement(this.Position, worldLayer, value);
        }
        public void InstantiateElement(Point position, SWorldLayer worldLayer, string identifier)
        {
            this.world.InstantiateElement(position, worldLayer, identifier);
        }
        public void InstantiateElement(Point position, SWorldLayer worldLayer, ISElement value)
        {
            this.world.InstantiateElement(position, worldLayer, value);
        }
        public bool TryInstantiateElement(string identifier)
        {
            return TryInstantiateElement(this.Layer, identifier);
        }
        public bool TryInstantiateElement(ISElement value)
        {
            return TryInstantiateElement(this.Layer, value);
        }
        public bool TryInstantiateElement(SWorldLayer worldLayer, string identifier)
        {
            return TryInstantiateElement(this.Position, worldLayer, identifier);
        }
        public bool TryInstantiateElement(SWorldLayer worldLayer, ISElement value)
        {
            return TryInstantiateElement(this.Position, worldLayer, value);
        }
        public bool TryInstantiateElement(Point position, SWorldLayer worldLayer, string identifier)
        {
            return this.world.TryInstantiateElement(position, worldLayer, identifier);
        }
        public bool TryInstantiateElement(Point position, SWorldLayer worldLayer, ISElement value)
        {
            return this.world.TryInstantiateElement(position, worldLayer, value);
        }

        public void UpdateElementPosition(Point newPosition)
        {
            UpdateElementPosition(newPosition, this.Layer);
        }
        public void UpdateElementPosition(Point newPosition, SWorldLayer worldLayer)
        {
            UpdateElementPosition(this.Position, newPosition, worldLayer);
        }
        public void UpdateElementPosition(Point oldPosition, Point newPosition, SWorldLayer worldLayer)
        {
            this.world.UpdateElementPosition(oldPosition, newPosition, worldLayer);
        }
        public bool TryUpdateElementPosition(Point newPosition)
        {
            return TryUpdateElementPosition(newPosition, this.Layer);
        }
        public bool TryUpdateElementPosition(Point newPosition, SWorldLayer worldLayer)
        {
            return TryUpdateElementPosition(this.Position, newPosition, worldLayer);
        }
        public bool TryUpdateElementPosition(Point oldPosition, Point newPosition, SWorldLayer worldLayer)
        {
            return this.world.TryUpdateElementPosition(oldPosition, newPosition, worldLayer);
        }

        public void SwappingElements(Point targetPosition)
        {
            SwappingElements(targetPosition, this.Layer);
        }
        public void SwappingElements(Point targetPosition, SWorldLayer worldLayer)
        {
            SwappingElements(this.Position, targetPosition, worldLayer);
        }
        public void SwappingElements(Point element1Position, Point element2Position, SWorldLayer worldLayer)
        {
            _ = TrySwappingElements(element1Position, element2Position, worldLayer);
        }
        public bool TrySwappingElements(Point targetPosition)
        {
            return TrySwappingElements(targetPosition, this.Layer);
        }
        public bool TrySwappingElements(Point targetPosition, SWorldLayer worldLayer)
        {
            return TrySwappingElements(this.Position, targetPosition, worldLayer);
        }
        public bool TrySwappingElements(Point element1Position, Point element2Position, SWorldLayer worldLayer)
        {
            return this.world.TrySwappingElements(element1Position, element2Position, worldLayer);
        }

        public void DestroyElement()
        {
            DestroyElement(this.Layer);
        }
        public void DestroyElement(SWorldLayer worldLayer)
        {
            this.world.DestroyElement(this.Position, worldLayer);
        }
        public void DestroyElement(Point position, SWorldLayer worldLayer)
        {
            this.world.DestroyElement(position, worldLayer);
        }
        public bool TryDestroyElement()
        {
            return TryDestroyElement(this.Layer);
        }
        public bool TryDestroyElement(SWorldLayer worldLayer)
        {
            return TryDestroyElement(this.Position, worldLayer);
        }
        public bool TryDestroyElement(Point position, SWorldLayer worldLayer)
        {
            return this.world.TryDestroyElement(position, worldLayer);
        }

        public void ReplaceElement(string identifier)
        {
            ReplaceElement(this.Layer, identifier);
        }
        public void ReplaceElement(ISElement value)
        {
            ReplaceElement(this.Layer, value);
        }
        public void ReplaceElement(SWorldLayer worldLayer, string identifier)
        {
            ReplaceElement(this.Position, worldLayer, identifier);
        }
        public void ReplaceElement(SWorldLayer worldLayer, ISElement value)
        {
            ReplaceElement(this.Position, worldLayer, value);
        }
        public void ReplaceElement(Point position, SWorldLayer worldLayer, string identifier)
        {
            this.world.ReplaceElement(position, worldLayer, identifier);
        }
        public void ReplaceElement(Point position, SWorldLayer worldLayer, ISElement value)
        {
            this.world.ReplaceElement(position, worldLayer, value);
        }
        public bool TryReplaceElement(string identifier)
        {
            return TryReplaceElement(this.Layer, identifier);
        }
        public bool TryReplaceElement(ISElement value)
        {
            return TryReplaceElement(this.Layer, value);
        }
        public bool TryReplaceElement(SWorldLayer worldLayer, string identifier)
        {
            return TryReplaceElement(this.Position, worldLayer, identifier);
        }
        public bool TryReplaceElement(SWorldLayer worldLayer, ISElement value)
        {
            return TryReplaceElement(this.Position, worldLayer, value);
        }
        public bool TryReplaceElement(Point position, SWorldLayer worldLayer, string identifier)
        {
            return this.world.TryReplaceElement(position, worldLayer, identifier);
        }
        public bool TryReplaceElement(Point position, SWorldLayer worldLayer, ISElement value)
        {
            return this.world.TryReplaceElement(position, worldLayer, value);
        }

        public ISElement GetElement()
        {
            return GetElement(this.Layer);
        }
        public ISElement GetElement(SWorldLayer worldLayer)
        {
            return GetElement(this.Position, worldLayer);
        }
        public ISElement GetElement(Point position, SWorldLayer worldLayer)
        {
            return this.world.GetElement(position, worldLayer);
        }
        public bool TryGetElement(out ISElement value)
        {
            return TryGetElement(this.Layer, out value);
        }
        public bool TryGetElement(SWorldLayer worldLayer, out ISElement value)
        {
            return TryGetElement(this.Position, worldLayer, out value);
        }
        public bool TryGetElement(Point position, SWorldLayer worldLayer, out ISElement value)
        {
            return this.world.TryGetElement(position, worldLayer, out value);
        }

        public IEnumerable<SWorldSlot> GetNeighboringSlots()
        {
            return GetNeighboringSlots(this.Position);
        }
        public IEnumerable<SWorldSlot> GetNeighboringSlots(Point position)
        {
            return this.world.GetNeighboringSlots(position);
        }

        public SWorldSlot GetWorldSlot()
        {
            return GetWorldSlot(this.Position);
        }
        public SWorldSlot GetWorldSlot(Point position)
        {
            return this.world.GetWorldSlot(position);
        }
        public bool TryGetWorldSlot(out SWorldSlot value)
        {
            return TryGetWorldSlot(this.Position, out value);
        }
        public bool TryGetWorldSlot(Point position, out SWorldSlot value)
        {
            return this.world.TryGetWorldSlot(position, out value);
        }

        public void SetElementTemperature(short value)
        {
            SetElementTemperature(this.Layer, value);
        }
        public void SetElementTemperature(SWorldLayer worldLayer, short value)
        {
            SetElementTemperature(this.Position, worldLayer, value);
        }
        public void SetElementTemperature(Point position, SWorldLayer worldLayer, short value)
        {
            this.world.SetElementTemperature(position, worldLayer, value);
        }
        public bool TrySetElementTemperature(short value)
        {
            return TrySetElementTemperature(this.Layer, value);
        }
        public bool TrySetElementTemperature(SWorldLayer worldLayer, short value)
        {
            return TrySetElementTemperature(this.Position, worldLayer, value);
        }
        public bool TrySetElementTemperature(Point position, SWorldLayer worldLayer, short value)
        {
            return this.world.TrySetElementTemperature(position, worldLayer, value);
        }

        public void SetElementFreeFalling(bool value)
        {
            SetElementFreeFalling(this.Layer, value);
        }
        public void SetElementFreeFalling(SWorldLayer worldLayer, bool value)
        {
            SetElementFreeFalling(this.Position, worldLayer, value);
        }
        public void SetElementFreeFalling(Point position, SWorldLayer worldLayer, bool value)
        {
            this.world.SetElementFreeFalling(position, worldLayer, value);
        }
        public bool TrySetElementFreeFalling(bool value)
        {
            return TrySetElementFreeFalling(this.Layer, value);
        }
        public bool TrySetElementFreeFalling(SWorldLayer worldLayer, bool value)
        {
            return TrySetElementFreeFalling(this.Position, worldLayer, value);
        }
        public bool TrySetElementFreeFalling(Point position, SWorldLayer worldLayer, bool value)
        {
            return this.world.TrySetElementFreeFalling(position, worldLayer, value);
        }

        public void SetElementColorModifier(Color value)
        {
            SetElementColorModifier(this.Layer, value);
        }
        public void SetElementColorModifier(SWorldLayer worldLayer, Color value)
        {
            SetElementColorModifier(this.Position, worldLayer, value);
        }
        public void SetElementColorModifier(Point position, SWorldLayer worldLayer, Color value)
        {
            this.world.SetElementColorModifier(position, worldLayer, value);
        }
        public bool TrySetElementColorModifier(Color value)
        {
            return TrySetElementColorModifier(this.Layer, value);
        }
        public bool TrySetElementColorModifier(SWorldLayer worldLayer, Color value)
        {
            return TrySetElementColorModifier(this.Position, worldLayer, value);
        }
        public bool TrySetElementColorModifier(Point position, SWorldLayer worldLayer, Color value)
        {
            return this.world.TrySetElementColorModifier(position, worldLayer, value);
        }

        // Tools
        public bool IsEmptyWorldSlot()
        {
            return IsEmptyWorldSlot(this.Position);
        }
        public bool IsEmptyWorldSlot(Point position)
        {
            return this.world.IsEmptyWorldSlot(position);
        }
        public bool IsEmptyWorldSlotLayer()
        {
            return IsEmptyWorldSlotLayer(this.Position, this.Layer);
        }
        public bool IsEmptyWorldSlotLayer(SWorldLayer worldLayer)
        {
            return IsEmptyWorldSlotLayer(this.Position, worldLayer);
        }
        public bool IsEmptyWorldSlotLayer(Point position, SWorldLayer worldLayer)
        {
            return this.world.IsEmptyWorldSlotLayer(position, worldLayer);
        }
        #endregion

        #region Chunks
        public void NotifyChunk()
        {
            NotifyChunk(this.Position);
        }
        public void NotifyChunk(Point position)
        {
            this.world.NotifyChunk(position);
        }
        public bool TryNotifyChunk()
        {
            return TryNotifyChunk(this.Position);
        }
        public bool TryNotifyChunk(Point position)
        {
            return this.world.TryNotifyChunk(position);
        }
        #endregion
    }
}