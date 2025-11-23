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
using StardustSandbox.UISystem.Elements.Graphics;
using StardustSandbox.UISystem.Elements.Textual;
using StardustSandbox.UISystem.Settings;
using StardustSandbox.UISystem.UIs.Tools;
using StardustSandbox.UISystem.Utilities;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.UISystem.UIs.HUD
{
    internal sealed class WorldSettingsUI : UI
    {
        private Point worldTargetSize;

        private ImageUIElement panelBackgroundElement;

        private LabelUIElement menuTitleElement;
        private LabelUIElement sizeSectionTitleElement;

        private readonly TooltipBoxUIElement tooltipBoxElement;

        private readonly UISlot[] menuButtonSlots;
        private readonly UISlot[] sizeButtonSlots;

        private readonly UIButton[] menuButtons;
        private readonly UIButton[] sizeButtons;

        private readonly ConfirmSettings changeWorldSizeConfirmSettings;

        private readonly ConfirmUI confirmUI;
        private readonly GameManager gameManager;
        private readonly UIManager uiManager;

        internal WorldSettingsUI(
            ConfirmUI confirmUI,
            GameManager gameManager,
            UIIndex index,
            TooltipBoxUIElement tooltipBoxElement,
            UIManager uiManager,
            World world
        ) : base(index)
        {
            this.confirmUI = confirmUI;
            this.gameManager = gameManager;
            this.tooltipBoxElement = tooltipBoxElement;
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
                new(AssetDatabase.GetTexture(TextureIndex.GuiButtons), new(224, 0, 32, 32), Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, ExitButtonAction),
            ];

            this.sizeButtons = [
                new(AssetDatabase.GetTexture(TextureIndex.GuiButtons), new(0, 128, 32, 32), Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_Small_Name, Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_Small_Description, () => { SetWorldSizeButtonAction(WorldConstants.WORLD_SIZES_TEMPLATE[0]); }),
                new(AssetDatabase.GetTexture(TextureIndex.GuiButtons), new(32, 128, 32, 32), Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_MediumSmall_Name, Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_MediumSmall_Description, () => { SetWorldSizeButtonAction(WorldConstants.WORLD_SIZES_TEMPLATE[1]); }),
                new(AssetDatabase.GetTexture(TextureIndex.GuiButtons), new(64, 128, 32, 32), Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_Medium_Name, Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_Medium_Description, () => { SetWorldSizeButtonAction(WorldConstants.WORLD_SIZES_TEMPLATE[2]); }),
                new(AssetDatabase.GetTexture(TextureIndex.GuiButtons), new(96, 128, 32, 32), Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_MediumLarge_Name, Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_MediumLarge_Description, () => { SetWorldSizeButtonAction(WorldConstants.WORLD_SIZES_TEMPLATE[3]); }),
                new(AssetDatabase.GetTexture(TextureIndex.GuiButtons), new(128, 128, 32, 32), Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_Large_Name, Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_Large_Description, () => { SetWorldSizeButtonAction(WorldConstants.WORLD_SIZES_TEMPLATE[4]); }),
                new(AssetDatabase.GetTexture(TextureIndex.GuiButtons), new(160, 128, 32, 32), Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_VeryLarge_Name, Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Button_VeryLarge_Description, () => { SetWorldSizeButtonAction(WorldConstants.WORLD_SIZES_TEMPLATE[5]); }),
            ];

            this.menuButtonSlots = new UISlot[this.menuButtons.Length];
            this.sizeButtonSlots = new UISlot[this.sizeButtons.Length];
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

        protected override void OnBuild(Layout layout)
        {
            BuildBackground(layout);
            BuildTitle(layout);
            BuildMenuButtons(layout);
            BuildSizeSection(layout);

            layout.AddElement(this.tooltipBoxElement);
        }

        private void BuildBackground(Layout layout)
        {
            ImageUIElement backgroundShadowElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Scale = new(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT),
                Size = new(1),
                Color = new(AAP64ColorPalette.DarkGray, 160)
            };

            this.panelBackgroundElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GuiBackgroundWorldSettings),
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
                SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm),
                Scale = new(0.12f),
                PositionAnchor = CardinalDirection.Northwest,
                OriginPivot = CardinalDirection.East,
                Margin = new(32, 40),
                Color = AAP64ColorPalette.White,
            };

            this.menuTitleElement.SetTextualContent(Localization_GUIs.HUD_Complements_WorldSettings_Title);
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
                UISlot slot = CreateButtonSlot(margin, button);

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

        private void BuildSizeSection(Layout layout)
        {
            this.sizeSectionTitleElement = new()
            {
                SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm),
                Scale = new(0.1f),
                Margin = new(32, 112),
                Color = AAP64ColorPalette.White,
            };

            this.sizeSectionTitleElement.SetTextualContent(Localization_GUIs.HUD_Complements_WorldSettings_Section_Size_Title);
            this.sizeSectionTitleElement.PositionRelativeToElement(this.panelBackgroundElement);

            layout.AddElement(this.sizeSectionTitleElement);

            // Buttons
            Vector2 margin = new(32, 80);

            for (int i = 0; i < this.sizeButtons.Length; i++)
            {
                UIButton button = this.sizeButtons[i];
                UISlot slot = CreateButtonSlot(margin, button);

                slot.BackgroundElement.PositionAnchor = CardinalDirection.South;
                slot.BackgroundElement.OriginPivot = CardinalDirection.Center;

                // Update
                slot.BackgroundElement.PositionRelativeToElement(this.sizeSectionTitleElement);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);

                // Save
                this.sizeButtonSlots[i] = slot;

                // Spacing
                margin.X += UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2);

                layout.AddElement(slot.BackgroundElement);
                layout.AddElement(slot.IconElement);
            }
        }

        // =============================================================== //

        private UISlot CreateButtonSlot(Vector2 margin, UIButton button)
        {
            ImageUIElement backgroundElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GuiButtons),
                TextureClipArea = new(320, 140, 32, 32),
                Scale = new(UIConstants.HUD_SLOT_SCALE),
                Size = new(UIConstants.HUD_GRID_SIZE),
                Margin = margin,
            };

            ImageUIElement iconElement = new()
            {
                Texture = button.IconTexture,
                TextureClipArea = button.IconTextureRectangle,
                OriginPivot = CardinalDirection.Center,
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

            this.tooltipBoxElement.IsVisible = false;

            UpdateMenuButtons();
            UpdateSizeButtons();

            this.tooltipBoxElement.RefreshDisplay(TooltipContent.Title, TooltipContent.Description);
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

        private void UpdateSizeButtons()
        {
            for (int i = 0; i < this.sizeButtons.Length; i++)
            {
                UISlot slot = this.sizeButtonSlots[i];

                Vector2 position = slot.BackgroundElement.Position;
                Vector2 size = new(UIConstants.HUD_GRID_SIZE);

                if (UIInteraction.OnMouseClick(position, size))
                {
                    this.sizeButtons[i].ClickAction?.Invoke();
                }

                if (UIInteraction.OnMouseOver(position, size))
                {
                    this.tooltipBoxElement.IsVisible = true;

                    TooltipContent.Title = this.sizeButtons[i].Name;
                    TooltipContent.Description = this.sizeButtons[i].Description;

                    slot.BackgroundElement.Color = AAP64ColorPalette.HoverColor;
                }
                else
                {
                    slot.BackgroundElement.Color = AAP64ColorPalette.White;
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
