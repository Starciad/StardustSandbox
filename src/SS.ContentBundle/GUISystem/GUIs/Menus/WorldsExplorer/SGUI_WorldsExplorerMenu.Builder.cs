using Microsoft.Xna.Framework;

using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.ContentBundle.GUISystem.Helpers.Interactive;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.GUISystem.GUIs.Hud.Complements;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.WorldsExplorer
{
    internal sealed partial class SGUI_WorldsExplorerMenu
    {
        private SGUIImageElement headerBackgroundElement;
        private SGUILabelElement pageIndexLabelElement;

        private readonly SGUIImageElement[] headerButtonElements;
        private readonly SGUILabelElement[] footerButtonElements;
        private readonly SSlotInfoElement[] slotInfoElements;

        protected override void OnBuild(ISGUILayoutBuilder layoutBuilder)
        {
            BuildHeader(layoutBuilder);
            BuildFooter(layoutBuilder);

            BuildingWorldDisplaySlots(layoutBuilder);
        }

        private void BuildHeader(ISGUILayoutBuilder layoutBuilder)
        {
            // Background
            this.headerBackgroundElement = new(this.SGameInstance)
            {
                Texture = this.particleTexture,
                Color = new(SColorPalette.DarkGray, 196),
                Size = SSize2.One,
                Scale = new(SScreenConstants.DEFAULT_SCREEN_WIDTH, 96f),
            };

            // Title
            SGUILabelElement titleLabelElement = new(this.SGameInstance)
            {
                Scale = new(0.15f),
                SpriteFont = this.bigApple3PMSpriteFont,
                PositionAnchor = SCardinalDirection.West,
                OriginPivot = SCardinalDirection.East,
                Margin = new(32f, 0f),
            };

            titleLabelElement.SetTextualContent("Worlds Explorer");
            titleLabelElement.SetAllBorders(true, SColorPalette.DarkGray, new(2f));
            titleLabelElement.PositionRelativeToElement(this.headerBackgroundElement);

            layoutBuilder.AddElement(this.headerBackgroundElement);
            layoutBuilder.AddElement(titleLabelElement);

            // Buttons
            Vector2 margin = new(-64f, 0);

            for (int i = 0; i < this.headerButtons.Length; i++)
            {
                SButton button = this.headerButtons[i];

                SGUIImageElement buttonBackgroundElement = new(this.SGameInstance)
                {
                    Texture = this.guiSmallButtonTexture,
                    PositionAnchor = SCardinalDirection.East,
                    OriginPivot = SCardinalDirection.Center,
                    Margin = margin,
                    Scale = new(2f),
                    Size = new(32f),
                };

                SGUIImageElement buttonIconElement = new(this.SGameInstance)
                {
                    Texture = button.IconTexture,
                    OriginPivot = SCardinalDirection.Center,
                    Scale = new(1.5f),
                    Size = new(32f),
                };

                buttonBackgroundElement.PositionRelativeToElement(this.headerBackgroundElement);
                buttonIconElement.PositionRelativeToElement(buttonBackgroundElement);

                layoutBuilder.AddElement(buttonBackgroundElement);
                layoutBuilder.AddElement(buttonIconElement);

                this.headerButtonElements[i] = buttonBackgroundElement;

                margin.X -= buttonBackgroundElement.Size.Width + 16f;
            }
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

            SGUILabelElement pageIndexTitleLabel = new(this.SGameInstance)
            {
                Scale = new(0.1f),
                SpriteFont = this.bigApple3PMSpriteFont,
                PositionAnchor = SCardinalDirection.Center,
                OriginPivot = SCardinalDirection.Center,
            };

            this.pageIndexLabelElement = new(this.SGameInstance)
            {
                Scale = new(0.1f),
                SpriteFont = this.bigApple3PMSpriteFont,
                PositionAnchor = SCardinalDirection.Center,
                OriginPivot = SCardinalDirection.Center,
            };

            SGUILabelElement previousButtonLabel = new(this.SGameInstance)
            {
                Scale = new(0.15f),
                SpriteFont = this.bigApple3PMSpriteFont,
                PositionAnchor = SCardinalDirection.West,
                OriginPivot = SCardinalDirection.Center,
            };

            SGUILabelElement nextButtonLabel = new(this.SGameInstance)
            {
                Scale = new(0.15f),
                SpriteFont = this.bigApple3PMSpriteFont,
                PositionAnchor = SCardinalDirection.East,
                OriginPivot = SCardinalDirection.Center,
            };

            this.footerButtonElements[0] = previousButtonLabel;
            this.footerButtonElements[1] = nextButtonLabel;

            pageIndexTitleLabel.SetTextualContent("Current Page");
            this.pageIndexLabelElement.SetTextualContent("1 / 1");
            previousButtonLabel.SetTextualContent("Previous");
            nextButtonLabel.SetTextualContent("Next");

            pageIndexTitleLabel.SetAllBorders(true, SColorPalette.DarkGray, new(2f));
            this.pageIndexLabelElement.SetAllBorders(true, SColorPalette.DarkGray, new(2f));
            previousButtonLabel.SetAllBorders(true, SColorPalette.DarkGray, new(2f));
            nextButtonLabel.SetAllBorders(true, SColorPalette.DarkGray, new(2f));

            pageIndexTitleLabel.Margin = new(0f, -16f);
            this.pageIndexLabelElement.Margin = new(0f, pageIndexTitleLabel.GetStringSize().Height);
            previousButtonLabel.Margin = new(previousButtonLabel.GetStringSize().Width + 32f, 0f);
            nextButtonLabel.Margin = new((nextButtonLabel.GetStringSize().Width + 32f) * -1, 0f);

            backgroundImage.PositionRelativeToScreen();
            pageIndexTitleLabel.PositionRelativeToElement(backgroundImage);
            this.pageIndexLabelElement.PositionRelativeToElement(pageIndexTitleLabel);
            previousButtonLabel.PositionRelativeToElement(backgroundImage);
            nextButtonLabel.PositionRelativeToElement(backgroundImage);

            layoutBuilder.AddElement(backgroundImage);
            layoutBuilder.AddElement(pageIndexTitleLabel);
            layoutBuilder.AddElement(this.pageIndexLabelElement);
            layoutBuilder.AddElement(previousButtonLabel);
            layoutBuilder.AddElement(nextButtonLabel);
        }

        // ========================================================================== //

        private void BuildingWorldDisplaySlots(ISGUILayoutBuilder layoutBuilder)
        {
            Vector2 slotMargin = new(32, (SGUI_WorldsExplorerConstants.SLOT_HEIGHT_SPACING / 2) + 32);

            int rows = SGUI_WorldsExplorerConstants.ITEMS_PER_ROW;
            int columns = SGUI_WorldsExplorerConstants.ITEMS_PER_COLUMN;

            int index = 0;
            for (int col = 0; col < columns; col++)
            {
                for (int row = 0; row < rows; row++)
                {
                    SGUIImageElement backgroundImageElement = new(this.SGameInstance)
                    {
                        Texture = this.guiButton2Texture,
                        Size = new(SGUI_WorldsExplorerConstants.SLOT_WIDTH, SGUI_WorldsExplorerConstants.SLOT_HEIGHT),
                        Margin = slotMargin
                    };

                    SGUIImageElement thumbnailImageElement = new(this.SGameInstance)
                    {
                        Scale = new(5.1f),
                        Size = SWorldConstants.WORLD_THUMBNAIL_SIZE,
                        PositionAnchor = SCardinalDirection.West,
                        OriginPivot = SCardinalDirection.East,
                        Margin = new(11.5f, 0f),
                    };

                    SGUILabelElement titleLabelElement = new(this.SGameInstance)
                    {
                        Color = SColorPalette.White,
                        SpriteFont = this.bigApple3PMSpriteFont,
                        OriginPivot = SCardinalDirection.East,
                        PositionAnchor = SCardinalDirection.North,
                        Scale = new(0.1f),
                        Margin = new(-52.5f, 23f),
                    };

                    // Setting
                    titleLabelElement.SetTextualContent("Title");

                    // Position
                    backgroundImageElement.PositionRelativeToElement(this.headerBackgroundElement);
                    thumbnailImageElement.PositionRelativeToElement(backgroundImageElement);
                    titleLabelElement.PositionRelativeToElement(backgroundImageElement);

                    // Spacing
                    slotMargin.X += SGUI_WorldsExplorerConstants.SLOT_WIDTH_SPACING;

                    this.slotInfoElements[index] = new()
                    {
                        BackgroundElement = backgroundImageElement,
                        ThumbnailElement = thumbnailImageElement,
                        TitleElement = titleLabelElement
                    };

                    index++;

                    // Adding
                    layoutBuilder.AddElement(backgroundImageElement);
                    layoutBuilder.AddElement(thumbnailImageElement);
                    layoutBuilder.AddElement(titleLabelElement);
                }

                slotMargin.X = 32;
                slotMargin.Y += SGUI_WorldsExplorerConstants.SLOT_HEIGHT_SPACING;
            }
        }
    }
}
