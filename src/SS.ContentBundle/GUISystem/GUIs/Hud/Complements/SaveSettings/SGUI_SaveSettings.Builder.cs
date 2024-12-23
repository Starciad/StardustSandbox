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
    internal sealed partial class SGUI_SaveSettings
    {
        private SGUISliceImageElement panelBackgroundElement;
        private SGUISliceImageElement titleBackgroundElement;

        private SGUILabelElement menuTitleElement;
        private SGUILabelElement nameSectionTitleElement;
        private SGUILabelElement descriptionSectionTitleElement;

        private SGUIImageElement titleInputFieldElement;
        private SGUIImageElement descriptionInputFieldElement;

        private readonly SSlot[] menuButtonSlots;

        protected override void OnBuild(ISGUILayoutBuilder layoutBuilder)
        {
            BuildGUIBackground(layoutBuilder);
            BuildMenuButtons(layoutBuilder);
            BuildTitle(layoutBuilder);
            BuildNameSection(layoutBuilder);
            BuildDescriptionSection(layoutBuilder);
            BuildThumbnailSection(layoutBuilder);
            BuildSaveButtons(layoutBuilder);
        }

        private void BuildGUIBackground(ISGUILayoutBuilder layoutBuilder)
        {
            SGUIImageElement guiBackground = new(this.SGameInstance)
            {
                Texture = this.particleTexture,
                Scale = new(SScreenConstants.DEFAULT_SCREEN_WIDTH, SScreenConstants.DEFAULT_SCREEN_HEIGHT),
                Size = new(1),
                Color = new Color(SColorPalette.DarkGray, 160)
            };

            this.panelBackgroundElement = new(this.SGameInstance)
            {
                Texture = this.guiBackgroundTexture,
                Scale = new Vector2(32, 15),
                Size = new(32),
                Margin = new Vector2(128f),
                Color = new Color(104, 111, 121, 255),
            };

            this.titleBackgroundElement = new(this.SGameInstance)
            {
                Texture = this.guiBackgroundTexture,
                Scale = new Vector2(32, 0.5f),
                Size = new(32),
                Color = SColorPalette.PurpleGray,
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
                baseMargin.X -= SHUDConstants.SLOT_SPACING + (SHUDConstants.SLOT_SIZE / 2);

                layoutBuilder.AddElement(slot.BackgroundElement);
                layoutBuilder.AddElement(slot.IconElement);
            }
        }

        private void BuildTitle(ISGUILayoutBuilder layoutBuilder)
        {
            this.menuTitleElement = new(this.SGameInstance)
            {
                SpriteFont = this.bigApple3PMSpriteFont,
                Scale = new Vector2(0.12f),
                PositionAnchor = SCardinalDirection.West,
                OriginPivot = SCardinalDirection.East,
                Margin = new Vector2(16, 0),
                Color = SColorPalette.White,
            };

            this.menuTitleElement.SetTextualContent("Save Settings");
            this.menuTitleElement.SetAllBorders(true, SColorPalette.DarkGray, new Vector2(3f));
            this.menuTitleElement.PositionRelativeToElement(this.titleBackgroundElement);

            layoutBuilder.AddElement(this.menuTitleElement);
        }

        private void BuildNameSection(ISGUILayoutBuilder layoutBuilder)
        {
            this.nameSectionTitleElement = new(this.SGameInstance)
            {
                Scale = new(0.1f),
                Margin = new(0, 64),
                Color = SColorPalette.White,
                SpriteFont = this.bigApple3PMSpriteFont,
            };

            this.titleInputFieldElement = new(this.SGameInstance)
            {
                Texture = this.guiField1Texture,
                TextureClipArea = new(new(0, 0), new(326, 38)),
                Scale = new(2f),
                Margin = new(0f, 48f),
            };

            this.nameSectionTitleElement.SetTextualContent("Name");
            this.nameSectionTitleElement.PositionRelativeToElement(this.panelBackgroundElement);
            this.titleInputFieldElement.PositionRelativeToElement(this.nameSectionTitleElement);

            layoutBuilder.AddElement(this.nameSectionTitleElement);
            layoutBuilder.AddElement(this.titleInputFieldElement);
        }

        private void BuildDescriptionSection(ISGUILayoutBuilder layoutBuilder)
        {
            this.descriptionSectionTitleElement = new(this.SGameInstance)
            {
                Scale = new(0.1f),
                Margin = new(0, 96f),
                Color = SColorPalette.White,
                SpriteFont = this.bigApple3PMSpriteFont,
            };

            this.descriptionInputFieldElement = new(this.SGameInstance)
            {
                Texture = this.guiField1Texture,
                TextureClipArea = new(new(0, 38), new(326, 76)),
                Scale = new(2f),
                Margin = new(0f, 48f),
            };

            this.descriptionSectionTitleElement.SetTextualContent("Description");
            this.descriptionSectionTitleElement.PositionRelativeToElement(this.titleInputFieldElement);
            this.descriptionInputFieldElement.PositionRelativeToElement(this.descriptionSectionTitleElement);

            layoutBuilder.AddElement(this.descriptionSectionTitleElement);
            layoutBuilder.AddElement(this.descriptionInputFieldElement);
        }

        private void BuildThumbnailSection(ISGUILayoutBuilder layoutBuilder)
        {

        }

        private void BuildSaveButtons(ISGUILayoutBuilder layoutBuilder)
        {

        }

        // =============================================================== //

        private SSlot CreateButtonSlot(Vector2 margin, Texture2D iconTexture)
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

            return new(backgroundElement, iconElement);
        }
    }
}
