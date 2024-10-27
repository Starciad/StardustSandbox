using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Constants.GUI;
using StardustSandbox.Game.Constants.GUI.Common;
using StardustSandbox.Game.GUI.Events;
using StardustSandbox.Game.GUISystem;
using StardustSandbox.Game.Items;
using StardustSandbox.Game.Mathematics.Primitives;
using StardustSandbox.Game.Resources.GUISystem.Elements.Graphics;

namespace StardustSandbox.Game.Resources.GUISystem.Bundle.Hud
{
    public sealed partial class SGUI_HUD : SGUISystem
    {
        private readonly Texture2D particleTexture;
        private readonly Texture2D squareShapeTexture;

        private int slotSelectedIndex = 0;

        public SGUI_HUD(SGame gameInstance, SGUIEvents guiEvents) : base(gameInstance, guiEvents)
        {
            this.Name = SGUIConstants.HUD_NAME;

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
                (SGUIImageElement slotBackground, _) = this.toolbarElementSlots[i];

                // Check if the mouse clicked on the current slot.
                if (this.GUIEvents.OnMouseClick(slotBackground.Position, new SSize2(SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE)))
                {
                    SelectItemSlot(i, (string)slotBackground.GetData(SHUDConstants.DATA_FILED_ELEMENT_ID));
                }

                // Highlight coloring of currently selected slot.
                if (i == this.slotSelectedIndex)
                {
                    slotBackground.SetColor(Color.Red);
                }
                // Highlight when mouse is over slot.
                else if (this.GUIEvents.OnMouseOver(slotBackground.Position, new SSize2(SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE)))
                {
                    slotBackground.SetColor(Color.DarkGray);
                }
                // If none of the above events occur, the slot continues with its normal color.
                else
                {
                    slotBackground.SetColor(Color.White);
                }
            }
            #endregion

            #region SEARCH BUTTON
            // Check if the mouse clicked on the search button.
            if (this.GUIEvents.OnMouseDown(this.toolbarElementSearchButton.Position, new SSize2(SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE)))
            {
                this.SGameInstance.GUIManager.CloseGUI(SGUIConstants.HUD_NAME);
                this.SGameInstance.GUIManager.ShowGUI(SGUIConstants.ELEMENT_EXPLORER_NAME);
            }

            if (this.GUIEvents.OnMouseOver(this.toolbarElementSearchButton.Position, new SSize2(SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE)))
            {
                this.toolbarElementSearchButton.SetColor(Color.DarkGray);
            }
            else
            {
                this.toolbarElementSearchButton.SetColor(Color.White);
            }
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
                (SGUIImageElement slotBackground, _) = this.toolbarElementSlots[i];

                if (slotBackground.ContainsData(SHUDConstants.DATA_FILED_ELEMENT_ID))
                {
                    if (item == GetGameItemById((string)slotBackground.GetData(SHUDConstants.DATA_FILED_ELEMENT_ID)))
                    {
                        SelectItemSlot(i, (string)slotBackground.GetData(SHUDConstants.DATA_FILED_ELEMENT_ID));
                        return;
                    }
                }
            }

            // ================================================= //
            // If the item is not present in the toolbar, it will be added to the first slot next to the canvas and will push all others in the opposite direction. The last item will be removed from the toolbar until it is added again later.

            for (int i = 0; i < SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_LENGTH - 1; i++)
            {
                (SGUIImageElement currentSlotBackground, SGUIImageElement currentSlotIcon) = this.toolbarElementSlots[i];
                (SGUIImageElement nextSlotBackground, SGUIImageElement nextSlotIcon) = this.toolbarElementSlots[i + 1];

                if (currentSlotBackground.ContainsData(SHUDConstants.DATA_FILED_ELEMENT_ID) &&
                    nextSlotBackground.ContainsData(SHUDConstants.DATA_FILED_ELEMENT_ID))
                {
                    currentSlotBackground.UpdateData(SHUDConstants.DATA_FILED_ELEMENT_ID, nextSlotBackground.GetData(SHUDConstants.DATA_FILED_ELEMENT_ID));
                    currentSlotIcon.SetTexture(nextSlotIcon.Texture);
                }
            }

            // Update last element slot.

            (SGUIImageElement lastElementBackground, SGUIImageElement lastElementIcon) = this.toolbarElementSlots[^1];

            if (lastElementBackground.ContainsData(SHUDConstants.DATA_FILED_ELEMENT_ID))
            {
                lastElementBackground.UpdateData(SHUDConstants.DATA_FILED_ELEMENT_ID, item.Identifier);
            }

            lastElementIcon.SetTexture(item.IconTexture);

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
