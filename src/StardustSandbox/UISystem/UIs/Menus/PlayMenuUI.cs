using Microsoft.Xna.Framework;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.UISystem;
using StardustSandbox.Managers;
using StardustSandbox.UISystem.Elements;
using StardustSandbox.UISystem.Information;

namespace StardustSandbox.UISystem.UIs.Menus
{
    internal class PlayMenuUI : UI
    {
        private readonly Label[] menuButtonElements;
        private readonly ButtonInfo[] menuButtons;

        private readonly UIManager uiManager;

        internal PlayMenuUI(
            UIIndex index,
            UIManager uiManager
        ) : base(index)
        {
            this.uiManager = uiManager;

            this.menuButtons = [
                new(AssetDatabase.GetTexture(TextureIndex.IconUi), new(0, 32, 32, 32), "Worlds", string.Empty, WorldsButtonAction),
                new(AssetDatabase.GetTexture(TextureIndex.IconUi), new(224, 0, 32, 32), "Return", string.Empty, ReturnButtonAction),
            ];

            this.menuButtonElements = new Label[this.menuButtons.Length];
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

        protected override void OnBuild(Container root)
        {
            BuildTitle(root);
            BuildMenuButtons(root);
        }

        private static void BuildTitle(Container root)
        {
            Image background = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Color = new(AAP64ColorPalette.DarkGray, 196),
                Size = Vector2.One,
                Scale = new(ScreenConstants.SCREEN_WIDTH, 128f),
            };

            Label title = new()
            {
                Scale = new(0.2f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Alignment = CardinalDirection.Center,
                TextContent = "Play Menu",

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 2f,
                BorderThickness = 2f,
            };

            background.AddChild(title);
            root.AddChild(background);
        }

        private void BuildMenuButtons(Container root)
        {
            Vector2 margin = Vector2.Zero;

            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                ButtonInfo button = this.menuButtons[i];

                Label buttonLabel = new()
                {
                    Scale = new(0.15f),
                    Margin = margin,
                    SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                    Alignment = CardinalDirection.Center,
                    TextContent = button.Name,

                    BorderColor = AAP64ColorPalette.DarkGray,
                    BorderDirections = LabelBorderDirection.All,
                    BorderOffset = 2f,
                    BorderThickness = 2f,
                };

                Image buttonIcon = new()
                {
                    Texture = button.IconTexture,
                    Alignment = CardinalDirection.West,
                    Margin = new((buttonLabel.MeasuredText.X + (button.IconTexture.Width / 2.0f)) * -1.0f, 0.0f),
                    Scale = new(2),
                };

                buttonLabel.AddChild(buttonIcon);
                root.AddChild(buttonLabel);

                margin.Y += buttonLabel.MeasuredText.X + 32.0f;

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
                Label label = this.menuButtonElements[i];
                Vector2 labelElementSize = label.MeasuredText / 2.0f;

                if (Interaction.OnMouseClick(label.Position, labelElementSize))
                {
                    this.menuButtons[i].ClickAction?.Invoke();
                }

                label.Color = Interaction.OnMouseOver(label.Position, labelElementSize) ? AAP64ColorPalette.LemonYellow : AAP64ColorPalette.White;
            }
        }

        #endregion
    }
}
