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

using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Enums.States;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.UI.Elements;

namespace StardustSandbox.Core.UI.Common
{
    internal sealed partial class OptionsUI : UIBase
    {
        private Image panelBackground, shadowBackground;

        private readonly TooltipBox tooltipBox;

        internal OptionsUI(
            ColorPickerUI colorPickerUI,
            CursorManager cursorManager,
            KeySelectorUI keySelectorUI,
            MessageUI messageUI,
            SliderUI sliderUI,
            TooltipBox tooltipBox,
            UIManager uiManager,
            VideoManager videoManager
        ) : base()
        {
            this.tooltipBox = tooltipBox;
        }

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);

            root.AddChild(this.tooltipBox);
        }

        private void BuildBackground(Container root)
        {
            this.shadowBackground = new()
            {
                TextureIndex = TextureIndex.Pixel,
                Scale = GameScreen.GetViewport(),
                Size = Vector2.One,
                Color = new(AAP64ColorPalette.DarkGray, 160)
            };

            this.panelBackground = new()
            {
                Alignment = UIDirection.Center,
                TextureIndex = TextureIndex.UIBackgroundOptions,
                Size = new(1084.0f, 607.0f),
            };

            root.AddChild(this.shadowBackground);
            root.AddChild(this.panelBackground);
        }

        protected override void OnResize(Vector2 newSize)
        {
            this.shadowBackground.Scale = newSize;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            this.tooltipBox.CanDraw = false;
        }

        protected override void OnOpened()
        {
            GameHandler.SetState(GameStates.IsCriticalMenuOpen);
        }

        protected override void OnClosed()
        {
            GameHandler.RemoveState(GameStates.IsCriticalMenuOpen);
        }
    }
}

