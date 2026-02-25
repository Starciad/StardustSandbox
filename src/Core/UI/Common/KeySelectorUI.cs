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

using StardustSandbox.Core.Audio;
using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Enums.States;
using StardustSandbox.Core.InputSystem;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.UI.Elements;

using System;

namespace StardustSandbox.Core.UI.Common
{
    internal sealed class KeySelectorUI : UIBase
    {
        private Action<Keys> keySelectionCallback;

        private Image shadowBackground;
        private Text message;

        private readonly GameWindow gameWindow;
        private readonly PlayerInputController inputController;
        private readonly UIManager uiManager;

        internal KeySelectorUI(
            GameWindow gameWindow,
            PlayerInputController inputController,
            UIManager uiManager
        ) : base()
        {
            this.gameWindow = gameWindow;
            this.inputController = inputController;
            this.uiManager = uiManager;
        }

        internal void Setup(string synopsis, Action<Keys> keySelectionCallback)
        {
            this.message.TextContent = synopsis;
            this.keySelectionCallback = keySelectionCallback;
        }

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildMessage(root);
        }

        private void BuildBackground(Container root)
        {
            this.shadowBackground = new()
            {
                TextureIndex = TextureIndex.Pixel,
                Scale = GameScreen.GetViewport(),
                Color = new(AAP64ColorPalette.DarkGray, 160),
                Size = Vector2.One,
            };

            root.AddChild(this.shadowBackground);
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

        protected override void OnScreenResize(Vector2 newSize)
        {
            this.shadowBackground.Scale = newSize;
        }

        protected override void OnOpened()
        {
            GameHandler.SetState(GameStates.IsCriticalMenuOpen);
            this.inputController.Disable();

            this.gameWindow.KeyDown += OnKeyDown;
        }

        protected override void OnClosed()
        {
            GameHandler.RemoveState(GameStates.IsCriticalMenuOpen);
            this.inputController.Enable();

            this.gameWindow.KeyDown -= OnKeyDown;
        }

        private void OnKeyDown(object sender, InputKeyEventArgs inputKeyEventArgs)
        {
            SoundEngine.Play(SoundEffectIndex.GUI_Accepted);

            this.uiManager.CloseUI();
            this.keySelectionCallback?.Invoke(inputKeyEventArgs.Key);
        }
    }
}

