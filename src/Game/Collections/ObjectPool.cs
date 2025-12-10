using StardustSandbox.Interfaces.Collections;

using System.Collections.Generic;

namespace StardustSandbox.Collections
{
    /// <summary>
    /// Provides a simple object pool for managing reusable <see cref="IPoolableObject"/> instances.
    /// This pool helps reduce memory allocations by recycling objects instead of creating new ones.
    /// </summary>
    internal sealed class ObjectPool
    {
        /// <summary>
        /// Gets the current number of objects available in the pool.
        /// </summary>
        internal int Count => this.pool.Count;

        /// <summary>
        /// The underlying queue storing pooled objects.
        /// </summary>
        private readonly Queue<IPoolableObject> pool = [];

        /// <summary>
        /// Attempts to dequeue an object from the pool.
        /// If successful, resets the object and returns <c>true</c>.
        /// </summary>
        /// <param name="value">
        /// When this method returns, contains the dequeued <see cref="IPoolableObject"/> if available; otherwise, <c>null</c>.
        /// </param>
        /// <returns>
        /// <c>true</c> if an object was dequeued; otherwise, <c>false</c>.
        /// </returns>
        internal bool TryDequeue(out IPoolableObject value)
        {
            value = null;

            if (this.pool.TryDequeue(out IPoolableObject result))
            {
                result.Reset();
                value = result;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Retrieves an object from the pool, resetting its state.
        /// Returns <c>null</c> if the pool is empty.
        /// </summary>
        /// <returns>
        /// An <see cref="IPoolableObject"/> instance if available; otherwise, <c>null</c>.
        /// </returns>
        internal IPoolableObject Dequeue()
        {
            _ = TryDequeue(out IPoolableObject value);
            return value;
        }

        /// <summary>
        /// Adds an <see cref="IPoolableObject"/> instance to the pool for future reuse.
        /// </summary>
        /// <param name="value">
        /// The <see cref="IPoolableObject"/> to enqueue.
        /// </param>
        internal void Enqueue(IPoolableObject value)
        {
            this.pool.Enqueue(value);
        }
    }
}