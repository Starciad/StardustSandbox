/*
 * Copyright (C) 2026  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
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

using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.InputSystem;
using StardustSandbox.Serialization;
using StardustSandbox.Serialization.Settings;

namespace StardustSandbox.Managers
{
    internal sealed class CursorManager
    {
        internal Vector2 Position => this.position;
        internal Vector2 Scale => this.scale;
        internal Color Color => this.color;

        private Vector2 position;
        private Vector2 scale;
        private Color color;

        private Vector2 backgroundPosition;
        private Color backgroundColor;

        private Texture2D cursorTexture;

        private static readonly Rectangle[] cursorClipAreas = [
            new(0, 0, 36, 36),
            new(0, 36, 36, 36),
        ];

        internal void Initialize()
        {
            this.cursorTexture = AssetDatabase.GetTexture(TextureIndex.Cursors);
            ApplySettings(SettingsSerializer.Load<CursorSettings>());
        }

        internal void Update()
        {
            Vector2 position = Input.MouseState.Position.ToVector2();

            this.backgroundPosition = position;
            this.position = position;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.cursorTexture, this.backgroundPosition, cursorClipAreas[1], this.backgroundColor, 0f, Vector2.Zero, this.scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(this.cursorTexture, this.position, cursorClipAreas[0], this.color, 0f, Vector2.Zero, this.scale, SpriteEffects.None, 0f);
        }

        internal void ApplySettings(in CursorSettings cursorSettings)
        {
            this.color = cursorSettings.Color;
            this.backgroundColor = cursorSettings.BackgroundColor;
            this.scale = new(cursorSettings.Scale);
        }
    }
}

