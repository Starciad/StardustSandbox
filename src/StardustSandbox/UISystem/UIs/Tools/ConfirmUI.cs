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
using StardustSandbox.UISystem.Elements.Graphics;
using StardustSandbox.UISystem.Elements.Textual;
using StardustSandbox.UISystem.Settings;
using StardustSandbox.UISystem.Utilities;

namespace StardustSandbox.UISystem.UIs.Tools
{
    internal sealed class ConfirmUI : UI
    {
        private ConfirmSettings confirmSettings;
        private TextUIElement captionElement;
        private TextUIElement messageElement;

        private readonly LabelUIElement[] menuButtonElements;

        private readonly UIButton[] menuButtons;

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

            this.menuButtonElements = new LabelUIElement[this.menuButtons.Length];
        }

        #region INITIALIZE

        internal void Configure(ConfirmSettings settings)
        {
            this.confirmSettings = settings;

            this.captionElement.SetTextualContent(settings.Caption);
            this.messageElement.SetTextualContent(settings.Message);
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

        protected override void OnBuild(Layout layout)
        {
            BuildBackground(layout);
            BuildCaption(layout);
            BuildMessage(layout);
            BuildMenuButtons(layout);
        }

        private void BuildBackground(Layout layout)
        {
            ImageUIElement guiBackground = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Scale = new(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT),
                Size = ScreenConstants.SCREEN_DIMENSIONS.ToVector2(),
                Color = new(AAP64ColorPalette.DarkGray, 160)
            };

            layout.AddElement(guiBackground);
        }

        private void BuildCaption(Layout layout)
        {
            this.captionElement = new()
            {
                Scale = new(0.1f),
                Margin = new(0, 96),
                LineHeight = 1.25f,
                TextAreaSize = new(850, 1000),
                SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.PixelOperator),
                PositionAnchor = CardinalDirection.North,
                OriginPivot = CardinalDirection.Center,
            };

            this.captionElement.SetTextualContent("Caption");
            this.captionElement.PositionRelativeToScreen();

            layout.AddElement(this.captionElement);
        }

        private void BuildMessage(Layout layout)
        {
            this.messageElement = new()
            {
                Scale = new(0.1f),
                Margin = new(0, -128),
                LineHeight = 1.25f,
                TextAreaSize = new(850, 1000),
                SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.PixelOperator),
                PositionAnchor = CardinalDirection.Center,
                OriginPivot = CardinalDirection.Center,
            };

            this.messageElement.SetTextualContent("Message");
            this.messageElement.PositionRelativeToScreen();

            layout.AddElement(this.messageElement);
        }

        private void BuildMenuButtons(Layout layout)
        {
            Vector2 margin = new(0, -64);

            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                UIButton button = this.menuButtons[i];

                LabelUIElement labelElement = new()
                {
                    SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm),
                    Scale = new(0.125f),
                    Margin = margin,
                    PositionAnchor = CardinalDirection.South,
                    OriginPivot = CardinalDirection.Center,
                };

                labelElement.SetTextualContent(button.Name);
                labelElement.PositionRelativeToScreen();
                labelElement.SetAllBorders(true, AAP64ColorPalette.DarkGray, new(2));

                margin.Y -= 72;

                layout.AddElement(labelElement);

                this.menuButtonElements[i] = labelElement;
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
                LabelUIElement labelElement = this.menuButtonElements[i];

                Vector2 size = labelElement.GetStringSize() / 2.0f;
                Vector2 position = labelElement.Position;

                if (UIInteraction.OnMouseClick(position, size))
                {
                    this.menuButtons[i].ClickAction?.Invoke();
                }

                labelElement.Color = UIInteraction.OnMouseOver(position, size) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        #endregion
    }
}
