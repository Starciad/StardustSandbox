using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Elements.Informational;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.GUI;
using StardustSandbox.Core.Constants.GUI.Common;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Items;
using StardustSandbox.Core.Mathematics.Primitives;
using StardustSandbox.Core.World;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud
{
    public sealed partial class SGUI_HUD : SGUISystem
    {
        private readonly struct SToolbarSlot(SGUIImageElement background, SGUIImageElement icon)
        {
            public SGUIImageElement Background => background;
            public SGUIImageElement Icon => icon;
        }

        private int slotSelectedIndex = 0;

        private readonly Texture2D particleTexture;
        private readonly Texture2D squareShapeTexture;
        private readonly Texture2D magnifyingGlassIconTexture;

        private readonly SWorld world;

        private readonly SGUITooltipBoxElement tooltipBoxElement;

        private string _currentTooltipElementId = null;

        public SGUI_HUD(ISGame gameInstance, string identifier, SGUIEvents guiEvents, SGUITooltipBoxElement tooltipBoxElement) : base(gameInstance, identifier, guiEvents)
        {
            SelectItemSlot(0, GetGameItemById("element_dirt").Identifier);

            this.particleTexture = this.SGameInstance.AssetDatabase.GetTexture("particle_1");
            this.squareShapeTexture = this.SGameInstance.AssetDatabase.GetTexture("shape_square_1");
            this.magnifyingGlassIconTexture = this.SGameInstance.AssetDatabase.GetTexture("icon_gui_1");

            this.world = gameInstance.World;
            this.tooltipBoxElement = tooltipBoxElement;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateTopToolbar();
            UpdateLeftToolbar();
            UpdateRightToolbar();
        }

        private void UpdateTopToolbar()
        {
            this.SGameInstance.GameInputController.Player.CanModifyEnvironment = !this.GUIEvents.OnMouseOver(this.topToolbarContainer.Position, this.topToolbarContainer.Size);

            bool tooltipVisible = false;
            string hoveredElementId = null;

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
                    tooltipVisible = true;
                    hoveredElementId = (string)slot.Background.GetData(SHUDConstants.DATA_FILED_ELEMENT_ID);
                }

                slot.Background.Color = this.slotSelectedIndex == i ?
                                        SColorPalette.OrangeRed :
                                        (isOver ? SColorPalette.EmeraldGreen : SColorPalette.White);
            }
            #endregion

            if (tooltipVisible)
            {
                if (this._currentTooltipElementId != hoveredElementId)
                {
                    this._currentTooltipElementId = hoveredElementId;
                    SItem item = GetGameItemById(hoveredElementId);

                    this.tooltipBoxElement.SetTitle(item.DisplayName);
                    this.tooltipBoxElement.SetDescription(item.Description);
                }

                if (!this.tooltipBoxElement.IsShowing)
                {
                    this.tooltipBoxElement.Show();
                }
            }
            else
            {
                this.tooltipBoxElement.Hide();
                this._currentTooltipElementId = null;
            }

            #region SEARCH BUTTON
            if (this.GUIEvents.OnMouseClick(this.toolbarElementSearchButton.Position, new SSize2(SHUDConstants.SLOT_SIZE)))
            {
                this.SGameInstance.GUIManager.CloseGUI(SGUIConstants.HUD_IDENTIFIER);
                this.SGameInstance.GUIManager.ShowGUI(SGUIConstants.HUD_ELEMENT_EXPLORER_IDENTIFIER);
            }

            this.toolbarElementSearchButton.Color = this.GUIEvents.OnMouseOver(this.toolbarElementSearchButton.Position, new SSize2(SHUDConstants.SLOT_SIZE))
                ? Color.DarkGray
                : Color.White;
            #endregion
        }

        private static void UpdateLeftToolbar()
        {
            return;
        }
        private static void UpdateRightToolbar()
        {
            return;
        }

        public void AddItemToToolbar(string elementId)
        {
            SItem item = GetGameItemById(elementId);

            // ================================================= //
            // Check if the item is already in the Toolbar. If so, it will be highlighted without changing the other items.

            for (int i = 0; i < SHUDConstants.ELEMENT_BUTTONS_LENGTH; i++)
            {
                SToolbarSlot slot = this.toolbarElementSlots[i];

                if (slot.Background.ContainsData(SHUDConstants.DATA_FILED_ELEMENT_ID))
                {
                    if (item == GetGameItemById((string)slot.Background.GetData(SHUDConstants.DATA_FILED_ELEMENT_ID)))
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

        public SItem GetGameItemById(string id)
        {
            return this.SGameInstance.ItemDatabase.GetItemById(id);
        }

        private void SelectItemSlot(int slotIndex, string itemId)
        {
            this.slotSelectedIndex = slotIndex;
            this.SGameInstance.GameInputController.Player.SelectItem(GetGameItemById(itemId));
        }
    }
}
