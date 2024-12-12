using Microsoft.Xna.Framework;

using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.World;

using System;

namespace StardustSandbox.Core.Elements.Contexts
{
    public sealed class SElementContext(ISWorld world) : ISElementContext
    {
        public ISWorldSlot Slot => this.worldSlot;
        public ISElement Element => this.worldSlot.Element;
        public Point Position => this.worldSlot.Position;

        private ISWorldSlot worldSlot;

        private readonly ISWorld world = world;

        public void UpdateInformation(ISWorldSlot worldSlot, Point position)
        {
            this.worldSlot = worldSlot;
        }

        #region World
        public void SetPosition(Point newPosition)
        {
            _ = TrySetPosition(newPosition);
        }
        public bool TrySetPosition(Point newPosition)
        {
            if (this.world.TryUpdateElementPosition(this.Position, newPosition))
            {
                this.worldSlot = GetElementSlot(newPosition);
                return true;
            }

            return false;
        }

        public void InstantiateElement<T>() where T : ISElement
        {
            InstantiateElement<T>(this.Position);
        }
        public void InstantiateElement(uint identifier)
        {
            InstantiateElement(this.Position, identifier);
        }
        public void InstantiateElement(ISElement value)
        {
            InstantiateElement(this.Position, value);
        }
        public void InstantiateElement<T>(Point position) where T : ISElement
        {
            this.world.InstantiateElement<T>(position);
        }
        public void InstantiateElement(Point position, uint identifier)
        {
            this.world.InstantiateElement(position, identifier);
        }
        public void InstantiateElement(Point position, ISElement value)
        {
            this.world.InstantiateElement(position, value);
        }
        public bool TryInstantiateElement<T>() where T : ISElement
        {
            return TryInstantiateElement<T>(this.Position);
        }
        public bool TryInstantiateElement(uint identifier)
        {
            return TryInstantiateElement(this.Position, identifier);
        }
        public bool TryInstantiateElement(ISElement value)
        {
            return TryInstantiateElement(this.Position, value);
        }
        public bool TryInstantiateElement<T>(Point position) where T : ISElement
        {
            return this.world.TryInstantiateElement<T>(position);
        }
        public bool TryInstantiateElement(Point position, uint identifier)
        {
            return this.world.TryInstantiateElement(position, identifier);
        }
        public bool TryInstantiateElement(Point position, ISElement value)
        {
            return this.world.TryInstantiateElement(position, value);
        }

        public void UpdateElementPosition(Point newPosition)
        {
            UpdateElementPosition(this.Position, newPosition);
        }
        public void UpdateElementPosition(Point oldPosition, Point newPosition)
        {
            this.world.UpdateElementPosition(oldPosition, newPosition);
        }
        public bool TryUpdateElementPosition(Point newPosition)
        {
            return TryUpdateElementPosition(this.Position, newPosition);
        }
        public bool TryUpdateElementPosition(Point oldPosition, Point newPosition)
        {
            return this.world.TryUpdateElementPosition(oldPosition, newPosition);
        }

        public void SwappingElements(Point targetPosition)
        {
            SwappingElements(this.Position, targetPosition);
        }
        public void SwappingElements(Point element1Position, Point element2Position)
        {
            _ = TrySwappingElements(element1Position, element2Position);
        }
        public bool TrySwappingElements(Point targetPosition)
        {
            return TrySwappingElements(this.Position, targetPosition);
        }
        public bool TrySwappingElements(Point element1Position, Point element2Position)
        {
            if (this.world.TrySwappingElements(element1Position, element2Position))
            {
                return true;
            }

            return false;
        }

        public void DestroyElement()
        {
            this.world.DestroyElement(this.Position);
        }
        public void DestroyElement(Point position)
        {
            this.world.DestroyElement(position);
        }
        public bool TryDestroyElement()
        {
            return TryDestroyElement(this.Position);
        }
        public bool TryDestroyElement(Point position)
        {
            return this.world.TryDestroyElement(position);
        }

        public void ReplaceElement<T>() where T : ISElement
        {
            ReplaceElement<T>(this.Position);
        }
        public void ReplaceElement(uint identifier)
        {
            ReplaceElement(this.Position, identifier);
        }
        public void ReplaceElement(ISElement value)
        {
            ReplaceElement(this.Position, value);
        }
        public void ReplaceElement<T>(Point position) where T : ISElement
        {
            this.world.ReplaceElement<T>(position);
        }
        public void ReplaceElement(Point position, uint identifier)
        {
            this.world.ReplaceElement(position, identifier);
        }
        public void ReplaceElement(Point position, ISElement value)
        {
            this.world.ReplaceElement(position, value);
        }
        public bool TryReplaceElement<T>() where T : ISElement
        {
            return TryReplaceElement<T>(this.Position);
        }
        public bool TryReplaceElement(uint identifier)
        {
            return TryReplaceElement(this.Position, identifier);
        }
        public bool TryReplaceElement(ISElement value)
        {
            return TryReplaceElement(this.Position, value);
        }
        public bool TryReplaceElement<T>(Point position) where T : ISElement
        {
            return this.world.TryReplaceElement<T>(position);
        }
        public bool TryReplaceElement(Point position, uint identifier)
        {
            return this.world.TryReplaceElement(position, identifier);
        }
        public bool TryReplaceElement(Point position, ISElement value)
        {
            return this.world.TryReplaceElement(position, value);
        }

        public ISElement GetElement()
        {
            return GetElement(this.Position);
        }
        public ISElement GetElement(Point position)
        {
            return this.world.GetElement(position);
        }
        public bool TryGetElement(out ISElement value)
        {
            return TryGetElement(this.Position, out value);
        }
        public bool TryGetElement(Point position, out ISElement value)
        {
            return this.world.TryGetElement(position, out value);
        }

        public ISWorldSlot[] GetElementNeighbors()
        {
            return GetElementNeighbors(this.Position);
        }
        public ISWorldSlot[] GetElementNeighbors(Point position)
        {
            return this.world.GetElementNeighbors(position);
        }
        public bool TryGetElementNeighbors(out ISWorldSlot[] neighbors)
        {
            return TryGetElementNeighbors(this.Position, out neighbors);
        }
        public bool TryGetElementNeighbors(Point position, out ISWorldSlot[] neighbors)
        {
            return this.world.TryGetElementNeighbors(position, out neighbors);
        }

        public ISWorldSlot GetElementSlot()
        {
            return GetElementSlot(this.Position);
        }
        public ISWorldSlot GetElementSlot(Point position)
        {
            return this.world.GetElementSlot(position);
        }
        public bool TryGetElementSlot(out ISWorldSlot value)
        {
            return TryGetElementSlot(this.Position, out value);
        }
        public bool TryGetElementSlot(Point position, out ISWorldSlot value)
        {
            return this.world.TryGetElementSlot(position, out value);
        }

        public void SetElementTemperature(short value)
        {
            SetElementTemperature(this.Position, value);
        }
        public void SetElementTemperature(Point position, short value)
        {
            this.world.SetElementTemperature(position, value);
        }
        public bool TrySetElementTemperature(short value)
        {
            return TrySetElementTemperature(this.Position, value);
        }
        public bool TrySetElementTemperature(Point position, short value)
        {
            return this.world.TrySetElementTemperature(position, value);
        }

        public void SetElementFreeFalling(bool value)
        {
            SetElementFreeFalling(this.Position, value);
        }
        public void SetElementFreeFalling(Point position, bool value)
        {
            this.world.SetElementFreeFalling(position, value);
        }

        public bool TrySetElementFreeFalling(bool value)
        {
            return TrySetElementFreeFalling(this.Position, value);
        }
        public bool TrySetElementFreeFalling(Point position, bool value)
        {
            return this.world.TrySetElementFreeFalling(position, value);
        }

        public void SetElementColor(Color value)
        {
            SetElementColor(this.Position, value);
        }
        public void SetElementColor(Point position, Color value)
        {
            this.world.SetElementColor(position, value);
        }

        public bool TryElementSetColor(Color value)
        {
            return TrySetElementColor(this.Position, value);
        }
        public bool TrySetElementColor(Point position, Color value)
        {
            return this.world.TrySetElementColor(position, value);
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