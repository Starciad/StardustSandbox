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

using StardustSandbox.Core.Enums.Directions;

namespace StardustSandbox.Core.UI.Elements
{
    internal sealed class ButtonElement : UIElement
    {
        internal bool HasBackground { get; private set; }
        internal bool HasIcon { get; private set; }
        internal bool HasText { get; private set; }

        private readonly ImageElement backgroundImage = new();
        private readonly ImageElement iconImage = new();
        private readonly TextElement nameText = new();

        public ButtonElement()
        {
            this.backgroundImage.AddChild(this.iconImage);
            this.backgroundImage.AddChild(this.nameText);
            AddChild(this.backgroundImage);
        }

        public override void Reset()
        {
            base.Reset();

            this.HasBackground = false;
            this.HasIcon = false;
            this.HasText = false;

            this.backgroundImage.Reset();
            this.iconImage.Reset();
            this.nameText.Reset();
        }

        internal void SetBackground(Texture2D texture, Rectangle? sourceRectangle)
        {
            this.HasBackground = true;
            this.backgroundImage.Texture = texture;
            this.backgroundImage.SourceRectangle = sourceRectangle;

            if (sourceRectangle != null)
            {
                this.backgroundImage.Size = new(sourceRectangle.Value.Width, sourceRectangle.Value.Height);
            }
        }

        internal void SetIcon(Texture2D texture, Rectangle? sourceRectangle, UIAlignment alignment)
        {
            this.HasIcon = true;
            this.iconImage.Texture = texture;
            this.iconImage.SourceRectangle = sourceRectangle;

            if (sourceRectangle != null)
            {
                this.iconImage.Size = new(sourceRectangle.Value.Width, sourceRectangle.Value.Height);
            }

            this.iconImage.Alignment = alignment;
        }

        internal void SetText(string name, SpriteFont spriteFont)
        {
            this.HasText = true;
            this.nameText.SetTextContent(name);
            this.nameText.SpriteFont = spriteFont;
        }
    }
}
