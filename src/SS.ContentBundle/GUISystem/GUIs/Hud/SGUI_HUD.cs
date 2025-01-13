﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.Enums.GUISystem.Tools.Confirm;
using StardustSandbox.ContentBundle.GUISystem.Elements.Informational;
using StardustSandbox.ContentBundle.GUISystem.Global;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Tools.Confirm;
using StardustSandbox.ContentBundle.GUISystem.Helpers.General;
using StardustSandbox.ContentBundle.GUISystem.Helpers.Interactive;
using StardustSandbox.ContentBundle.GUISystem.Helpers.Tools.Settings;
using StardustSandbox.ContentBundle.Localization.GUIs;
using StardustSandbox.ContentBundle.Localization.Messages;
using StardustSandbox.ContentBundle.Localization.Statements;
using StardustSandbox.ContentBundle.Localization.Tools;
using StardustSandbox.Core.Catalog;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Constants.GUISystem;
using StardustSandbox.Core.Constants.GUISystem.GUIs.Hud;
using StardustSandbox.Core.Enums.GameInput.Pen;
using StardustSandbox.Core.Enums.Simulation;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud
{
    internal sealed partial class SGUI_HUD : SGUISystem
    {
        private int slotSelectedIndex = 0;

        private readonly Texture2D guiButtonTexture;

        private readonly Texture2D guiVerticalDrawerButtonsTexture;
        private readonly Texture2D guiHorizontalDrawerButtonsTexture;

        private readonly Texture2D guiHorizontalBackgroundTexture;
        private readonly Texture2D guiVerticalBackgroundTexture;

        private readonly Texture2D magnifyingGlassIconTexture;
        private readonly Texture2D weatherIconTexture;
        private readonly Texture2D pencilIconTexture;
        private readonly Texture2D penIconTexture;
        private readonly Texture2D settingsIconTexture;
        private readonly Texture2D pauseIconTexture;
        private readonly Texture2D resumeIconTexture;
        private readonly Texture2D menuIconTexture;
        private readonly Texture2D saveIconTexture;
        private readonly Texture2D trashIconTexture;
        private readonly Texture2D reloadIconTexture;
        private readonly Texture2D infoIconTexture;
        private readonly Texture2D[] speedIconTextures;

        private readonly ISWorld world;

        private readonly SButton[] leftPanelTopButtons;
        private readonly SButton[] leftPanelBottomButtons;
        private readonly SButton[] rightPanelTopButtons;
        private readonly SButton[] rightPanelBottomButtons;

        private readonly SGUI_Confirm guiConfirm;

        private readonly SConfirmSettings reloadSimulationConfirmSettings;
        private readonly SConfirmSettings eraseEverythingConfirmSettings;

        private readonly SGUITooltipBoxElement tooltipBoxElement;

        internal SGUI_HUD(ISGame gameInstance, string identifier, SGUIEvents guiEvents, SGUI_Confirm guiConfirm, SGUITooltipBoxElement tooltipBoxElement) : base(gameInstance, identifier, guiEvents)
        {
            this.guiConfirm = guiConfirm;

            this.world = gameInstance.World;
            this.tooltipBoxElement = tooltipBoxElement;

            this.reloadSimulationConfirmSettings = new()
            {
                Caption = SLocalization_Messages.Confirm_Simulation_Reload_Title,
                Message = SLocalization_Messages.Confirm_Simulation_Reload_Description,
                OnConfirmCallback = (SConfirmStatus status) =>
                {
                    if (status == SConfirmStatus.Confirmed)
                    {
                        this.world.Reload();
                    }

                    this.SGameInstance.GameManager.GameState.IsCriticalMenuOpen = false;
                },
            };

            this.eraseEverythingConfirmSettings = new()
            {
                Caption = SLocalization_Messages.Confirm_Simulation_EraseEverything_Title,
                Message = SLocalization_Messages.Confirm_Simulation_EraseEverything_Description,
                OnConfirmCallback = (SConfirmStatus status) =>
                {
                    if (status == SConfirmStatus.Confirmed)
                    {
                        this.world.Reset();
                    }

                    this.SGameInstance.GameManager.GameState.IsCriticalMenuOpen = false;
                },
            };

            SelectItemSlot(0, SElementConstants.IDENTIFIER_DIRT);

            this.guiButtonTexture = this.SGameInstance.AssetDatabase.GetTexture("gui_button_1");

            this.guiVerticalBackgroundTexture = this.SGameInstance.AssetDatabase.GetTexture("gui_background_15");
            this.guiHorizontalBackgroundTexture = this.SGameInstance.AssetDatabase.GetTexture("gui_background_16");
            this.guiHorizontalDrawerButtonsTexture = this.SGameInstance.AssetDatabase.GetTexture("gui_button_6");
            this.guiVerticalDrawerButtonsTexture = this.SGameInstance.AssetDatabase.GetTexture("gui_button_7");

            this.magnifyingGlassIconTexture = this.SGameInstance.AssetDatabase.GetTexture("icon_gui_1");
            this.trashIconTexture = this.SGameInstance.AssetDatabase.GetTexture("icon_gui_2");
            this.reloadIconTexture = this.SGameInstance.AssetDatabase.GetTexture("icon_gui_5");
            this.menuIconTexture = this.SGameInstance.AssetDatabase.GetTexture("icon_gui_6");
            this.saveIconTexture = this.SGameInstance.AssetDatabase.GetTexture("icon_gui_7");
            this.pauseIconTexture = this.SGameInstance.AssetDatabase.GetTexture("icon_gui_8");
            this.resumeIconTexture = this.SGameInstance.AssetDatabase.GetTexture("icon_gui_9");
            this.pencilIconTexture = this.SGameInstance.AssetDatabase.GetTexture("icon_gui_10");
            this.weatherIconTexture = this.SGameInstance.AssetDatabase.GetTexture("icon_gui_11");
            this.settingsIconTexture = this.SGameInstance.AssetDatabase.GetTexture("icon_gui_14");
            this.penIconTexture = this.SGameInstance.AssetDatabase.GetTexture("icon_gui_19");
            this.infoIconTexture = this.SGameInstance.AssetDatabase.GetTexture("icon_gui_28");
            this.speedIconTextures = [
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_44"),
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_45"),
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_46"),
            ];

            this.leftPanelTopButtons = [
                new(this.weatherIconTexture, SLocalization_GUIs.HUD_Button_EnvironmentSettings_Name, SLocalization_GUIs.HUD_Button_EnvironmentSettings_Description, EnvironmentSettingsButtonAction),
                new(this.pencilIconTexture, SLocalization_GUIs.HUD_Button_PenSettings_Name, SLocalization_GUIs.HUD_Button_PenSettings_Description, PenSettingsButtonAction),
                new(this.settingsIconTexture, SLocalization_GUIs.HUD_Button_WorldSettings_Name, SLocalization_GUIs.HUD_Button_WorldSettings_Description, WorldSettingsButtonAction),
                new(this.infoIconTexture, SLocalization_GUIs.HUD_Button_Information_Name, SLocalization_GUIs.HUD_Button_Information_Description, InfoButtonAction),
            ];

            this.leftPanelBottomButtons = [
                new(this.pauseIconTexture, SLocalization_GUIs.HUD_Button_PauseSimulation_Name, SLocalization_GUIs.HUD_Button_PauseSimulation_Description, PauseSimulationButtonAction),
                new(this.speedIconTextures[0], SLocalization_GUIs.HUD_Button_Speed_Name, SLocalization_GUIs.HUD_Button_Speed_Description, ChangeSimulationSpeedButtonAction),
            ];

            this.rightPanelTopButtons = [
                new(this.menuIconTexture, SLocalization_GUIs.HUD_Button_GameMenu_Name, SLocalization_GUIs.HUD_Button_GameMenu_Description, GameMenuButtonAction),
                new(this.saveIconTexture, SLocalization_GUIs.HUD_Button_SaveMenu_Name, SLocalization_GUIs.HUD_Button_SaveMenu_Description, SaveMenuButtonAction),
            ];

            this.rightPanelBottomButtons = [
                new(this.trashIconTexture, SLocalization_GUIs.HUD_Button_EraseEverything_Name, SLocalization_GUIs.HUD_Button_EraseEverything_Description, EraseEverythingButtonAction),
                new(this.reloadIconTexture, SLocalization_GUIs.HUD_Button_ReloadSimulation_Name, SLocalization_GUIs.HUD_Button_ReloadSimulation_Description, ReloadSimulationButtonAction),
            ];

            this.leftPanelTopButtonElements = new SSlot[this.leftPanelTopButtons.Length];
            this.leftPanelBottomButtonElements = new SSlot[this.leftPanelBottomButtons.Length];
            this.rightPanelTopButtonElements = new SSlot[this.rightPanelTopButtons.Length];
            this.rightPanelBottomButtonElements = new SSlot[this.rightPanelBottomButtons.Length];
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.tooltipBoxElement.IsVisible = false;

            UpdateSlotIcons();
            UpdateToolbars();
            UpdateDrawerButtons();
            SetPlayerInteractionWhenToolbarHovered();

            this.tooltipBoxElement.RefreshDisplay(SGUIGlobalTooltip.Title, SGUIGlobalTooltip.Description);
        }

        private void UpdateSlotIcons()
        {
            // Pause
            this.leftPanelBottomButtonElements[0].IconElement.Texture = this.SGameInstance.GameManager.GameState.IsSimulationPaused ? this.resumeIconTexture : this.pauseIconTexture;

            // Speed
            this.leftPanelBottomButtonElements[1].IconElement.Texture = this.SGameInstance.World.Simulation.CurrentSpeed switch
            {
                SSimulationSpeed.Normal => this.speedIconTextures[(byte)SSimulationSpeed.Normal],
                SSimulationSpeed.Fast => this.speedIconTextures[(byte)SSimulationSpeed.Fast],
                SSimulationSpeed.VeryFast => this.speedIconTextures[(byte)SSimulationSpeed.VeryFast],
                _ => this.speedIconTextures[(byte)SSimulationSpeed.Normal],
            };
        }

        private void UpdateToolbars()
        {
            UpdateTopToolbar();
            UpdateLeftToolbar();
            UpdateRightToolbar();
        }

        private void UpdateTopToolbar()
        {
            if (!this.topToolbarContainerElement.IsVisible)
            {
                return;
            }

            UpdateTopToolbarToolPreview();
            UpdateTopToolbarItemButtons();
            UpdateTopToolbarSearchButton();
        }

        private void UpdateTopToolbarToolPreview()
        {
            if (this.GUIEvents.OnMouseOver(this.toolbarCurrentlySelectedToolIconElement.Position, new(SGUI_HUDConstants.GRID_SIZE)))
            {
                this.tooltipBoxElement.IsVisible = true;

                switch (this.SGameInstance.GameInputController.Pen.Tool)
                {
                    case SPenTool.Visualization:
                        SGUIGlobalTooltip.Title = SLocalization_Tools.Visualization_Name;
                        SGUIGlobalTooltip.Description = SLocalization_Tools.Visualization_Description;
                        break;

                    case SPenTool.Pencil:
                        SGUIGlobalTooltip.Title = SLocalization_Tools.Pencil_Name;
                        SGUIGlobalTooltip.Description = SLocalization_Tools.Pencil_Description;
                        break;

                    case SPenTool.Eraser:
                        SGUIGlobalTooltip.Title = SLocalization_Tools.Eraser_Name;
                        SGUIGlobalTooltip.Description = SLocalization_Tools.Eraser_Description;
                        break;

                    case SPenTool.Fill:
                        SGUIGlobalTooltip.Title = SLocalization_Tools.Fill_Name;
                        SGUIGlobalTooltip.Description = SLocalization_Tools.Fill_Description;
                        break;

                    case SPenTool.Replace:
                        SGUIGlobalTooltip.Title = SLocalization_Tools.Replace_Name;
                        SGUIGlobalTooltip.Description = SLocalization_Tools.Replace_Description;
                        break;

                    default:
                        SGUIGlobalTooltip.Title = SLocalization_Statements.Unknown;
                        SGUIGlobalTooltip.Description = string.Empty;
                        break;
                }
            }
        }

        private void UpdateTopToolbarItemButtons()
        {
            for (int i = 0; i < SGUI_HUDConstants.ELEMENT_BUTTONS_LENGTH; i++)
            {
                SSlot slot = this.toolbarElementSlots[i];
                bool isOver = this.GUIEvents.OnMouseOver(slot.BackgroundElement.Position, new(SGUI_HUDConstants.GRID_SIZE));

                if (this.GUIEvents.OnMouseClick(slot.BackgroundElement.Position, new(SGUI_HUDConstants.GRID_SIZE)))
                {
                    SelectItemSlot(i, (SItem)slot.BackgroundElement.GetData(SGUIConstants.DATA_ITEM));
                }

                if (isOver)
                {
                    this.tooltipBoxElement.IsVisible = true;

                    SItem item = (SItem)slot.BackgroundElement.GetData(SGUIConstants.DATA_ITEM);

                    SGUIGlobalTooltip.Title = item.Name;
                    SGUIGlobalTooltip.Description = item.Description;
                }

                slot.BackgroundElement.Color = this.slotSelectedIndex == i ?
                                        SColorPalette.OrangeRed :
                                        (isOver ? SColorPalette.EmeraldGreen : SColorPalette.White);
            }
        }

        private void UpdateTopToolbarSearchButton()
        {
            if (this.GUIEvents.OnMouseClick(this.toolbarElementSearchButtonElement.Position, new(SGUI_HUDConstants.GRID_SIZE)))
            {
                this.SGameInstance.GUIManager.OpenGUI(SGUIConstants.HUD_ITEM_EXPLORER_IDENTIFIER);
            }

            if (this.GUIEvents.OnMouseOver(this.toolbarElementSearchButtonElement.Position, new(SGUI_HUDConstants.GRID_SIZE)))
            {
                this.toolbarElementSearchButtonElement.Color = SColorPalette.Graphite;
                this.tooltipBoxElement.IsVisible = true;

                SGUIGlobalTooltip.Title = SLocalization_GUIs.HUD_Button_ItemExplorer_Name;
                SGUIGlobalTooltip.Description = SLocalization_GUIs.HUD_Button_ItemExplorer_Description;
            }
            else
            {
                this.toolbarElementSearchButtonElement.Color = SColorPalette.White;
            }
        }

        private void UpdateLeftToolbar()
        {
            if (!this.leftToolbarContainerElement.IsVisible)
            {
                return;
            }

            CycleThroughArrayOfButtons(this.leftPanelTopButtonElements, this.leftPanelTopButtons, this.leftPanelTopButtonElements.Length);
            CycleThroughArrayOfButtons(this.leftPanelBottomButtonElements, this.leftPanelBottomButtons, this.leftPanelBottomButtonElements.Length);
        }

        private void UpdateRightToolbar()
        {
            if (!this.rightToolbarContainerElement.IsVisible)
            {
                return;
            }

            CycleThroughArrayOfButtons(this.rightPanelTopButtonElements, this.rightPanelTopButtons, this.rightPanelTopButtonElements.Length);
            CycleThroughArrayOfButtons(this.rightPanelBottomButtonElements, this.rightPanelBottomButtons, this.rightPanelBottomButtonElements.Length);
        }

        private void CycleThroughArrayOfButtons(SSlot[] toolbarSlots, SButton[] buttons, int length)
        {
            for (int i = 0; i < length; i++)
            {
                SSlot toolbarSlot = toolbarSlots[i];
                SButton button = buttons[i];

                bool isOver = this.GUIEvents.OnMouseOver(toolbarSlot.BackgroundElement.Position, new(SGUI_HUDConstants.GRID_SIZE));

                if (this.GUIEvents.OnMouseClick(toolbarSlot.BackgroundElement.Position, new(SGUI_HUDConstants.GRID_SIZE)))
                {
                    button.ClickAction?.Invoke();
                }

                if (isOver)
                {
                    toolbarSlot.BackgroundElement.Color = SColorPalette.EmeraldGreen;
                    this.tooltipBoxElement.IsVisible = true;

                    SGUIGlobalTooltip.Title = button.Name;
                    SGUIGlobalTooltip.Description = button.Description;
                }
                else
                {
                    toolbarSlot.BackgroundElement.Color = SColorPalette.White;
                }
            }
        }

        private void SetPlayerInteractionWhenToolbarHovered()
        {
            if (this.topToolbarContainerElement.IsVisible && this.GUIEvents.OnMouseOver(this.topToolbarContainerElement.Position, this.topToolbarContainerElement.Size))
            {
                this.SGameInstance.GameInputController.Player.CanModifyEnvironment = false;
            }
            else
            {
                this.SGameInstance.GameInputController.Player.CanModifyEnvironment = true;
            }

            if (this.leftToolbarContainerElement.IsVisible && this.GUIEvents.OnMouseOver(this.leftToolbarContainerElement.Position, this.topToolbarContainerElement.Size))
            {
                this.SGameInstance.GameInputController.Player.CanModifyEnvironment = false;
            }
            else
            {
                this.SGameInstance.GameInputController.Player.CanModifyEnvironment = true;
            }

            if (this.rightToolbarContainerElement.IsVisible && this.GUIEvents.OnMouseOver(this.rightToolbarContainerElement.Position, this.topToolbarContainerElement.Size))
            {
                this.SGameInstance.GameInputController.Player.CanModifyEnvironment = false;
            }
            else
            {
                this.SGameInstance.GameInputController.Player.CanModifyEnvironment = true;
            }
        }

        private void UpdateDrawerButtons()
        {
            UpdateTopDrawerButton();
            UpdateLeftDrawerButton();
            UpdateRightDrawerButton();
        }

        private void UpdateTopDrawerButton()
        {
            SSize2 size = this.topDrawerButtonElement.Size / 2;
            Vector2 position = new(this.topDrawerButtonElement.Position.X, this.topDrawerButtonElement.Position.Y - 24f);

            if (this.GUIEvents.OnMouseClick(position, size))
            {
                this.topToolbarContainerElement.IsVisible = !this.topToolbarContainerElement.IsVisible;
            }

            this.topDrawerButtonElement.Color = this.GUIEvents.OnMouseOver(position, size) ? SColorPalette.LemonYellow : SColorPalette.White;
            this.topDrawerButtonElement.Margin = this.topToolbarContainerElement.IsVisible ? new(0, 128) : new(0, 32);
            
            this.topDrawerButtonElement.PositionRelativeToScreen();
        }

        private void UpdateLeftDrawerButton()
        {
            SSize2 size = this.leftDrawerButtonElement.Size / 2;
            Vector2 position = new(this.leftDrawerButtonElement.Position.X -  24f, this.leftDrawerButtonElement.Position.Y);

            if (this.GUIEvents.OnMouseClick(position, size))
            {
                this.leftToolbarContainerElement.IsVisible = !this.leftToolbarContainerElement.IsVisible;
            }

            this.leftDrawerButtonElement.Color = this.GUIEvents.OnMouseOver(position, size) ? SColorPalette.LemonYellow : SColorPalette.White;
            this.leftDrawerButtonElement.Margin = this.leftToolbarContainerElement.IsVisible ? new(128, 0) : new(32, 0);

            this.leftDrawerButtonElement.PositionRelativeToScreen();
        }

        private void UpdateRightDrawerButton()
        {
            SSize2 size = this.rightDrawerButtonElement.Size / 2;
            Vector2 position = new(this.rightDrawerButtonElement.Position.X - 32f, this.rightDrawerButtonElement.Position.Y);

            if (this.GUIEvents.OnMouseClick(position, size))
            {
                this.rightToolbarContainerElement.IsVisible = !this.rightToolbarContainerElement.IsVisible;
            }

            this.rightDrawerButtonElement.Color = this.GUIEvents.OnMouseOver(position, size) ? SColorPalette.LemonYellow : SColorPalette.White;
            this.rightDrawerButtonElement.Margin = this.rightToolbarContainerElement.IsVisible ? new(-80, 0) : new(16, 0);

            this.rightDrawerButtonElement.PositionRelativeToScreen();
        }

        internal void AddItemToToolbar(SItem item)
        {
            // ================================================= //
            // Check if the item is already in the Toolbar. If so, it will be highlighted without changing the other items.

            for (int i = 0; i < SGUI_HUDConstants.ELEMENT_BUTTONS_LENGTH; i++)
            {
                SSlot slot = this.toolbarElementSlots[i];

                if (slot.BackgroundElement.ContainsData(SGUIConstants.DATA_ITEM))
                {
                    SItem otherItem = (SItem)slot.BackgroundElement.GetData(SGUIConstants.DATA_ITEM);

                    if (item == otherItem)
                    {
                        SelectItemSlot(i, otherItem);
                        return;
                    }
                }
            }

            // ================================================= //
            // If the item is not present in the toolbar, it will be added to the first slot next to the canvas and will push all others in the opposite direction. The last item will be removed from the toolbar until it is added again later.

            for (int i = 0; i < SGUI_HUDConstants.ELEMENT_BUTTONS_LENGTH - 1; i++)
            {
                SSlot currentSlot = this.toolbarElementSlots[i];
                SSlot nextSlot = this.toolbarElementSlots[i + 1];

                if (currentSlot.BackgroundElement.ContainsData(SGUIConstants.DATA_ITEM) &&
                    nextSlot.BackgroundElement.ContainsData(SGUIConstants.DATA_ITEM))
                {
                    currentSlot.BackgroundElement.UpdateData(SGUIConstants.DATA_ITEM, nextSlot.BackgroundElement.GetData(SGUIConstants.DATA_ITEM));
                    currentSlot.IconElement.Texture = nextSlot.IconElement.Texture;
                }
            }

            // Update last element slot.

            SSlot lastSlot = this.toolbarElementSlots[^1];

            if (lastSlot.BackgroundElement.ContainsData(SGUIConstants.DATA_ITEM))
            {
                lastSlot.BackgroundElement.UpdateData(SGUIConstants.DATA_ITEM, item);
            }

            lastSlot.IconElement.Texture = item.IconTexture;

            // Select last slot.

            SelectItemSlot(this.toolbarElementSlots.Length - 1, item);
        }

        internal bool ItemIsEquipped(SItem item)
        {
            for (int i = 0; i < SGUI_HUDConstants.ELEMENT_BUTTONS_LENGTH; i++)
            {
                SItem hudItem = (SItem)this.toolbarElementSlots[i].BackgroundElement.GetData(SGUIConstants.DATA_ITEM);

                if (item == hudItem)
                {
                    return true;
                }
            }

            return false;
        }

        internal void SetToolIcon(Texture2D iconTexture)
        {
            this.toolbarCurrentlySelectedToolIconElement.Texture = iconTexture;
        }

        private void SelectItemSlot(int slotIndex, string itemIdentifier)
        {
            SelectItemSlot(slotIndex, this.SGameInstance.CatalogDatabase.GetItem(itemIdentifier));
        }

        private void SelectItemSlot(int slotIndex, SItem item)
        {
            this.slotSelectedIndex = slotIndex;
            this.SGameInstance.GameInputController.Player.SelectItem(item);
        }
    }
}
