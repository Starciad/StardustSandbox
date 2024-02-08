using PixelDust.Core.Worlding;
using PixelDust.Core.Worlding.World.Slots;
using PixelDust.Mathematics;

using System;

namespace PixelDust.Core.Elements.Context
{
    /// <summary>
    /// The <see cref="PElementContext"/> class wraps around the <see cref="PWorld"/> class and includes various additional contextual details associated with a specific element.
    /// </summary>
    /// <remarks>
    /// This wrapper provides a comprehensive set of methods and information that facilitate smoother and more intuitive communication and interaction with the game through the <see cref="PWorld"/> class. It allows a pre-selected element to execute multiple operations in a simplified manner, without the necessity of directly defining or communicating with the underlying code of the <see cref="PWorld"/> class.
    /// <br/><br/>
    /// The information within this context is updated every frame, automatically adapting to a newly selected element through internal engine processes. This eliminates the concern about the specific information being manipulated at any given moment.
    /// </remarks>
    public sealed class PElementContext
    {
        /// <summary>
        /// The slot that the current element is located.
        /// </summary>
        public PWorldElementSlot Slot => this._element;

        /// <summary>
        /// The position that the current element is located, based on world coordinates.
        /// </summary>
        /// <remarks>
        /// Contains only integer values.
        /// </remarks>
        public Vector2Int Position => this._position;

        /// <summary>
        /// Current element class.
        /// </summary>
        public PElement Element => this._element.Instance;

        private PWorldElementSlot _element;
        private Vector2Int _position;

        private readonly PWorld _world;

        public PElementContext(PWorld world)
        {
            this._world = world;
        }

        /// <summary>
        /// Updates the parameters and information of the element's current context.
        /// </summary>
        /// <param name="slot">Slot the current element is in.</param>
        /// <param name="position">Position the current element is at.</param>
        internal void Update(PWorldElementSlot slot, Vector2Int position)
        {
            this._element = slot;
            this._position = position;
        }

        #region World
        /// <summary>
        /// Attempts to instantiate an element of type <typeparamref name="T"/> at a specific position in the <see cref="_world"/>.
        /// </summary>
        /// <typeparam name="T">The type of element to instantiate.</typeparam>
        /// <param name="pos">The position where the element should be instantiated.</param>
        /// <returns><c>true</c> if the instantiation was successful, otherwise <c>false</c>.</returns>
        public bool TryInstantiate<T>(Vector2Int pos) where T : PElement
        {
            return this._world.TryInstantiateElement<T>(pos);
        }

        /// <summary>
        /// Attempts to reposition the current element to another desired position in the world.
        /// </summary>
        /// <param name="pos">The desired position to move the element to.</param>
        /// <returns><c>true</c> if the repositioning was successful, otherwise <c>false</c>.</returns>
        public bool TrySetPosition(Vector2Int pos)
        {
            if (this._world.TryUpdateElementPosition(this._position, pos))
            {
                _ = TryGetSlot(pos, out this._element);
                Update(this._element, this._position);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries to make two elements swap positions with each other.
        /// </summary>
        /// <param name="oldPos">The position of the first element.</param>
        /// <param name="newPos">The position of the second element.</param>
        /// <returns><c>true</c> if the position swap was successful, otherwise <c>false</c>.</returns>
        public bool TrySwitchPosition(Vector2Int oldPos, Vector2Int newPos)
        {
            if (this._world.TrySwappingElements(oldPos, newPos))
            {
                _ = TryGetSlot(newPos, out this._element);
                Update(this._element, newPos);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to destroy the element at the specified position.
        /// </summary>
        /// <param name="pos">The position of the element to destroy.</param>
        /// <returns><c>true</c> if the destruction was successful, otherwise <c>false</c>.</returns>
        public bool TryDestroy(Vector2Int pos)
        {
            return this._world.TryDestroyElement(pos);
        }

        /// <summary>
        /// Attempts to retrieve the instance of an element present at the specified position.
        /// </summary>
        /// <param name="pos">The position to query for the element.</param>
        /// <param name="value">The retrieved element instance, if found.</param>
        /// <returns><c>true</c> if an element was found at the position, otherwise <c>false</c>.</returns>
        public bool TryGetElement(Vector2Int pos, out PElement value)
        {
            return this._world.TryGetElement(pos, out value);
        }

        /// <summary>
        /// Attempts to retrieve a slot from the current <see cref="_world"/> instance at the specified position.
        /// </summary>
        /// <param name="pos">The position to query for the slot.</param>
        /// <param name="value">The retrieved slot instance, if found.</param>
        /// <returns><c>true</c> if a slot was found at the position, otherwise <c>false</c>.</returns>
        public bool TryGetSlot(Vector2Int pos, out PWorldElementSlot value)
        {
            return this._world.TryGetElementSlot(pos, out value);
        }

        /// <summary>
        /// Attempts to replace the element present at the specified position with an instance of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of element to replace with.</typeparam>
        /// <param name="pos">The position of the element to replace.</param>
        /// <returns><c>true</c> if the replacement was successful, otherwise <c>false</c>.</returns>
        public bool TryReplace<T>(Vector2Int pos) where T : PElement
        {
            return this._world.TryReplaceElement<T>(pos);
        }

        /// <summary>
        /// Attempts to retrieve all neighbors in a 3x3 area around the specified position.
        /// </summary>
        /// <param name="pos">Position specifies which will be used.t.</param>
        /// <param name="neighbors">An array containing positions and corresponding slots of neighboring elements.</param>
        /// <returns><c>true</c> if neighbors were found, otherwise <c>false</c>.</returns>
        public bool TryGetNeighbors(Vector2Int pos, out ReadOnlySpan<(Vector2Int, PWorldElementSlot)> neighbors)
        {
            return this._world.TryGetElementNeighbors(pos, out neighbors);
        }

        public bool TrySetTemperature(Vector2Int pos, short value)
        {
            return this._world.TrySetElementTemperature(pos, value);
        }

        /// <summary>
        /// Checks whether the slot at a given position is empty.
        /// </summary>
        /// <param name="pos">The position to check.</param>
        /// <returns><c>true</c> if the slot is empty, otherwise <c>false</c>.</returns>
        public bool IsEmpty(Vector2Int pos)
        {
            return this._world.IsEmptyElementSlot(pos);
        }
        #endregion

        #region Chunks
        /// <summary>
        /// Notifies a specific chunk of the world to perform an update on the next game frame refresh.
        /// </summary>
        /// <param name="pos">The position of the chunk to notify.</param>
        /// <returns><c>true</c> if the notification was successful, otherwise <c>false</c>.</returns>
        public bool TryNotifyChunk(Vector2Int pos)
        {
            return this._world.TryNotifyChunk(pos);
        }
        #endregion
    }
}