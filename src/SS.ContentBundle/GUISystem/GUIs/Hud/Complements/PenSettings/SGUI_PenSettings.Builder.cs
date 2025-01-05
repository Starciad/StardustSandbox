using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.ContentBundle.GUISystem.Specials.General;
using StardustSandbox.ContentBundle.GUISystem.Specials.Interactive;
using StardustSandbox.ContentBundle.Localization.GUIs;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.GUISystem.GUIs.Hud;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces.GUI;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements
{
    internal sealed partial class SGUI_PenSettings
    {
        private SGUISliceImageElement panelBackgroundElement;
        private SGUISliceImageElement titleBackgroundElement;

        private SGUILabelElement menuTitleElement;
        private SGUILabelElement brushSectionTitleElement;
        private SGUILabelElement toolsSectionTitleElement;
        private SGUILabelElement layerSectionTitleElement;
        private SGUILabelElement shapeSectionTitleElement;

        private SGUIImageElement brushSizeSliderElement;

        private readonly SSlot[] menuButtonSlots;
        private readonly SSlot[] toolButtonSlots;
        private readonly SSlot[] layerButtonSlots;
        private readonly SSlot[] shapeButtonSlots;

        protected override void OnBuild(ISGUILayoutBuilder layoutBuilder)
        {
            BuildGUIBackground(layoutBuilder);
            BuildMenuButtons(layoutBuilder);
            BuildTitle(layoutBuilder);
            BuildBrushSizeSection(layoutBuilder);
            BuildToolSection(layoutBuilder);
            BuildLayerSection(layoutBuilder);
            BuildShapeSection(layoutBuilder);

            layoutBuilder.AddElement(this.tooltipBoxElement);
        }

        private void BuildGUIBackground(ISGUILayoutBuilder layoutBuilder)
        {
            SGUIImageElement guiBackground = new(this.SGameInstance)
            {
                Texture = this.particleTexture,
                Scale = new(SScreenConstants.DEFAULT_SCREEN_WIDTH, SScreenConstants.DEFAULT_SCREEN_HEIGHT),
                Size = new(1),
                Color = new(SColorPalette.DarkGray, 160)
            };

            this.panelBackgroundElement = new(this.SGameInstance)
            {
                Texture = this.guiBackgroundTexture,
                Scale = new(32, 15),
                Size = new(32),
                Margin = new(128f),
                Color = new(104, 111, 121, 255),
            };

            this.titleBackgroundElement = new(this.SGameInstance)
            {
                Texture = this.guiBackgroundTexture,
                Scale = new(32, 0.5f),
                Size = new(32),
                Color = SColorPalette.Rust,
            };

            this.panelBackgroundElement.PositionRelativeToScreen();
            this.titleBackgroundElement.PositionRelativeToElement(this.panelBackgroundElement);

            layoutBuilder.AddElement(guiBackground);
            layoutBuilder.AddElement(this.panelBackgroundElement);
            layoutBuilder.AddElement(this.titleBackgroundElement);
        }

        private void BuildMenuButtons(ISGUILayoutBuilder layoutBuilder)
        {
            Vector2 baseMargin = new(-2, -72);

            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                SButton button = this.menuButtons[i];
                SSlot slot = CreateButtonSlot(baseMargin, button.IconTexture);

                slot.BackgroundElement.PositionAnchor = SCardinalDirection.Northeast;
                slot.BackgroundElement.OriginPivot = SCardinalDirection.Center;

                // Update
                slot.BackgroundElement.PositionRelativeToElement(this.panelBackgroundElement);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);

                // Save
                this.menuButtonSlots[i] = slot;

                // Spacing
                baseMargin.X -= SGUI_HUDConstants.SLOT_SPACING + (SGUI_HUDConstants.SLOT_SIZE / 2);

                layoutBuilder.AddElement(slot.BackgroundElement);
                layoutBuilder.AddElement(slot.IconElement);
            }
        }

        private void BuildTitle(ISGUILayoutBuilder layoutBuilder)
        {
            this.menuTitleElement = new(this.SGameInstance)
            {
                SpriteFont = this.bigApple3PMSpriteFont,
                Scale = new(0.12f),
                PositionAnchor = SCardinalDirection.West,
                OriginPivot = SCardinalDirection.East,
                Margin = new(16, 0),
                Color = SColorPalette.White,
            };

            this.menuTitleElement.SetTextualContent(SLocalization_GUIs.HUD_Complements_PenSettings_Title);
            this.menuTitleElement.SetAllBorders(true, SColorPalette.DarkGray, new(3f));
            this.menuTitleElement.PositionRelativeToElement(this.titleBackgroundElement);

            layoutBuilder.AddElement(this.menuTitleElement);
        }

        private void BuildBrushSizeSection(ISGUILayoutBuilder layoutBuilder)
        {
            this.brushSectionTitleElement = new(this.SGameInstance)
            {
                Scale = new(0.1f),
                Margin = new(18, 64),
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

            this.brushSectionTitleElement.SetTextualContent(SLocalization_GUIs.HUD_Complements_PenSettings_Section_BrushSize_Title);

            this.brushSectionTitleElement.PositionRelativeToElement(this.panelBackgroundElement);
            this.brushSizeSliderElement.PositionRelativeToElement(this.brushSectionTitleElement);

            layoutBuilder.AddElement(this.brushSectionTitleElement);
            layoutBuilder.AddElement(this.brushSizeSliderElement);
        }

        private void BuildToolSection(ISGUILayoutBuilder layoutBuilder)
        {
            this.toolsSectionTitleElement = new(this.SGameInstance)
            {
                Scale = new(0.1f),
                Margin = new(0, 128),
                Color = SColorPalette.White,
                SpriteFont = this.bigApple3PMSpriteFont,
            };

            this.toolsSectionTitleElement.SetTextualContent(SLocalization_GUIs.HUD_Complements_PenSettings_Section_Tool_Title);
            this.toolsSectionTitleElement.PositionRelativeToElement(this.brushSectionTitleElement);

            layoutBuilder.AddElement(this.toolsSectionTitleElement);

            // Buttons
            Vector2 baseMargin = new(32, 80);

            for (int i = 0; i < this.toolButtons.Length; i++)
            {
                SButton button = this.toolButtons[i];
                SSlot slot = CreateButtonSlot(baseMargin, button.IconTexture);

                slot.BackgroundElement.PositionAnchor = SCardinalDirection.South;
                slot.BackgroundElement.OriginPivot = SCardinalDirection.Center;

                // Update
                slot.BackgroundElement.PositionRelativeToElement(this.toolsSectionTitleElement);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);

                // Save
                this.toolButtonSlots[i] = slot;

                // Spacing
                baseMargin.X += SGUI_HUDConstants.SLOT_SPACING + (SGUI_HUDConstants.SLOT_SIZE / 2);

                layoutBuilder.AddElement(slot.BackgroundElement);
                layoutBuilder.AddElement(slot.IconElement);
            }
        }

        private void BuildLayerSection(ISGUILayoutBuilder layoutBuilder)
        {
            this.layerSectionTitleElement = new(this.SGameInstance)
            {
                Scale = new(0.1f),
                Color = SColorPalette.White,
                SpriteFont = this.bigApple3PMSpriteFont,
                Margin = new(this.toolsSectionTitleElement.Size.Width + (SGUI_HUDConstants.SLOT_SIZE * SGUI_HUDConstants.SLOT_SCALE * this.toolButtonSlots.Length) + 96, 0f)
            };

            this.layerSectionTitleElement.SetTextualContent(SLocalization_GUIs.HUD_Complements_PenSettings_Section_Layer_Title);
            this.layerSectionTitleElement.PositionRelativeToElement(this.toolsSectionTitleElement);

            layoutBuilder.AddElement(this.layerSectionTitleElement);

            // Buttons
            Vector2 baseMargin = new(32, 80);

            for (int i = 0; i < this.layerButtons.Length; i++)
            {
                SButton button = this.layerButtons[i];
                SSlot slot = CreateButtonSlot(baseMargin, button.IconTexture);

                slot.BackgroundElement.PositionAnchor = SCardinalDirection.South;
                slot.BackgroundElement.OriginPivot = SCardinalDirection.Center;

                // Update
                slot.BackgroundElement.PositionRelativeToElement(this.layerSectionTitleElement);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);

                // Save
                this.layerButtonSlots[i] = slot;

                // Spacing
                baseMargin.X += SGUI_HUDConstants.SLOT_SPACING + (SGUI_HUDConstants.SLOT_SIZE / 2);

                layoutBuilder.AddElement(slot.BackgroundElement);
                layoutBuilder.AddElement(slot.IconElement);
            }
        }

        private void BuildShapeSection(ISGUILayoutBuilder layoutBuilder)
        {
            this.shapeSectionTitleElement = new(this.SGameInstance)
            {
                Scale = new(0.1f),
                Color = SColorPalette.White,
                SpriteFont = this.bigApple3PMSpriteFont,
                Margin = new(this.layerSectionTitleElement.Size.Width + (SGUI_HUDConstants.SLOT_SIZE * SGUI_HUDConstants.SLOT_SCALE * this.layerButtonSlots.Length) + 48, 0f)
            };

            this.shapeSectionTitleElement.SetTextualContent(SLocalization_GUIs.HUD_Complements_PenSettings_Section_Shape_Title);
            this.shapeSectionTitleElement.PositionRelativeToElement(this.layerSectionTitleElement);

            layoutBuilder.AddElement(this.shapeSectionTitleElement);

            // Buttons
            Vector2 baseMargin = new(32, 80);

            for (int i = 0; i < this.shapeButtons.Length; i++)
            {
                SButton button = this.shapeButtons[i];
                SSlot slot = CreateButtonSlot(baseMargin, button.IconTexture);

                slot.BackgroundElement.PositionAnchor = SCardinalDirection.South;
                slot.BackgroundElement.OriginPivot = SCardinalDirection.Center;

                // Update
                slot.BackgroundElement.PositionRelativeToElement(this.shapeSectionTitleElement);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);

                // Save
                this.shapeButtonSlots[i] = slot;

                // Spacing
                baseMargin.X += SGUI_HUDConstants.SLOT_SPACING + (SGUI_HUDConstants.SLOT_SIZE / 2);

                layoutBuilder.AddElement(slot.BackgroundElement);
                layoutBuilder.AddElement(slot.IconElement);
            }
        }

        // =============================================================== //

        private SSlot CreateButtonSlot(Vector2 margin, Texture2D iconTexture)
        {
            SGUIImageElement backgroundElement = new(this.SGameInstance)
            {
                Texture = this.guiButton1Texture,
                Scale = new(SGUI_HUDConstants.SLOT_SCALE),
                Size = new(SGUI_HUDConstants.SLOT_SIZE),
                Margin = margin,
            };

            SGUIImageElement iconElement = new(this.SGameInstance)
            {
                Texture = iconTexture,
                OriginPivot = SCardinalDirection.Center,
                Scale = new(1.5f),
                Size = new(SGUI_HUDConstants.SLOT_SIZE)
            };

            return new(backgroundElement, iconElement);
        }
    }
}
