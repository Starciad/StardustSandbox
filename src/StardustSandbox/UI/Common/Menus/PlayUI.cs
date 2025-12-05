using Microsoft.Xna.Framework;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.UI;
using StardustSandbox.Localization;
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
                new(TextureIndex.IconUI, new(0, 32, 32, 32), Localization_Statements.Worlds, string.Empty, () => this.uiManager.OpenGUI(UIIndex.WorldExplorerMenu)),
                new(TextureIndex.IconUI, new(224, 0, 32, 32), Localization_Statements.Return, string.Empty, this.uiManager.CloseGUI),
            ];

            this.menuButtonLabels = new Label[this.menuButtonInfos.Length];
        }

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
                Scale = new(ScreenConstants.SCREEN_WIDTH, 128.0f),
                Size = Vector2.One,
            };

            Label title = new()
            {
                Scale = new(0.2f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Alignment = CardinalDirection.Center,
                TextContent = Localization_GUIs.Menu_Play_Title,

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
            float marginY = 0.0f;

            for (int i = 0; i < this.menuButtonInfos.Length; i++)
            {
                ButtonInfo button = this.menuButtonInfos[i];

                Label label = new()
                {
                    Scale = new(0.15f),
                    Margin = new(0.0f, marginY),
                    SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                    Alignment = CardinalDirection.Center,
                    TextContent = button.Name,

                    BorderColor = AAP64ColorPalette.DarkGray,
                    BorderDirections = LabelBorderDirection.All,
                    BorderOffset = 2.0f,
                    BorderThickness = 2.0f,
                };

                Image icon = new()
                {
                    Texture = button.Texture,
                    SourceRectangle = button.TextureSourceRectangle,
                    Margin = new(-96.0f, 0.0f),
                    Scale = new(2),
                };

                label.AddChild(icon);
                root.AddChild(label);

                marginY += label.Size.Y + 64.0f;

                this.menuButtonLabels[i] = label;
            }
        }

        internal override void Update(in GameTime gameTime)
        {
            for (int i = 0; i < this.menuButtonLabels.Length; i++)
            {
                Label label = this.menuButtonLabels[i];

                if (Interaction.OnMouseLeftClick(label))
                {
                    this.menuButtonInfos[i].ClickAction?.Invoke();
                }

                label.Color = Interaction.OnMouseOver(label) ? AAP64ColorPalette.LemonYellow : AAP64ColorPalette.White;
            }

            base.Update(gameTime);
        }
    }
}
