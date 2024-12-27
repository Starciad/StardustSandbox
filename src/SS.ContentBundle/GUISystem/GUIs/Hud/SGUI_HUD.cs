using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using StardustSandbox.ContentBundle.GUISystem.Elements.Informational;
using StardustSandbox.ContentBundle.GUISystem.Global;
using StardustSandbox.ContentBundle.GUISystem.Specials.General;
using StardustSandbox.ContentBundle.GUISystem.Specials.Interactive;
using StardustSandbox.ContentBundle.Localization;
using StardustSandbox.Core.Catalog;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.GUI;
using StardustSandbox.Core.Constants.GUI.Common;
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
            SelectItemSlot(0, this.SGameInstance.CatalogDatabase.GetItemByIdentifier("element_dirt").Identifier);

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

                // [14] Replacement
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_21"),
            ];

            this.world = gameInstance.World;
            this.tooltipBoxElement = tooltipBoxElement;

            this.leftPanelTopButtons = [
                new(this.iconTextures[01], "Environment Settings", string.Empty, EnvironmentSettingsButtonAction),
                new(this.iconTextures[02], "Pen Settings", string.Empty, PenSettingsButtonAction),
                new(this.iconTextures[03], "Screenshot Settings", string.Empty,ScreenshotButtonAction),
                new(this.iconTextures[04], "World Settings", string.Empty, WorldSettingsButtonAction),
            ];

            this.leftPanelBottomButtons = [
                new(this.iconTextures[05], "Pause Simulation", string.Empty, PauseSimulationButtonAction),
            ];

            this.rightPanelTopButtons = [
                new(this.iconTextures[07], "Game Menu", string.Empty, GameMenuButtonAction),
                new(this.iconTextures[08], "Save Menu", string.Empty, SaveMenuButtonAction),
            ];

            this.rightPanelBottomButtons = [
                new(this.iconTextures[11], "Erase Everything", string.Empty, EraseEverythingButtonAction),
                new(this.iconTextures[10], "Reload Simulation", string.Empty, ReloadSimulationButtonAction),
                new(this.iconTextures[09], "Eraser", string.Empty, EraserButtonAction),
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

            #region ELEMENT SLOTS
            for (int i = 0; i < SHUDConstants.ELEMENT_BUTTONS_LENGTH; i++)
            {
                SSlot slot = this.toolbarElementSlots[i];
                bool isOver = this.GUIEvents.OnMouseOver(slot.BackgroundElement.Position, new(SHUDConstants.SLOT_SIZE));

                if (this.GUIEvents.OnMouseClick(slot.BackgroundElement.Position, new(SHUDConstants.SLOT_SIZE)))
                {
                    SelectItemSlot(i, (string)slot.BackgroundElement.GetData(SHUDConstants.DATA_FILED_ELEMENT_ID));
                }

                if (isOver)
                {
                    this.tooltipBoxElement.IsVisible = true;

                    if (!this.tooltipBoxElement.HasContent)
                    {
                        SItem item = this.SGameInstance.CatalogDatabase.GetItemByIdentifier((string)slot.BackgroundElement.GetData(SHUDConstants.DATA_FILED_ELEMENT_ID));

                        SGUIGlobalTooltip.Title = item.DisplayName;
                        SGUIGlobalTooltip.Description = item.Description;
                    }
                }

                slot.BackgroundElement.Color = this.slotSelectedIndex == i ?
                                        SColorPalette.OrangeRed :
                                        (isOver ? SColorPalette.EmeraldGreen : SColorPalette.White);
            }
            #endregion

            #region SEARCH BUTTON
            if (this.GUIEvents.OnMouseClick(this.toolbarElementSearchButton.Position, new(SHUDConstants.SLOT_SIZE)))
            {
                this.SGameInstance.GUIManager.OpenGUI(SGUIConstants.HUD_ITEM_EXPLORER_IDENTIFIER);
            }

            if (this.GUIEvents.OnMouseOver(this.toolbarElementSearchButton.Position, new(SHUDConstants.SLOT_SIZE)))
            {
                this.toolbarElementSearchButton.Color = SColorPalette.Graphite;
                this.tooltipBoxElement.IsVisible = true;

                if (!this.tooltipBoxElement.HasContent)
                {
                    SGUIGlobalTooltip.Title = SLocalization.GUI_HUD_Button_ItemExplorer_Title;
                    SGUIGlobalTooltip.Description = SLocalization.GUI_HUD_Button_ItemExplorer_Description;
                }
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

                    bool isOver = this.GUIEvents.OnMouseOver(toolbarSlot.BackgroundElement.Position, new(SHUDConstants.SLOT_SIZE));

                    if (this.GUIEvents.OnMouseClick(toolbarSlot.BackgroundElement.Position, new(SHUDConstants.SLOT_SIZE)))
                    {
                        button.ClickAction.Invoke();
                    }

                    if (isOver)
                    {
                        toolbarSlot.BackgroundElement.Color = SColorPalette.EmeraldGreen;
                        this.tooltipBoxElement.IsVisible = true;

                        if (!this.tooltipBoxElement.HasContent)
                        {
                            SGUIGlobalTooltip.Title = button.DisplayName;
                            SGUIGlobalTooltip.Description = button.Description;
                        }
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

        internal void AddItemToToolbar(string elementId)
        {
            SItem item = this.SGameInstance.CatalogDatabase.GetItemByIdentifier(elementId);

            // ================================================= //
            // Check if the item is already in the Toolbar. If so, it will be highlighted without changing the other items.

            for (int i = 0; i < SHUDConstants.ELEMENT_BUTTONS_LENGTH; i++)
            {
                SSlot slot = this.toolbarElementSlots[i];

                if (slot.BackgroundElement.ContainsData(SHUDConstants.DATA_FILED_ELEMENT_ID))
                {
                    if (item == this.SGameInstance.CatalogDatabase.GetItemByIdentifier((string)slot.BackgroundElement.GetData(SHUDConstants.DATA_FILED_ELEMENT_ID)))
                    {
                        SelectItemSlot(i, (string)slot.BackgroundElement.GetData(SHUDConstants.DATA_FILED_ELEMENT_ID));
                        return;
                    }
                }
            }

            // ================================================= //
            // If the item is not present in the toolbar, it will be added to the first slot next to the canvas and will push all others in the opposite direction. The last item will be removed from the toolbar until it is added again later.

            for (int i = 0; i < SHUDConstants.ELEMENT_BUTTONS_LENGTH - 1; i++)
            {
                SSlot currentSlot = this.toolbarElementSlots[i];
                SSlot nextSlot = this.toolbarElementSlots[i + 1];

                if (currentSlot.BackgroundElement.ContainsData(SHUDConstants.DATA_FILED_ELEMENT_ID) &&
                    nextSlot.BackgroundElement.ContainsData(SHUDConstants.DATA_FILED_ELEMENT_ID))
                {
                    currentSlot.BackgroundElement.UpdateData(SHUDConstants.DATA_FILED_ELEMENT_ID, nextSlot.BackgroundElement.GetData(SHUDConstants.DATA_FILED_ELEMENT_ID));
                    currentSlot.IconElement.Texture = nextSlot.IconElement.Texture;
                }
            }

            // Update last element slot.

            SSlot lastSlot = this.toolbarElementSlots[^1];

            if (lastSlot.BackgroundElement.ContainsData(SHUDConstants.DATA_FILED_ELEMENT_ID))
            {
                lastSlot.BackgroundElement.UpdateData(SHUDConstants.DATA_FILED_ELEMENT_ID, item.Identifier);
            }

            lastSlot.IconElement.Texture = item.IconTexture;

            // Select last slot.

            SelectItemSlot(this.toolbarElementSlots.Length - 1, item.Identifier);
        }

        private void SelectItemSlot(int slotIndex, string itemId)
        {
            this.slotSelectedIndex = slotIndex;
            this.SGameInstance.GameInputController.Player.SelectItem(this.SGameInstance.CatalogDatabase.GetItemByIdentifier(itemId));
        }
    }
}
