using Microsoft.Xna.Framework;

using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.GUISystem.Common.Elements.Graphics;
using StardustSandbox.Core.GUISystem.Common.Elements.Textual;
using StardustSandbox.Core.GUISystem.Common.Helpers.Interactive;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Mathematics.Primitives;

using System;

namespace StardustSandbox.GameContent.GUISystem.GUIs.Menus.WorldExplorer.Complements
{
    internal sealed partial class SGUI_WorldDetailsMenu
    {
        private SGUIImageElement headerBackgroundElement;

        private SGUILabelElement worldTitleElement;
        private SGUIImageElement worldThumbnailElement;
        private SGUITextElement worldDescriptionElement;
        private SGUILabelElement worldVersionElement;
        private SGUILabelElement worldCreationTimestampElement;

        private readonly SGUILabelElement[] worldButtonElements;

        protected override void OnBuild(ISGUILayoutBuilder layoutBuilder)
        {
            BuildBackground(layoutBuilder);
            BuildHeader(layoutBuilder);
            BuildThumbnail(layoutBuilder);
            BuildDescription(layoutBuilder);
            BuildCreationTimestamp(layoutBuilder);
            BuildVersion(layoutBuilder);
            BuildWorldButtons(layoutBuilder);
        }

        private void BuildBackground(ISGUILayoutBuilder layoutBuilder)
        {
            SGUIImageElement guiBackground = new(this.SGameInstance)
            {
                Texture = this.particleTexture,
                Scale = new(SScreenConstants.DEFAULT_SCREEN_WIDTH, SScreenConstants.DEFAULT_SCREEN_HEIGHT),
                Size = SScreenConstants.DEFAULT_SCREEN_SIZE,
                Color = new Color(SColorPalette.DarkGray, 160)
            };

            layoutBuilder.AddElement(guiBackground);
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
            this.worldTitleElement = new(this.SGameInstance)
            {
                Scale = new(0.15f),
                SpriteFont = this.bigApple3PMSpriteFont,
                PositionAnchor = SCardinalDirection.West,
                OriginPivot = SCardinalDirection.East,
                Margin = new(32f, 0f),
            };

            this.worldTitleElement.SetTextualContent("Title");
            this.worldTitleElement.SetAllBorders(true, SColorPalette.DarkGray, new(2f));
            this.worldTitleElement.PositionRelativeToElement(this.headerBackgroundElement);

            layoutBuilder.AddElement(this.headerBackgroundElement);
            layoutBuilder.AddElement(this.worldTitleElement);
        }

        private void BuildThumbnail(ISGUILayoutBuilder layoutBuilder)
        {
            this.worldThumbnailElement = new(this.SGameInstance)
            {
                Scale = new(12),
                Size = SWorldConstants.WORLD_THUMBNAIL_SIZE,
                Margin = new(32f, 128f),
            };

            this.worldThumbnailElement.PositionRelativeToScreen();

            layoutBuilder.AddElement(this.worldThumbnailElement);
        }

        private void BuildDescription(ISGUILayoutBuilder layoutBuilder)
        {
            this.worldDescriptionElement = new(this.SGameInstance)
            {
                Scale = new(0.078f),
                Margin = new(32f, 0f),
                LineHeight = 1.25f,
                SpriteFont = this.pixelOperatorSpriteFont,
                TextAreaSize = new(930, 600),
                PositionAnchor = SCardinalDirection.Northeast,
            };

            this.worldDescriptionElement.SetTextualContent("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.");
            this.worldDescriptionElement.PositionRelativeToElement(this.worldThumbnailElement);

            layoutBuilder.AddElement(this.worldDescriptionElement);
        }

        private void BuildCreationTimestamp(ISGUILayoutBuilder layoutBuilder)
        {
            this.worldCreationTimestampElement = new(this.SGameInstance)
            {
                SpriteFont = this.bigApple3PMSpriteFont,
                Scale = new(0.075f),
                Margin = new(-8),
                PositionAnchor = SCardinalDirection.Southeast,
                OriginPivot = SCardinalDirection.Northwest,
            };

            this.worldCreationTimestampElement.SetTextualContent(DateTime.Now.ToString());
            this.worldCreationTimestampElement.PositionRelativeToScreen();

            layoutBuilder.AddElement(this.worldCreationTimestampElement);
        }

        private void BuildVersion(ISGUILayoutBuilder layoutBuilder)
        {
            this.worldVersionElement = new(this.SGameInstance)
            {
                SpriteFont = this.bigApple3PMSpriteFont,
                Scale = new(0.075f),
                Margin = new(0f, this.worldCreationTimestampElement.GetStringSize().Height + (64f * -1f)),
                PositionAnchor = SCardinalDirection.Northeast,
                OriginPivot = SCardinalDirection.Northwest,
            };

            this.worldVersionElement.SetTextualContent("Version 1.0.0");
            this.worldVersionElement.PositionRelativeToElement(this.worldCreationTimestampElement);

            layoutBuilder.AddElement(this.worldVersionElement);
        }

        private void BuildWorldButtons(ISGUILayoutBuilder layoutBuilder)
        {
            Vector2 margin = new(32f, -32f);

            for (int i = 0; i < this.worldButtons.Length; i++)
            {
                SButton button = this.worldButtons[i];

                SGUILabelElement buttonLabel = new(this.SGameInstance)
                {
                    Scale = new(0.12f),
                    Margin = margin,
                    SpriteFont = this.bigApple3PMSpriteFont,
                    PositionAnchor = SCardinalDirection.Southwest,
                    OriginPivot = SCardinalDirection.East,
                };

                buttonLabel.SetTextualContent(button.Name);
                buttonLabel.SetAllBorders(true, SColorPalette.DarkGray, new(2f));
                buttonLabel.PositionRelativeToScreen();

                layoutBuilder.AddElement(buttonLabel);
                margin.Y -= buttonLabel.GetStringSize().Height + 8f;

                this.worldButtonElements[i] = buttonLabel;
            }
        }
    }
}
