using Microsoft.Xna.Framework;

using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.ContentBundle.GUISystem.Specials.General;
using StardustSandbox.ContentBundle.GUISystem.Specials.Interactive;
using StardustSandbox.Core.Catalog;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.GUISystem;
using StardustSandbox.Core.Constants.GUISystem.GUIs.Hud;
using StardustSandbox.Core.Constants.GUISystem.GUIs.Hud.Complements;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.Interfaces.GUI;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements
{
    internal sealed partial class SGUI_ItemExplorer
    {
        private SGUILabelElement explorerTitleLabel;

        private SGUISliceImageElement panelBackgroundElement;
        private SGUISliceImageElement itemGridBackground;

        private readonly SSlot[] menuButtonSlots;
        private readonly SSlot[] itemButtonSlots;
        private readonly SSlot[] categoryButtonSlots;
        private readonly SSlot[] subcategoryButtonSlots;

        protected override void OnBuild(ISGUILayoutBuilder layoutBuilder)
        {
            BuildGUIBackground(layoutBuilder);
            BuildMenuButtons(layoutBuilder);
            BuildExplorer(layoutBuilder);
            BuildItemCatalogSlots(layoutBuilder, this.itemGridBackground);
            BuildCategoryButtons(layoutBuilder);
            BuildSubcategoryButtons(layoutBuilder);

            layoutBuilder.AddElement(this.tooltipBoxElement);
        }

        private void BuildGUIBackground(ISGUILayoutBuilder layoutBuilder)
        {
            SGUIImageElement guiBackground = new(this.SGameInstance)
            {
                Texture = this.particleTexture,
                Scale = new(SScreenConstants.DEFAULT_SCREEN_WIDTH, SScreenConstants.DEFAULT_SCREEN_HEIGHT),
                Size = new(32),
                Color = new(SColorPalette.DarkGray, 160)
            };

            layoutBuilder.AddElement(guiBackground);

            // ============================================== //

            this.panelBackgroundElement = new(this.SGameInstance)
            {
                Texture = this.guiBackgroundTexture,
                Scale = new(32, 15),
                Size = new(32),
                Margin = new(128f),
                Color = new(104, 111, 121, 255)
            };

            this.panelBackgroundElement.PositionRelativeToScreen();

            this.explorerTitleLabel = new(this.SGameInstance)
            {
                Scale = new(0.15f),
                Margin = new(18, -16),
                Color = SColorPalette.White,
                SpriteFont = this.bigApple3PMSpriteFont,
            };

            this.explorerTitleLabel.SetTextualContent("TITLE");
            this.explorerTitleLabel.SetAllBorders(true, new(45, 53, 74, 255), new(4.4f));
            this.explorerTitleLabel.PositionRelativeToElement(this.panelBackgroundElement);

            layoutBuilder.AddElement(this.panelBackgroundElement);
            layoutBuilder.AddElement(this.explorerTitleLabel);
        }

        private void BuildMenuButtons(ISGUILayoutBuilder layoutBuilder)
        {
            Vector2 margin = new(-2, -72);

            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                SButton button = this.menuButtons[i];

                SGUIImageElement backgroundElement = new(this.SGameInstance)
                {
                    Texture = this.guiButton1Texture,
                    Scale = new(SGUI_HUDConstants.SLOT_SCALE),
                    Size = new(SGUI_HUDConstants.SLOT_SIZE),
                    Margin = margin,
                };

                SGUIImageElement iconElement = new(this.SGameInstance)
                {
                    Texture = button.IconTexture,
                    OriginPivot = SCardinalDirection.Center,
                    Scale = new(1.5f),
                    Size = new(SGUI_HUDConstants.SLOT_SIZE)
                };

                SSlot slot = new(backgroundElement, iconElement);

                slot.BackgroundElement.PositionAnchor = SCardinalDirection.Northeast;
                slot.BackgroundElement.OriginPivot = SCardinalDirection.Center;

                // Update
                slot.BackgroundElement.PositionRelativeToElement(this.panelBackgroundElement);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);

                // Save
                this.menuButtonSlots[i] = slot;

                // Spacing
                margin.X -= SGUI_HUDConstants.SLOT_SPACING + (SGUI_HUDConstants.SLOT_SIZE / 2);

                layoutBuilder.AddElement(backgroundElement);
                layoutBuilder.AddElement(iconElement);
            }
        }

        private void BuildExplorer(ISGUILayoutBuilder layoutBuilder)
        {
            // Background
            this.itemGridBackground = new(this.SGameInstance)
            {
                Texture = this.guiBackgroundTexture,
                Scale = new(30, 10),
                Margin = new(32, 88),
                Color = new Color(94, 101, 110, 255)
            };

            this.itemGridBackground.PositionRelativeToElement(this.panelBackgroundElement);

            layoutBuilder.AddElement(this.itemGridBackground);
        }

        private void BuildItemCatalogSlots(ISGUILayoutBuilder layoutBuilder, SGUIElement parent)
        {
            Vector2 slotMargin = new(32, 40);

            int rows = SGUI_ItemExplorerConstants.ITEMS_PER_ROW;
            int columns = SGUI_ItemExplorerConstants.ITEMS_PER_COLUMN;

            int index = 0;
            for (int col = 0; col < columns; col++)
            {
                for (int row = 0; row < rows; row++)
                {
                    SGUIImageElement slotBackground = new(this.SGameInstance)
                    {
                        Texture = this.guiButton1Texture,
                        OriginPivot = SCardinalDirection.Center,
                        Scale = new(SGUI_ItemExplorerConstants.SLOT_SCALE),
                        PositionAnchor = SCardinalDirection.West,
                        Size = new(SGUI_ItemExplorerConstants.SLOT_SIZE),
                        Margin = slotMargin
                    };

                    SGUIImageElement slotIcon = new(this.SGameInstance)
                    {
                        OriginPivot = SCardinalDirection.Center,
                        Scale = new(1.5f),
                        Size = new(SGUI_ItemExplorerConstants.SLOT_SIZE)
                    };

                    // Position
                    slotBackground.PositionRelativeToElement(parent);
                    slotIcon.PositionRelativeToElement(slotBackground);

                    // Spacing
                    slotMargin.X += SGUI_ItemExplorerConstants.SLOT_SPACING + (SGUI_ItemExplorerConstants.SLOT_SIZE / 2);
                    this.itemButtonSlots[index] = new(slotBackground, slotIcon);
                    index++;

                    // Adding
                    layoutBuilder.AddElement(slotBackground);
                    layoutBuilder.AddElement(slotIcon);
                }

                slotMargin.X = 32;
                slotMargin.Y += SGUI_ItemExplorerConstants.SLOT_SPACING + (SGUI_ItemExplorerConstants.SLOT_SIZE / 2);
            }
        }

        private void BuildCategoryButtons(ISGUILayoutBuilder layoutBuilder)
        {
            Vector2 slotMargin = new(-30, -160);

            int index = 0;

            foreach (SCategory category in this.SGameInstance.CatalogDatabase.Categories)
            {
                SGUIImageElement slotBackground = new(this.SGameInstance)
                {
                    Texture = this.guiButton1Texture,
                    OriginPivot = SCardinalDirection.Center,
                    PositionAnchor = SCardinalDirection.Northwest,
                    Scale = new(SGUI_ItemExplorerConstants.SLOT_SCALE),
                    Size = new(SGUI_ItemExplorerConstants.SLOT_SIZE),
                    Margin = slotMargin
                };

                SGUIImageElement slotIcon = new(this.SGameInstance)
                {
                    Texture = category.IconTexture,
                    OriginPivot = SCardinalDirection.Center,
                    Scale = new(1.5f),
                    Size = new(SGUI_ItemExplorerConstants.SLOT_SIZE)
                };

                // Data
                if (!slotBackground.ContainsData(SGUIConstants.DATA_CATEGORY))
                {
                    slotBackground.AddData(SGUIConstants.DATA_CATEGORY, category);
                }

                // Position
                slotBackground.PositionRelativeToElement(this.itemGridBackground);
                slotIcon.PositionRelativeToElement(slotBackground);

                // Spacing
                slotMargin.X += SGUI_ItemExplorerConstants.SLOT_SPACING + (SGUI_ItemExplorerConstants.SLOT_SIZE / 2);
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
            int sideCounts = SGUI_ItemExplorerConstants.SUB_CATEGORY_BUTTONS_LENGTH / 2;

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
                        Scale = new(SGUI_ItemExplorerConstants.SLOT_SCALE),
                        Size = new(SGUI_ItemExplorerConstants.SLOT_SIZE),
                        Margin = margin
                    };

                    SGUIImageElement slotIcon = new(this.SGameInstance)
                    {
                        OriginPivot = SCardinalDirection.Center,
                        Scale = new(1.5f),
                        Size = new(SGUI_ItemExplorerConstants.SLOT_SIZE)
                    };

                    // Position
                    slotBackground.PositionRelativeToElement(this.itemGridBackground);
                    slotIcon.PositionRelativeToElement(slotBackground);

                    // Spacing
                    margin.Y += SGUI_ItemExplorerConstants.SLOT_SPACING + (SGUI_ItemExplorerConstants.SLOT_SIZE / 2);
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
