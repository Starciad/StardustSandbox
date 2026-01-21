/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using StardustSandbox.Core.Interfaces.Collections;

using System.Collections.Generic;

namespace StardustSandbox.Core
{
    internal sealed class ObjectPool
    {
        internal int Count => this.pool.Count;

        private readonly Queue<IPoolableObject> pool = [];

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

        internal IPoolableObject Dequeue()
        {
            _ = TryDequeue(out IPoolableObject value);
            return value;
        }

        internal void Enqueue(in IPoolableObject value)
        {
            this.pool.Enqueue(value);
        }
    }
}
