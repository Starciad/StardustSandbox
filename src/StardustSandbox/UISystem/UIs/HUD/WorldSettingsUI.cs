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
using StardustSandbox.WorldSystem;

namespace StardustSandbox.UISystem.UIs.HUD
{
    internal sealed class WorldSettingsUI : UI
    {
        private Point worldTargetSize;

        private Image panelBackgroundElement;

        private Label menuTitleElement;
        private Label sizeSectionTitleElement;

        private readonly TooltipBox tooltipBox;

        private readonly SlotInfo[] menuButtonSlots;
        private readonly SlotInfo[] sizeButtonSlots;

        private readonly ButtonInfo[] menuButtons;
        private readonly ButtonInfo[] sizeButtons;

        private readonly ConfirmSettings changeWorldSizeConfirmSettings;

        private readonly ConfirmUI confirmUI;
        private readonly GameManager gameManager;
        private readonly UIManager uiManager;

        internal WorldSettingsUI(
            ConfirmUI confirmUI,
            GameManager gameManager,
            UIIndex index,
            TooltipBox tooltipBox,
            UIManager uiManager,
            World world
        ) : base(index)
        {
            this.confirmUI = confirmUI;
            this.gameManager = gameManager;
            this.tooltipBox = tooltipBox;
            this.uiManager = uiManager;

            this.changeWorldSizeConfirmSettings = new()
            {
                Caption = Localization_Messages.Confirm_World_Resize_Title,
                Message = Localization_Messages.Confirm_World_Resize_Description,
                OnConfirmCallback = status =>
                {
                    if (status == ConfirmStatus.Confirmed)
                    {
                        world.StartNew(this.worldTargetSize);
                    }

                    gameManager.RemoveState(GameStates.IsCriticalMenuOpen);
                },
            };

            this.menuButtons = [
                new(TextureIndex.UIButtons, new(224, 0, 32, 32), Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, ExitButtonAction),
            ];

            this.sizeButtons = [
                new(TextureIndex.UIButtons, new(0, 128, 32, 32), Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_Small_Name, Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_Small_Description, () => { SetWorldSizeButtonAction(WorldConstants.WORLD_SIZES_TEMPLATE[0]); }),
                new(TextureIndex.UIButtons, new(32, 128, 32, 32), Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_MediumSmall_Name, Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_MediumSmall_Description, () => { SetWorldSizeButtonAction(WorldConstants.WORLD_SIZES_TEMPLATE[1]); }),
                new(TextureIndex.UIButtons, new(64, 128, 32, 32), Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_Medium_Name, Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_Medium_Description, () => { SetWorldSizeButtonAction(WorldConstants.WORLD_SIZES_TEMPLATE[2]); }),
                new(TextureIndex.UIButtons, new(96, 128, 32, 32), Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_MediumLarge_Name, Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_MediumLarge_Description, () => { SetWorldSizeButtonAction(WorldConstants.WORLD_SIZES_TEMPLATE[3]); }),
                new(TextureIndex.UIButtons, new(128, 128, 32, 32), Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_Large_Name, Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_Large_Description, () => { SetWorldSizeButtonAction(WorldConstants.WORLD_SIZES_TEMPLATE[4]); }),
                new(TextureIndex.UIButtons, new(160, 128, 32, 32), Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_VeryLarge_Name, Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_VeryLarge_Description, () => { SetWorldSizeButtonAction(WorldConstants.WORLD_SIZES_TEMPLATE[5]); }),
            ];

            this.menuButtonSlots = new SlotInfo[this.menuButtons.Length];
            this.sizeButtonSlots = new SlotInfo[this.sizeButtons.Length];
        }

        #region ACTIONS

        // Menu
        private void ExitButtonAction()
        {
            this.uiManager.CloseGUI();
        }

        // Sizes
        private void SetWorldSizeButtonAction(Point size)
        {
            this.uiManager.CloseGUI();
            this.gameManager.SetState(GameStates.IsCriticalMenuOpen);
            this.worldTargetSize = size;
            this.confirmUI.Configure(this.changeWorldSizeConfirmSettings);
            this.uiManager.OpenGUI(this.confirmUI.Index);
        }

        #endregion

        #region BUILDER

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildTitle();
            BuildMenuButtons();
            BuildSizeSection();

            root.AddChild(this.tooltipBox);
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
                Texture = AssetDatabase.GetTexture(TextureIndex.UIBackgroundWorldSettings),
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
                TextContent = Localization_GUIs.HUD_Complements_WorldSettings_Title,

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 3f,
                BorderThickness = 3f,
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

        private void BuildSizeSection()
        {
            this.sizeSectionTitleElement = new()
            {
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.1f),
                Margin = new(32, 112),
                Color = AAP64ColorPalette.White,
                TextContent = Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Title
            };

            this.panelBackgroundElement.AddChild(this.sizeSectionTitleElement);

            // Buttons
            Vector2 margin = new(32, 80);

            for (int i = 0; i < this.sizeButtons.Length; i++)
            {
                ButtonInfo button = this.sizeButtons[i];
                SlotInfo slot = CreateButtonSlot(margin, button);

                slot.Background.Alignment = CardinalDirection.South;

                // Update
                this.sizeSectionTitleElement.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                // Save
                this.sizeButtonSlots[i] = slot;

                // Spacing
                margin.X += UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2);
            }
        }

        // =============================================================== //

        private SlotInfo CreateButtonSlot(Vector2 margin, ButtonInfo button)
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

        #region UPDATING

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.tooltipBox.CanDraw = false;

            UpdateMenuButtons();
            UpdateSizeButtons();

            this.tooltipBox.RefreshDisplay(TooltipBoxContent.Title, TooltipBoxContent.Description);
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

        private void UpdateSizeButtons()
        {
            for (int i = 0; i < this.sizeButtons.Length; i++)
            {
                SlotInfo slot = this.sizeButtonSlots[i];

                Vector2 position = slot.Background.Position;
                Vector2 size = new(UIConstants.HUD_GRID_SIZE);

                if (Interaction.OnMouseClick(position, size))
                {
                    this.sizeButtons[i].ClickAction?.Invoke();
                }

                if (Interaction.OnMouseOver(position, size))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.Title = this.sizeButtons[i].Name;
                    TooltipBoxContent.Description = this.sizeButtons[i].Description;

                    slot.Background.Color = AAP64ColorPalette.HoverColor;
                }
                else
                {
                    slot.Background.Color = AAP64ColorPalette.White;
                }
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
