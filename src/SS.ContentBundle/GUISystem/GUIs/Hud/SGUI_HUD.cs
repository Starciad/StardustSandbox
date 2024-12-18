using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Elements.Informational;
using StardustSandbox.ContentBundle.GUISystem.Global;
using StardustSandbox.ContentBundle.GUISystem.Specials.Interactive;
using StardustSandbox.ContentBundle.Localization;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.GUI;
using StardustSandbox.Core.Constants.GUI.Common;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.Items;
using StardustSandbox.Core.Mathematics.Primitives;

using System;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud
{
    internal sealed partial class SGUI_HUD : SGUISystem
    {
        private enum SIconIndex : byte
        {
            MagnifyingGlass = 00,
            Weather = 01,
            Pen = 02,
            Screenshot = 03,
            Settings = 04,
            Pause = 05,
            Resume = 06,
            Menu = 07,
            Save = 08,
            Eraser = 09,
            Reload = 10,
            Trash = 11
        }

        private readonly struct SToolbarSlot(SGUIImageElement background, SGUIImageElement icon)
        {
            internal SGUIImageElement Background => background;
            internal SGUIImageElement Icon => icon;
        }

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
            SelectItemSlot(0, this.SGameInstance.ItemDatabase.GetItemById("element_dirt").Identifier);

            this.particleTexture = this.SGameInstance.AssetDatabase.GetTexture("particle_1");
            this.guiButtonTexture = this.SGameInstance.AssetDatabase.GetTexture("gui_button_1");

            this.iconTextures = [
                // Magnifying Glass
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_1"),

                // Weather
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_11"),

                // Pen
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_10"),

                // Screenshot
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_13"),

                // Settings
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_14"),

                // Pause
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_8"),

                // Resume
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_9"),

                // Menu
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_6"),

                // Save
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_7"),

                // Eraser
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_4"),

                // Reload
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_5"),

                // Trash
                this.SGameInstance.AssetDatabase.GetTexture("icon_gui_2"),
            ];

            this.world = gameInstance.World;
            this.tooltipBoxElement = tooltipBoxElement;

            this.leftPanelTopButtons = [
                new(this.iconTextures[(byte)SIconIndex.Weather], "Weather Settings", string.Empty, WeatherSettingsButtonAction),
                new(this.iconTextures[(byte)SIconIndex.Pen], "Pen Settings", string.Empty, PenSettingsButtonAction),
                new(this.iconTextures[(byte)SIconIndex.Screenshot], "Screenshot Settings", string.Empty,ScreenshotButtonAction),
                new(this.iconTextures[(byte)SIconIndex.Settings], "World Settings", string.Empty, WorldSettingsButtonAction),
            ];

            this.leftPanelBottomButtons = [
                new(this.iconTextures[(byte)SIconIndex.Pause], "Pause Simulation", string.Empty, PauseSimulationButtonAction),
            ];

            this.rightPanelTopButtons = [
                new(this.iconTextures[(byte)SIconIndex.Menu], "Game Menu", string.Empty, GameMenuButtonAction),
                new(this.iconTextures[(byte)SIconIndex.Save], "Save Menu", string.Empty, SaveMenuButtonAction),
            ];

            this.rightPanelBottomButtons = [
                new(this.iconTextures[(byte)SIconIndex.Trash], "Erase Everything", string.Empty, EraseEverythingButtonAction),
                new(this.iconTextures[(byte)SIconIndex.Reload], "Reload Simulation", string.Empty, ReloadSimulationButtonAction),
                new(this.iconTextures[(byte)SIconIndex.Eraser], "Eraser", string.Empty, EraserButtonAction),
            ];

            this.leftPanelTopButtonElements = new SToolbarSlot[this.leftPanelTopButtons.Length];
            this.leftPanelBottomButtonElements = new SToolbarSlot[this.leftPanelBottomButtons.Length];
            this.rightPanelTopButtonElements = new SToolbarSlot[this.rightPanelTopButtons.Length];
            this.rightPanelBottomButtonElements = new SToolbarSlot[this.rightPanelBottomButtons.Length];
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.tooltipBoxElement.IsVisible = false;

            SetPlayerInteractionWhenToolbarHovered();
            UpdateTopToolbar();
            UpdateLeftToolbar();
            UpdateRightToolbar();

            this.tooltipBoxElement.RefreshDisplay(SGUIGlobalTooltip.Title, SGUIGlobalTooltip.Description);
        }

        private void SetPlayerInteractionWhenToolbarHovered()
        {
            if (this.GUIEvents.OnMouseOver(this.topToolbarContainer.Position, this.topToolbarContainer.Size) ||
                this.GUIEvents.OnMouseOver(this.leftToolbarContainer.Position, this.leftToolbarContainer.Size) ||
                this.GUIEvents.OnMouseOver(this.rightToolbarContainer.Position, this.rightToolbarContainer.Size))
            {
                this.SGameInstance.GameInputController.Player.CanModifyEnvironment = false;
            }
            else
            {
                this.SGameInstance.GameInputController.Player.CanModifyEnvironment = true;
            }
        }

        private void UpdateTopToolbar()
        {
            UpdateReturnInput();

            #region ELEMENT SLOTS
            for (int i = 0; i < SHUDConstants.ELEMENT_BUTTONS_LENGTH; i++)
            {
                SToolbarSlot slot = this.toolbarElementSlots[i];
                bool isOver = this.GUIEvents.OnMouseOver(slot.Background.Position, new SSize2(SHUDConstants.SLOT_SIZE));

                if (this.GUIEvents.OnMouseClick(slot.Background.Position, new SSize2(SHUDConstants.SLOT_SIZE)))
                {
                    SelectItemSlot(i, (string)slot.Background.GetData(SHUDConstants.DATA_FILED_ELEMENT_ID));
                }

                if (isOver)
                {
                    this.tooltipBoxElement.IsVisible = true;

                    if (!this.tooltipBoxElement.HasContent)
                    {
                        SItem item = this.SGameInstance.ItemDatabase.GetItemById((string)slot.Background.GetData(SHUDConstants.DATA_FILED_ELEMENT_ID));

                        SGUIGlobalTooltip.Title = item.DisplayName;
                        SGUIGlobalTooltip.Description = item.Description;
                    }
                }

                slot.Background.Color = this.slotSelectedIndex == i ?
                                        SColorPalette.OrangeRed :
                                        (isOver ? SColorPalette.EmeraldGreen : SColorPalette.White);
            }
            #endregion

            #region SEARCH BUTTON
            if (this.GUIEvents.OnMouseClick(this.toolbarElementSearchButton.Position, new SSize2(SHUDConstants.SLOT_SIZE)))
            {
                this.SGameInstance.GUIManager.OpenGUI(SGUIConstants.HUD_ITEM_EXPLORER_IDENTIFIER);
            }

            if (this.GUIEvents.OnMouseOver(this.toolbarElementSearchButton.Position, new SSize2(SHUDConstants.SLOT_SIZE)))
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

            void CycleThroughArrayOfButtons(SToolbarSlot[] toolbarSlots, SButton[] buttons, int length)
            {
                for (int i = 0; i < length; i++)
                {
                    SToolbarSlot toolbarSlot = toolbarSlots[i];
                    SButton button = buttons[i];

                    bool isOver = this.GUIEvents.OnMouseOver(toolbarSlot.Background.Position, new SSize2(SHUDConstants.SLOT_SIZE));

                    if (this.GUIEvents.OnMouseClick(toolbarSlot.Background.Position, new SSize2(SHUDConstants.SLOT_SIZE)))
                    {
                        button.ClickAction.Invoke();
                    }

                    if (isOver)
                    {
                        toolbarSlot.Background.Color = SColorPalette.EmeraldGreen;
                        this.tooltipBoxElement.IsVisible = true;

                        if (!this.tooltipBoxElement.HasContent)
                        {
                            SGUIGlobalTooltip.Title = button.DisplayName;
                            SGUIGlobalTooltip.Description = button.Description;
                        }
                    }
                    else
                    {
                        toolbarSlot.Background.Color = SColorPalette.White;
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
            SItem item = this.SGameInstance.ItemDatabase.GetItemById(elementId);

            // ================================================= //
            // Check if the item is already in the Toolbar. If so, it will be highlighted without changing the other items.

            for (int i = 0; i < SHUDConstants.ELEMENT_BUTTONS_LENGTH; i++)
            {
                SToolbarSlot slot = this.toolbarElementSlots[i];

                if (slot.Background.ContainsData(SHUDConstants.DATA_FILED_ELEMENT_ID))
                {
                    if (item == this.SGameInstance.ItemDatabase.GetItemById((string)slot.Background.GetData(SHUDConstants.DATA_FILED_ELEMENT_ID)))
                    {
                        SelectItemSlot(i, (string)slot.Background.GetData(SHUDConstants.DATA_FILED_ELEMENT_ID));
                        return;
                    }
                }
            }

            // ================================================= //
            // If the item is not present in the toolbar, it will be added to the first slot next to the canvas and will push all others in the opposite direction. The last item will be removed from the toolbar until it is added again later.

            for (int i = 0; i < SHUDConstants.ELEMENT_BUTTONS_LENGTH - 1; i++)
            {
                SToolbarSlot currentSlot = this.toolbarElementSlots[i];
                SToolbarSlot nextSlot = this.toolbarElementSlots[i + 1];

                if (currentSlot.Background.ContainsData(SHUDConstants.DATA_FILED_ELEMENT_ID) &&
                    nextSlot.Background.ContainsData(SHUDConstants.DATA_FILED_ELEMENT_ID))
                {
                    currentSlot.Background.UpdateData(SHUDConstants.DATA_FILED_ELEMENT_ID, nextSlot.Background.GetData(SHUDConstants.DATA_FILED_ELEMENT_ID));
                    currentSlot.Icon.Texture = nextSlot.Icon.Texture;
                }
            }

            // Update last element slot.

            SToolbarSlot lastSlot = this.toolbarElementSlots[^1];

            if (lastSlot.Background.ContainsData(SHUDConstants.DATA_FILED_ELEMENT_ID))
            {
                lastSlot.Background.UpdateData(SHUDConstants.DATA_FILED_ELEMENT_ID, item.Identifier);
            }

            lastSlot.Icon.Texture = item.IconTexture;

            // Select last slot.

            SelectItemSlot(this.toolbarElementSlots.Length - 1, item.Identifier);
        }

        private void SelectItemSlot(int slotIndex, string itemId)
        {
            this.slotSelectedIndex = slotIndex;
            this.SGameInstance.GameInputController.Player.SelectItem(this.SGameInstance.ItemDatabase.GetItemById(itemId));
        }
    }
}
