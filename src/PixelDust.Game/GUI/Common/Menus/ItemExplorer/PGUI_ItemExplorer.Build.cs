using Microsoft.Xna.Framework;

using PixelDust.Game.Constants;
using PixelDust.Game.Constants.GUI.Common;
using PixelDust.Game.Enums.General;
using PixelDust.Game.GUI.Elements;
using PixelDust.Game.GUI.Elements.Common;
using PixelDust.Game.GUI.Elements.Common.Graphics;
using PixelDust.Game.Interfaces.GUI;
using PixelDust.Game.Items;
using PixelDust.Game.Mathematics;

using System;
using System.Reflection;

namespace PixelDust.Game.GUI.Common.Menus.ItemExplorer
{
    public sealed partial class PGUI_ItemExplorer
    {
        private IPGUILayoutBuilder _layout;
        private PGUIRootElement _rootElement;

        private PGUILabelElement explorerTitleLabel;
        private (PGUIImageElement background, PGUIImageElement icon)[] itemSlots;

        protected override void OnBuild(IPGUILayoutBuilder layout)
        {
            this._layout = layout;
            this._rootElement = layout.RootElement;

            BuildGUIBackground();
            BuildCategoryButtons();
            BuildExplorer();

            SelectItemCatalog(0, 0);
        }

        private void BuildGUIBackground()
        {
            PGUIImageElement guiBackground = this._layout.CreateElement<PGUIImageElement>();
            guiBackground.SetTexture(this.particleTexture);
            guiBackground.SetScale(this._rootElement.Size.ToVector2());
            guiBackground.SetSize(this._rootElement.Size);
            guiBackground.SetColor(new Color(Color.Black, 160));
        }

        // ================================== //

        private void BuildCategoryButtons()
        {

        }

        private void BuildExplorer()
        {
            #region Background & Title
            PGUISliceImageElement explorerBackground = this._layout.CreateElement<PGUISliceImageElement>();
            explorerBackground.SetTexture(this.guiBackgroundTexture);
            explorerBackground.SetScale(new Vector2(32, 15));
            explorerBackground.SetMargin(new Vector2(128, 128));
            explorerBackground.SetColor(new Color(104, 111, 121, 255));
            explorerBackground.PositionRelativeToElement(this._rootElement);

            this.explorerTitleLabel = this._layout.CreateElement<PGUILabelElement>();
            this.explorerTitleLabel.SetTextContent("TITLE");
            this.explorerTitleLabel.SetScale(new Vector2(0.15f));
            this.explorerTitleLabel.SetMargin(new Vector2(18, -16));
            this.explorerTitleLabel.SetColor(new Color(206, 214, 237, 255));
            this.explorerTitleLabel.SetFontFamily(PFontFamilyConstants.BIG_APPLE_3PM);
            this.explorerTitleLabel.SetBorders(true);
            this.explorerTitleLabel.SetBordersColor(new Color(45, 53, 74, 255));
            this.explorerTitleLabel.SetBorderOffset(new Vector2(4.4f));
            this.explorerTitleLabel.PositionRelativeToElement(explorerBackground);
            #endregion

            #region ITEM DISPLAY
            // Background
            PGUISliceImageElement itemGridBackground = this._layout.CreateElement<PGUISliceImageElement>();
            itemGridBackground.SetTexture(this.guiBackgroundTexture);
            itemGridBackground.SetScale(new Vector2(30, 10));
            itemGridBackground.SetMargin(new Vector2(32, 88));
            itemGridBackground.SetColor(new Color(94, 101, 110, 255));
            itemGridBackground.PositionRelativeToElement(explorerBackground);

            BuildItemsGrid(itemGridBackground);
            #endregion

            #region Pagination
            // [...]
            #endregion
        }

        // ================================== //
        // Updates

        private void BuildItemsGrid(PGUIElement parent)
        {
            int slotSize = PHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE;
            int slotScale = PHUDConstants.SLOT_SCALE;
            int slotSpacing = slotSize * 2;

            Vector2 slotMargin = new(32, 40);

            int rows = PItemExplorerConstants.ITEMS_PER_ROW;
            int columns = PItemExplorerConstants.ITEMS_PER_COLUMN;

            this.itemSlots = new (PGUIImageElement background, PGUIImageElement icon)[rows * columns];

            int index = 0;
            for (int col = 0; col < columns; col++)
            {
                for (int row = 0; row < rows; row++)
                {
                    PGUIImageElement slotBackground = this._layout.CreateElement<PGUIImageElement>();
                    slotBackground.SetTexture(this.squareShapeTexture);
                    slotBackground.SetOriginPivot(PCardinalDirection.Center);
                    slotBackground.SetScale(new Vector2(slotScale));
                    slotBackground.SetPositionAnchor(PCardinalDirection.West);
                    slotBackground.SetSize(new Size2(slotSize));
                    slotBackground.SetMargin(slotMargin);

                    PGUIImageElement slotIcon = this._layout.CreateElement<PGUIImageElement>();
                    slotIcon.SetOriginPivot(PCardinalDirection.Center);
                    slotIcon.SetScale(new Vector2(1.5f));
                    slotIcon.SetSize(new Size2(slotSize));

                    slotBackground.PositionRelativeToElement(parent);
                    slotIcon.PositionRelativeToElement(slotBackground);

                    slotMargin.X += slotSpacing + (slotSize / 2);
                    this.itemSlots[index] = (slotBackground, slotIcon);
                    index++;
                }

                slotMargin.X = 32;
                slotMargin.Y += slotSpacing + (slotSize / 2);
            }
        }

        private void UpdateItemsGrid()
        {
            for (int i = 0; i < this.itemSlots.Length; i++)
            {
                (PGUIImageElement itemSlotbackground, PGUIImageElement itemSlotIcon) = this.itemSlots[i];

                if (i < selectedItems.Length)
                {
                    PItem item = this.selectedItems[i];
                    itemSlotIcon.SetTexture(item.IconTexture);
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
