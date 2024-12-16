using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Elements.Informational;
using StardustSandbox.ContentBundle.GUISystem.Global;
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

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud
{
    internal sealed partial class SGUI_HUD : SGUISystem
    {
        private readonly struct SToolbarSlot(SGUIImageElement background, SGUIImageElement icon)
        {
            internal SGUIImageElement Background => background;
            internal SGUIImageElement Icon => icon;
        }

        private int slotSelectedIndex = 0;

        private readonly Texture2D particleTexture;
        private readonly Texture2D squareShapeTexture;
        private readonly Texture2D magnifyingGlassIconTexture;

        private readonly ISWorld world;

        private readonly SGUITooltipBoxElement tooltipBoxElement;

        internal SGUI_HUD(ISGame gameInstance, string identifier, SGUIEvents guiEvents, SGUITooltipBoxElement tooltipBoxElement) : base(gameInstance, identifier, guiEvents)
        {
            SelectItemSlot(0, this.SGameInstance.ItemDatabase.GetItemById("element_dirt").Identifier);

            this.particleTexture = this.SGameInstance.AssetDatabase.GetTexture("particle_1");
            this.squareShapeTexture = this.SGameInstance.AssetDatabase.GetTexture("shape_square_1");
            this.magnifyingGlassIconTexture = this.SGameInstance.AssetDatabase.GetTexture("icon_gui_1");

            this.world = gameInstance.World;
            this.tooltipBoxElement = tooltipBoxElement;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.tooltipBoxElement.IsVisible = false;

            UpdateTopToolbar();
            UpdateLeftToolbar();
            UpdateRightToolbar();

            this.tooltipBoxElement.RefreshDisplay(SGUIGlobalTooltip.Title, SGUIGlobalTooltip.Description);
        }

        private void UpdateTopToolbar()
        {
            this.SGameInstance.GameInputController.Player.CanModifyEnvironment = !this.GUIEvents.OnMouseOver(this.topToolbarContainer.Position, this.topToolbarContainer.Size);

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
