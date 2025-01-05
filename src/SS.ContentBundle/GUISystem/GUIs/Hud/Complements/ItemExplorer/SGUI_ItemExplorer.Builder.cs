using Microsoft.Xna.Framework;

using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.ContentBundle.GUISystem.Specials.General;
using StardustSandbox.ContentBundle.GUISystem.Specials.Interactive;
using StardustSandbox.ContentBundle.Localization.GUIs;
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
        private SGUIImageElement panelBackgroundElement;

        private SGUILabelElement menuTitleElement;

        private readonly SSlot[] menuButtonSlots;
        private readonly SSlot[] itemButtonSlots;
        private readonly SSlot[] categoryButtonSlots;
        private readonly SSlot[] subcategoryButtonSlots;

        protected override void OnBuild(ISGUILayoutBuilder layoutBuilder)
        {
            BuildBackground(layoutBuilder);
            BuildTitle(layoutBuilder);
            BuildMenuButtons(layoutBuilder);
            BuildItemCatalogSlots(layoutBuilder);
            BuildCategoryButtons(layoutBuilder);
            BuildSubcategoryButtons(layoutBuilder);

            layoutBuilder.AddElement(this.tooltipBoxElement);
        }

        private void BuildBackground(ISGUILayoutBuilder layoutBuilder)
        {
            SGUIImageElement backgroundShadowElement = new(this.SGameInstance)
            {
                Texture = this.particleTexture,
                Scale = new(SScreenConstants.DEFAULT_SCREEN_WIDTH, SScreenConstants.DEFAULT_SCREEN_HEIGHT),
                Size = new(1),
                Color = new(SColorPalette.DarkGray, 160)
            };

            this.panelBackgroundElement = new(this.SGameInstance)
            {
                Texture = this.panelBackgroundTexture,
                Size = new(1084, 540),
                Margin = new(98, 90),
            };

            this.panelBackgroundElement.PositionRelativeToScreen();

            layoutBuilder.AddElement(backgroundShadowElement);
            layoutBuilder.AddElement(this.panelBackgroundElement);
        }

        private void BuildTitle(ISGUILayoutBuilder layoutBuilder)
        {
            this.menuTitleElement = new(this.SGameInstance)
            {
                SpriteFont = this.bigApple3PMSpriteFont,
                Scale = new(0.12f),
                PositionAnchor = SCardinalDirection.Northwest,
                OriginPivot = SCardinalDirection.East,
                Margin = new(32, 40),
                Color = SColorPalette.White,
            };

            this.menuTitleElement.SetTextualContent(SLocalization_GUIs.HUD_Complements_ItemExplorer_Title);
            this.menuTitleElement.SetAllBorders(true, SColorPalette.DarkGray, new(3f));
            this.menuTitleElement.PositionRelativeToElement(this.panelBackgroundElement);

            layoutBuilder.AddElement(this.menuTitleElement);
        }

        private void BuildMenuButtons(ISGUILayoutBuilder layoutBuilder)
        {
            Vector2 margin = new(-32f, -40f);

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

        private void BuildItemCatalogSlots(ISGUILayoutBuilder layoutBuilder)
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
                    slotBackground.PositionRelativeToElement(this.panelBackgroundElement);
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
                slotBackground.PositionRelativeToElement(this.panelBackgroundElement);
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
                    slotBackground.PositionRelativeToElement(this.panelBackgroundElement);
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
