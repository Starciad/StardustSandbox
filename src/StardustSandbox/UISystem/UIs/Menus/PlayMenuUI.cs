using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.UISystem;
using StardustSandbox.Managers;
using StardustSandbox.UISystem.Elements.Graphics;
using StardustSandbox.UISystem.Elements.Textual;
using StardustSandbox.UISystem.Utilities;

namespace StardustSandbox.UISystem.UIs.Menus
{
    internal class PlayMenuUI : UI
    {
        private readonly LabelUIElement[] menuButtonElements;

        private readonly Texture2D particleTexture;
        private readonly Texture2D returnIconTexture;
        private readonly Texture2D worldIconTexture;
        private readonly SpriteFont bigApple3PMSpriteFont;

        private readonly UIButton[] menuButtons;

        private readonly UIManager uiManager;

        internal PlayMenuUI(
            UIIndex index,
            UIManager uiManager
        ) : base(index)
        {
            this.uiManager = uiManager;

            this.particleTexture = AssetDatabase.GetTexture("texture_particle_1");
            this.returnIconTexture = AssetDatabase.GetTexture("texture_icon_gui_16");
            this.worldIconTexture = AssetDatabase.GetTexture("texture_icon_gui_17");
            this.bigApple3PMSpriteFont = AssetDatabase.GetSpriteFont("font_2");

            this.menuButtons = [
                new(this.worldIconTexture, "Worlds", string.Empty, WorldsButtonAction),
                new(this.returnIconTexture, "Return", string.Empty, ReturnButtonAction),
            ];

            this.menuButtonElements = new LabelUIElement[this.menuButtons.Length];
        }

        #region ACTIONS

        private void WorldsButtonAction()
        {
            this.uiManager.OpenGUI(UIIndex.WorldExplorerMenu);
        }

        private void ReturnButtonAction()
        {
            this.uiManager.CloseGUI();
        }

        #endregion

        #region BUILDER

        protected override void OnBuild(Layout layout)
        {
            BuildTitle(layout);
            BuildMenuButtons(layout);
        }

        private void BuildTitle(Layout layout)
        {
            ImageUIElement backgroundImage = new()
            {
                Texture = this.particleTexture,
                Color = new(AAP64ColorPalette.DarkGray, 196),
                Size = Vector2.One,
                Scale = new(ScreenConstants.DEFAULT_SCREEN_WIDTH, 128f),
            };

            LabelUIElement titleLabel = new()
            {
                Scale = new(0.2f),
                SpriteFont = this.bigApple3PMSpriteFont,
                PositionAnchor = CardinalDirection.Center,
                OriginPivot = CardinalDirection.Center,
            };

            titleLabel.SetTextualContent("Play Menu");
            titleLabel.SetAllBorders(true, AAP64ColorPalette.DarkGray, new(2.0f));
            titleLabel.PositionRelativeToElement(backgroundImage);

            layout.AddElement(backgroundImage);
            layout.AddElement(titleLabel);
        }

        private void BuildMenuButtons(Layout layout)
        {
            Vector2 margin = Vector2.Zero;

            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                UIButton button = this.menuButtons[i];

                LabelUIElement buttonLabel = new()
                {
                    Scale = new(0.15f),
                    Margin = margin,
                    SpriteFont = this.bigApple3PMSpriteFont,
                    PositionAnchor = CardinalDirection.Center,
                    OriginPivot = CardinalDirection.Center,
                };

                buttonLabel.SetTextualContent(button.Name);
                buttonLabel.SetAllBorders(true, AAP64ColorPalette.DarkGray, new(2f));

                ImageUIElement buttonIcon = new()
                {
                    Texture = button.IconTexture,
                    PositionAnchor = CardinalDirection.West,
                    OriginPivot = CardinalDirection.Center,
                    Margin = new((buttonLabel.GetStringSize().X + (button.IconTexture.Width / 2.0f)) * -1.0f, 0.0f),
                    Scale = new(2),
                };

                buttonLabel.PositionRelativeToScreen();
                buttonIcon.PositionRelativeToElement(buttonLabel);

                layout.AddElement(buttonLabel);
                layout.AddElement(buttonIcon);

                margin.Y += buttonLabel.GetStringSize().X + 32.0f;

                this.menuButtonElements[i] = buttonLabel;
            }
        }

        #endregion

        #region UPDATING

        internal override void Update(GameTime gameTime)
        {
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            for (int i = 0; i < this.menuButtonElements.Length; i++)
            {
                LabelUIElement labelElement = this.menuButtonElements[i];
                Vector2 labelElementSize = labelElement.GetStringSize() / 2.0f;

                if (UIInteraction.OnMouseClick(labelElement.Position, labelElementSize))
                {
                    this.menuButtons[i].ClickAction?.Invoke();
                }

                labelElement.Color = UIInteraction.OnMouseOver(labelElement.Position, labelElementSize) ? AAP64ColorPalette.LemonYellow : AAP64ColorPalette.White;
            }
        }

        #endregion
    }
}
