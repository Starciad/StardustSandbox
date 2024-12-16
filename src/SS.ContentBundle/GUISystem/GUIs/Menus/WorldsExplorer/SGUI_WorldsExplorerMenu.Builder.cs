﻿using Microsoft.Xna.Framework;

using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.GUI.Common;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    internal sealed partial class SGUI_WorldsExplorerMenu
    {
        private SGUIImageElement headerBackgroundElement;

        protected override void OnBuild(ISGUILayoutBuilder layoutBuilder)
        {
            BuildHeader(layoutBuilder);
            BuildFooter(layoutBuilder);

            BuildingWorldDisplaySlots(layoutBuilder);
        }

        private void BuildHeader(ISGUILayoutBuilder layoutBuilder)
        {
            this.headerBackgroundElement = new(this.SGameInstance)
            {
                Texture = this.particleTexture,
                Color = new(SColorPalette.DarkGray, 196),
                Size = SSize2.One,
                Scale = new(SScreenConstants.DEFAULT_SCREEN_WIDTH, 96f),
            };

            SGUILabelElement titleLabel = new(this.SGameInstance)
            {
                Scale = new(0.15f),
                SpriteFont = this.bigApple3PMSpriteFont,
                PositionAnchor = SCardinalDirection.West,
                OriginPivot = SCardinalDirection.East,
                Margin = new(32f, 0f),
            };

            titleLabel.SetTextualContent("Worlds Explorer");
            titleLabel.SetAllBorders(true, SColorPalette.DarkGray, new(2f));
            titleLabel.PositionRelativeToElement(this.headerBackgroundElement);

            layoutBuilder.AddElement(this.headerBackgroundElement);
            layoutBuilder.AddElement(titleLabel);
        }

        private void BuildFooter(ISGUILayoutBuilder layoutBuilder)
        {
            SGUIImageElement backgroundImage = new(this.SGameInstance)
            {
                Texture = this.particleTexture,
                Color = new(SColorPalette.DarkGray, 196),
                Size = SSize2.One,
                Scale = new(SScreenConstants.DEFAULT_SCREEN_WIDTH, 96f),
                PositionAnchor = SCardinalDirection.Southwest,
                Margin = new(0f, -96f),
            };

            SGUILabelElement indexLabel = new(this.SGameInstance)
            {
                Scale = new(0.1f),
                SpriteFont = this.bigApple3PMSpriteFont,
                PositionAnchor = SCardinalDirection.Center,
                OriginPivot = SCardinalDirection.Center,
            };

            SGUILabelElement indexNumbersLabel = new(this.SGameInstance)
            {
                Scale = new(0.1f),
                SpriteFont = this.bigApple3PMSpriteFont,
                PositionAnchor = SCardinalDirection.Center,
                OriginPivot = SCardinalDirection.Center,
            };

            SGUILabelElement nextButtonLabel = new(this.SGameInstance)
            {
                Scale = new(0.15f),
                SpriteFont = this.bigApple3PMSpriteFont,
                PositionAnchor = SCardinalDirection.East,
                OriginPivot = SCardinalDirection.West,
                Margin = new(-32f, 0f)
            };

            SGUILabelElement previousButtonLabel = new(this.SGameInstance)
            {
                Scale = new(0.15f),
                SpriteFont = this.bigApple3PMSpriteFont,
                PositionAnchor = SCardinalDirection.West,
                OriginPivot = SCardinalDirection.East,
                Margin = new(32f, 0f),
            };

            indexLabel.SetTextualContent("Current Page");
            indexNumbersLabel.SetTextualContent("1/1");
            nextButtonLabel.SetTextualContent("Next");
            previousButtonLabel.SetTextualContent("Previous");

            indexLabel.SetAllBorders(true, SColorPalette.DarkGray, new(2f));
            indexNumbersLabel.SetAllBorders(true, SColorPalette.DarkGray, new(2f));
            nextButtonLabel.SetAllBorders(true, SColorPalette.DarkGray, new(2f));
            previousButtonLabel.SetAllBorders(true, SColorPalette.DarkGray, new(2f));

            indexLabel.Margin = new(0, -16f);
            indexNumbersLabel.Margin = new(0, indexLabel.GetStringSize().Height);

            backgroundImage.PositionRelativeToScreen();
            indexLabel.PositionRelativeToElement(backgroundImage);
            indexNumbersLabel.PositionRelativeToElement(indexLabel);
            nextButtonLabel.PositionRelativeToElement(backgroundImage);
            previousButtonLabel.PositionRelativeToElement(backgroundImage);

            layoutBuilder.AddElement(backgroundImage);
            layoutBuilder.AddElement(indexLabel);
            layoutBuilder.AddElement(indexNumbersLabel);
            layoutBuilder.AddElement(nextButtonLabel);
            layoutBuilder.AddElement(previousButtonLabel);
        }

        // ========================================================================== //

        private void BuildingWorldDisplaySlots(ISGUILayoutBuilder layoutBuilder)
        {
            Vector2 slotMargin = new(32, SWorldsExplorerConstants.SLOT_HEIGHT_SPACING / 2 + 32);

            int rows = SWorldsExplorerConstants.ITEMS_PER_ROW;
            int columns = SWorldsExplorerConstants.ITEMS_PER_COLUMN;

            SGUIImageElement[] itemButtonSlots = new SGUIImageElement[SWorldsExplorerConstants.ITEMS_PER_PAGE];

            int index = 0;
            for (int col = 0; col < columns; col++)
            {
                for (int row = 0; row < rows; row++)
                {
                    SGUIImageElement slotBackground = new(this.SGameInstance)
                    {
                        Texture = this.particleTexture,
                        Scale = new(SWorldsExplorerConstants.SLOT_WIDTH, SWorldsExplorerConstants.SLOT_HEIGHT),
                        Size = new(1),
                        Margin = slotMargin
                    };

                    // Position
                    slotBackground.PositionRelativeToElement(this.headerBackgroundElement);

                    // Spacing
                    slotMargin.X += SWorldsExplorerConstants.SLOT_WIDTH_SPACING;
                    itemButtonSlots[index] = slotBackground;
                    index++;

                    // Adding
                    layoutBuilder.AddElement(slotBackground);
                }

                slotMargin.X = 32;
                slotMargin.Y += SWorldsExplorerConstants.SLOT_HEIGHT_SPACING;
            }
        }
    }
}