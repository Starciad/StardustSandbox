using Microsoft.Xna.Framework;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UISystem;
using StardustSandbox.Enums.UISystem.Tools;
using StardustSandbox.LocalizationSystem;
using StardustSandbox.Managers;
using StardustSandbox.UISystem.Elements;
using StardustSandbox.UISystem.Information;
using StardustSandbox.UISystem.Settings;
using StardustSandbox.UISystem.UIs.Tools;

namespace StardustSandbox.UISystem.UIs.HUD
{
    internal sealed class PauseUI : UI
    {
        private Image panelBackgroundElement;
        private Label menuTitleElement;

        private readonly ButtonInfo[] menuButtons;
        private readonly SlotInfo[] menuButtonSlots;

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

            this.menuButtons = [
                new(TextureIndex.None, null, Localization_Statements.Resume, string.Empty, ResumeButtonAction),
                new(TextureIndex.None, null, Localization_Statements.Options, string.Empty, OptionsButtonAction),
                new(TextureIndex.None, null, Localization_Statements.Exit, string.Empty, ExitButtonAction),
            ];

            this.menuButtonSlots = new SlotInfo[this.menuButtons.Length];
        }

        #region ACTIONS

        private void ResumeButtonAction()
        {
            this.uiManager.CloseGUI();
        }

        private void OptionsButtonAction()
        {
            this.uiManager.OpenGUI(UIIndex.OptionsMenu);
            this.gameManager.SetState(GameStates.IsCriticalMenuOpen);
        }

        private void ExitButtonAction()
        {
            this.confirmUI.Configure(this.exitConfirmSettings);
            this.uiManager.OpenGUI(UIIndex.Confirm);
            this.gameManager.SetState(GameStates.IsCriticalMenuOpen);
        }

        #endregion

        #region BUILDERS

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildTitle();
            BuildMenuButtons();
        }

        private void BuildBackground(Container root)
        {
            Image backgroundShadowElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Scale = new(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT),
                Size = new(1),
                Color = new(AAP64ColorPalette.DarkGray, 160)
            };

            this.panelBackgroundElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.UIBackgroundPause),
                Size = new(542, 540),
                Margin = new(AssetDatabase.GetTexture(TextureIndex.UIBackgroundPause).Width / 2 * -1, 90),
                Alignment = CardinalDirection.North,
            };

            root.AddChild(backgroundShadowElement);
            root.AddChild(this.panelBackgroundElement);
        }

        private void BuildTitle()
        {
            this.menuTitleElement = new()
            {
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.12f),
                Alignment = CardinalDirection.North,
                Margin = new(0f, 40f),
                Color = AAP64ColorPalette.White,
                TextContent = Localization_GUIs.HUD_Complements_Pause_Title,

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 3f,
                BorderThickness = 3f
            };

            this.panelBackgroundElement.AddChild(this.menuTitleElement);
        }

        private void BuildMenuButtons()
        {
            float marginY = 118f;

            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                ButtonInfo button = this.menuButtons[i];

                Image backgroundElement = new()
                {
                    Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                    SourceRectangle = new(0, 140, 320, 80),
                    Color = AAP64ColorPalette.PurpleGray,
                    Size = new(320, 80),
                    Margin = new(-160.0f, marginY),
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
                    BorderOffset = 2f,
                    BorderThickness = 2f,
                };

                this.panelBackgroundElement.AddChild(backgroundElement);
                backgroundElement.AddChild(label);

                this.menuButtonSlots[i] = new(backgroundElement, null, label);

                marginY += backgroundElement.Size.Y + 32;
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
                SlotInfo slot = this.menuButtonSlots[i];

                Vector2 size = slot.Background.Size / 2;
                Vector2 position = slot.Background.Position + size;

                if (Interaction.OnMouseClick(position, size))
                {
                    this.menuButtons[i].ClickAction?.Invoke();
                }

                slot.Background.Color = Interaction.OnMouseOver(position, size) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
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
