using Microsoft.Xna.Framework;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UI;
using StardustSandbox.LocalizationSystem;
using StardustSandbox.Managers;
using StardustSandbox.UI.Elements;
using StardustSandbox.UI.Information;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.UI.Common.HUD
{
    internal sealed class InformationUI : UIBase
    {
        private Image panelBackgroundElement;
        private Label menuTitleElement;

        private readonly Label[] infoElements;
        private readonly SlotInfo[] menuButtonSlots;
        private readonly ButtonInfo[] menuButtons;

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

            this.menuButtons = [
                new(TextureIndex.UIButtons, new(224, 0, 32, 32), Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, ExitButtonAction),
            ];

            this.menuButtonSlots = new SlotInfo[this.menuButtons.Length];
            this.infoElements = new Label[UIConstants.HUD_INFORMATION_AMOUNT];
        }

        #region ACTIONS

        // Menu
        private void ExitButtonAction()
        {
            this.uiManager.CloseGUI();
        }

        #endregion

        #region BUILDER

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildTitle();
            BuildMenuButtons();
            BuildInfoFields();
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
                Texture = AssetDatabase.GetTexture(TextureIndex.UIBackgroundInformation),
                Size = new(1084, 540),
                Margin = new(98, 90),
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
                Alignment = CardinalDirection.Northwest,
                Margin = new(32, 40),
                Color = AAP64ColorPalette.White,
                TextContent = Localization_GUIs.HUD_Complements_Information_Title,

                BorderDirections = LabelBorderDirection.All,
                BorderColor = AAP64ColorPalette.DarkGray,
                BorderThickness = 3f,
                BorderOffset = 3f,
            };

            this.panelBackgroundElement.AddChild(this.menuTitleElement);
        }

        private void BuildMenuButtons()
        {
            Vector2 margin = new(-32f, -40f);

            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                ButtonInfo button = this.menuButtons[i];
                SlotInfo slot = CreateButtonSlot(margin, button);

                slot.Background.Alignment = CardinalDirection.Northeast;

                // Update
                this.panelBackgroundElement.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                // Save
                this.menuButtonSlots[i] = slot;

                // Spacing
                margin.X -= UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2);
            }
        }

        private void BuildInfoFields()
        {
            Vector2 margin = new(32, 144);

            for (int i = 0; i < this.infoElements.Length; i++)
            {
                Label label = new()
                {
                    SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                    Scale = new(0.1f),
                    Alignment = CardinalDirection.Northwest,
                    Margin = margin,
                    Color = AAP64ColorPalette.White,
                    TextContent = string.Concat("Info ", i)
                };

                this.panelBackgroundElement.AddChild(label);

                // Save
                this.infoElements[i] = label;

                // Spacing
                margin.Y += label.Size.Y + 8;
            }
        }

        // =============================================================== //

        private static SlotInfo CreateButtonSlot(Vector2 margin, ButtonInfo button)
        {
            Image backgroundElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                SourceRectangle = new(320, 140, 32, 32),
                Scale = new(UIConstants.HUD_SLOT_SCALE),
                Size = new(UIConstants.HUD_GRID_SIZE),
                Margin = margin,
            };

            Image iconElement = new()
            {
                Texture = button.IconTexture,
                SourceRectangle = button.IconTextureRectangle,
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
                SlotInfo slot = this.menuButtonSlots[i];

                if (Interaction.OnMouseClick(slot.Background.Position, new(UIConstants.HUD_GRID_SIZE)))
                {
                    this.menuButtons[i].ClickAction?.Invoke();
                }

                slot.Background.Color = Interaction.OnMouseOver(slot.Background.Position, new(UIConstants.HUD_GRID_SIZE)) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
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

            this.infoElements[0].TextContent = string.Concat(Localization_Statements.Size, ": ", worldSize);
            this.infoElements[1].TextContent = string.Concat(Localization_Statements.Time, ": ", this.world.Time.CurrentTime.ToString(@"hh\:mm\:ss"));
            this.infoElements[2].TextContent = string.Concat(Localization_Statements.Elements, ": ", this.world.GetTotalElementCount(), '/', limitOfElementsOnTheMap);
            this.infoElements[3].TextContent = string.Concat(Localization_GUIs.HUD_Complements_Information_Field_ForegroundElements, ": ", this.world.GetTotalForegroundElementCount(), '/', limitOfElementsPerLayer);
            this.infoElements[4].TextContent = string.Concat(Localization_GUIs.HUD_Complements_Information_Field_BackgroundElements, ": ", this.world.GetTotalBackgroundElementCount(), '/', limitOfElementsPerLayer);
        }

        protected override void OnClosed()
        {
            this.gameManager.RemoveState(GameStates.IsCriticalMenuOpen);
        }

        #endregion
    }
}
