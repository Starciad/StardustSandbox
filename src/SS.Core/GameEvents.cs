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

using StardustSandbox.Core.Interfaces.Events;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core
{
    internal sealed class GameEvents
    {
        private readonly Dictionary<Type, List<Delegate>> subscribers = [];

        public void Subscribe<T>(Action<T> handler) where T : IGameEvent
        {
            Type eventType = typeof(T);

            if (!this.subscribers.TryGetValue(eventType, out List<Delegate> handlers))
            {
                handlers = [];
                this.subscribers.Add(eventType, handlers);
            }

            handlers.Add(handler);
        }

        public void Publish<T>(T domainEvent) where T : IGameEvent
        {
            Type eventType = typeof(T);

            if (!this.subscribers.TryGetValue(eventType, out List<Delegate> handlers))
            {
                return;
            }

            for (int i = 0; i < handlers.Count; i++)
            {
                ((Action<T>)handlers[i]).Invoke(domainEvent);
            }
        }

        internal void Unsubscribe(Type eventType, Delegate handler)
        {
            if (!this.subscribers.TryGetValue(eventType, out List<Delegate> handlers))
            {
                return;
            }

            _ = handlers.Remove(handler);

            if (handlers.Count == 0)
            {
                _ = this.subscribers.Remove(eventType);
            }
        }
    }
}
