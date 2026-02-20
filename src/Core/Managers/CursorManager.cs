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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.InputSystem;
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.Serialization.Settings;

namespace StardustSandbox.Core.Managers
{
    internal sealed class CursorManager
    {
        internal Vector2 Position { get; set; }
        internal Color Color { get; set; }
        internal Color BackgroundColor { get; set; }
        internal float Scale { get; set; }
        internal float Opacity { get; set; }

        private Vector2 backgroundPosition;
        private bool canDraw;

        private Texture2D cursorTexture;

        private static readonly Rectangle[] cursorClipAreas = [
            new(0, 0, 36, 36),
            new(0, 36, 36, 36),
        ];

        internal void Initialize()
        {
            this.cursorTexture = AssetDatabase.GetTexture(TextureIndex.Cursors);
            this.canDraw = true;

            CursorSettings cursorSettings = SettingsSerializer.Load<CursorSettings>();

            this.Color = cursorSettings.Color;
            this.BackgroundColor = cursorSettings.BackgroundColor;
            this.Scale = cursorSettings.Scale;
            this.Opacity = cursorSettings.Opacity;
        }

        internal void Update()
        {
            Vector2 position = InputEngine.CurrentMouseState.Position.ToVector2();

            // Toggle cursor visibility with D2 key
            if (GameParameters.CanHideMouse && InputEngine.CurrentKeyboardState.IsKeyDown(Keys.D2) && !InputEngine.PreviousKeyboardState.IsKeyDown(Keys.D2))
            {
                this.canDraw = !this.canDraw;
            }

            this.backgroundPosition = position;
            this.Position = position;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            if (!this.canDraw)
            {
                return;
            }

            spriteBatch.Draw(this.cursorTexture, this.backgroundPosition, cursorClipAreas[1], new(this.BackgroundColor, this.Opacity), 0f, Vector2.Zero, this.Scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(this.cursorTexture, this.Position, cursorClipAreas[0], new(this.Color, this.Opacity), 0f, Vector2.Zero, this.Scale, SpriteEffects.None, 0f);
        }
    }
}
