﻿using Microsoft.Xna.Framework;

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

        private SSlot[] itemButtonSlots;
        private SSlot[] categoryButtonSlots;

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
            SGUISliceImageElement itemGridBackground = new(this.SGameInstance)
            {
                Texture = this.guiBackgroundTexture,
                Scale = new(30, 10),
                Margin = new(32, 88),
                Color = new Color(94, 101, 110, 255)
            };

            itemGridBackground.PositionRelativeToElement(explorerBackground);

            layoutBuilder.AddElement(itemGridBackground);
            BuildItemCatalogSlots(layoutBuilder, itemGridBackground);
            #endregion

            #region CATEGORY BUTTONS
            BuildCategoryButtons(layoutBuilder, itemGridBackground);
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

            this.itemButtonSlots = new SSlot[SItemExplorerConstants.ITEMS_PER_PAGE];

            int index = 0;
            for (int col = 0; col < columns; col++)
            {
                for (int row = 0; row < rows; row++)
                {
                    SGUIImageElement slotBackground = new(this.SGameInstance)
                    {
                        Texture = this.guiButton1Texture,
                        OriginPivot = SCardinalDirection.Center,
                        Scale = new(SHUDConstants.SLOT_SCALE),
                        PositionAnchor = SCardinalDirection.West,
                        Size = new(SHUDConstants.SLOT_SIZE),
                        Margin = slotMargin
                    };

                    SGUIImageElement slotIcon = new(this.SGameInstance)
                    {
                        OriginPivot = SCardinalDirection.Center,
                        Scale = new(1.5f),
                        Size = new(SHUDConstants.SLOT_SIZE)
                    };

                    // Position
                    slotBackground.PositionRelativeToElement(parent);
                    slotIcon.PositionRelativeToElement(slotBackground);

                    // Spacing
                    slotMargin.X += SHUDConstants.SLOT_SPACING + (SHUDConstants.SLOT_SIZE / 2);
                    this.itemButtonSlots[index] = new(slotBackground, slotIcon);
                    index++;

                    // Adding
                    layoutBuilder.AddElement(slotBackground);
                    layoutBuilder.AddElement(slotIcon);
                }

                slotMargin.X = 32;
                slotMargin.Y += SHUDConstants.SLOT_SPACING + (SHUDConstants.SLOT_SIZE / 2);
            }
        }

        private void BuildCategoryButtons(ISGUILayoutBuilder layoutBuilder, SGUIElement parent)
        {
            Vector2 slotMargin = new(0, -160);

            this.categoryButtonSlots = new SSlot[this.SGameInstance.ItemDatabase.TotalCategoryCount];

            int index = 0;

            foreach (SCategory category in this.SGameInstance.ItemDatabase.Categories)
            {
                SGUIImageElement slotBackground = new(this.SGameInstance)
                {
                    Texture = this.guiButton1Texture,
                    OriginPivot = SCardinalDirection.Center,
                    PositionAnchor = SCardinalDirection.Northwest,
                    Scale = new(SHUDConstants.SLOT_SCALE),
                    Size = new(SHUDConstants.SLOT_SIZE),
                    Margin = slotMargin
                };

                SGUIImageElement slotIcon = new(this.SGameInstance)
                {
                    Texture = category.IconTexture,
                    OriginPivot = SCardinalDirection.Center,
                    Scale = new(1.5f),
                    Size = new(SHUDConstants.SLOT_SIZE)
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
                slotMargin.X += SHUDConstants.SLOT_SPACING + (SHUDConstants.SLOT_SIZE / 2);
                this.categoryButtonSlots[index] = new(slotBackground, slotIcon);
                index++;

                // Adding
                layoutBuilder.AddElement(slotBackground);
                layoutBuilder.AddElement(slotIcon);
            }
        }
    }
}
