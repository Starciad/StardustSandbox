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
            InstantiateElement<T>(this._position);
        }
        public void InstantiateElement(uint id)
        {
            InstantiateElement(this._position, id);
        }
        public void InstantiateElement(SElement value)
        {
            InstantiateElement(this._position, value);
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
            return TryInstantiateElement<T>(this._position);
        }
        public bool TryInstantiateElement(uint id)
        {
            return TryInstantiateElement(this._position, id);
        }
        public bool TryInstantiateElement(SElement value)
        {
            return TryInstantiateElement(this._position, value);
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
            UpdateElementPosition(this._position, newPos);
        }
        public void UpdateElementPosition(Point oldPos, Point newPos)
        {
            this._world.UpdateElementPosition(oldPos, newPos);
        }
        public bool TryUpdateElementPosition(Point newPos)
        {
            return TryUpdateElementPosition(this._position, newPos);
        }
        public bool TryUpdateElementPosition(Point oldPos, Point newPos)
        {
            return this._world.TryUpdateElementPosition(oldPos, newPos);
        }

        public void SwappingElements(Point targetPos)
        {
            SwappingElements(this._position, targetPos);
        }
        public void SwappingElements(Point element1Pos, Point element2Pos)
        {
            _ = TrySwappingElements(element1Pos, element2Pos);
        }
        public bool TrySwappingElements(Point targetPos)
        {
            return TrySwappingElements(this._position, targetPos);
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
            this._world.DestroyElement(this._position);
        }
        public void DestroyElement(Point pos)
        {
            this._world.DestroyElement(pos);
        }
        public bool TryDestroyElement()
        {
            return TryDestroyElement(this._position);
        }
        public bool TryDestroyElement(Point pos)
        {
            return this._world.TryDestroyElement(pos);
        }

        public void ReplaceElement<T>() where T : SElement
        {
            ReplaceElement<T>(this._position);
        }
        public void ReplaceElement(uint id)
        {
            ReplaceElement(this._position, id);
        }
        public void ReplaceElement(SElement value)
        {
            ReplaceElement(this._position, value);
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
            return TryReplaceElement<T>(this._position);
        }
        public bool TryReplaceElement(uint id)
        {
            return TryReplaceElement(this._position, id);
        }
        public bool TryReplaceElement(SElement value)
        {
            return TryReplaceElement(this._position, value);
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
            return GetElement(this._position);
        }
        public SElement GetElement(Point pos)
        {
            return this._world.GetElement(pos);
        }
        public bool TryGetElement(out SElement value)
        {
            return TryGetElement(this._position, out value);
        }
        public bool TryGetElement(Point pos, out SElement value)
        {
            return this._world.TryGetElement(pos, out value);
        }

        public ReadOnlySpan<(Point, SWorldSlot)> GetElementNeighbors()
        {
            return GetElementNeighbors(this._position);
        }
        public ReadOnlySpan<(Point, SWorldSlot)> GetElementNeighbors(Point pos)
        {
            return this._world.GetElementNeighbors(pos);
        }
        public bool TryGetElementNeighbors(out ReadOnlySpan<(Point, SWorldSlot)> neighbors)
        {
            return TryGetElementNeighbors(this._position, out neighbors);
        }
        public bool TryGetElementNeighbors(Point pos, out ReadOnlySpan<(Point, SWorldSlot)> neighbors)
        {
            return this._world.TryGetElementNeighbors(pos, out neighbors);
        }

        public SWorldSlot GetElementSlot()
        {
            return GetElementSlot(this._position);
        }
        public SWorldSlot GetElementSlot(Point pos)
        {
            return this._world.GetElementSlot(pos);
        }
        public bool TryGetElementSlot(out SWorldSlot value)
        {
            return TryGetElementSlot(this._position, out value);
        }
        public bool TryGetElementSlot(Point pos, out SWorldSlot value)
        {
            return this._world.TryGetElementSlot(pos, out value);
        }

        public void SetElementTemperature(short value)
        {
            SetElementTemperature(this._position, value);
        }
        public void SetElementTemperature(Point pos, short value)
        {
            this._world.SetElementTemperature(pos, value);
        }
        public bool TrySetElementTemperature(short value)
        {
            return TrySetElementTemperature(this._position, value);
        }
        public bool TrySetElementTemperature(Point pos, short value)
        {
            return this._world.TrySetElementTemperature(pos, value);
        }

        public void SetElementFreeFalling(bool value)
        {
            SetElementFreeFalling(this._position, value);
        }
        public void SetElementFreeFalling(Point pos, bool value)
        {
            this._world.SetElementFreeFalling(pos, value);
        }

        public bool TrySetElementFreeFalling(bool value)
        {
            return TrySetElementFreeFalling(this._position, value);
        }
        public bool TrySetElementFreeFalling(Point pos, bool value)
        {
            return this._world.TrySetElementFreeFalling(pos, value);
        }

        public void SetElementColor(Color value)
        {
            SetElementColor(this._position, value);
        }
        public void SetElementColor(Point pos, Color value)
        {
            this._world.SetElementColor(pos, value);
        }

        public bool TryElementSetColor(Color value)
        {
            return TrySetElementColor(this._position, value);
        }
        public bool TrySetElementColor(Point pos, Color value)
        {
            return this._world.TrySetElementColor(pos, value);
        }

        // Tools
        public bool IsEmptyElementSlot()
        {
            return IsEmptyElementSlot(this._position);
        }
        public bool IsEmptyElementSlot(Point pos)
        {
            return this._world.IsEmptyElementSlot(pos);
        }
        #endregion

        #region Chunks
        public void NotifyChunk()
        {
            NotifyChunk(this._position);
        }
        public void NotifyChunk(Point pos)
        {
            this._world.NotifyChunk(pos);
        }
        public bool TryNotifyChunk()
        {
            return TryNotifyChunk(this._position);
        }
        public bool TryNotifyChunk(Point pos)
        {
            return this._world.TryNotifyChunk(pos);
        }
        #endregion
    }
}