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
    internal sealed partial class SGUI_EnvironmentSettings
    {
        private SGUISliceImageElement panelBackgroundElement;
        private SGUISliceImageElement titleBackgroundElement;

        private SGUILabelElement menuTitleElement;
        private SGUILabelElement timeStateSectionTitleElement;
        private SGUILabelElement timeSectionTitleElement;

        private readonly SSlot[] menuButtonSlots;
        private readonly SSlot[] timeStateButtonSlots;
        private readonly SSlot[] timeButtonSlots;

        protected override void OnBuild(ISGUILayoutBuilder layoutBuilder)
        {
            BuildGUIBackground(layoutBuilder);
            BuildMenuButtons(layoutBuilder);
            BuildTitle(layoutBuilder);
            BuildTimeStateSection(layoutBuilder);
            BuildTimeSection(layoutBuilder);

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
                Margin = new(128),
                Color = new(104, 111, 121, 255),
            };

            this.titleBackgroundElement = new(this.SGameInstance)
            {
                Texture = this.guiBackgroundTexture,
                Scale = new(32, 0.5f),
                Size = new(32),
                Color = SColorPalette.DarkGreen,
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

            this.menuTitleElement.SetTextualContent("Environment Settings");
            this.menuTitleElement.SetAllBorders(true, SColorPalette.DarkGray, new(3f));
            this.menuTitleElement.PositionRelativeToElement(this.titleBackgroundElement);

            layoutBuilder.AddElement(this.menuTitleElement);
        }

        private void BuildTimeStateSection(ISGUILayoutBuilder layoutBuilder)
        {
            this.timeStateSectionTitleElement = new(this.SGameInstance)
            {
                Scale = new(0.1f),
                Margin = new(18, 64),
                Color = SColorPalette.White,
                SpriteFont = this.bigApple3PMSpriteFont,
            };

            this.timeStateSectionTitleElement.SetTextualContent("Time State");
            this.timeStateSectionTitleElement.PositionRelativeToElement(this.panelBackgroundElement);

            layoutBuilder.AddElement(this.timeStateSectionTitleElement);

            // Buttons
            Vector2 baseMargin = new(32, 80);

            for (int i = 0; i < this.timeStateButtonSlots.Length; i++)
            {
                SButton button = this.timeStateButtons[i];
                SSlot slot = CreateButtonSlot(baseMargin, button.IconTexture);

                slot.BackgroundElement.PositionAnchor = SCardinalDirection.South;
                slot.BackgroundElement.OriginPivot = SCardinalDirection.Center;

                // Update
                slot.BackgroundElement.PositionRelativeToElement(this.timeStateSectionTitleElement);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);

                // Save
                this.timeStateButtonSlots[i] = slot;

                // Spacing
                baseMargin.X += SGUI_HUDConstants.SLOT_SPACING + (SGUI_HUDConstants.SLOT_SIZE / 2);

                layoutBuilder.AddElement(slot.BackgroundElement);
                layoutBuilder.AddElement(slot.IconElement);
            }
        }

        private void BuildTimeSection(ISGUILayoutBuilder layoutBuilder)
        {
            this.timeSectionTitleElement = new(this.SGameInstance)
            {
                Scale = new(0.1f),
                Margin = new(this.timeStateSectionTitleElement.Size.Width + (SGUI_HUDConstants.SLOT_SIZE * SGUI_HUDConstants.SLOT_SCALE * this.timeStateButtonSlots.Length) + 64, 0f),
                Color = SColorPalette.White,
                SpriteFont = this.bigApple3PMSpriteFont,
            };

            this.timeSectionTitleElement.SetTextualContent("Time");
            this.timeSectionTitleElement.PositionRelativeToElement(this.timeStateSectionTitleElement);

            layoutBuilder.AddElement(this.timeSectionTitleElement);

            // Buttons
            Vector2 baseMargin = new(32, 80);

            for (int i = 0; i < this.timeButtonSlots.Length; i++)
            {
                SButton button = this.timeButtons[i];
                SSlot slot = CreateButtonSlot(baseMargin, button.IconTexture);

                slot.BackgroundElement.PositionAnchor = SCardinalDirection.South;
                slot.BackgroundElement.OriginPivot = SCardinalDirection.Center;

                // Update
                slot.BackgroundElement.PositionRelativeToElement(this.timeSectionTitleElement);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);

                // Save
                this.timeButtonSlots[i] = slot;

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
