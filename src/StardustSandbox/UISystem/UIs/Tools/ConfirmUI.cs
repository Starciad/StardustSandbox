using Microsoft.Xna.Framework;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.UISystem;
using StardustSandbox.Enums.UISystem.Tools;
using StardustSandbox.LocalizationSystem;
using StardustSandbox.Managers;
using StardustSandbox.UISystem.Elements;
using StardustSandbox.UISystem.Information;
using StardustSandbox.UISystem.Settings;

namespace StardustSandbox.UISystem.UIs.Tools
{
    internal sealed class ConfirmUI : UI
    {
        private ConfirmSettings confirmSettings;
        private Text captionElement;
        private Text messageElement;

        private readonly Label[] menuButtonElements;

        private readonly ButtonInfo[] menuButtons;

        private readonly UIManager uiManager;

        internal ConfirmUI(
            UIIndex index,
            UIManager uiManager
        ) : base(index)
        {
            this.uiManager = uiManager;

            this.menuButtons = [
                new(null, null, Localization_Statements.Cancel, string.Empty, CancelButtonAction),
                new(null, null, Localization_Statements.Confirm, string.Empty, ConfirmButtonAction),
            ];

            this.menuButtonElements = new Label[this.menuButtons.Length];
        }

        #region INITIALIZE

        internal void Configure(ConfirmSettings settings)
        {
            this.confirmSettings = settings;

            this.captionElement.TextContent = settings.Caption;
            this.messageElement.TextContent = settings.Message;
        }

        #endregion

        #region ACTIONS

        private void CancelButtonAction()
        {
            this.uiManager.CloseGUI();
            this.confirmSettings?.OnConfirmCallback?.Invoke(ConfirmStatus.Cancelled);
        }

        private void ConfirmButtonAction()
        {
            this.uiManager.CloseGUI();
            this.confirmSettings?.OnConfirmCallback?.Invoke(ConfirmStatus.Confirmed);
        }

        #endregion

        #region BUILDER

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildCaption(root);
            BuildMessage(root);
            BuildMenuButtons(root);
        }

        private static void BuildBackground(Container root)
        {
            Image guiBackground = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Scale = new(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT),
                Size = ScreenConstants.SCREEN_DIMENSIONS.ToVector2(),
                Color = new(AAP64ColorPalette.DarkGray, 160)
            };

            root.AddChild(guiBackground);
        }

        private void BuildCaption(Container root)
        {
            this.captionElement = new()
            {
                Scale = new(0.1f),
                Margin = new(0, 96),
                LineHeight = 1.25f,
                TextAreaSize = new(850, 1000),
                SpriteFontIndex = SpriteFontIndex.PixelOperator,
                Alignment = CardinalDirection.North,
                TextContent = "Caption"
            };

            root.AddChild(this.captionElement);
        }

        private void BuildMessage(Container root)
        {
            this.messageElement = new()
            {
                Scale = new(0.1f),
                Margin = new(0, -128),
                LineHeight = 1.25f,
                TextAreaSize = new(850, 1000),
                SpriteFontIndex = SpriteFontIndex.PixelOperator,
                Alignment = CardinalDirection.Center,
                TextContent = "Message",
            };

            root.AddChild(this.messageElement);
        }

        private void BuildMenuButtons(Container root)
        {
            Vector2 margin = new(0, -64);

            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                ButtonInfo button = this.menuButtons[i];

                Label label = new()
                {
                    SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                    Scale = new(0.125f),
                    Margin = margin,
                    Alignment = CardinalDirection.South,
                    TextContent = button.Name,

                    BorderColor = AAP64ColorPalette.DarkGray,
                    BorderDirections = LabelBorderDirection.All,
                    BorderOffset = 2f,
                    BorderThickness = 2f,
                };

                margin.Y -= 72;

                root.AddChild(label);

                this.menuButtonElements[i] = label;
            }
        }

        #endregion

        #region UPDATING

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateMenuButtons();
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                Label label = this.menuButtonElements[i];

                Vector2 size = label.MeasuredText / 2.0f;
                Vector2 position = label.Position;

                if (Interaction.OnMouseClick(position, size))
                {
                    this.menuButtons[i].ClickAction?.Invoke();
                }

                label.Color = Interaction.OnMouseOver(position, size) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        #endregion
    }
}
