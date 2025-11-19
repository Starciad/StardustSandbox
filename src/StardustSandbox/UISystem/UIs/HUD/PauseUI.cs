using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.Indexers;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UISystem.Tools;
using StardustSandbox.LocalizationSystem;
using StardustSandbox.Managers;
using StardustSandbox.UISystem.Elements.Graphics;
using StardustSandbox.UISystem.Elements.Textual;
using StardustSandbox.UISystem.Settings;
using StardustSandbox.UISystem.UIs.Tools;
using StardustSandbox.UISystem.Utilities;

namespace StardustSandbox.UISystem.UIs.HUD
{
    internal sealed class PauseUI : UI
    {
        private ImageUIElement panelBackgroundElement;
        private LabelUIElement menuTitleElement;

        private readonly UIButton[] menuButtons;
        private readonly UISlot[] menuButtonSlots;

        private readonly Texture2D particleTexture;
        private readonly Texture2D panelBackgroundTexture;
        private readonly Texture2D guiLargeButtonTexture;
        private readonly SpriteFont bigApple3PMSpriteFont;

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

            this.particleTexture = AssetDatabase.GetTexture("texture_particle_1");
            this.panelBackgroundTexture = AssetDatabase.GetTexture("texture_gui_background_14");
            this.guiLargeButtonTexture = AssetDatabase.GetTexture("texture_gui_button_3");
            this.bigApple3PMSpriteFont = AssetDatabase.GetSpriteFont("font_2");

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
                new(null, Localization_Statements.Resume, string.Empty, ResumeButtonAction),
                new(null, Localization_Statements.Options, string.Empty, OptionsButtonAction),
                new(null, Localization_Statements.Exit, string.Empty, ExitButtonAction),
            ];

            this.menuButtonSlots = new UISlot[this.menuButtons.Length];
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

        protected override void OnBuild(Layout layout)
        {
            BuildBackground(layout);
            BuildTitle(layout);
            BuildMenuButtons(layout);
        }

        private void BuildBackground(Layout layout)
        {
            ImageUIElement backgroundShadowElement = new()
            {
                Texture = this.particleTexture,
                Scale = new(ScreenConstants.DEFAULT_SCREEN_WIDTH, ScreenConstants.DEFAULT_SCREEN_HEIGHT),
                Size = new(1),
                Color = new(AAP64ColorPalette.DarkGray, 160)
            };

            this.panelBackgroundElement = new()
            {
                Texture = this.panelBackgroundTexture,
                Size = new(542, 540),
                Margin = new(this.panelBackgroundTexture.Width / 2 * -1, 90),
                PositionAnchor = CardinalDirection.North,
            };

            this.panelBackgroundElement.PositionRelativeToScreen();

            layout.AddElement(backgroundShadowElement);
            layout.AddElement(this.panelBackgroundElement);
        }

        private void BuildTitle(Layout layout)
        {
            this.menuTitleElement = new()
            {
                SpriteFont = this.bigApple3PMSpriteFont,
                Scale = new(0.12f),
                PositionAnchor = CardinalDirection.North,
                OriginPivot = CardinalDirection.Center,
                Margin = new(0f, 40f),
                Color = AAP64ColorPalette.White,
            };

            this.menuTitleElement.SetTextualContent(Localization_GUIs.HUD_Complements_Pause_Title);
            this.menuTitleElement.SetAllBorders(true, AAP64ColorPalette.DarkGray, new(3f));
            this.menuTitleElement.PositionRelativeToElement(this.panelBackgroundElement);

            layout.AddElement(this.menuTitleElement);
        }

        private void BuildMenuButtons(Layout layout)
        {
            float marginY = 118f;

            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                UIButton button = this.menuButtons[i];

                ImageUIElement backgroundElement = new()
                {
                    Texture = this.guiLargeButtonTexture,
                    Color = AAP64ColorPalette.PurpleGray,
                    Size = new(320, 80),
                    Margin = new(this.guiLargeButtonTexture.Width / 2 * -1, marginY),
                    PositionAnchor = CardinalDirection.North,
                };

                LabelUIElement labelElement = new()
                {
                    Scale = new(0.1f),
                    Color = AAP64ColorPalette.White,
                    SpriteFont = this.bigApple3PMSpriteFont,
                    PositionAnchor = CardinalDirection.Center,
                    OriginPivot = CardinalDirection.Center
                };

                labelElement.SetTextualContent(button.Name);
                labelElement.SetAllBorders(true, AAP64ColorPalette.DarkGray, new(2));

                backgroundElement.PositionRelativeToElement(this.panelBackgroundElement);
                labelElement.PositionRelativeToElement(backgroundElement);

                layout.AddElement(backgroundElement);
                layout.AddElement(labelElement);

                this.menuButtonSlots[i] = new(backgroundElement, null, labelElement);

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
                UISlot slot = this.menuButtonSlots[i];

                Vector2 size = slot.BackgroundElement.Size / 2;
                Vector2 position = slot.BackgroundElement.Position + size;

                if (UIInteraction.OnMouseClick(position, size))
                {
                    this.menuButtons[i].ClickAction?.Invoke();
                }

                slot.BackgroundElement.Color = UIInteraction.OnMouseOver(position, size) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
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
