using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using StardustSandbox.ContentBundle.GUISystem.Elements.Informational;
using StardustSandbox.ContentBundle.GUISystem.Global;
using StardustSandbox.ContentBundle.GUISystem.Specials.General;
using StardustSandbox.ContentBundle.GUISystem.Specials.Interactive;
using StardustSandbox.ContentBundle.Localization.GUIs;
using StardustSandbox.ContentBundle.Localization.Statements;
using StardustSandbox.ContentBundle.Localization.Tools;
using StardustSandbox.Core.Catalog;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Constants.GUISystem;
using StardustSandbox.Core.Constants.GUISystem.GUIs.Hud;
using StardustSandbox.Core.Enums.GameInput.Pen;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.World;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud
{
    internal sealed partial class SGUI_HUD : SGUISystem
    {
        private int slotSelectedIndex = 0;

        private readonly Texture2D particleTexture;
        private readonly Texture2D guiButtonTexture;

        private readonly Texture2D[] iconTextures;

        private readonly ISWorld world;

        private readonly SButton[] leftPanelTopButtons;
        private readonly SButton[] leftPanelBottomButtons;
        private readonly SButton[] rightPanelTopButtons;
        private readonly SButton[] rightPanelBottomButtons;

        private readonly SGUITooltipBoxElement tooltipBoxElement;

        internal SGUI_HUD(ISGame gameInstance, string identifier, SGUIEvents guiEvents, SGUITooltipBoxElement tooltipBoxElement) : base(gameInstance, identifier, guiEvents)
        {
            SelectItemSlot(0, SElementConstants.IDENTIFIER_DIRT);

            this.particleTexture = this.SGameInstance.AssetDatabase.GetTexture("particle_1");
            this.guiButtonTexture = this.SGameInstance.AssetDatabase.GetTexture("gui_button_1");

            this.iconTextures = [
                // [00] Magnifying Glass
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_1"),

                // [01] Weather
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_11"),

                // [02] Pen
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_10"),

                // [03] Screenshot
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_13"),

                // [04] Settings
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_14"),

                // [05] Pause
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_8"),

                // [06] Resume
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_9"),

                // [07] Menu
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_6"),

                // [08] Save
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_7"),

                // [09] Eraser
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_4"),

                // [10] Reload
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_5"),

                // [11] Trash
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_2"),

                // [12] Eye
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_27"),

                // [13] Pen
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_19"),

                // [14] Paint Bucket
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_20"),

                // [15] Replacement
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_21"),
                
                // [16] Info
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_28"),
            ];

            this.world = gameInstance.World;
            this.tooltipBoxElement = tooltipBoxElement;

            this.leftPanelTopButtons = [
                new(this.iconTextures[01], SLocalization_GUIs.HUD_EnvironmentSettings_Name, SLocalization_GUIs.HUD_EnvironmentSettings_Description, EnvironmentSettingsButtonAction),
                new(this.iconTextures[02], SLocalization_GUIs.HUD_Button_PenSettings_Name, SLocalization_GUIs.HUD_Button_PenSettings_Description, PenSettingsButtonAction),
                new(this.iconTextures[03], SLocalization_GUIs.HUD_Button_ScreenshotSettings_Name, SLocalization_GUIs.HUD_Button_ScreenshotSettings_Description, ScreenshotButtonAction),
                new(this.iconTextures[04], SLocalization_GUIs.HUD_Button_WorldSettings_Name, SLocalization_GUIs.HUD_Button_WorldSettings_Description, WorldSettingsButtonAction),
                new(this.iconTextures[16], SLocalization_GUIs.HUD_Button_Information_Name, SLocalization_GUIs.HUD_Button_Information_Description, InfoButtonAction),
            ];

            this.leftPanelBottomButtons = [
                new(this.iconTextures[05], SLocalization_GUIs.HUD_Button_PauseSimulation_Name, SLocalization_GUIs.HUD_Button_PauseSimulation_Description, PauseSimulationButtonAction),
            ];

            this.rightPanelTopButtons = [
                new(this.iconTextures[07], SLocalization_GUIs.HUD_Button_GameMenu_Name, SLocalization_GUIs.HUD_Button_GameMenu_Description, GameMenuButtonAction),
                new(this.iconTextures[08], SLocalization_GUIs.HUD_Button_SaveMenu_Name, SLocalization_GUIs.HUD_Button_SaveMenu_Description, SaveMenuButtonAction),
            ];

            this.rightPanelBottomButtons = [
                new(this.iconTextures[11], SLocalization_GUIs.HUD_Button_EraseEverything_Name, SLocalization_GUIs.HUD_Button_EraseEverything_Description, EraseEverythingButtonAction),
                new(this.iconTextures[10], SLocalization_GUIs.HUD_Button_ReloadSimulation_Name, SLocalization_GUIs.HUD_Button_ReloadSimulation_Description, ReloadSimulationButtonAction),
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
            SetPlayerInteractionWhenToolbarHovered();
            UpdateTopToolbar();
            UpdateLeftToolbar();
            UpdateRightToolbar();

            this.tooltipBoxElement.RefreshDisplay(SGUIGlobalTooltip.Title, SGUIGlobalTooltip.Description);
        }

        private void UpdateSlotIcons()
        {
            // Pause
            this.leftPanelBottomButtonElements[0].IconElement.Texture = this.SGameInstance.GameManager.GameState.IsSimulationPaused ? this.iconTextures[06] : this.iconTextures[05];

            // Current Tool
            this.toolbarCurrentlySelectedToolIcon.Texture = this.SGameInstance.GameInputController.Pen.Tool switch
            {
                SPenTool.Visualization => this.iconTextures[12],
                SPenTool.Pencil => this.iconTextures[13],
                SPenTool.Eraser => this.iconTextures[09],
                SPenTool.Fill => this.iconTextures[14],
                SPenTool.Replace => this.iconTextures[15],
                _ => this.iconTextures[12],
            };
        }

        private void SetPlayerInteractionWhenToolbarHovered()
        {
            this.SGameInstance.GameInputController.Player.CanModifyEnvironment = !this.GUIEvents.OnMouseOver(this.topToolbarContainer.Position, this.topToolbarContainer.Size) &&
                                                                                 !this.GUIEvents.OnMouseOver(this.leftToolbarContainer.Position, this.leftToolbarContainer.Size) &&
                                                                                 !this.GUIEvents.OnMouseOver(this.rightToolbarContainer.Position, this.rightToolbarContainer.Size);
        }

        private void UpdateTopToolbar()
        {
            UpdateReturnInput();

            #region TOOL SLOT
            if (this.GUIEvents.OnMouseOver(this.toolbarCurrentlySelectedToolIcon.Position, new(SGUI_HUDConstants.SLOT_SIZE)))
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
            #endregion

            #region ELEMENT SLOTS
            for (int i = 0; i < SGUI_HUDConstants.ELEMENT_BUTTONS_LENGTH; i++)
            {
                SSlot slot = this.toolbarElementSlots[i];
                bool isOver = this.GUIEvents.OnMouseOver(slot.BackgroundElement.Position, new(SGUI_HUDConstants.SLOT_SIZE));

                if (this.GUIEvents.OnMouseClick(slot.BackgroundElement.Position, new(SGUI_HUDConstants.SLOT_SIZE)))
                {
                    SelectItemSlot(i, (SItem)slot.BackgroundElement.GetData(SGUIConstants.DATA_ITEM));
                }

                if (isOver)
                {
                    this.tooltipBoxElement.IsVisible = true;

                    SItem item = (SItem)slot.BackgroundElement.GetData(SGUIConstants.DATA_ITEM);

                    SGUIGlobalTooltip.Title = item.DisplayName;
                    SGUIGlobalTooltip.Description = item.Description;
                }

                slot.BackgroundElement.Color = this.slotSelectedIndex == i ?
                                        SColorPalette.OrangeRed :
                                        (isOver ? SColorPalette.EmeraldGreen : SColorPalette.White);
            }
            #endregion

            #region SEARCH BUTTON
            if (this.GUIEvents.OnMouseClick(this.toolbarElementSearchButton.Position, new(SGUI_HUDConstants.SLOT_SIZE)))
            {
                this.SGameInstance.GUIManager.OpenGUI(SGUIConstants.HUD_ITEM_EXPLORER_IDENTIFIER);
            }

            if (this.GUIEvents.OnMouseOver(this.toolbarElementSearchButton.Position, new(SGUI_HUDConstants.SLOT_SIZE)))
            {
                this.toolbarElementSearchButton.Color = SColorPalette.Graphite;
                this.tooltipBoxElement.IsVisible = true;

                SGUIGlobalTooltip.Title = SLocalization_GUIs.HUD_Button_ItemExplorer_Name;
                SGUIGlobalTooltip.Description = SLocalization_GUIs.HUD_Button_ItemExplorer_Description;
            }
            else
            {
                this.toolbarElementSearchButton.Color = SColorPalette.White;
            }
            #endregion

            #region MENU BUTTONS

            void CycleThroughArrayOfButtons(SSlot[] toolbarSlots, SButton[] buttons, int length)
            {
                for (int i = 0; i < length; i++)
                {
                    SSlot toolbarSlot = toolbarSlots[i];
                    SButton button = buttons[i];

                    bool isOver = this.GUIEvents.OnMouseOver(toolbarSlot.BackgroundElement.Position, new(SGUI_HUDConstants.SLOT_SIZE));

                    if (this.GUIEvents.OnMouseClick(toolbarSlot.BackgroundElement.Position, new(SGUI_HUDConstants.SLOT_SIZE)))
                    {
                        button.ClickAction.Invoke();
                    }

                    if (isOver)
                    {
                        toolbarSlot.BackgroundElement.Color = SColorPalette.EmeraldGreen;
                        this.tooltipBoxElement.IsVisible = true;

                        SGUIGlobalTooltip.Title = button.DisplayName;
                        SGUIGlobalTooltip.Description = button.Description;
                    }
                    else
                    {
                        toolbarSlot.BackgroundElement.Color = SColorPalette.White;
                    }
                }
            }

            CycleThroughArrayOfButtons(this.leftPanelTopButtonElements, this.leftPanelTopButtons, this.leftPanelTopButtonElements.Length);
            CycleThroughArrayOfButtons(this.leftPanelBottomButtonElements, this.leftPanelBottomButtons, this.leftPanelBottomButtonElements.Length);
            CycleThroughArrayOfButtons(this.rightPanelTopButtonElements, this.rightPanelTopButtons, this.rightPanelTopButtonElements.Length);
            CycleThroughArrayOfButtons(this.rightPanelBottomButtonElements, this.rightPanelBottomButtons, this.rightPanelBottomButtonElements.Length);

            #endregion
        }

        private void UpdateReturnInput()
        {
            if (this.SGameInstance.InputManager.KeyboardState.IsKeyDown(Keys.Escape))
            {
                this.SGameInstance.GUIManager.CloseGUI();
            }
        }

        private static void UpdateLeftToolbar()
        {
            return;
        }
        private static void UpdateRightToolbar()
        {
            return;
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
