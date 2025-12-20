using Microsoft.Xna.Framework;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UI;
using StardustSandbox.Enums.UI.Tools;
using StardustSandbox.Localization;
using StardustSandbox.Managers;
using StardustSandbox.UI.Elements;
using StardustSandbox.UI.Information;
using StardustSandbox.UI.Settings;

namespace StardustSandbox.UI.Common.Tools
{
    internal sealed class ConfirmUI : UIBase
    {
        private ConfirmSettings confirmSettings;

        private Label caption;
        private Text message;

        private readonly Label[] buttonLabels;
        private readonly ButtonInfo[] buttonInfos;

        private readonly GameManager gameManager;
        private readonly UIManager uiManager;

        internal ConfirmUI(
            GameManager gameManager,
            UIIndex index,
            UIManager uiManager
        ) : base(index)
        {
            this.gameManager = gameManager;
            this.uiManager = uiManager;

            this.buttonInfos = [
                new(TextureIndex.None, null, Localization_Statements.Cancel, string.Empty, () =>
                {
                    this.uiManager.CloseGUI();
                    this.confirmSettings.OnConfirmCallback?.Invoke(ConfirmStatus.Cancelled);
                }),
                new(TextureIndex.None, null, Localization_Statements.Confirm, string.Empty, () =>
                {
                    this.uiManager.CloseGUI();
                    this.confirmSettings.OnConfirmCallback?.Invoke(ConfirmStatus.Confirmed);
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
            Image shadow = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Scale = ScreenConstants.SCREEN_DIMENSIONS.ToVector2(),
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

            root.AddChild(shadow);
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

        #endregion

        internal override void Update(GameTime gameTime)
        {
            for (int i = 0; i < this.buttonInfos.Length; i++)
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

        protected override void OnOpened()
        {
            this.gameManager.SetState(GameStates.IsCriticalMenuOpen);
        }

        protected override void OnClosed()
        {
            this.gameManager.RemoveState(GameStates.IsCriticalMenuOpen);
        }
    }
}
