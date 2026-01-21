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

using StardustSandbox.Core.Audio;
using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Enums.States;
using StardustSandbox.Core.InputSystem.Game;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.UI.Elements;
using StardustSandbox.Core.UI.Settings;

namespace StardustSandbox.Core.UI.Common.Tools
{
    internal sealed class KeySelectorUI : UIBase
    {
        private KeySelectorSettings settings;

        private Text message;

        private readonly GameWindow gameWindow;
        private readonly InputController inputController;
        private readonly UIManager uiManager;

        internal KeySelectorUI(
            GameWindow gameWindow,
            InputController inputController,
            UIManager uiManager
        ) : base()
        {
            this.gameWindow = gameWindow;
            this.inputController = inputController;
            this.uiManager = uiManager;
        }

        internal void Configure(in KeySelectorSettings settings)
        {
            this.settings = settings;
            this.message.TextContent = settings.Synopsis;
        }

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildMessage(root);
        }

        private static void BuildBackground(Container root)
        {
            Image background = new()
            {
                TextureIndex = TextureIndex.Pixel,
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
            this.settings.OnSelectedKey?.Invoke(inputKeyEventArgs.Key);
        }
    }
}

