using Microsoft.Xna.Framework;

using StardustSandbox.Game.Constants;
using StardustSandbox.Game.Constants.GUI.Common;
using StardustSandbox.Game.Enums.General;
using StardustSandbox.Game.GUI.Elements;
using StardustSandbox.Game.GUI.Elements.Common;
using StardustSandbox.Game.GUI.Elements.Common.Graphics;
using StardustSandbox.Game.Interfaces.GUI;
using StardustSandbox.Game.Items;
using StardustSandbox.Game.Mathematics;

namespace StardustSandbox.Game.GUI.Common.Menus.ItemExplorer
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
            SGUIImageElement guiBackground = this._layout.CreateElement<SGUIImageElement>();
            guiBackground.SetTexture(this.particleTexture);
            guiBackground.SetScale(this._rootElement.Size.ToVector2());
            guiBackground.SetSize(this._rootElement.Size);
            guiBackground.SetColor(new Color(Color.Black, 160));
        }

        // ================================== //

        private void BuildExplorer()
        {
            #region Background & Title
            SGUISliceImageElement explorerBackground = this._layout.CreateElement<SGUISliceImageElement>();
            explorerBackground.SetTexture(this.guiBackgroundTexture);
            explorerBackground.SetScale(new Vector2(32, 15));
            explorerBackground.SetMargin(new Vector2(128, 128));
            explorerBackground.SetColor(new Color(104, 111, 121, 255));
            explorerBackground.PositionRelativeToElement(this._rootElement);

            this.explorerTitleLabel = this._layout.CreateElement<SGUILabelElement>();
            this.explorerTitleLabel.SetTextContent("TITLE");
            this.explorerTitleLabel.SetScale(new Vector2(0.15f));
            this.explorerTitleLabel.SetMargin(new Vector2(18, -16));
            this.explorerTitleLabel.SetColor(new Color(206, 214, 237, 255));
            this.explorerTitleLabel.SetFontFamily(SFontFamilyConstants.BIG_APPLE_3PM);
            this.explorerTitleLabel.SetBorders(true);
            this.explorerTitleLabel.SetBordersColor(new Color(45, 53, 74, 255));
            this.explorerTitleLabel.SetBorderOffset(new Vector2(4.4f));
            this.explorerTitleLabel.PositionRelativeToElement(explorerBackground);
            #endregion

            #region ITEM DISPLAY
            // Background
            SGUISliceImageElement itemGridBackground = this._layout.CreateElement<SGUISliceImageElement>();
            itemGridBackground.SetTexture(this.guiBackgroundTexture);
            itemGridBackground.SetScale(new Vector2(30, 10));
            itemGridBackground.SetMargin(new Vector2(32, 88));
            itemGridBackground.SetColor(new Color(94, 101, 110, 255));
            itemGridBackground.PositionRelativeToElement(explorerBackground);

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
                    SGUIImageElement slotBackground = this._layout.CreateElement<SGUIImageElement>();
                    slotBackground.SetTexture(this.squareShapeTexture);
                    slotBackground.SetOriginPivot(SCardinalDirection.Center);
                    slotBackground.SetScale(new Vector2(slotScale));
                    slotBackground.SetPositionAnchor(SCardinalDirection.West);
                    slotBackground.SetSize(new SSize2(slotSize));
                    slotBackground.SetMargin(slotMargin);

                    SGUIImageElement slotIcon = this._layout.CreateElement<SGUIImageElement>();
                    slotIcon.SetOriginPivot(SCardinalDirection.Center);
                    slotIcon.SetScale(new Vector2(1.5f));
                    slotIcon.SetSize(new SSize2(slotSize));

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
