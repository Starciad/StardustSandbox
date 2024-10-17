using Microsoft.Xna.Framework;

using StardustSandbox.Game.Constants;
using StardustSandbox.Game.Constants.GUI.Common;
using StardustSandbox.Game.Enums.General;
using StardustSandbox.Game.GameContent.GUI.Elements;
using StardustSandbox.Game.GameContent.GUI.Elements.Graphics;
using StardustSandbox.Game.GUI.Elements;
using StardustSandbox.Game.Interfaces.GUI;
using StardustSandbox.Game.Items;
using StardustSandbox.Game.Mathematics;

using System.Configuration;

namespace StardustSandbox.Game.GameContent.GUI.Content.Menus.ItemExplorer
{
    public sealed partial class SGUI_ItemExplorer
    {
        private ISGUILayoutBuilder _layout;
        private SGUIRootElement _rootElement;

        private SGUILabelElement explorerTitleLabel;
        private (SGUIImageElement background, SGUIImageElement icon)[] itemSlots;

        protected override void OnBuild(ISGUILayoutBuilder layout)
        {
            this._layout = layout;
            this._rootElement = layout.RootElement;

            BuildGUIBackground();
            BuildExplorer();

            SelectItemCatalog(0, 0);
        }

        private void BuildGUIBackground()
        {
            SGUIImageElement guiBackground = new(this.SGameInstance);
            guiBackground.SetTexture(this.particleTexture);
            guiBackground.SetScale(this._rootElement.Size.ToVector2());
            guiBackground.SetSize(this._rootElement.Size);
            guiBackground.SetColor(new Color(Color.Black, 160));

            this._layout.AddElement(guiBackground);
        }

        // ================================== //

        private void BuildExplorer()
        {
            #region Background & Title
            SGUISliceImageElement explorerBackground = new(this.SGameInstance);
            explorerBackground.SetTexture(this.guiBackgroundTexture);
            explorerBackground.SetScale(new Vector2(32, 15));
            explorerBackground.SetMargin(new Vector2(128, 128));
            explorerBackground.SetColor(new Color(104, 111, 121, 255));
            explorerBackground.PositionRelativeToElement(this._rootElement);

            this.explorerTitleLabel = new(this.SGameInstance);
            this.explorerTitleLabel.SetTextContent("TITLE");
            this.explorerTitleLabel.SetScale(new Vector2(0.15f));
            this.explorerTitleLabel.SetMargin(new Vector2(18, -16));
            this.explorerTitleLabel.SetColor(new Color(206, 214, 237, 255));
            this.explorerTitleLabel.SetFontFamily(SFontFamilyConstants.BIG_APPLE_3PM);
            this.explorerTitleLabel.SetBorders(true);
            this.explorerTitleLabel.SetBordersColor(new Color(45, 53, 74, 255));
            this.explorerTitleLabel.SetBorderOffset(new Vector2(4.4f));
            this.explorerTitleLabel.PositionRelativeToElement(explorerBackground);

            this._layout.AddElement(explorerBackground);
            this._layout.AddElement(this.explorerTitleLabel);
            #endregion

            #region ITEM DISPLAY
            // Background
            SGUISliceImageElement itemGridBackground = new(this.SGameInstance);
            itemGridBackground.SetTexture(this.guiBackgroundTexture);
            itemGridBackground.SetScale(new Vector2(30, 10));
            itemGridBackground.SetMargin(new Vector2(32, 88));
            itemGridBackground.SetColor(new Color(94, 101, 110, 255));
            itemGridBackground.PositionRelativeToElement(explorerBackground);

            this._layout.AddElement(itemGridBackground);
            BuildItemCatalog(itemGridBackground);
            #endregion

            #region CATEGORY BUTTONS
            BuildCategoryButtons(itemGridBackground);
            #endregion

            #region Pagination
            // [...]
            #endregion
        }

        private void BuildItemCatalog(SGUIElement parent)
        {
            int slotSize = SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE;
            int slotScale = SHUDConstants.SLOT_SCALE;
            int slotSpacing = slotSize * 2;

            Vector2 slotMargin = new(32, 40);

            int rows = SItemExplorerConstants.ITEMS_PER_ROW;
            int columns = SItemExplorerConstants.ITEMS_PER_COLUMN;

            this.itemSlots = new (SGUIImageElement background, SGUIImageElement icon)[rows * columns];

            int index = 0;
            for (int col = 0; col < columns; col++)
            {
                for (int row = 0; row < rows; row++)
                {
                    SGUIImageElement slotBackground = new(this.SGameInstance);
                    SGUIImageElement slotIcon = new(this.SGameInstance);

                    // Background
                    slotBackground.SetTexture(this.squareShapeTexture);
                    slotBackground.SetOriginPivot(SCardinalDirection.Center);
                    slotBackground.SetScale(new Vector2(slotScale));
                    slotBackground.SetPositionAnchor(SCardinalDirection.West);
                    slotBackground.SetSize(new SSize2(slotSize));
                    slotBackground.SetMargin(slotMargin);

                    // Icon
                    slotIcon.SetOriginPivot(SCardinalDirection.Center);
                    slotIcon.SetScale(new Vector2(1.5f));
                    slotIcon.SetSize(new SSize2(slotSize));

                    // Position
                    slotBackground.PositionRelativeToElement(parent);
                    slotIcon.PositionRelativeToElement(slotBackground);

                    // Spacing
                    slotMargin.X += slotSpacing + (slotSize / 2);
                    this.itemSlots[index] = (slotBackground, slotIcon);
                    index++;

                    // Adding
                    this._layout.AddElement(slotBackground);
                    this._layout.AddElement(slotIcon);
                }

                slotMargin.X = 32;
                slotMargin.Y += slotSpacing + (slotSize / 2);
            }
        }
        private void BuildCategoryButtons(SGUIElement parent)
        {

        }

        // ================================== //
        // Updates
        private void UpdateItemCatalog()
        {
            for (int i = 0; i < this.itemSlots.Length; i++)
            {
                (SGUIImageElement itemSlotbackground, SGUIImageElement itemSlotIcon) = this.itemSlots[i];

                if (i < this.selectedItems.Length)
                {
                    SItem item = this.selectedItems[i];
                    itemSlotIcon.SetTexture(item.IconTexture);

                    // Add or Update Data
                    if (!itemSlotbackground.ContainsData(SHUDConstants.DATA_FILED_ELEMENT_ID))
                    {
                        itemSlotbackground.AddData(SHUDConstants.DATA_FILED_ELEMENT_ID, item.Identifier);
                    }
                    else
                    {
                        itemSlotbackground.UpdateData(SHUDConstants.DATA_FILED_ELEMENT_ID, item.Identifier);
                    }
                }
                else
                {
                    itemSlotbackground.SetTexture(null);
                    itemSlotIcon.SetTexture(null);
                }
            }
        }
    }
}
