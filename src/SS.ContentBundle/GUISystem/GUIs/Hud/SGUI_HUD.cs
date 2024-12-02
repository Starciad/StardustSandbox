using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements;
using StardustSandbox.Core.Constants.GUI;
using StardustSandbox.Core.Constants.GUI.Common;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Items;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud
{
    public sealed partial class SGUI_HUD : SGUISystem
    {
        private readonly Texture2D particleTexture;
        private readonly Texture2D squareShapeTexture;

        private int slotSelectedIndex = 0;

        public SGUI_HUD(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance, identifier, guiEvents)
        {
            this.particleTexture = this.SGameInstance.AssetDatabase.GetTexture("particle_1");
            this.squareShapeTexture = this.SGameInstance.AssetDatabase.GetTexture("shape_square_1");

            SelectItemSlot(0, GetGameItemById("element_dirt").Identifier);
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
            // If the mouse is over the header, the player will not be able to interact with the environment. Otherwise, this permission is conceived.
            this.SGameInstance.GameInputManager.CanModifyEnvironment = !this.GUIEvents.OnMouseOver(this.topToolbarContainer.Position, this.topToolbarContainer.Size);

            #region ELEMENT SLOTS
            // Individually check all element slots present in the HEADER.
            for (int i = 0; i < SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_LENGTH; i++)
            {
                SToolbarSlot slot = this.toolbarElementSlots[i];

                // Check if the mouse clicked on the current slot.
                if (this.GUIEvents.OnMouseClick(slot.Background.Position, new SSize2(SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE)))
                {
                    SelectItemSlot(i, (string)slot.Background.GetData(SHUDConstants.DATA_FILED_ELEMENT_ID));
                }

                // Highlight coloring of currently selected slot.
                slot.Background.Color = i == this.slotSelectedIndex
                    ? Color.Red
                    : this.GUIEvents.OnMouseOver(slot.Background.Position, new SSize2(SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE))
                        ? Color.DarkGray
                        : Color.White;
            }
            #endregion

            #region SEARCH BUTTON
            // Check if the mouse clicked on the search button.
            if (this.GUIEvents.OnMouseClick(this.toolbarElementSearchButton.Position, new SSize2(SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE)))
            {
                this.SGameInstance.GUIManager.CloseGUI(SGUIConstants.HUD_IDENTIFIER);
                this.SGameInstance.GUIManager.ShowGUI(SGUIConstants.HUD_ELEMENT_EXPLORER_IDENTIFIER);
            }

            this.toolbarElementSearchButton.Color = this.GUIEvents.OnMouseOver(this.toolbarElementSearchButton.Position, new SSize2(SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE))
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

            for (int i = 0; i < SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_LENGTH; i++)
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

            for (int i = 0; i < SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_LENGTH - 1; i++)
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
            this.SGameInstance.GameInputManager.SelectItem(GetGameItemById(itemId));
        }

        private SItem GetGameItemById(string id)
        {
            return this.SGameInstance.ItemDatabase.GetItemById(id);
        }
    }
}
