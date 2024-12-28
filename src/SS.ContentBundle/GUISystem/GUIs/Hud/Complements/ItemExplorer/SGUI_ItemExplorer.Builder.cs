using Microsoft.Xna.Framework;

using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.ContentBundle.GUISystem.Specials.General;
using StardustSandbox.Core.Catalog;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.GUI.Common;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.Interfaces.GUI;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements
{
    internal sealed partial class SGUI_ItemExplorer
    {
        private SGUILabelElement explorerTitleLabel;

        private SGUISliceImageElement itemGridBackground;

        private readonly SSlot[] itemButtonSlots;
        private readonly SSlot[] categoryButtonSlots;
        private readonly SSlot[] subcategoryButtonSlots;

        protected override void OnBuild(ISGUILayoutBuilder layoutBuilder)
        {
            BuildGUIBackground(layoutBuilder);
            BuildExplorer(layoutBuilder);

            layoutBuilder.AddElement(this.tooltipBoxElement);
        }

        private void BuildGUIBackground(ISGUILayoutBuilder layoutBuilder)
        {
            SGUIImageElement guiBackground = new(this.SGameInstance)
            {
                Texture = this.particleTexture,
                Scale = new(SScreenConstants.DEFAULT_SCREEN_WIDTH, SScreenConstants.DEFAULT_SCREEN_HEIGHT),
                Size = SScreenConstants.DEFAULT_SCREEN_SIZE,
                Color = new(SColorPalette.DarkGray, 160)
            };

            layoutBuilder.AddElement(guiBackground);
        }

        // ================================== //

        private void BuildExplorer(ISGUILayoutBuilder layoutBuilder)
        {
            #region BACKGROUND & TITLE
            SGUISliceImageElement explorerBackground = new(this.SGameInstance)
            {
                Texture = this.guiBackgroundTexture,
                Scale = new(32, 15),
                Margin = new(128f),
                Color = new(104, 111, 121, 255)
            };

            explorerBackground.PositionRelativeToScreen();

            this.explorerTitleLabel = new(this.SGameInstance)
            {
                Scale = new(0.15f),
                Margin = new(18, -16),
                Color = SColorPalette.White,
                SpriteFont = this.bigApple3PMSpriteFont,
            };

            this.explorerTitleLabel.SetTextualContent("TITLE");
            this.explorerTitleLabel.SetAllBorders(true, new(45, 53, 74, 255), new(4.4f));
            this.explorerTitleLabel.PositionRelativeToElement(explorerBackground);

            layoutBuilder.AddElement(explorerBackground);
            layoutBuilder.AddElement(this.explorerTitleLabel);
            #endregion

            #region ITEM DISPLAY
            // Background
            this.itemGridBackground = new(this.SGameInstance)
            {
                Texture = this.guiBackgroundTexture,
                Scale = new(30, 10),
                Margin = new(32, 88),
                Color = new Color(94, 101, 110, 255)
            };

            this.itemGridBackground.PositionRelativeToElement(explorerBackground);

            layoutBuilder.AddElement(this.itemGridBackground);
            BuildItemCatalogSlots(layoutBuilder, this.itemGridBackground);
            #endregion

            #region CATEGORY BUTTONS
            BuildCategoryButtons(layoutBuilder);
            BuildSubcategoryButtons(layoutBuilder);
            #endregion

            #region PAGINATION
            // [...]
            #endregion
        }

        private void BuildItemCatalogSlots(ISGUILayoutBuilder layoutBuilder, SGUIElement parent)
        {
            Vector2 slotMargin = new(32, 40);

            int rows = SItemExplorerConstants.ITEMS_PER_ROW;
            int columns = SItemExplorerConstants.ITEMS_PER_COLUMN;

            int index = 0;
            for (int col = 0; col < columns; col++)
            {
                for (int row = 0; row < rows; row++)
                {
                    SGUIImageElement slotBackground = new(this.SGameInstance)
                    {
                        Texture = this.guiButton1Texture,
                        OriginPivot = SCardinalDirection.Center,
                        Scale = new(SItemExplorerConstants.SLOT_SCALE),
                        PositionAnchor = SCardinalDirection.West,
                        Size = new(SItemExplorerConstants.SLOT_SIZE),
                        Margin = slotMargin
                    };

                    SGUIImageElement slotIcon = new(this.SGameInstance)
                    {
                        OriginPivot = SCardinalDirection.Center,
                        Scale = new(1.5f),
                        Size = new(SItemExplorerConstants.SLOT_SIZE)
                    };

                    // Position
                    slotBackground.PositionRelativeToElement(parent);
                    slotIcon.PositionRelativeToElement(slotBackground);

                    // Spacing
                    slotMargin.X += SItemExplorerConstants.SLOT_SPACING + (SItemExplorerConstants.SLOT_SIZE / 2);
                    this.itemButtonSlots[index] = new(slotBackground, slotIcon);
                    index++;

                    // Adding
                    layoutBuilder.AddElement(slotBackground);
                    layoutBuilder.AddElement(slotIcon);
                }

                slotMargin.X = 32;
                slotMargin.Y += SItemExplorerConstants.SLOT_SPACING + (SItemExplorerConstants.SLOT_SIZE / 2);
            }
        }

        private void BuildCategoryButtons(ISGUILayoutBuilder layoutBuilder)
        {
            Vector2 slotMargin = new(0, -160);

            int index = 0;

            foreach (SCategory category in this.SGameInstance.CatalogDatabase.Categories)
            {
                SGUIImageElement slotBackground = new(this.SGameInstance)
                {
                    Texture = this.guiButton1Texture,
                    OriginPivot = SCardinalDirection.Center,
                    PositionAnchor = SCardinalDirection.Northwest,
                    Scale = new(SItemExplorerConstants.SLOT_SCALE),
                    Size = new(SItemExplorerConstants.SLOT_SIZE),
                    Margin = slotMargin
                };

                SGUIImageElement slotIcon = new(this.SGameInstance)
                {
                    Texture = category.IconTexture,
                    OriginPivot = SCardinalDirection.Center,
                    Scale = new(1.5f),
                    Size = new(SItemExplorerConstants.SLOT_SIZE)
                };

                // Data
                if (!slotBackground.ContainsData(SItemExplorerConstants.DATA_CATEGORY))
                {
                    slotBackground.AddData(SItemExplorerConstants.DATA_CATEGORY, category);
                }

                // Position
                slotBackground.PositionRelativeToElement(this.itemGridBackground);
                slotIcon.PositionRelativeToElement(slotBackground);

                // Spacing
                slotMargin.X += SItemExplorerConstants.SLOT_SPACING + (SItemExplorerConstants.SLOT_SIZE / 2);
                this.categoryButtonSlots[index] = new(slotBackground, slotIcon);
                index++;

                // Adding
                layoutBuilder.AddElement(slotBackground);
                layoutBuilder.AddElement(slotIcon);
            }
        }

        private void BuildSubcategoryButtons(ISGUILayoutBuilder layoutBuilder)
        {
            int index = 0;
            int sideCounts = SItemExplorerConstants.SUB_CATEGORY_BUTTONS_LENGTH / 2;

            Vector2 margin = new(-111, -88);
            BuildSlots();
            margin = new(1071, -88);
            BuildSlots();

            // =============================== //

            void BuildSlots()
            {
                for (int i = 0; i < sideCounts; i++)
                {
                    SGUIImageElement slotBackground = new(this.SGameInstance)
                    {
                        Texture = this.guiButton1Texture,
                        OriginPivot = SCardinalDirection.Center,
                        Scale = new(SItemExplorerConstants.SLOT_SCALE),
                        Size = new(SItemExplorerConstants.SLOT_SIZE),
                        Margin = margin
                    };

                    SGUIImageElement slotIcon = new(this.SGameInstance)
                    {
                        OriginPivot = SCardinalDirection.Center,
                        Scale = new(1.5f),
                        Size = new(SItemExplorerConstants.SLOT_SIZE)
                    };

                    // Position
                    slotBackground.PositionRelativeToElement(this.itemGridBackground);
                    slotIcon.PositionRelativeToElement(slotBackground);

                    // Spacing
                    margin.Y += SItemExplorerConstants.SLOT_SPACING + (SItemExplorerConstants.SLOT_SIZE / 2);
                    this.subcategoryButtonSlots[index] = new(slotBackground, slotIcon);
                    index++;

                    // Adding
                    layoutBuilder.AddElement(slotBackground);
                    layoutBuilder.AddElement(slotIcon);
                }
            }
        }
    }
}
