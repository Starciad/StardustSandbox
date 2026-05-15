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

namespace StardustSandbox.Core
{
    internal sealed class GameScreen
    {
        private readonly GraphicsDeviceManager graphicsDeviceManager;

        internal GameScreen(GraphicsDeviceManager graphicsDeviceManager)
        {
            this.graphicsDeviceManager = graphicsDeviceManager;
        }

        internal Vector2 GetViewport()
        {
            return new(
                this.graphicsDeviceManager.GraphicsDevice.Viewport.Width,
                this.graphicsDeviceManager.GraphicsDevice.Viewport.Height
            );
        }

        internal Vector2 GetViewportCenter()
        {
            return GetViewport() / 2.0f;
        }
    }
}
