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
using Microsoft.Xna.Framework.Input;

using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Enums.Indexers;
using StardustSandbox.Core.Enums.States;
using StardustSandbox.Core.InputSystem;
using StardustSandbox.Core.UI.Builders;
using StardustSandbox.Core.UI.Dependencies;
using StardustSandbox.Core.UI.Elements.Common;
using StardustSandbox.Core.UI.Handlers;
using StardustSandbox.Core.UI.Models;

using System;

namespace StardustSandbox.Core.UI.Common
{
    internal sealed class KeySelectorUI : UIBase<KeySelectorUIDependencies, KeySelectorUIModel>
    {
        private Image shadowBackground;
        private Text message;

        internal KeySelectorUI(KeySelectorUIDependencies dependencies, UIElementHandler elementHandler) : base(dependencies, elementHandler)
        {

        }

        protected override void OnBuild(UIBuildContext context, KeySelectorUIModel model)
        {
            BuildBackground(root);
            BuildMessage(root);
        }

        private void BuildBackground(Container root)
        {
            this.shadowBackground = new(this.assetDatabase.GetTexture(TextureIndex.Pixel))
            {
                Scale = this.GameScreen.Viewport,
                Color = new(AAP64ColorPalette.DarkGray, 160),
                Size = Vector2.One,
            };

            root.AddChild(this.shadowBackground);
        }

        private void BuildMessage(Container root)
        {
            this.message = new(this.assetDatabase.GetSpriteFont(SpriteFontIndex.PixelOperator))
            {
                Scale = new(0.1f),
                Margin = new(0.0f, 96.0f),
                LineHeight = 1.25f,
                TextAreaSize = new(850.0f, 1000.0f),
                Alignment = UIDirection.North,
            };

            root.AddChild(this.message);
        }

        protected override void OnScreenResize()
        {
            this.shadowBackground.Scale = this.GameScreen.Viewport;
        }

        protected override void OnOpened()
        {
            this.gameHandler.SetState(GameStates.IsCriticalMenuOpen);
            this.playerInputController.Disable();

            this.gameWindow.KeyDown += OnKeyDown;
        }

        protected override void OnClosed()
        {
            this.gameHandler.RemoveState(GameStates.IsCriticalMenuOpen);
            this.playerInputController.Enable();

            this.gameWindow.KeyDown -= OnKeyDown;
        }

        private void OnKeyDown(object sender, InputKeyEventArgs inputKeyEventArgs)
        {
            this.soundEffectManager.Play(SoundEffectIndex.GUI_Accepted);

            this.uiManager.CloseUI();
            this.keySelectionCallback?.Invoke(inputKeyEventArgs.Key);
        }
    }
}

