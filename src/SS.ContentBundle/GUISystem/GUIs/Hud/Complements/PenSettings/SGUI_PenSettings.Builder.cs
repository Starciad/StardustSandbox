using Microsoft.Xna.Framework;

using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces.GUI;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements
{
    internal sealed partial class SGUI_PenSettings
    {
        private SGUISliceImageElement panelBackgroundElement;

        protected override void OnBuild(ISGUILayoutBuilder layoutBuilder)
        {
            BuildGUIBackground(layoutBuilder);
            BuildPanel(layoutBuilder);
            BuildTools(layoutBuilder);
            BuildBrushSizes(layoutBuilder);
            BuildBrushTypes(layoutBuilder);
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

        private void BuildPanel(ISGUILayoutBuilder layoutBuilder)
        {
            this.panelBackgroundElement = new(this.SGameInstance)
            {
                Texture = this.guiBackgroundTexture,
                Scale = new Vector2(32, 15),
                Size = new(32),
                Margin = new Vector2(128f),
                Color = new Color(104, 111, 121, 255)
            };

            SGUISliceImageElement titleBackgroundElement = new(this.SGameInstance)
            {
                Texture = this.guiBackgroundTexture,
                Scale = new Vector2(32, 0.5f),
                Size = new(32),
                Color = SColorPalette.Rust,
            };
            
            SGUILabelElement titleLabelElement = new(this.SGameInstance)
            {
                SpriteFont = this.bigApple3PMSpriteFont,
                Scale = new Vector2(0.12f),
                PositionAnchor = SCardinalDirection.West,
                OriginPivot = SCardinalDirection.East,
                Margin = new Vector2(16, 0),
                Color = SColorPalette.White,
            };

            titleLabelElement.SetTextualContent("Pen Settings");
            titleLabelElement.SetAllBorders(true, SColorPalette.DarkGray, new Vector2(3f));

            this.panelBackgroundElement.PositionRelativeToScreen();
            titleBackgroundElement.PositionRelativeToElement(this.panelBackgroundElement);
            titleLabelElement.PositionRelativeToElement(titleBackgroundElement);

            layoutBuilder.AddElement(this.panelBackgroundElement);
            layoutBuilder.AddElement(titleBackgroundElement);
            layoutBuilder.AddElement(titleLabelElement);
        }

        private void BuildBrushSizes(ISGUILayoutBuilder layoutBuilder)
        {
            SGUILabelElement titleLabelElement = new(this.SGameInstance)
            {
                Scale = new Vector2(0.1f),
                Margin = new Vector2(18, 64),
                Color = SColorPalette.White,
                SpriteFont = this.bigApple3PMSpriteFont,
            };

            SGUIImageElement brushSizeSliderElement = new(this.SGameInstance)
            {
                Texture = this.guiSliderTexture,
                TextureClipArea = new(new(0, 0), new(326, 38)),
                Size = new(326, 38),
                Scale = new(2f),
                Margin = new(0, 48),
                PositionAnchor = SCardinalDirection.South,
            };

            titleLabelElement.SetTextualContent("Brush Size");
            // titleLabelElement.SetAllBorders(true, SColorPalette.DarkGray, new Vector2(3.5f));
            
            titleLabelElement.PositionRelativeToElement(this.panelBackgroundElement);
            brushSizeSliderElement.PositionRelativeToElement(titleLabelElement);

            layoutBuilder.AddElement(titleLabelElement);
            layoutBuilder.AddElement(brushSizeSliderElement);
        }

        private void BuildTools(ISGUILayoutBuilder layoutBuilder)
        {

        }

        private void BuildBrushTypes(ISGUILayoutBuilder layoutBuilder)
        {

        }
    }
}
