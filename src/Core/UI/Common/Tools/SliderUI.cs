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
using StardustSandbox.Core.InputSystem;
using StardustSandbox.Core.Localization;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.UI.Elements;
using StardustSandbox.Core.UI.Information;
using StardustSandbox.Core.UI.Settings;

namespace StardustSandbox.Core.UI.Common.Tools
{
    internal sealed class SliderUI : UIBase
    {
        private Text synopsis;
        private Label valueLabel;
        private Image sliderBackground, sliderButton;

        private int currentValue, maximumValue, minimumValue;

        private SliderSettings settings;

        private readonly Label[] menuButtonLabels;
        private readonly ButtonInfo[] menuButtonInfos;

        internal SliderUI(
            UIManager uiManager
        ) : base()
        {
            this.menuButtonInfos = [
                new(TextureIndex.None, null, Localization_Statements.Cancel, string.Empty, () =>
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Returning);
                    uiManager.CloseUI();
                }),
                new(TextureIndex.None, null, Localization_Statements.Send, string.Empty, () =>
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Accepted);
                    uiManager.CloseUI();
                    this.settings.OnSendCallback?.Invoke(this.currentValue);
                }),
            ];

            this.menuButtonLabels = new Label[this.menuButtonInfos.Length];
        }

        internal void Configure(in SliderSettings settings)
        {
            this.settings = settings;

            this.synopsis.TextContent = settings.Synopsis;
            this.valueLabel.TextContent = settings.CurrentValue.ToString();

            this.currentValue = settings.CurrentValue;
            this.minimumValue = settings.MinimumValue;
            this.maximumValue = settings.MaximumValue;

            UpdateSliderButtonPosition();
        }

        protected override void OnBuild(Container root)
        {
            // Shadow
            root.AddChild(new Image()
            {
                TextureIndex = TextureIndex.Pixel,
                Scale = GameScreen.GetViewport(),
                Color = new(AAP64ColorPalette.DarkGray, 160),
                Size = Vector2.One,
            });

            BuildSynopsis(root);
            BuildSlider(root);
            BuildMenuButtons(root);
        }

        private void BuildSynopsis(Container root)
        {
            this.synopsis = new()
            {
                Scale = new(0.1f),
                Margin = new(0.0f, 128.0f),
                LineHeight = 1.25f,
                TextAreaSize = new(850.0f, 1000.0f),
                SpriteFontIndex = SpriteFontIndex.PixelOperator,
                Alignment = UIDirection.North,
            };

            root.AddChild(this.synopsis);
        }

        private void BuildSlider(Container root)
        {
            this.sliderBackground = new()
            {
                TextureIndex = TextureIndex.UISliderInputOrnament,
                SourceRectangle = new(0, 0, 630, 32),
                Size = new(630.0f, 32.0f),
                Alignment = UIDirection.Center,
            };

            this.sliderButton = new()
            {
                TextureIndex = TextureIndex.UIButtons,
                SourceRectangle = new(320, 172, 32, 32),
                Size = new(32.0f, 32.0f),
            };

            this.valueLabel = new()
            {
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.125f),
                Margin = new(0.0f, 48.0f),
                Alignment = UIDirection.Center,
                TextContent = this.currentValue.ToString(),
            };

            root.AddChild(this.valueLabel);
            root.AddChild(this.sliderBackground);

            this.sliderBackground.AddChild(this.sliderButton);
        }

        private void BuildMenuButtons(Container root)
        {
            float marginY = -48.0f;

            for (int i = 0; i < this.menuButtonInfos.Length; i++)
            {
                ButtonInfo button = this.menuButtonInfos[i];

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

                marginY -= 72;

                root.AddChild(label);

                this.menuButtonLabels[i] = label;
            }
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            UpdateMenuButtons();
            UpdateSliderButton();
            UpdateSliderButtonPosition();
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtonInfos.Length; i++)
            {
                Label label = this.menuButtonLabels[i];

                if (Interaction.OnMouseLeftClick(label))
                {
                    this.menuButtonInfos[i].ClickAction?.Invoke();
                    break;
                }

                label.Color = Interaction.OnMouseOver(label) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        private void UpdateSliderButton()
        {
            if (Interaction.OnMouseLeftDown(this.sliderBackground) || Interaction.OnMouseLeftDown(this.sliderButton))
            {
                Vector2 mousePosition = InputEngine.GetMousePosition();
                Vector2 sliderPosition = this.sliderBackground.Position;

                float relativeX = MathHelper.Clamp(mousePosition.X - sliderPosition.X, 0.0f, this.sliderBackground.Size.X);
                float percentage = relativeX / this.sliderBackground.Size.X;
                int newValue = this.minimumValue + (int)((this.maximumValue - this.minimumValue) * percentage);

                if (newValue != this.currentValue)
                {
                    this.currentValue = newValue;
                    this.valueLabel.TextContent = this.currentValue.ToString();
                }
            }
        }

        private void UpdateSliderButtonPosition()
        {
            float percentage = (this.currentValue - this.minimumValue) / (float)(this.maximumValue - this.minimumValue);
            float buttonX = percentage * this.sliderBackground.Size.X;
            this.sliderButton.Margin = new(buttonX - (this.sliderButton.Size.X / 2), 0.0f);
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

