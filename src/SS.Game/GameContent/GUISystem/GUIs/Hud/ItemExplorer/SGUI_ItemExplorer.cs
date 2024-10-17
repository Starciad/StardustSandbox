using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Constants.GUI;
using StardustSandbox.Game.Constants.GUI.Common;
using StardustSandbox.Game.GameContent.GUISystem.Elements.Graphics;
using StardustSandbox.Game.GUI.Events;
using StardustSandbox.Game.GUISystem;
using StardustSandbox.Game.Items;
using StardustSandbox.Game.Mathematics;

using System;
using System.Linq;

namespace StardustSandbox.Game.GameContent.GUISystem.GUIs.Hud.ItemExplorer
{
    public sealed partial class SGUI_ItemExplorer : SGUISystem
    {
        private readonly Texture2D particleTexture;
        private readonly Texture2D guiBackgroundTexture;
        private readonly Texture2D squareShapeTexture;

        private string selectedCategoryName;
        private int selectedPageIndex;
        private SItem[] selectedItems;

        private readonly SGUI_HUD _guiHUD;

        public SGUI_ItemExplorer(SGame gameInstance, SGUIEvents guiEvents, SGUI_HUD guiHUD) : base(gameInstance, guiEvents)
        {
            this.Name = SGUIConstants.ELEMENT_EXPLORER_NAME;

            this.particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
            this.guiBackgroundTexture = gameInstance.AssetDatabase.GetTexture("gui_background_1");
            this.squareShapeTexture = gameInstance.AssetDatabase.GetTexture("shape_square_1");

            this._guiHUD = guiHUD;
        }

        protected override void OnLoad()
        {
            this.SGameInstance.GameInputManager.CanModifyEnvironment = false;
        }

        protected override void OnUnload()
        {
            this.SGameInstance.GameInputManager.CanModifyEnvironment = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateItemCatalog();
        }

        private void UpdateItemCatalog()
        {
            // Individually check all element slots present in the item catalog.
            for (int i = 0; i < this.itemSlots.Length; i++)
            {
                (SGUIImageElement itemSlotBackground, _) = this.itemSlots[i];

                if (!itemSlotBackground.IsVisible)
                {
                    continue;
                }

                // Check if the mouse clicked on the current slot.
                if (this.GUIEvents.OnMouseClick(itemSlotBackground.Position, new SSize2(SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE)))
                {
                    this._guiHUD.AddItemToToolbar((string)itemSlotBackground.GetData(SHUDConstants.DATA_FILED_ELEMENT_ID));

                    this.SGameInstance.GUIManager.CloseGUI(SGUIConstants.ELEMENT_EXPLORER_NAME);
                    this.SGameInstance.GUIManager.ShowGUI(SGUIConstants.HUD_NAME);
                }

                // Highlight when mouse is over slot.
                if (this.GUIEvents.OnMouseOver(itemSlotBackground.Position, new SSize2(SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE)))
                {
                    itemSlotBackground.SetColor(Color.DarkGray);
                }
                // If none of the above events occur, the slot continues with its normal color.
                else
                {
                    itemSlotBackground.SetColor(Color.White);
                }
            }
        }

        // ============================================== //

        private void SelectItemCatalog(int categoryIndex, int pageIndex)
        {
            SelectItemCatalog(this.SGameInstance.ItemDatabase.Categories[categoryIndex], pageIndex);
        }

        private void SelectItemCatalog(SItemCategory category, int pageIndex)
        {
            this.explorerTitleLabel.SetTextContent(category.DisplayName);

            this.selectedCategoryName = category.Identifier;
            this.selectedPageIndex = pageIndex;

            int itemsPerPage = SItemExplorerConstants.ITEMS_PER_PAGE;

            int startIndex = pageIndex * itemsPerPage;
            int endIndex = startIndex + itemsPerPage;

            endIndex = Math.Min(endIndex, this.SGameInstance.ItemDatabase.Items.Length);

            this.selectedItems = [.. category.Items.Take(new Range(startIndex, endIndex - startIndex))];

            ChangeItemCatalog();
        }

        private void ChangeItemCatalog()
        {
            for (int i = 0; i < this.itemSlots.Length; i++)
            {
                (SGUIImageElement itemSlotBackground, SGUIImageElement itemSlotIcon) = this.itemSlots[i];

                if (i < this.selectedItems.Length)
                {
                    itemSlotBackground.IsVisible = true;
                    itemSlotIcon.IsVisible = true;

                    SItem item = this.selectedItems[i];
                    itemSlotIcon.SetTexture(item.IconTexture);

                    // Add or Update Data
                    if (!itemSlotBackground.ContainsData(SHUDConstants.DATA_FILED_ELEMENT_ID))
                    {
                        itemSlotBackground.AddData(SHUDConstants.DATA_FILED_ELEMENT_ID, item.Identifier);
                    }
                    else
                    {
                        itemSlotBackground.UpdateData(SHUDConstants.DATA_FILED_ELEMENT_ID, item.Identifier);
                    }
                }
                else
                {
                    itemSlotBackground.IsVisible = false;
                    itemSlotIcon.IsVisible = false;
                }
            }
        }
    }
}
