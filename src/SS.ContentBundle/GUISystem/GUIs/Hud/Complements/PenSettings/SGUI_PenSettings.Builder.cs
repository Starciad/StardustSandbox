using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.ContentBundle.GUISystem.Specials.General;
using StardustSandbox.ContentBundle.GUISystem.Specials.Interactive;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.GUI.Common;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements
{
    internal sealed partial class SGUI_PenSettings
    {
        private SGUISliceImageElement panelBackgroundElement;

        private SGUILabelElement menuTitleElement;
        private SGUILabelElement brushSectionTitleElement;
        private SGUILabelElement toolsSectionTitleElement;
        private SGUILabelElement layerSectionTitleElement;

        private SGUIImageElement brushSizeSliderElement;

        private readonly SSlot[] toolButtonSlots;
        private readonly SSlot[] layerButtonSlots;

        protected override void OnBuild(ISGUILayoutBuilder layoutBuilder)
        {
            BuildGUIBackground(layoutBuilder);
            BuildPanel(layoutBuilder);
            BuildBrushSizeSection(layoutBuilder);
            BuildToolSection(layoutBuilder);
            BuildLayerSection(layoutBuilder);
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

        private void BuildBrushSizeSection(ISGUILayoutBuilder layoutBuilder)
        {
            this.brushSectionTitleElement = new(this.SGameInstance)
            {
                Scale = new Vector2(0.1f),
                Margin = new Vector2(18, 64),
                Color = SColorPalette.White,
                SpriteFont = this.bigApple3PMSpriteFont,
            };

            this.brushSizeSliderElement = new(this.SGameInstance)
            {
                Texture = this.guiSliderTexture,
                TextureClipArea = new(new(0, 0), new(326, 38)),
                Size = new(326, 38),
                Scale = new(2f),
                Margin = new(0, 48),
                PositionAnchor = SCardinalDirection.South,
            };

            this.brushSectionTitleElement.SetTextualContent("Brush Size");

            this.brushSectionTitleElement.PositionRelativeToElement(this.panelBackgroundElement);
            this.brushSizeSliderElement.PositionRelativeToElement(this.brushSectionTitleElement);

            layoutBuilder.AddElement(this.brushSectionTitleElement);
            layoutBuilder.AddElement(this.brushSizeSliderElement);
        }

        private void BuildToolSection(ISGUILayoutBuilder layoutBuilder)
        {
            this.toolsSectionTitleElement = new(this.SGameInstance)
            {
                Scale = new Vector2(0.1f),
                Margin = new Vector2(0, 128),
                Color = SColorPalette.White,
                SpriteFont = this.bigApple3PMSpriteFont,
            };

            this.toolsSectionTitleElement.SetTextualContent("Tool");
            this.toolsSectionTitleElement.PositionRelativeToElement(this.brushSectionTitleElement);

            layoutBuilder.AddElement(this.toolsSectionTitleElement);

            // Buttons
            Vector2 baseMargin = new(32, 80);

            for (int i = 0; i < this.toolButtons.Length; i++)
            {
                SButton button = this.toolButtons[i];
                SSlot slot = CreateButtonSlot(baseMargin, button.IconTexture, button.DisplayName);

                slot.BackgroundElement.PositionAnchor = SCardinalDirection.South;
                slot.BackgroundElement.OriginPivot = SCardinalDirection.Center;

                slot.LabelElement.PositionAnchor = SCardinalDirection.South;
                slot.LabelElement.OriginPivot = SCardinalDirection.Center;

                // Update
                slot.BackgroundElement.PositionRelativeToElement(this.toolsSectionTitleElement);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);
                slot.LabelElement.PositionRelativeToElement(slot.BackgroundElement);

                // Save
                this.toolButtonSlots[i] = slot;

                // Spacing
                baseMargin.X += SHUDConstants.SLOT_SPACING + (SHUDConstants.SLOT_SIZE / 2);

                layoutBuilder.AddElement(slot.BackgroundElement);
                layoutBuilder.AddElement(slot.IconElement);
                layoutBuilder.AddElement(slot.LabelElement);
            }
        }

        private void BuildLayerSection(ISGUILayoutBuilder layoutBuilder)
        {
            this.brushSectionTitleElement = new(this.SGameInstance)
            {
                Scale = new Vector2(0.1f),
                Color = SColorPalette.White,
                SpriteFont = this.bigApple3PMSpriteFont,
                Margin = new(this.toolsSectionTitleElement.Size.Width + SHUDConstants.SLOT_SIZE * SHUDConstants.SLOT_SCALE * this.toolButtonSlots.Length + 96, 0f)
            };

            this.brushSectionTitleElement.SetTextualContent("Layer");
            this.brushSectionTitleElement.PositionRelativeToElement(this.toolsSectionTitleElement);

            layoutBuilder.AddElement(this.brushSectionTitleElement);

            // Buttons
            Vector2 baseMargin = new(32, 80);

            for (int i = 0; i < this.layerButtons.Length; i++)
            {
                SButton button = this.layerButtons[i];
                SSlot slot = CreateButtonSlot(baseMargin, button.IconTexture, button.DisplayName);

                slot.BackgroundElement.PositionAnchor = SCardinalDirection.South;
                slot.BackgroundElement.OriginPivot = SCardinalDirection.Center;

                slot.LabelElement.PositionAnchor = SCardinalDirection.South;
                slot.LabelElement.OriginPivot = SCardinalDirection.Center;

                // Update
                slot.BackgroundElement.PositionRelativeToElement(this.brushSectionTitleElement);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);
                slot.LabelElement.PositionRelativeToElement(slot.BackgroundElement);

                // Save
                this.layerButtonSlots[i] = slot;

                // Spacing
                baseMargin.X += SHUDConstants.SLOT_SPACING + (SHUDConstants.SLOT_SIZE / 2);

                layoutBuilder.AddElement(slot.BackgroundElement);
                layoutBuilder.AddElement(slot.IconElement);
                layoutBuilder.AddElement(slot.LabelElement);
            }
        }

        // =============================================================== //

        private SSlot CreateButtonSlot(Vector2 margin, Texture2D iconTexture, string labelContent)
        {
            SGUIImageElement backgroundElement = new(this.SGameInstance)
            {
                Texture = this.guiButton1Texture,
                Scale = new Vector2(SHUDConstants.SLOT_SCALE),
                Size = new SSize2(SHUDConstants.SLOT_SIZE),
                Margin = margin,
            };

            SGUIImageElement iconElement = new(this.SGameInstance)
            {
                Texture = iconTexture,
                OriginPivot = SCardinalDirection.Center,
                Scale = new Vector2(1.5f),
                Size = new SSize2(SHUDConstants.SLOT_SIZE)
            };

            SGUILabelElement labelElement = new(this.SGameInstance)
            {
                SpriteFont = this.bigApple3PMSpriteFont,
                Scale = new(0.05f),
                Margin = new(-32, -16),
            };

            labelElement.SetTextualContent(labelContent);

            return new(backgroundElement, iconElement, labelElement);
        }
    }
}
