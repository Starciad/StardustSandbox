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

using System;

namespace StardustSandbox.Core.Events
{
    internal sealed class SubscriptionToken
    {
        private readonly GameEvents hub;
        private readonly Type eventType;
        private readonly Delegate handler;

        private bool isDisposed;

        internal SubscriptionToken(GameEvents hub, Type eventType, Delegate handler)
        {
            this.hub = hub;
            this.eventType = eventType;
            this.handler = handler;
        }

        public void Dispose()
        {
            if (this.isDisposed)
            {
                return;
            }

            this.hub.Unsubscribe(this.eventType, this.handler);

            this.isDisposed = true;
        }
    }
}
