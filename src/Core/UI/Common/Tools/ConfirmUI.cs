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
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Enums.States;
using StardustSandbox.Core.Enums.UI;
using StardustSandbox.Core.Enums.UI.Tools;
using StardustSandbox.Core.Localization;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.UI.Elements;
using StardustSandbox.Core.UI.Information;
using StardustSandbox.Core.UI.Settings;

namespace StardustSandbox.Core.UI.Common.Tools
{
    internal sealed class ConfirmUI : UIBase
    {
        private ConfirmSettings confirmSettings;

        private Image shadowBackground;
        private Label caption;
        private Text message;

        private readonly Label[] buttonLabels;
        private readonly ButtonInfo[] buttonInfos;

        private readonly UIManager uiManager;

        internal ConfirmUI(
            UIManager uiManager
        ) : base()
        {
            this.uiManager = uiManager;

            this.buttonInfos = [
                new(TextureIndex.None, null, Localization_Statements.Cancel, string.Empty, () =>
                {
                    this.uiManager.CloseUI();
                    this.confirmSettings.OnConfirmCallback?.Invoke(ConfirmStatus.Cancelled);
                }),
                new(TextureIndex.None, null, Localization_Statements.Confirm, string.Empty, () =>
                {
                    this.uiManager.CloseUI();
                    this.confirmSettings.OnConfirmCallback?.Invoke(ConfirmStatus.Confirmed);
                }),
            ];

            this.buttonLabels = new Label[this.buttonInfos.Length];
        }

        internal void Configure(in ConfirmSettings settings)
        {
            this.confirmSettings = settings;

            this.caption.TextContent = settings.Caption;
            this.message.TextContent = settings.Message;
        }

        protected override void OnBuild(Container root)
        {
            this.shadowBackground = new()
            {
                TextureIndex = TextureIndex.Pixel,
                Scale = GameScreen.GetViewport(),
                Size = Vector2.One,
                Color = new(AAP64ColorPalette.DarkGray, 160)
            };

            this.caption = new()
            {
                Scale = new(0.1f),
                Margin = new(0.0f, 64.0f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Alignment = UIDirection.North,

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 2.0f,
                BorderThickness = 2.0f,
            };

            this.message = new()
            {
                Scale = new(0.1f),
                LineHeight = 1.25f,
                Margin = new(0.0f, -32.0f),
                TextAreaSize = new(850.0f, 1000.0f),
                SpriteFontIndex = SpriteFontIndex.PixelOperator,
                Alignment = UIDirection.Center,
            };

            root.AddChild(this.shadowBackground);
            root.AddChild(this.caption);
            root.AddChild(this.message);

            BuildMenuButtons(root);
        }

        private void BuildMenuButtons(Container root)
        {
            float marginY = -64.0f;

            for (int i = 0; i < this.buttonInfos.Length; i++)
            {
                ButtonInfo button = this.buttonInfos[i];

                Label label = new()
                {
                    SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                    Scale = new(0.125f),
                    Margin = new(0.0f, marginY),
                    Alignment = UIDirection.South,
                    TextContent = button.Name,

                    BorderColor = AAP64ColorPalette.DarkGray,
                    BorderDirections = LabelBorderDirection.All,
                    BorderOffset = 2.0f,
                    BorderThickness = 2.0f,
                };

                marginY -= 72.0f;

                root.AddChild(label);

                this.buttonLabels[i] = label;
            }
        }

        protected override void OnResize(Vector2 size)
        {
            this.shadowBackground.Scale = size;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            for (int i = 0; i < this.buttonInfos.Length; i++)
            {
                Label label = this.buttonLabels[i];

                if (Interaction.OnMouseEnter(label))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(label))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Click);
                    this.buttonInfos[i].ClickAction?.Invoke();
                    break;
                }

                label.Color = Interaction.OnMouseOver(label) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
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

