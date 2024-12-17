using Microsoft.Xna.Framework;

using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.ContentBundle.GUISystem.Specials.Interactive;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.Fonts;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.Complements
{
    internal sealed partial class SGUI_WorldDetailsMenu
    {
        private SGUIImageElement headerBackgroundElement;

        private SGUILabelElement worldTitleElement;
        private SGUIImageElement worldThumbnailElement;
        private SGUITextElement worldDescriptionElement;

        protected override void OnBuild(ISGUILayoutBuilder layoutBuilder)
        {
            BuildGUIBackground(layoutBuilder);
            BuildHeader(layoutBuilder);
            BuildThumbnail(layoutBuilder);
            BuildDescription(layoutBuilder);
        }

        private void BuildGUIBackground(ISGUILayoutBuilder layoutBuilder)
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
            SGUILabelElement titleLabelElement = new(this.SGameInstance)
            {
                Scale = new(0.15f),
                SpriteFont = this.bigApple3PMSpriteFont,
                PositionAnchor = SCardinalDirection.West,
                OriginPivot = SCardinalDirection.East,
                Margin = new(32f, 0f),
            };

            titleLabelElement.SetTextualContent("Title");
            titleLabelElement.SetAllBorders(true, SColorPalette.DarkGray, new(2f));
            titleLabelElement.PositionRelativeToElement(this.headerBackgroundElement);

            layoutBuilder.AddElement(this.headerBackgroundElement);
            layoutBuilder.AddElement(titleLabelElement);
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
    }
}
