using Microsoft.Xna.Framework;

using StardustSandbox.Audio;
using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UI;
using StardustSandbox.InputSystem;
using StardustSandbox.Localization;
using StardustSandbox.Managers;
using StardustSandbox.UI.Elements;
using StardustSandbox.UI.Information;
using StardustSandbox.UI.Settings;

namespace StardustSandbox.UI.Common.Tools
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
            UIIndex index,
            UIManager uiManager
        ) : base(index)
        {
            this.menuButtonInfos = [
                new(TextureIndex.None, null, Localization_Statements.Cancel, string.Empty, () =>
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Returning);
                    uiManager.CloseGUI();
                }),
                new(TextureIndex.None, null, Localization_Statements.Send, string.Empty, () =>
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Accepted);
                    uiManager.CloseGUI();
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
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Scale = new(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT),
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
                Texture = AssetDatabase.GetTexture(TextureIndex.UISliderInputOrnament),
                SourceRectangle = new(0, 0, 630, 32),
                Size = new(630.0f, 32.0f),
                Alignment = UIDirection.Center,
            };

            this.sliderButton = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
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

        internal override void Update(GameTime gameTime)
        {
            UpdateMenuButtons();
            UpdateSliderButton();
            UpdateSliderButtonPosition();

            base.Update(gameTime);
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
                Vector2 mousePosition = Input.GetScaledMousePosition();
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
