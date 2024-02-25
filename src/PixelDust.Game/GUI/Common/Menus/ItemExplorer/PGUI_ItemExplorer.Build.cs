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

namespace PixelDust.Game.GUI.Common.Menus.ItemExplorer
{
    public sealed partial class PGUI_ItemExplorer
    {
        private IPGUILayoutBuilder _layout;
        private PGUIRootElement _rootElement;

        private PGUILabelElement explorerTitleLabel;
        private PGUISliceImageElement itemGridBackground;

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
            this.itemGridBackground = this._layout.CreateElement<PGUISliceImageElement>();
            this.itemGridBackground.SetTexture(this.guiBackgroundTexture);
            this.itemGridBackground.SetScale(new Vector2(30, 10));
            this.itemGridBackground.SetMargin(new Vector2(32, 88));
            this.itemGridBackground.SetColor(new Color(94, 101, 110, 255));
            this.itemGridBackground.PositionRelativeToElement(explorerBackground);
            #endregion

            #region Pagination
            // [...]
            #endregion
        }

        // ================================== //
        // Updates

        private void UpdateItemsGrid()
        {
            int slotSize = PHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE;
            int slotScale = PHUDConstants.SLOT_SCALE;
            int slotSpacing = slotSize * 2;

            Vector2 slotMargin = new(32);

            int itemsPerRow = PItemExplorerConstants.ITEMS_PER_ROW;

            int rows = (int)Math.Ceiling((double)this.selectedItems.Length / itemsPerRow);
            int columns = Math.Min(itemsPerRow, this.selectedItems.Length);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    int index = row * itemsPerRow + col;

                    if (index < this.selectedItems.Length)
                    {
                        PItem item = this.selectedItems[index];

                        PGUIImageElement slotBackground = this._layout.CreateElement<PGUIImageElement>();
                        slotBackground.SetTexture(this.squareShapeTexture);
                        slotBackground.SetOriginPivot(PCardinalDirection.Center);
                        slotBackground.SetScale(new Vector2(slotScale));
                        slotBackground.SetPositionAnchor(PCardinalDirection.West);
                        slotBackground.SetSize(new Size2(slotSize));
                        slotBackground.SetMargin(slotMargin);

                        PGUIImageElement slotIcon = this._layout.CreateElement<PGUIImageElement>();
                        slotIcon.SetTexture(item.IconTexture);
                        slotIcon.SetOriginPivot(PCardinalDirection.Center);
                        slotIcon.SetScale(new Vector2(1.5f));
                        slotIcon.SetSize(new Size2(slotSize));

                        slotBackground.PositionRelativeToElement(this.itemGridBackground);
                        slotIcon.PositionRelativeToElement(slotBackground);

                        slotMargin.X += slotSpacing + (slotSize / 2);
                    }
                }

                slotMargin.X = 32;
                slotMargin.Y += slotSpacing + (slotSize / 2);
            }
        }
    }
}
