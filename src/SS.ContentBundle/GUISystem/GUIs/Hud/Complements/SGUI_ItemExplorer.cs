using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants.GUI;
using StardustSandbox.Core.Constants.GUI.Common;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Elements.Graphics;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Items;
using StardustSandbox.Core.Mathematics.Primitives;

using System;
using System.Linq;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements
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

        public SGUI_ItemExplorer(ISGame gameInstance, string identifier, SGUIEvents guiEvents, SGUI_HUD guiHUD) : base(gameInstance, identifier, guiEvents)
        {
            this.particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
            this.guiBackgroundTexture = gameInstance.AssetDatabase.GetTexture("gui_background_1");
            this.squareShapeTexture = gameInstance.AssetDatabase.GetTexture("shape_square_1");

            this._guiHUD = guiHUD;
        }

        protected override void OnLoad()
        {
            this.SGameInstance.GameManager.GameState.IsSimulationPaused = true;
        }

        protected override void OnUnload()
        {
            this.SGameInstance.GameManager.GameState.IsSimulationPaused = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateCategoryButtons();
            UpdateItemCatalog();
        }

        private void UpdateCategoryButtons()
        {
            for (int i = 0; i < this.categoryButtonSlots.Length; i++)
            {
                (SGUIImageElement categoryButtonBackground, _) = this.categoryButtonSlots[i];

                // Check if the mouse clicked on the current slot.
                if (this.GUIEvents.OnMouseClick(categoryButtonBackground.Position, new SSize2(SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE)))
                {
                    SelectItemCatalog((string)categoryButtonBackground.GetData("category_id"), 0);
                }
            }
        }

        private void UpdateItemCatalog()
        {
            // Individually check all element slots present in the item catalog.
            for (int i = 0; i < this.itemButtonSlots.Length; i++)
            {
                (SGUIImageElement itemSlotBackground, _) = this.itemButtonSlots[i];

                if (!itemSlotBackground.IsVisible)
                {
                    continue;
                }

                // Check if the mouse clicked on the current slot.
                if (this.GUIEvents.OnMouseClick(itemSlotBackground.Position, new SSize2(SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE)))
                {
                    this._guiHUD.AddItemToToolbar((string)itemSlotBackground.GetData(SHUDConstants.DATA_FILED_ELEMENT_ID));

                    this.SGameInstance.GUIManager.CloseGUI(SGUIConstants.HUD_ELEMENT_EXPLORER_IDENTIFIER);
                    this.SGameInstance.GUIManager.ShowGUI(SGUIConstants.HUD_IDENTIFIER);
                }

                // Highlight when mouse is over slot.
                itemSlotBackground.Color = this.GUIEvents.OnMouseOver(itemSlotBackground.Position, new SSize2(SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE))
                    ? Color.DarkGray
                    : Color.White;
            }
        }

        // ============================================== //

        private void SelectItemCatalog(string categoryId, int pageIndex)
        {
            SelectItemCatalog(this.SGameInstance.ItemDatabase.GetCategoryById(categoryId), pageIndex);
        }

        private void SelectItemCatalog(SItemCategory category, int pageIndex)
        {
            this.explorerTitleLabel.SetTextContent(category.DisplayName);

            this.selectedCategoryName = category.Identifier;
            this.selectedPageIndex = pageIndex;

            int itemsPerPage = SItemExplorerConstants.ITEMS_PER_PAGE;

            int startIndex = pageIndex * itemsPerPage;
            int endIndex = startIndex + itemsPerPage;

            endIndex = Math.Min(endIndex, this.SGameInstance.ItemDatabase.TotalItemCount);

            this.selectedItems = [.. category.Items.Take(new Range(startIndex, endIndex - startIndex))];

            ChangeItemCatalog();
        }

        private void ChangeItemCatalog()
        {
            for (int i = 0; i < this.itemButtonSlots.Length; i++)
            {
                (SGUIImageElement itemSlotBackground, SGUIImageElement itemSlotIcon) = this.itemButtonSlots[i];

                if (i < this.selectedItems.Length)
                {
                    itemSlotBackground.IsVisible = true;
                    itemSlotIcon.IsVisible = true;

                    SItem item = this.selectedItems[i];
                    itemSlotIcon.Texture = item.IconTexture;

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
