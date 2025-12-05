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
using StardustSandbox.UI.Common.Tools;
using StardustSandbox.UI.Elements;
using StardustSandbox.UI.Information;
using StardustSandbox.UI.Settings;

namespace StardustSandbox.UI.Common.HUD
{
    internal sealed class PauseUI : UIBase
    {
        private Image background;
        private Label menuTitle;

        private readonly ButtonInfo[] menuButtonInfos;
        private readonly SlotInfo[] menuButtonSlotInfos;

        private readonly ConfirmUI confirmUI;
        private readonly ConfirmSettings exitConfirmSettings;

        private readonly GameManager gameManager;
        private readonly UIManager uiManager;

        internal PauseUI(
            ConfirmUI confirmUI,
            GameManager gameManager,
            UIIndex index,
            UIManager uiManager
        ) : base(index)
        {
            this.confirmUI = confirmUI;
            this.gameManager = gameManager;
            this.uiManager = uiManager;

            this.exitConfirmSettings = new()
            {
                Caption = Localization_Messages.Confirm_Simulation_Exit_Title,
                Message = Localization_Messages.Confirm_Simulation_Exit_Description,
                OnConfirmCallback = status =>
                {
                    if (status == ConfirmStatus.Confirmed)
                    {
                        uiManager.Reset();
                        uiManager.OpenGUI(UIIndex.MainMenu);
                    }
                }
            };

            this.menuButtonInfos = [
                new(TextureIndex.None, null, Localization_Statements.Resume, string.Empty, this.uiManager.CloseGUI),
                new(TextureIndex.None, null, Localization_Statements.Options, string.Empty, () =>
                {
                    this.uiManager.OpenGUI(UIIndex.OptionsMenu);
                    this.gameManager.SetState(GameStates.IsCriticalMenuOpen);
                }),
                new(TextureIndex.None, null, Localization_Statements.Exit, string.Empty, () =>
                {
                    this.confirmUI.Configure(this.exitConfirmSettings);
                    this.uiManager.OpenGUI(UIIndex.Confirm);
                    this.gameManager.SetState(GameStates.IsCriticalMenuOpen);
                }),
            ];

            this.menuButtonSlotInfos = new SlotInfo[this.menuButtonInfos.Length];
        }

        #region BUILDERS

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildTitle();
            BuildMenuButtons();
        }

        private void BuildBackground(Container root)
        {
            Image shadow = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Scale = new(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT),
                Color = new(AAP64ColorPalette.DarkGray, 160),
                Size = Vector2.One,
            };

            this.background = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.UIBackgroundPause),
                Size = new(542.0f, 540.0f),
                Alignment = CardinalDirection.Center,
            };

            root.AddChild(shadow);
            root.AddChild(this.background);
        }

        private void BuildTitle()
        {
            this.menuTitle = new()
            {
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.12f),
                Alignment = CardinalDirection.North,
                Margin = new(0.0f, 10.0f),
                Color = AAP64ColorPalette.White,
                TextContent = Localization_GUIs.HUD_Complements_Pause_Title,

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 3.0f,
                BorderThickness = 3.0f
            };

            this.background.AddChild(this.menuTitle);
        }

        private void BuildMenuButtons()
        {
            float marginY = 118.0f;

            for (int i = 0; i < this.menuButtonInfos.Length; i++)
            {
                ButtonInfo button = this.menuButtonInfos[i];

                Image background = new()
                {
                    Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                    SourceRectangle = new(0, 140, 320, 80),
                    Color = AAP64ColorPalette.PurpleGray,
                    Size = new(320.0f, 80.0f),
                    Margin = new(0.0f, marginY),
                    Alignment = CardinalDirection.North,
                };

                Label label = new()
                {
                    Scale = new(0.1f),
                    Color = AAP64ColorPalette.White,
                    SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                    Alignment = CardinalDirection.Center,
                    TextContent = button.Name,

                    BorderColor = AAP64ColorPalette.DarkGray,
                    BorderDirections = LabelBorderDirection.All,
                    BorderOffset = 2.0f,
                    BorderThickness = 2.0f,
                };

                this.background.AddChild(background);
                background.AddChild(label);

                this.menuButtonSlotInfos[i] = new(background, null, label);

                marginY += background.Size.Y + 32.0f;
            }
        }

        #endregion

        #region UPDATING

        internal override void Update(in GameTime gameTime)
        {
            UpdateMenuButtons();

            base.Update(gameTime);
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtonInfos.Length; i++)
            {
                SlotInfo slot = this.menuButtonSlotInfos[i];

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    this.menuButtonInfos[i].ClickAction?.Invoke();
                }

                slot.Background.Color = Interaction.OnMouseOver(slot.Background) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        #endregion

        #region EVENTS

        protected override void OnOpened()
        {
            this.gameManager.SetState(GameStates.IsCriticalMenuOpen);
        }

        protected override void OnClosed()
        {
            this.gameManager.RemoveState(GameStates.IsCriticalMenuOpen);
        }

        #endregion
    }
}
