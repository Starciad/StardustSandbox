using Microsoft.Xna.Framework;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.GUI.Common;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.GUISystem.Elements.Graphics;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Items;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements
{
    public sealed partial class SGUI_ItemExplorer
    {
        private ISGUILayoutBuilder layout;

        private SGUILabelElement explorerTitleLabel;

        private (SGUIImageElement background, SGUIImageElement icon)[] itemButtonSlots;
        private (SGUIImageElement background, SGUIImageElement icon)[] categoryButtonSlots;

        protected override void OnBuild(ISGUILayoutBuilder layout)
        {
            this.layout = layout;

            BuildGUIBackground();
            BuildExplorer();

            SelectItemCatalog("powders", 0);
        }

        private void BuildGUIBackground()
        {
            SGUIImageElement guiBackground = new(this.SGameInstance)
            {
                Texture = this.particleTexture,
                Scale = new(SScreenConstants.DEFAULT_SCREEN_WIDTH, SScreenConstants.DEFAULT_SCREEN_HEIGHT),
                Size = SScreenConstants.DEFAULT_SCREEN_SIZE,
                Color = new Color(Color.Black, 160)
            };

            this.layout.AddElement(guiBackground);
        }

        // ================================== //

        private void BuildExplorer()
        {
            #region BACKGROUND & TITLE
            SGUISliceImageElement explorerBackground = new(this.SGameInstance)
            {
                Texture = this.guiBackgroundTexture,
                Scale = new Vector2(32, 15),
                Margin = new Vector2(128, 128),
                Color = new Color(104, 111, 121, 255)
            };

            explorerBackground.PositionRelativeToScreen();

            this.explorerTitleLabel = new(this.SGameInstance)
            {
                Scale = new Vector2(0.15f),
                Margin = new Vector2(18, -16),
                Color = new Color(206, 214, 237, 255),
                BorderOffset = new Vector2(4.4f)
            };

            this.explorerTitleLabel.SetTextContent("TITLE");
            this.explorerTitleLabel.SetFontFamily(SFontFamilyConstants.BIG_APPLE_3PM);
            this.explorerTitleLabel.SetBorders(true);
            this.explorerTitleLabel.SetBordersColor(new Color(45, 53, 74, 255));
            this.explorerTitleLabel.PositionRelativeToElement(explorerBackground);

            this.layout.AddElement(explorerBackground);
            this.layout.AddElement(this.explorerTitleLabel);
            #endregion

            #region ITEM DISPLAY
            // Background
            SGUISliceImageElement itemGridBackground = new(this.SGameInstance)
            {
                Texture = this.guiBackgroundTexture,
                Scale = new Vector2(30, 10),
                Margin = new Vector2(32, 88),
                Color = new Color(94, 101, 110, 255)
            };

            itemGridBackground.PositionRelativeToElement(explorerBackground);

            this.layout.AddElement(itemGridBackground);
            BuildItemCatalogSlots(itemGridBackground);
            #endregion

            #region CATEGORY BUTTONS
            BuildCategoryButtons(itemGridBackground);
            #endregion

            #region PAGINATION
            // [...]
            #endregion
        }

        private void BuildItemCatalogSlots(SGUIElement parent)
        {
            int slotSize = SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE;
            int slotScale = SHUDConstants.SLOT_SCALE;
            int slotSpacing = slotSize * 2;

            Vector2 slotMargin = new(32, 40);

            int rows = SItemExplorerConstants.ITEMS_PER_ROW;
            int columns = SItemExplorerConstants.ITEMS_PER_COLUMN;

            this.itemButtonSlots = new (SGUIImageElement background, SGUIImageElement icon)[rows * columns];

            int index = 0;
            for (int col = 0; col < columns; col++)
            {
                for (int row = 0; row < rows; row++)
                {
                    SGUIImageElement slotBackground = new(this.SGameInstance)
                    {
                        Texture = this.squareShapeTexture,
                        OriginPivot = SCardinalDirection.Center,
                        Scale = new Vector2(slotScale),
                        PositionAnchor = SCardinalDirection.West,
                        Size = new SSize2(slotSize),
                        Margin = slotMargin
                    };

                    SGUIImageElement slotIcon = new(this.SGameInstance)
                    {
                        OriginPivot = SCardinalDirection.Center,
                        Scale = new Vector2(1.5f),
                        Size = new SSize2(slotSize)
                    };

                    // Position
                    slotBackground.PositionRelativeToElement(parent);
                    slotIcon.PositionRelativeToElement(slotBackground);

                    // Spacing
                    slotMargin.X += slotSpacing + (slotSize / 2);
                    this.itemButtonSlots[index] = (slotBackground, slotIcon);
                    index++;

                    // Adding
                    this.layout.AddElement(slotBackground);
                    this.layout.AddElement(slotIcon);
                }

                slotMargin.X = 32;
                slotMargin.Y += slotSpacing + (slotSize / 2);
            }
        }

        private void BuildCategoryButtons(SGUIElement parent)
        {
            int slotSize = SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE;
            int slotScale = SHUDConstants.SLOT_SCALE;
            int slotSpacing = slotSize * 2;

            Vector2 slotMargin = new(0, -160);

            this.categoryButtonSlots = new (SGUIImageElement background, SGUIImageElement icon)[this.SGameInstance.ItemDatabase.TotalCategoryCount];

            int index = 0;

            foreach (SItemCategory category in this.SGameInstance.ItemDatabase.Categories)
            {
                SGUIImageElement slotBackground = new(this.SGameInstance)
                {
                    Texture = this.squareShapeTexture,
                    OriginPivot = SCardinalDirection.Center,
                    Scale = new Vector2(slotScale),
                    PositionAnchor = SCardinalDirection.Northwest,
                    Size = new SSize2(slotSize),
                    Margin = slotMargin
                };

                SGUIImageElement slotIcon = new(this.SGameInstance)
                {
                    Texture = category.IconTexture,
                    OriginPivot = SCardinalDirection.Center,
                    Scale = new Vector2(1.5f),
                    Size = new SSize2(slotSize)
                };

                // Data
                if (!slotBackground.ContainsData("category_id"))
                {
                    slotBackground.AddData("category_id", category.Identifier);
                }

                // Position
                slotBackground.PositionRelativeToElement(parent);
                slotIcon.PositionRelativeToElement(slotBackground);

                // Spacing
                slotMargin.X += slotSpacing + (slotSize / 2);
                this.categoryButtonSlots[index] = (slotBackground, slotIcon);
                index++;

                // Adding
                this.layout.AddElement(slotBackground);
                this.layout.AddElement(slotIcon);
            }
        }
    }
}
