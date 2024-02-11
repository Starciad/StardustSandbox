using PixelDust.Game.Databases;
using PixelDust.Game.Elements.Interfaces;
using PixelDust.Game.Mathematics;
using PixelDust.Game.World;
using PixelDust.Game.World.Slots;

using System;

namespace PixelDust.Game.Elements.Contexts
{
    /// <summary>
    /// The <see cref="PElementContext"/> class wraps around the <see cref="PWorld"/> class and includes various additional contextual details associated with a specific element.
    /// </summary>
    /// <remarks>
    /// This wrapper provides a comprehensive set of methods and information that facilitate smoother and more intuitive communication and interaction with the game through the <see cref="PWorld"/> class. It allows a pre-selected element to execute multiple operations in a simplified manner, without the necessity of directly defining or communicating with the underlying code of the <see cref="PWorld"/> class.
    /// <br/><br/>
    /// The information within this context is updated every frame, automatically adapting to a newly selected element through public engine processes. This eliminates the concern about the specific information being manipulated at any given moment.
    /// </remarks>
    public struct PElementContext(PWorld world, PElementDatabase elementDatabase, PWorldElementSlot slot, Vector2Int position) : IElementManager
    {
        public readonly PWorldElementSlot Slot => this._element;
        public readonly Vector2Int Position => this._position;
        public readonly PElement Element => this.ElementDatabase.GetElementById(this._element.Id);
        public readonly PElementDatabase ElementDatabase => elementDatabase;

        private PWorldElementSlot _element = slot;
        private Vector2Int _position = position;
        private readonly PWorld _world = world;

        #region World
        public void SetPosition(Vector2Int newPos)
        {
            _ = TrySetPosition(newPos);
        }
        public bool TrySetPosition(Vector2Int newPos)
        {
            if (this._world.TryUpdateElementPosition(this._position, newPos))
            {
                this._element = GetElementSlot(newPos);
                this._position = newPos;

                return true;
            }

            return false;
        }

        public readonly void InstantiateElement<T>() where T : PElement
        {
            InstantiateElement<T>(this.Position);
        }
        public readonly void InstantiateElement(uint id)
        {
            InstantiateElement(this.Position, id);
        }
        public readonly void InstantiateElement(PElement value)
        {
            InstantiateElement(this.Position, value);
        }
        public readonly void InstantiateElement<T>(Vector2Int pos) where T : PElement
        {
            this._world.InstantiateElement<T>(pos);
        }
        public readonly void InstantiateElement(Vector2Int pos, uint id)
        {
            this._world.InstantiateElement(pos, id);
        }
        public readonly void InstantiateElement(Vector2Int pos, PElement value)
        {
            this._world.InstantiateElement(pos, value);
        }
        public readonly bool TryInstantiateElement<T>() where T : PElement
        {
            return TryInstantiateElement<T>(this.Position);
        }
        public readonly bool TryInstantiateElement(uint id)
        {
            return TryInstantiateElement(this.Position, id);
        }
        public readonly bool TryInstantiateElement(PElement value)
        {
            return TryInstantiateElement(this.Position, value);
        }
        public readonly bool TryInstantiateElement<T>(Vector2Int pos) where T : PElement
        {
            return this._world.TryInstantiateElement<T>(pos);
        }
        public readonly bool TryInstantiateElement(Vector2Int pos, uint id)
        {
            return this._world.TryInstantiateElement(pos, id);
        }
        public readonly bool TryInstantiateElement(Vector2Int pos, PElement value)
        {
            return this._world.TryInstantiateElement(pos, value);
        }

        public readonly void UpdateElementPosition(Vector2Int newPos)
        {
            UpdateElementPosition(this.Position, newPos);
        }
        public readonly void UpdateElementPosition(Vector2Int oldPos, Vector2Int newPos)
        {
            this._world.UpdateElementPosition(oldPos, newPos);
        }
        public readonly bool TryUpdateElementPosition(Vector2Int newPos)
        {
            return TryUpdateElementPosition(this.Position, newPos);
        }
        public readonly bool TryUpdateElementPosition(Vector2Int oldPos, Vector2Int newPos)
        {
            return this._world.TryUpdateElementPosition(oldPos, newPos);
        }

        public void SwappingElements(Vector2Int element2)
        {
            SwappingElements(this.Position, element2);
        }
        public void SwappingElements(Vector2Int element1, Vector2Int element2)
        {
            _ = TrySwappingElements(element1, element2);
        }
        public bool TrySwappingElements(Vector2Int element2)
        {
            return TrySwappingElements(this.Position, element2);
        }
        public bool TrySwappingElements(Vector2Int element1, Vector2Int element2)
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
        public readonly void DestroyElement(Vector2Int pos)
        {
            this._world.DestroyElement(pos);
        }
        public readonly bool TryDestroyElement()
        {
            return TryDestroyElement(this.Position);
        }
        public readonly bool TryDestroyElement(Vector2Int pos)
        {
            return this._world.TryDestroyElement(pos);
        }

        public readonly void ReplaceElement<T>() where T : PElement
        {
            ReplaceElement<T>(this.Position);
        }
        public readonly void ReplaceElement(uint id)
        {
            ReplaceElement(this.Position, id);
        }
        public readonly void ReplaceElement(PElement value)
        {
            ReplaceElement(this.Position, value);
        }
        public readonly void ReplaceElement<T>(Vector2Int pos) where T : PElement
        {
            this._world.ReplaceElement<T>(pos);
        }
        public readonly void ReplaceElement(Vector2Int pos, uint id)
        {
            this._world.ReplaceElement(pos, id);
        }
        public readonly void ReplaceElement(Vector2Int pos, PElement value)
        {
            this._world.ReplaceElement(pos, value);
        }
        public readonly bool TryReplaceElement<T>() where T : PElement
        {
            return TryReplaceElement<T>(this.Position);
        }
        public readonly bool TryReplaceElement(uint id)
        {
            return TryReplaceElement(this.Position, id);
        }
        public readonly bool TryReplaceElement(PElement value)
        {
            return TryReplaceElement(this.Position, value);
        }
        public readonly bool TryReplaceElement<T>(Vector2Int pos) where T : PElement
        {
            return this._world.TryReplaceElement<T>(pos);
        }
        public readonly bool TryReplaceElement(Vector2Int pos, uint id)
        {
            return this._world.TryReplaceElement(pos, id);
        }
        public readonly bool TryReplaceElement(Vector2Int pos, PElement value)
        {
            return this._world.TryReplaceElement(pos, value);
        }

        public readonly PElement GetElement()
        {
            return GetElement(this.Position);
        }
        public readonly PElement GetElement(Vector2Int pos)
        {
            return this._world.GetElement(pos);
        }
        public readonly bool TryGetElement(out PElement value)
        {
            return TryGetElement(this.Position, out value);
        }
        public readonly bool TryGetElement(Vector2Int pos, out PElement value)
        {
            return this._world.TryGetElement(pos, out value);
        }

        public readonly ReadOnlySpan<(Vector2Int, PWorldElementSlot)> GetElementNeighbors()
        {
            return GetElementNeighbors(this.Position);
        }
        public readonly ReadOnlySpan<(Vector2Int, PWorldElementSlot)> GetElementNeighbors(Vector2Int pos)
        {
            return this._world.GetElementNeighbors(pos);
        }
        public readonly bool TryGetElementNeighbors(out ReadOnlySpan<(Vector2Int, PWorldElementSlot)> neighbors)
        {
            return TryGetElementNeighbors(this.Position, out neighbors);
        }
        public readonly bool TryGetElementNeighbors(Vector2Int pos, out ReadOnlySpan<(Vector2Int, PWorldElementSlot)> neighbors)
        {
            return this._world.TryGetElementNeighbors(pos, out neighbors);
        }

        public readonly PWorldElementSlot GetElementSlot()
        {
            return GetElementSlot(this.Position);
        }
        public readonly PWorldElementSlot GetElementSlot(Vector2Int pos)
        {
            return this._world.GetElementSlot(pos);
        }
        public readonly bool TryGetElementSlot(out PWorldElementSlot value)
        {
            return TryGetElementSlot(this.Position, out value);
        }
        public readonly bool TryGetElementSlot(Vector2Int pos, out PWorldElementSlot value)
        {
            return this._world.TryGetElementSlot(pos, out value);
        }

        public readonly void SetElementTemperature(short value)
        {
            SetElementTemperature(this.Position, value);
        }
        public readonly void SetElementTemperature(Vector2Int pos, short value)
        {
            this._world.SetElementTemperature(pos, value);
        }
        public readonly bool TrySetElementTemperature(short value)
        {
            return TrySetElementTemperature(this.Position, value);
        }
        public readonly bool TrySetElementTemperature(Vector2Int pos, short value)
        {
            return this._world.TrySetElementTemperature(pos, value);
        }

        // Tools
        public readonly bool IsEmptyElementSlot()
        {
            return IsEmptyElementSlot(this.Position);
        }
        public readonly bool IsEmptyElementSlot(Vector2Int pos)
        {
            return this._world.IsEmptyElementSlot(pos);
        }
        #endregion

        #region Chunks
        public readonly bool TryNotifyChunk(Vector2Int pos)
        {
            return this._world.TryNotifyChunk(pos);
        }
        #endregion
    }
}