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

using Microsoft.Xna.Framework;

using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.WorldSystem;

namespace StardustSandbox.Core.Tools
{
    internal sealed class ToolContext(World world)
    {
        internal World World => world;
        internal Point Position => this.position;
        internal Layer Layer => this.layer;

        private Point position;
        private Layer layer;

        internal void Update(in Point position, in Layer layer)
        {
            this.position = position;
            this.layer = layer;
        }
    }
}

