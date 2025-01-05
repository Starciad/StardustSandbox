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
    internal sealed partial class SGUI_SaveSettings
    {
        private SGUIImageElement panelBackgroundElement;

        private SGUILabelElement menuTitleElement;
        private SGUILabelElement nameSectionTitleElement;
        private SGUILabelElement descriptionSectionTitleElement;
        private SGUILabelElement thumbnailSectionTitleElement;

        private SGUIImageElement titleInputFieldElement;
        private SGUIImageElement descriptionInputFieldElement;

        private SGUILabelElement titleTextualContentElement;
        private SGUILabelElement descriptionTextualContentElement;

        private SGUIImageElement thumbnailPreviewElement;

        private readonly SSlot[] menuButtonSlots;
        private readonly SSlot[] fieldButtonSlots;
        private readonly SSlot[] footerButtonSlots;

        protected override void OnBuild(ISGUILayoutBuilder layoutBuilder)
        {
            BuildBackground(layoutBuilder);
            BuildTitle(layoutBuilder);
            BuildMenuButtons(layoutBuilder);
            BuildNameSection(layoutBuilder);
            BuildDescriptionSection(layoutBuilder);
            BuildThumbnailSection(layoutBuilder);
            BuildFooterButtons(layoutBuilder);

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

            this.menuTitleElement.SetTextualContent(SLocalization_GUIs.HUD_Complements_SaveSettings_Title);
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
                SSlot slot = CreateButtonSlot(margin, button.IconTexture);

                slot.BackgroundElement.PositionAnchor = SCardinalDirection.Northeast;
                slot.BackgroundElement.OriginPivot = SCardinalDirection.Center;

                // Update
                slot.BackgroundElement.PositionRelativeToElement(this.panelBackgroundElement);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);

                // Save
                this.menuButtonSlots[i] = slot;

                // Spacing
                margin.X -= SGUI_HUDConstants.SLOT_SPACING + (SGUI_HUDConstants.SLOT_SIZE / 2);

                layoutBuilder.AddElement(slot.BackgroundElement);
                layoutBuilder.AddElement(slot.IconElement);
            }
        }

        private void BuildNameSection(ISGUILayoutBuilder layoutBuilder)
        {
            this.nameSectionTitleElement = new(this.SGameInstance)
            {
                Scale = new(0.1f),
                Margin = new(32, 112),
                SpriteFont = this.bigApple3PMSpriteFont,
            };

            this.titleInputFieldElement = new(this.SGameInstance)
            {
                Texture = this.guiFieldTexture,
                Scale = new(2f),
                Size = new(163f, 38f),
                Margin = new(0f, 48f),
            };

            this.titleTextualContentElement = new(this.SGameInstance)
            {
                Scale = new(0.1f),
                Margin = new(16f, 0f),
                SpriteFont = this.pixelOperatorSpriteFont,
                OriginPivot = SCardinalDirection.East,
                PositionAnchor = SCardinalDirection.West
            };

            this.nameSectionTitleElement.SetTextualContent(SLocalization_GUIs.HUD_Complements_SaveSettings_Section_Name_Title);

            this.nameSectionTitleElement.PositionRelativeToElement(this.panelBackgroundElement);
            this.titleInputFieldElement.PositionRelativeToElement(this.nameSectionTitleElement);
            this.titleTextualContentElement.PositionRelativeToElement(this.titleInputFieldElement);

            this.fieldButtonSlots[0] = new(this.titleInputFieldElement, null, this.titleTextualContentElement);

            layoutBuilder.AddElement(this.nameSectionTitleElement);
            layoutBuilder.AddElement(this.titleInputFieldElement);
            layoutBuilder.AddElement(this.titleTextualContentElement);
        }

        private void BuildDescriptionSection(ISGUILayoutBuilder layoutBuilder)
        {
            this.descriptionSectionTitleElement = new(this.SGameInstance)
            {
                Scale = new(0.1f),
                Margin = new(0, 96f),
                SpriteFont = this.bigApple3PMSpriteFont,
            };

            this.descriptionInputFieldElement = new(this.SGameInstance)
            {
                Texture = this.guiFieldTexture,
                Scale = new(2f),
                Size = new(163f, 38f),
                Margin = new(0f, 48f),
            };

            this.descriptionTextualContentElement = new(this.SGameInstance)
            {
                Scale = new(0.1f),
                Margin = new(16f, 0f),
                SpriteFont = this.pixelOperatorSpriteFont,
                OriginPivot = SCardinalDirection.East,
                PositionAnchor = SCardinalDirection.West
            };

            this.descriptionSectionTitleElement.SetTextualContent(SLocalization_GUIs.HUD_Complements_SaveSettings_Section_Description_Title);

            this.descriptionSectionTitleElement.PositionRelativeToElement(this.titleInputFieldElement);
            this.descriptionInputFieldElement.PositionRelativeToElement(this.descriptionSectionTitleElement);
            this.descriptionTextualContentElement.PositionRelativeToElement(this.descriptionInputFieldElement);

            this.fieldButtonSlots[1] = new(this.descriptionInputFieldElement, null, this.descriptionTextualContentElement);

            layoutBuilder.AddElement(this.descriptionSectionTitleElement);
            layoutBuilder.AddElement(this.descriptionInputFieldElement);
            layoutBuilder.AddElement(this.descriptionTextualContentElement);
        }

        private void BuildThumbnailSection(ISGUILayoutBuilder layoutBuilder)
        {
            this.thumbnailSectionTitleElement = new(this.SGameInstance)
            {
                Scale = new(0.1f),
                Margin = new(-32f, 112f),
                SpriteFont = this.bigApple3PMSpriteFont,
                PositionAnchor = SCardinalDirection.Northeast,
                OriginPivot = SCardinalDirection.Southwest
            };

            this.thumbnailPreviewElement = new(this.SGameInstance)
            {
                Scale = new(12.5f),
                Margin = new(0f, 48f),
                OriginPivot = SCardinalDirection.Southwest,
            };

            this.thumbnailSectionTitleElement.SetTextualContent(SLocalization_GUIs.HUD_Complements_SaveSettings_Section_Thumbnail_Title);
            this.thumbnailSectionTitleElement.PositionRelativeToElement(this.panelBackgroundElement);
            this.thumbnailPreviewElement.PositionRelativeToElement(this.thumbnailSectionTitleElement);

            layoutBuilder.AddElement(this.thumbnailSectionTitleElement);
            layoutBuilder.AddElement(this.thumbnailPreviewElement);
        }

        private void BuildFooterButtons(ISGUILayoutBuilder layoutBuilder)
        {
            Vector2 margin = new(32f, -96f);

            for (int i = 0; i < this.footerButtons.Length; i++)
            {
                SButton button = this.footerButtons[i];

                SGUIImageElement backgroundElement = new(this.SGameInstance)
                {
                    Texture = this.guiLargeButtonTexture,
                    Color = SColorPalette.PurpleGray,
                    Scale = new(1f),
                    Size = new(320, 80),
                    Margin = margin,
                    PositionAnchor = SCardinalDirection.Southwest,
                };

                SGUILabelElement labelElement = new(this.SGameInstance)
                {
                    Scale = new(0.1f),
                    Color = SColorPalette.White,
                    SpriteFont = this.bigApple3PMSpriteFont,
                    PositionAnchor = SCardinalDirection.Center,
                    OriginPivot = SCardinalDirection.Center
                };

                labelElement.SetTextualContent(button.DisplayName);
                labelElement.SetAllBorders(true, SColorPalette.DarkGray, new(2));

                backgroundElement.PositionRelativeToElement(this.panelBackgroundElement);
                labelElement.PositionRelativeToElement(backgroundElement);

                layoutBuilder.AddElement(backgroundElement);
                layoutBuilder.AddElement(labelElement);

                this.footerButtonSlots[i] = new(backgroundElement, null, labelElement);

                margin.X += backgroundElement.Size.Width + 32;
            }
        }

        // =============================================================== //

        private SSlot CreateButtonSlot(Vector2 margin, Texture2D iconTexture)
        {
            SGUIImageElement backgroundElement = new(this.SGameInstance)
            {
                Texture = this.guiSmallButtonTexture,
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
