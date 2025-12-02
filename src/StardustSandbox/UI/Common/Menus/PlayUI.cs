using Microsoft.Xna.Framework;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.UI;
using StardustSandbox.Managers;
using StardustSandbox.UI.Elements;
using StardustSandbox.UI.Information;

namespace StardustSandbox.UI.Common.Menus
{
    internal class PlayUI : UIBase
    {
        private readonly Label[] menuButtonLabels;
        private readonly ButtonInfo[] menuButtonInfos;

        private readonly UIManager uiManager;

        internal PlayUI(
            UIIndex index,
            UIManager uiManager
        ) : base(index)
        {
            this.uiManager = uiManager;

            this.menuButtonInfos = [
                new(TextureIndex.IconUI, new(0, 32, 32, 32), "Worlds", string.Empty, () => this.uiManager.OpenGUI(UIIndex.WorldExplorerMenu)),
                new(TextureIndex.IconUI, new(224, 0, 32, 32), "Return", string.Empty, this.uiManager.CloseGUI),
            ];

            this.menuButtonLabels = new Label[this.menuButtonInfos.Length];
        }

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
                Scale = new(ScreenConstants.SCREEN_WIDTH, 128.0f),
            };

            Label title = new()
            {
                Scale = new(0.2f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Alignment = CardinalDirection.Center,
                TextContent = "Play Menu",

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 2.0f,
                BorderThickness = 2.0f,
            };

            background.AddChild(title);
            root.AddChild(background);
        }

        private void BuildMenuButtons(Container root)
        {
            Vector2 margin = Vector2.Zero;

            for (byte i = 0; i < this.menuButtonInfos.Length; i++)
            {
                ButtonInfo button = this.menuButtonInfos[i];

                Label buttonLabel = new()
                {
                    Scale = new(0.15f),
                    Margin = margin,
                    SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                    Alignment = CardinalDirection.Center,
                    TextContent = button.Name,

                    BorderColor = AAP64ColorPalette.DarkGray,
                    BorderDirections = LabelBorderDirection.All,
                    BorderOffset = 2.0f,
                    BorderThickness = 2.0f,
                };

                Image buttonIcon = new()
                {
                    Texture = button.Texture,
                    SourceRectangle = button.TextureSourceRectangle,
                    Alignment = CardinalDirection.West,
                    Margin = new((buttonLabel.Size.X + (button.Texture.Width / 2.0f)) * -1.0f, 0.0f),
                    Scale = new(2),
                };

                buttonLabel.AddChild(buttonIcon);
                root.AddChild(buttonLabel);

                margin.Y += buttonLabel.Size.X + 32.0f;

                this.menuButtonLabels[i] = buttonLabel;
            }
        }

        #endregion

        internal override void Update(GameTime gameTime)
        {
            for (byte i = 0; i < this.menuButtonLabels.Length; i++)
            {
                Label label = this.menuButtonLabels[i];

                if (Interaction.OnMouseLeftClick(label))
                {
                    this.menuButtonInfos[i].ClickAction?.Invoke();
                }

                label.Color = Interaction.OnMouseOver(label) ? AAP64ColorPalette.LemonYellow : AAP64ColorPalette.White;
            }
        }
    }
}
