using Microsoft.Xna.Framework;

using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Interfaces.Elements;
using StardustSandbox.Game.World;
using StardustSandbox.Game.World.Data;

using System;

namespace StardustSandbox.Game.Elements.Contexts
{
    public struct SElementContext(SWorld world, SElementDatabase elementDatabase, SWorldSlot slot, Point position) : ISElementManager
    {
        public readonly bool IsEmpty => this._world == null;

        public readonly SWorldSlot Slot => this._element;
        public readonly Point Position => this._position;
        public readonly SElement Element => this.ElementDatabase.GetElementById(this._element.Id);
        public readonly SElementDatabase ElementDatabase => this._elementDatabase;

        private SWorldSlot _element = slot;
        private Point _position = position;

        private readonly SElementDatabase _elementDatabase = elementDatabase;
        private readonly SWorld _world = world;

        #region World
        public void SetPosition(Point newPos)
        {
            _ = TrySetPosition(newPos);
        }
        public bool TrySetPosition(Point newPos)
        {
            if (this._world.TryUpdateElementPosition(this._position, newPos))
            {
                this._element = GetElementSlot(newPos);
                this._position = newPos;

                return true;
            }

            return false;
        }

        public readonly void InstantiateElement<T>() where T : SElement
        {
            InstantiateElement<T>(this.Position);
        }
        public readonly void InstantiateElement(uint id)
        {
            InstantiateElement(this.Position, id);
        }
        public readonly void InstantiateElement(SElement value)
        {
            InstantiateElement(this.Position, value);
        }
        public readonly void InstantiateElement<T>(Point pos) where T : SElement
        {
            this._world.InstantiateElement<T>(pos);
        }
        public readonly void InstantiateElement(Point pos, uint id)
        {
            this._world.InstantiateElement(pos, id);
        }
        public readonly void InstantiateElement(Point pos, SElement value)
        {
            this._world.InstantiateElement(pos, value);
        }
        public readonly bool TryInstantiateElement<T>() where T : SElement
        {
            return TryInstantiateElement<T>(this.Position);
        }
        public readonly bool TryInstantiateElement(uint id)
        {
            return TryInstantiateElement(this.Position, id);
        }
        public readonly bool TryInstantiateElement(SElement value)
        {
            return TryInstantiateElement(this.Position, value);
        }
        public readonly bool TryInstantiateElement<T>(Point pos) where T : SElement
        {
            return this._world.TryInstantiateElement<T>(pos);
        }
        public readonly bool TryInstantiateElement(Point pos, uint id)
        {
            return this._world.TryInstantiateElement(pos, id);
        }
        public readonly bool TryInstantiateElement(Point pos, SElement value)
        {
            return this._world.TryInstantiateElement(pos, value);
        }

        public readonly void UpdateElementPosition(Point newPos)
        {
            UpdateElementPosition(this.Position, newPos);
        }
        public readonly void UpdateElementPosition(Point oldPos, Point newPos)
        {
            this._world.UpdateElementPosition(oldPos, newPos);
        }
        public readonly bool TryUpdateElementPosition(Point newPos)
        {
            return TryUpdateElementPosition(this.Position, newPos);
        }
        public readonly bool TryUpdateElementPosition(Point oldPos, Point newPos)
        {
            return this._world.TryUpdateElementPosition(oldPos, newPos);
        }

        public void SwappingElements(Point element2)
        {
            SwappingElements(this.Position, element2);
        }
        public void SwappingElements(Point element1, Point element2)
        {
            _ = TrySwappingElements(element1, element2);
        }
        public bool TrySwappingElements(Point element2)
        {
            return TrySwappingElements(this.Position, element2);
        }
        public bool TrySwappingElements(Point element1, Point element2)
        {
            if (this._world.TrySwappingElements(element1, element2))
            {
                this._position = element2;
                return true;
            }

            return false;
        }

        public readonly void DestroyElement()
        {
            this._world.DestroyElement(this.Position);
        }
        public readonly void DestroyElement(Point pos)
        {
            this._world.DestroyElement(pos);
        }
        public readonly bool TryDestroyElement()
        {
            return TryDestroyElement(this.Position);
        }
        public readonly bool TryDestroyElement(Point pos)
        {
            return this._world.TryDestroyElement(pos);
        }

        public readonly void ReplaceElement<T>() where T : SElement
        {
            ReplaceElement<T>(this.Position);
        }
        public readonly void ReplaceElement(uint id)
        {
            ReplaceElement(this.Position, id);
        }
        public readonly void ReplaceElement(SElement value)
        {
            ReplaceElement(this.Position, value);
        }
        public readonly void ReplaceElement<T>(Point pos) where T : SElement
        {
            this._world.ReplaceElement<T>(pos);
        }
        public readonly void ReplaceElement(Point pos, uint id)
        {
            this._world.ReplaceElement(pos, id);
        }
        public readonly void ReplaceElement(Point pos, SElement value)
        {
            this._world.ReplaceElement(pos, value);
        }
        public readonly bool TryReplaceElement<T>() where T : SElement
        {
            return TryReplaceElement<T>(this.Position);
        }
        public readonly bool TryReplaceElement(uint id)
        {
            return TryReplaceElement(this.Position, id);
        }
        public readonly bool TryReplaceElement(SElement value)
        {
            return TryReplaceElement(this.Position, value);
        }
        public readonly bool TryReplaceElement<T>(Point pos) where T : SElement
        {
            return this._world.TryReplaceElement<T>(pos);
        }
        public readonly bool TryReplaceElement(Point pos, uint id)
        {
            return this._world.TryReplaceElement(pos, id);
        }
        public readonly bool TryReplaceElement(Point pos, SElement value)
        {
            return this._world.TryReplaceElement(pos, value);
        }

        public readonly SElement GetElement()
        {
            return GetElement(this.Position);
        }
        public readonly SElement GetElement(Point pos)
        {
            return this._world.GetElement(pos);
        }
        public readonly bool TryGetElement(out SElement value)
        {
            return TryGetElement(this.Position, out value);
        }
        public readonly bool TryGetElement(Point pos, out SElement value)
        {
            return this._world.TryGetElement(pos, out value);
        }

        public readonly ReadOnlySpan<(Point, SWorldSlot)> GetElementNeighbors()
        {
            return GetElementNeighbors(this.Position);
        }
        public readonly ReadOnlySpan<(Point, SWorldSlot)> GetElementNeighbors(Point pos)
        {
            return this._world.GetElementNeighbors(pos);
        }
        public readonly bool TryGetElementNeighbors(out ReadOnlySpan<(Point, SWorldSlot)> neighbors)
        {
            return TryGetElementNeighbors(this.Position, out neighbors);
        }
        public readonly bool TryGetElementNeighbors(Point pos, out ReadOnlySpan<(Point, SWorldSlot)> neighbors)
        {
            return this._world.TryGetElementNeighbors(pos, out neighbors);
        }

        public readonly SWorldSlot GetElementSlot()
        {
            return GetElementSlot(this.Position);
        }
        public readonly SWorldSlot GetElementSlot(Point pos)
        {
            return this._world.GetElementSlot(pos);
        }
        public readonly bool TryGetElementSlot(out SWorldSlot value)
        {
            return TryGetElementSlot(this.Position, out value);
        }
        public readonly bool TryGetElementSlot(Point pos, out SWorldSlot value)
        {
            return this._world.TryGetElementSlot(pos, out value);
        }

        public readonly void SetElementTemperature(short value)
        {
            SetElementTemperature(this.Position, value);
        }
        public readonly void SetElementTemperature(Point pos, short value)
        {
            this._world.SetElementTemperature(pos, value);
        }
        public readonly bool TrySetElementTemperature(short value)
        {
            return TrySetElementTemperature(this.Position, value);
        }
        public readonly bool TrySetElementTemperature(Point pos, short value)
        {
            return this._world.TrySetElementTemperature(pos, value);
        }

        // Tools
        public readonly bool IsEmptyElementSlot()
        {
            return IsEmptyElementSlot(this.Position);
        }
        public readonly bool IsEmptyElementSlot(Point pos)
        {
            return this._world.IsEmptyElementSlot(pos);
        }
        #endregion

        #region Chunks
        public readonly bool TryNotifyChunk(Point pos)
        {
            return this._world.TryNotifyChunk(pos);
        }
        #endregion
    }
}