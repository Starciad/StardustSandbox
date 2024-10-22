using Microsoft.Xna.Framework;

using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Interfaces.Elements;
using StardustSandbox.Game.World;
using StardustSandbox.Game.World.Data;

using System;

namespace StardustSandbox.Game.Elements.Contexts
{
    public sealed class SElementContext(SWorld world, SElementDatabase elementDatabase) : ISElementContext
    {
        public SWorldSlot Slot => this._worldSlot;
        public Point Position => this._position;
        public SElement Element => this._worldSlot.Element;

        private SWorldSlot _worldSlot;
        private Point _position;

        private readonly SElementDatabase _elementDatabase = elementDatabase;
        private readonly SWorld _world = world;

        public void UpdateInformation(SWorldSlot worldSlot, Point position)
        {
            this._worldSlot = worldSlot;
            this._position = position;
        }

        #region World
        public void SetPosition(Point newPos)
        {
            _ = TrySetPosition(newPos);
        }
        public bool TrySetPosition(Point newPos)
        {
            if (this._world.TryUpdateElementPosition(this._position, newPos))
            {
                this._worldSlot = GetElementSlot(newPos);
                this._position = newPos;

                return true;
            }

            return false;
        }

        public void InstantiateElement<T>() where T : SElement
        {
            InstantiateElement<T>(this.Position);
        }
        public void InstantiateElement(uint id)
        {
            InstantiateElement(this.Position, id);
        }
        public void InstantiateElement(SElement value)
        {
            InstantiateElement(this.Position, value);
        }
        public void InstantiateElement<T>(Point pos) where T : SElement
        {
            this._world.InstantiateElement<T>(pos);
        }
        public void InstantiateElement(Point pos, uint id)
        {
            this._world.InstantiateElement(pos, id);
        }
        public void InstantiateElement(Point pos, SElement value)
        {
            this._world.InstantiateElement(pos, value);
        }
        public bool TryInstantiateElement<T>() where T : SElement
        {
            return TryInstantiateElement<T>(this.Position);
        }
        public bool TryInstantiateElement(uint id)
        {
            return TryInstantiateElement(this.Position, id);
        }
        public bool TryInstantiateElement(SElement value)
        {
            return TryInstantiateElement(this.Position, value);
        }
        public bool TryInstantiateElement<T>(Point pos) where T : SElement
        {
            return this._world.TryInstantiateElement<T>(pos);
        }
        public bool TryInstantiateElement(Point pos, uint id)
        {
            return this._world.TryInstantiateElement(pos, id);
        }
        public bool TryInstantiateElement(Point pos, SElement value)
        {
            return this._world.TryInstantiateElement(pos, value);
        }

        public void UpdateElementPosition(Point newPos)
        {
            UpdateElementPosition(this.Position, newPos);
        }
        public void UpdateElementPosition(Point oldPos, Point newPos)
        {
            this._world.UpdateElementPosition(oldPos, newPos);
        }
        public bool TryUpdateElementPosition(Point newPos)
        {
            return TryUpdateElementPosition(this.Position, newPos);
        }
        public bool TryUpdateElementPosition(Point oldPos, Point newPos)
        {
            return this._world.TryUpdateElementPosition(oldPos, newPos);
        }

        public void SwappingElements(Point targetPos)
        {
            SwappingElements(this.Position, targetPos);
        }
        public void SwappingElements(Point element1Pos, Point element2Pos)
        {
            _ = TrySwappingElements(element1Pos, element2Pos);
        }
        public bool TrySwappingElements(Point targetPos)
        {
            return TrySwappingElements(this.Position, targetPos);
        }
        public bool TrySwappingElements(Point element1Pos, Point element2Pos)
        {
            if (this._world.TrySwappingElements(element1Pos, element2Pos))
            {
                this._position = element2Pos;
                return true;
            }

            return false;
        }

        public void DestroyElement()
        {
            this._world.DestroyElement(this.Position);
        }
        public void DestroyElement(Point pos)
        {
            this._world.DestroyElement(pos);
        }
        public bool TryDestroyElement()
        {
            return TryDestroyElement(this.Position);
        }
        public bool TryDestroyElement(Point pos)
        {
            return this._world.TryDestroyElement(pos);
        }

        public void ReplaceElement<T>() where T : SElement
        {
            ReplaceElement<T>(this.Position);
        }
        public void ReplaceElement(uint id)
        {
            ReplaceElement(this.Position, id);
        }
        public void ReplaceElement(SElement value)
        {
            ReplaceElement(this.Position, value);
        }
        public void ReplaceElement<T>(Point pos) where T : SElement
        {
            this._world.ReplaceElement<T>(pos);
        }
        public void ReplaceElement(Point pos, uint id)
        {
            this._world.ReplaceElement(pos, id);
        }
        public void ReplaceElement(Point pos, SElement value)
        {
            this._world.ReplaceElement(pos, value);
        }
        public bool TryReplaceElement<T>() where T : SElement
        {
            return TryReplaceElement<T>(this.Position);
        }
        public bool TryReplaceElement(uint id)
        {
            return TryReplaceElement(this.Position, id);
        }
        public bool TryReplaceElement(SElement value)
        {
            return TryReplaceElement(this.Position, value);
        }
        public bool TryReplaceElement<T>(Point pos) where T : SElement
        {
            return this._world.TryReplaceElement<T>(pos);
        }
        public bool TryReplaceElement(Point pos, uint id)
        {
            return this._world.TryReplaceElement(pos, id);
        }
        public bool TryReplaceElement(Point pos, SElement value)
        {
            return this._world.TryReplaceElement(pos, value);
        }

        public SElement GetElement()
        {
            return GetElement(this.Position);
        }
        public SElement GetElement(Point pos)
        {
            return this._world.GetElement(pos);
        }
        public bool TryGetElement(out SElement value)
        {
            return TryGetElement(this.Position, out value);
        }
        public bool TryGetElement(Point pos, out SElement value)
        {
            return this._world.TryGetElement(pos, out value);
        }

        public ReadOnlySpan<(Point, SWorldSlot)> GetElementNeighbors()
        {
            return GetElementNeighbors(this.Position);
        }
        public ReadOnlySpan<(Point, SWorldSlot)> GetElementNeighbors(Point pos)
        {
            return this._world.GetElementNeighbors(pos);
        }
        public bool TryGetElementNeighbors(out ReadOnlySpan<(Point, SWorldSlot)> neighbors)
        {
            return TryGetElementNeighbors(this.Position, out neighbors);
        }
        public bool TryGetElementNeighbors(Point pos, out ReadOnlySpan<(Point, SWorldSlot)> neighbors)
        {
            return this._world.TryGetElementNeighbors(pos, out neighbors);
        }

        public SWorldSlot GetElementSlot()
        {
            return GetElementSlot(this.Position);
        }
        public SWorldSlot GetElementSlot(Point pos)
        {
            return this._world.GetElementSlot(pos);
        }
        public bool TryGetElementSlot(out SWorldSlot value)
        {
            return TryGetElementSlot(this.Position, out value);
        }
        public bool TryGetElementSlot(Point pos, out SWorldSlot value)
        {
            return this._world.TryGetElementSlot(pos, out value);
        }

        public void SetElementTemperature(short value)
        {
            SetElementTemperature(this.Position, value);
        }
        public void SetElementTemperature(Point pos, short value)
        {
            this._world.SetElementTemperature(pos, value);
        }
        public bool TrySetElementTemperature(short value)
        {
            return TrySetElementTemperature(this.Position, value);
        }
        public bool TrySetElementTemperature(Point pos, short value)
        {
            return this._world.TrySetElementTemperature(pos, value);
        }

        public void SetElementFreeFalling(bool value)
        {
            SetElementFreeFalling(this.Position, value);
        }
        public void SetElementFreeFalling(Point pos, bool value)
        {
            this._world.SetElementFreeFalling(pos, value);
        }

        public bool TrySetElementFreeFalling(bool value)
        {
            return TrySetElementFreeFalling(this.Position, value);
        }
        public bool TrySetElementFreeFalling(Point pos, bool value)
        {
            return this._world.TrySetElementFreeFalling(pos, value);
        }

        // Tools
        public bool IsEmptyElementSlot()
        {
            return IsEmptyElementSlot(this.Position);
        }
        public bool IsEmptyElementSlot(Point pos)
        {
            return this._world.IsEmptyElementSlot(pos);
        }

        #endregion

        #region Chunks
        public bool TryNotifyChunk(Point pos)
        {
            return this._world.TryNotifyChunk(pos);
        }
        #endregion
    }
}