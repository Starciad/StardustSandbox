using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UISystem;
using StardustSandbox.LocalizationSystem;
using StardustSandbox.Managers;
using StardustSandbox.UISystem.Elements.Graphics;
using StardustSandbox.UISystem.Elements.Textual;
using StardustSandbox.UISystem.Utilities;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.UISystem.UIs.HUD
{
    internal sealed class InformationUI : UI
    {
        private ImageUIElement panelBackgroundElement;
        private LabelUIElement menuTitleElement;

        private readonly LabelUIElement[] infoElements;
        private readonly UISlot[] menuButtonSlots;
        private readonly UIButton[] menuButtons;

        private readonly Texture2D particleTexture;
        private readonly Texture2D panelBackgroundTexture;
        private readonly Texture2D guiSmallButtonTexture;
        private readonly Texture2D[] iconTextures;
        private readonly SpriteFont bigApple3PMSpriteFont;

        private readonly GameManager gameManager;
        private readonly UIManager uiManager;
        private readonly World world;

        internal InformationUI(
            GameManager gameManager,
            UIIndex index,
            UIManager uiManager,
            World world
        ) : base(index)
        {
            this.gameManager = gameManager;
            this.uiManager = uiManager;
            this.world = world;

            this.particleTexture = AssetDatabase.GetTexture("texture_particle_1");
            this.panelBackgroundTexture = AssetDatabase.GetTexture("texture_gui_background_10");
            this.guiSmallButtonTexture = AssetDatabase.GetTexture("texture_gui_button_1");
            this.bigApple3PMSpriteFont = AssetDatabase.GetSpriteFont("font_2");

            this.iconTextures = [
                AssetDatabase.GetTexture("texture_icon_gui_16"),
            ];

            this.menuButtons = [
                new(this.iconTextures[0], Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, ExitButtonAction),
            ];

            this.menuButtonSlots = new UISlot[this.menuButtons.Length];
            this.infoElements = new LabelUIElement[UIConstants.HUD_INFORMATION_AMOUNT];
        }

        #region ACTIONS

        // Menu
        private void ExitButtonAction()
        {
            this.uiManager.CloseGUI();
        }

        #endregion

        #region BUILDER

        protected override void OnBuild(Layout layout)
        {
            BuildBackground(layout);
            BuildTitle(layout);
            BuildMenuButtons(layout);
            BuildInfoFields(layout);
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
                Size = new(1084, 540),
                Margin = new(98, 90),
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
                PositionAnchor = CardinalDirection.Northwest,
                OriginPivot = CardinalDirection.East,
                Margin = new(32, 40),
                Color = AAP64ColorPalette.White,
            };

            this.menuTitleElement.SetTextualContent(Localization_GUIs.HUD_Complements_Information_Title);
            this.menuTitleElement.SetAllBorders(true, AAP64ColorPalette.DarkGray, new(3f));
            this.menuTitleElement.PositionRelativeToElement(this.panelBackgroundElement);

            layout.AddElement(this.menuTitleElement);
        }

        private void BuildMenuButtons(Layout layout)
        {
            Vector2 margin = new(-32f, -40f);

            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                UIButton button = this.menuButtons[i];
                UISlot slot = CreateButtonSlot(margin, button.IconTexture);

                slot.BackgroundElement.PositionAnchor = CardinalDirection.Northeast;
                slot.BackgroundElement.OriginPivot = CardinalDirection.Center;

                // Update
                slot.BackgroundElement.PositionRelativeToElement(this.panelBackgroundElement);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);

                // Save
                this.menuButtonSlots[i] = slot;

                // Spacing
                margin.X -= UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2);

                layout.AddElement(slot.BackgroundElement);
                layout.AddElement(slot.IconElement);
            }
        }

        private void BuildInfoFields(Layout layout)
        {
            Vector2 margin = new(32, 144);

            for (int i = 0; i < this.infoElements.Length; i++)
            {
                LabelUIElement labelElement = new()
                {
                    SpriteFont = this.bigApple3PMSpriteFont,
                    Scale = new(0.1f),
                    PositionAnchor = CardinalDirection.Northwest,
                    OriginPivot = CardinalDirection.East,
                    Margin = margin,
                    Color = AAP64ColorPalette.White,
                };

                labelElement.SetTextualContent(string.Concat("Info ", i));
                labelElement.PositionRelativeToElement(this.panelBackgroundElement);

                // Save
                this.infoElements[i] = labelElement;

                // Spacing
                margin.Y += labelElement.GetStringSize().Y + 8;

                layout.AddElement(labelElement);
            }
        }

        // =============================================================== //

        private UISlot CreateButtonSlot(Vector2 margin, Texture2D iconTexture)
        {
            ImageUIElement backgroundElement = new()
            {
                Texture = this.guiSmallButtonTexture,
                Scale = new(UIConstants.HUD_SLOT_SCALE),
                Size = new(UIConstants.HUD_GRID_SIZE),
                Margin = margin,
            };

            ImageUIElement iconElement = new()
            {
                Texture = iconTexture,
                OriginPivot = CardinalDirection.Center,
                Scale = new(1.5f),
                Size = new(UIConstants.HUD_GRID_SIZE)
            };

            return new(backgroundElement, iconElement);
        }

        #endregion

        #region UPDATE

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateMenuButtons();
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtonSlots.Length; i++)
            {
                UISlot slot = this.menuButtonSlots[i];

                if (UIInteraction.OnMouseClick(slot.BackgroundElement.Position, new(UIConstants.HUD_GRID_SIZE)))
                {
                    this.menuButtons[i].ClickAction?.Invoke();
                }

                slot.BackgroundElement.Color = UIInteraction.OnMouseOver(slot.BackgroundElement.Position, new(UIConstants.HUD_GRID_SIZE)) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        #endregion

        #region EVENTS

        protected override void OnOpened()
        {
            this.gameManager.SetState(GameStates.IsCriticalMenuOpen);

            Point worldSize = this.world.Information.Size;

            uint limitOfElementsOnTheMap = (uint)(worldSize.X * worldSize.Y * 2);
            uint limitOfElementsPerLayer = (uint)(worldSize.X * worldSize.Y);

            this.infoElements[0].SetTextualContent(string.Concat(Localization_Statements.Size, ": ", worldSize));
            this.infoElements[1].SetTextualContent(string.Concat(Localization_Statements.Time, ": ", this.world.Time.CurrentTime));
            this.infoElements[2].SetTextualContent(string.Concat(Localization_Statements.Elements, ": ", this.world.GetTotalElementCount(), '/', limitOfElementsOnTheMap));
            this.infoElements[3].SetTextualContent(string.Concat(Localization_GUIs.HUD_Complements_Information_Field_ForegroundElements, ": ", this.world.GetTotalForegroundElementCount(), '/', limitOfElementsPerLayer));
            this.infoElements[4].SetTextualContent(string.Concat(Localization_GUIs.HUD_Complements_Information_Field_BackgroundElements, ": ", this.world.GetTotalBackgroundElementCount(), '/', limitOfElementsPerLayer));
        }

        protected override void OnClosed()
        {
            this.gameManager.RemoveState(GameStates.IsCriticalMenuOpen);
        }

        #endregion
    }
}
