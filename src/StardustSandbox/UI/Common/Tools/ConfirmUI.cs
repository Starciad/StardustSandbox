using Microsoft.Xna.Framework;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.UI;
using StardustSandbox.Enums.UI.Tools;
using StardustSandbox.LocalizationSystem;
using StardustSandbox.Managers;
using StardustSandbox.UI.Elements;
using StardustSandbox.UI.Information;
using StardustSandbox.UI.Settings;

namespace StardustSandbox.UI.Common.Tools
{
    internal sealed class ConfirmUI : UIBase
    {
        private ConfirmSettings confirmSettings;
        private Text caption, message;

        private readonly Label[] buttonLabels;
        private readonly ButtonInfo[] buttonInfos;

        private readonly UIManager uiManager;

        internal ConfirmUI(
            UIIndex index,
            UIManager uiManager
        ) : base(index)
        {
            this.uiManager = uiManager;

            this.buttonInfos = [
                new(TextureIndex.None, null, Localization_Statements.Cancel, string.Empty, () =>
                {
                    this.uiManager.CloseGUI();
                    this.confirmSettings?.OnConfirmCallback?.Invoke(ConfirmStatus.Cancelled);
                }),
                new(TextureIndex.None, null, Localization_Statements.Confirm, string.Empty, () =>
                {
                    this.uiManager.CloseGUI();
                    this.confirmSettings?.OnConfirmCallback?.Invoke(ConfirmStatus.Confirmed);
                }),
            ];

            this.buttonLabels = new Label[this.buttonInfos.Length];
        }

        internal void Configure(ConfirmSettings settings)
        {
            this.confirmSettings = settings;

            this.caption.TextContent = settings.Caption;
            this.message.TextContent = settings.Message;
        }

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
            this.caption = new()
            {
                Scale = new(0.1f),
                Margin = new(0.0f, 96.0f),
                LineHeight = 1.25f,
                TextAreaSize = new(850.0f, 1000.0f),
                SpriteFontIndex = SpriteFontIndex.PixelOperator,
                Alignment = CardinalDirection.North,
                TextContent = "Caption"
            };

            root.AddChild(this.caption);
        }

        private void BuildMessage(Container root)
        {
            this.message = new()
            {
                Scale = new(0.1f),
                Margin = new(0.0f, -128.0f),
                LineHeight = 1.25f,
                TextAreaSize = new(850.0f, 1000.0f),
                SpriteFontIndex = SpriteFontIndex.PixelOperator,
                Alignment = CardinalDirection.Center,
                TextContent = "Message",
            };

            root.AddChild(this.message);
        }

        private void BuildMenuButtons(Container root)
        {
            float marginY = -64.0f;

            for (byte i = 0; i < this.buttonInfos.Length; i++)
            {
                ButtonInfo button = this.buttonInfos[i];

                Label label = new()
                {
                    SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                    Scale = new(0.125f),
                    Margin = new(0.0f, marginY),
                    Alignment = CardinalDirection.South,
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

        #endregion

        #region UPDATING

        internal override void Update(GameTime gameTime)
        {
            for (byte i = 0; i < this.buttonInfos.Length; i++)
            {
                Label label = this.buttonLabels[i];

                if (Interaction.OnMouseLeftClick(label))
                {
                    this.buttonInfos[i].ClickAction?.Invoke();
                }

                label.Color = Interaction.OnMouseOver(label) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }

            base.Update(gameTime);
        }

        #endregion
    }
}
