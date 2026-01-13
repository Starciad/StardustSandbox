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

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UI;
using StardustSandbox.Localization;
using StardustSandbox.Managers;
using StardustSandbox.UI.Elements;

namespace StardustSandbox.UI.Common.Tools
{
    internal sealed class MessageUI : UIBase
    {
        private Text message;
        private Label continueButtonLabel;

        private readonly UIManager uiManager;

        internal MessageUI(
            UIIndex index,
            UIManager uiManager
        ) : base(index)
        {
            this.uiManager = uiManager;
        }

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildMessage(root);
            BuildButton(root);
        }

        private static void BuildBackground(Container root)
        {
            Image background = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Scale = new(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT),
                Color = new(AAP64ColorPalette.DarkGray, 160),
                Size = Vector2.One,
            };

            root.AddChild(background);
        }

        private void BuildMessage(Container root)
        {
            this.message = new()
            {
                Scale = new(0.1f),
                Margin = new(0.0f, 96.0f),
                LineHeight = 1.25f,
                TextAreaSize = new(850.0f, 1000.0f),
                SpriteFontIndex = SpriteFontIndex.PixelOperator,
                Alignment = UIDirection.North,
            };

            root.AddChild(this.message);
        }

        private void BuildButton(Container root)
        {
            this.continueButtonLabel = new()
            {
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.13f),
                Margin = new(0.0f, -96.0f),
                Alignment = UIDirection.South,
                TextContent = Localization_Statements.Continue,

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 2.0f,
                BorderThickness = 2.0f,
            };

            root.AddChild(this.continueButtonLabel);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (Interaction.OnMouseLeftClick(this.continueButtonLabel))
            {
                this.uiManager.CloseUI();
            }

            this.continueButtonLabel.Color = Interaction.OnMouseOver(this.continueButtonLabel) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
        }

        internal void SetContent(string text)
        {
            this.message.TextContent = text;
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

