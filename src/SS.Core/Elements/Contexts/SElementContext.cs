using Microsoft.Xna.Framework;

using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.World;

using System;

namespace StardustSandbox.Core.Elements.Contexts
{
    internal sealed class SElementContext(ISWorld world) : ISElementContext
    {
        public ISWorldSlot Slot => this.worldSlot;
        public Point Position => this.worldSlot.Position;

        private ISWorldSlot worldSlot;

        private readonly ISWorld world = world;

        public void UpdateInformation(SWorldLayer worldLayer, ISWorldSlot worldSlot, Point position)
        {
            this.worldSlot = worldSlot;
        }

        #region World
        public void SetPosition(SWorldLayer worldLayer, Point newPosition)
        {
            _ = TrySetPosition(worldLayer, newPosition);
        }
        public bool TrySetPosition(SWorldLayer worldLayer, Point newPosition)
        {
            if (this.world.TryUpdateElementPosition(worldLayer, this.Position, newPosition))
            {
                this.worldSlot = GetElementSlot(worldLayer, newPosition);
                return true;
            }

            return false;
        }

        public void InstantiateElement<T>(SWorldLayer worldLayer) where T : ISElement
        {
            InstantiateElement<T>(worldLayer, this.Position);
        }
        public void InstantiateElement(SWorldLayer worldLayer, uint identifier)
        {
            InstantiateElement(worldLayer, this.Position, identifier);
        }
        public void InstantiateElement(SWorldLayer worldLayer, ISElement value)
        {
            InstantiateElement(worldLayer, this.Position, value);
        }
        public void InstantiateElement<T>(SWorldLayer worldLayer, Point position) where T : ISElement
        {
            this.world.InstantiateElement<T>(worldLayer, position);
        }
        public void InstantiateElement(SWorldLayer worldLayer, Point position, uint identifier)
        {
            this.world.InstantiateElement(worldLayer, position, identifier);
        }
        public void InstantiateElement(SWorldLayer worldLayer, Point position, ISElement value)
        {
            this.world.InstantiateElement(worldLayer, position, value);
        }
        public bool TryInstantiateElement<T>(SWorldLayer worldLayer) where T : ISElement
        {
            return TryInstantiateElement<T>(worldLayer, this.Position);
        }
        public bool TryInstantiateElement(SWorldLayer worldLayer, uint identifier)
        {
            return TryInstantiateElement(worldLayer, this.Position, identifier);
        }
        public bool TryInstantiateElement(SWorldLayer worldLayer, ISElement value)
        {
            return TryInstantiateElement(worldLayer, this.Position, value);
        }
        public bool TryInstantiateElement<T>(SWorldLayer worldLayer, Point position) where T : ISElement
        {
            return this.world.TryInstantiateElement<T>(worldLayer, position);
        }
        public bool TryInstantiateElement(SWorldLayer worldLayer, Point position, uint identifier)
        {
            return this.world.TryInstantiateElement(worldLayer, position, identifier);
        }
        public bool TryInstantiateElement(SWorldLayer worldLayer, Point position, ISElement value)
        {
            return this.world.TryInstantiateElement(worldLayer, position, value);
        }

        public void UpdateElementPosition(SWorldLayer worldLayer, Point newPosition)
        {
            UpdateElementPosition(worldLayer, this.Position, newPosition);
        }
        public void UpdateElementPosition(SWorldLayer worldLayer, Point oldPosition, Point newPosition)
        {
            this.world.UpdateElementPosition(worldLayer, oldPosition, newPosition);
        }
        public bool TryUpdateElementPosition(SWorldLayer worldLayer, Point newPosition)
        {
            return TryUpdateElementPosition(worldLayer, this.Position, newPosition);
        }
        public bool TryUpdateElementPosition(SWorldLayer worldLayer, Point oldPosition, Point newPosition)
        {
            return this.world.TryUpdateElementPosition(worldLayer, oldPosition, newPosition);
        }

        public void SwappingElements(SWorldLayer worldLayer, Point targetPosition)
        {
            SwappingElements(worldLayer, this.Position, targetPosition);
        }
        public void SwappingElements(SWorldLayer worldLayer, Point element1Position, Point element2Position)
        {
            _ = TrySwappingElements(worldLayer, element1Position, element2Position);
        }
        public bool TrySwappingElements(SWorldLayer worldLayer, Point targetPosition)
        {
            return TrySwappingElements(worldLayer, this.Position, targetPosition);
        }
        public bool TrySwappingElements(SWorldLayer worldLayer, Point element1Position, Point element2Position)
        {
            return this.world.TrySwappingElements(worldLayer, element1Position, element2Position);
        }

        public void DestroyElement(SWorldLayer worldLayer)
        {
            this.world.DestroyElement(worldLayer, this.Position);
        }
        public void DestroyElement(SWorldLayer worldLayer, Point position)
        {
            this.world.DestroyElement(worldLayer, position);
        }
        public bool TryDestroyElement(SWorldLayer worldLayer)
        {
            return TryDestroyElement(worldLayer, this.Position);
        }
        public bool TryDestroyElement(SWorldLayer worldLayer, Point position)
        {
            return this.world.TryDestroyElement(worldLayer, position);
        }

        public void ReplaceElement<T>(SWorldLayer worldLayer) where T : ISElement
        {
            ReplaceElement<T>(worldLayer, this.Position);
        }
        public void ReplaceElement(SWorldLayer worldLayer, uint identifier)
        {
            ReplaceElement(worldLayer, this.Position, identifier);
        }
        public void ReplaceElement(SWorldLayer worldLayer, ISElement value)
        {
            ReplaceElement(worldLayer, this.Position, value);
        }
        public void ReplaceElement<T>(SWorldLayer worldLayer, Point position) where T : ISElement
        {
            this.world.ReplaceElement<T>(worldLayer, position);
        }
        public void ReplaceElement(SWorldLayer worldLayer, Point position, uint identifier)
        {
            this.world.ReplaceElement(worldLayer, position, identifier);
        }
        public void ReplaceElement(SWorldLayer worldLayer, Point position, ISElement value)
        {
            this.world.ReplaceElement(worldLayer, position, value);
        }
        public bool TryReplaceElement<T>(SWorldLayer worldLayer) where T : ISElement
        {
            return TryReplaceElement<T>(worldLayer, this.Position);
        }
        public bool TryReplaceElement(SWorldLayer worldLayer, uint identifier)
        {
            return TryReplaceElement(worldLayer, this.Position, identifier);
        }
        public bool TryReplaceElement(SWorldLayer worldLayer, ISElement value)
        {
            return TryReplaceElement(worldLayer, this.Position, value);
        }
        public bool TryReplaceElement<T>(SWorldLayer worldLayer, Point position) where T : ISElement
        {
            return this.world.TryReplaceElement<T>(worldLayer, position);
        }
        public bool TryReplaceElement(SWorldLayer worldLayer, Point position, uint identifier)
        {
            return this.world.TryReplaceElement(worldLayer, position, identifier);
        }
        public bool TryReplaceElement(SWorldLayer worldLayer, Point position, ISElement value)
        {
            return this.world.TryReplaceElement(worldLayer, position, value);
        }

        public ISElement GetElement(SWorldLayer worldLayer)
        {
            return GetElement(worldLayer, this.Position);
        }
        public ISElement GetElement(SWorldLayer worldLayer, Point position)
        {
            return this.world.GetElement(worldLayer, position);
        }
        public bool TryGetElement(SWorldLayer worldLayer, out ISElement value)
        {
            return TryGetElement(worldLayer, this.Position, out value);
        }
        public bool TryGetElement(SWorldLayer worldLayer, Point position, out ISElement value)
        {
            return this.world.TryGetElement(worldLayer, position, out value);
        }

        public ReadOnlySpan<ISWorldSlot> GetNeighboringSlots()
        {
            return GetNeighboringSlots(this.Position);
        }
        public ReadOnlySpan<ISWorldSlot> GetNeighboringSlots(Point position)
        {
            return this.world.GetNeighboringSlots(position);
        }
        public bool TryGetNeighboringSlots(out ISWorldSlot[] neighbors)
        {
            return TryGetNeighboringSlots(this.Position, out neighbors);
        }
        public bool TryGetNeighboringSlots(Point position, out ISWorldSlot[] neighbors)
        {
            return this.world.TryGetNeighboringSlots(position, out neighbors);
        }

        public ISWorldSlot GetElementSlot(SWorldLayer worldLayer)
        {
            return GetElementSlot(worldLayer, this.Position);
        }
        public ISWorldSlot GetElementSlot(SWorldLayer worldLayer, Point position)
        {
            return this.world.GetElementSlot(worldLayer, position);
        }
        public bool TryGetElementSlot(SWorldLayer worldLayer, out ISWorldSlot value)
        {
            return TryGetElementSlot(worldLayer, this.Position, out value);
        }
        public bool TryGetElementSlot(SWorldLayer worldLayer, Point position, out ISWorldSlot value)
        {
            return this.world.TryGetElementSlot(worldLayer, position, out value);
        }

        public void SetElementTemperature(SWorldLayer worldLayer, short value)
        {
            SetElementTemperature(worldLayer, this.Position, value);
        }
        public void SetElementTemperature(SWorldLayer worldLayer, Point position, short value)
        {
            this.world.SetElementTemperature(worldLayer, position, value);
        }
        public bool TrySetElementTemperature(SWorldLayer worldLayer, short value)
        {
            return TrySetElementTemperature(worldLayer, this.Position, value);
        }
        public bool TrySetElementTemperature(SWorldLayer worldLayer, Point position, short value)
        {
            return this.world.TrySetElementTemperature(worldLayer, position, value);
        }

        public void SetElementFreeFalling(SWorldLayer worldLayer, bool value)
        {
            SetElementFreeFalling(worldLayer, this.Position, value);
        }
        public void SetElementFreeFalling(SWorldLayer worldLayer, Point position, bool value)
        {
            this.world.SetElementFreeFalling(worldLayer, position, value);
        }

        public bool TrySetElementFreeFalling(SWorldLayer worldLayer, bool value)
        {
            return TrySetElementFreeFalling(worldLayer, this.Position, value);
        }
        public bool TrySetElementFreeFalling(SWorldLayer worldLayer, Point position, bool value)
        {
            return this.world.TrySetElementFreeFalling(worldLayer, position, value);
        }

        public void SetElementColorModifier(SWorldLayer worldLayer, Color value)
        {
            SetElementColorModifier(worldLayer, this.Position, value);
        }
        public void SetElementColorModifier(SWorldLayer worldLayer, Point position, Color value)
        {
            this.world.SetElementColorModifier(worldLayer, position, value);
        }

        public bool TrySetElementColorModifier(SWorldLayer worldLayer, Color value)
        {
            return TrySetElementColorModifier(worldLayer, this.Position, value);
        }
        public bool TrySetElementColorModifier(SWorldLayer worldLayer, Point position, Color value)
        {
            return this.world.TrySetElementColorModifier(worldLayer, position, value);
        }

        // Tools
        public bool IsEmptyElementSlot()
        {
            return IsEmptyElementSlot(this.Position);
        }
        public bool IsEmptyElementSlot(Point position)
        {
            return this.world.IsEmptyElementSlot(position);
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