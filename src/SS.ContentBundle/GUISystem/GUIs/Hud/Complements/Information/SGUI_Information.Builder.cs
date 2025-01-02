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

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements
{
    internal sealed partial class SGUI_Information
    {
        private SGUISliceImageElement panelBackgroundElement;
        private SGUISliceImageElement titleBackgroundElement;

        private SGUILabelElement menuTitleElement;

        private readonly SSlot[] menuButtonSlots;

        protected override void OnBuild(ISGUILayoutBuilder layoutBuilder)
        {
            BuildGUIBackground(layoutBuilder);
            BuildMenuButtons(layoutBuilder);
            BuildTitle(layoutBuilder);
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
                Scale = new(0.12f),
                PositionAnchor = SCardinalDirection.West,
                OriginPivot = SCardinalDirection.East,
                Margin = new(16, 0),
                Color = SColorPalette.White,
            };

            this.menuTitleElement.SetTextualContent("Information");
            this.menuTitleElement.SetAllBorders(true, SColorPalette.DarkGray, new(3f));
            this.menuTitleElement.PositionRelativeToElement(this.titleBackgroundElement);

            layoutBuilder.AddElement(this.menuTitleElement);
        }

        // =============================================================== //

        private SSlot CreateButtonSlot(Vector2 margin, Texture2D iconTexture)
        {
            SGUIImageElement backgroundElement = new(this.SGameInstance)
            {
                Texture = this.guiButton1Texture,
                Scale = new(SHUDConstants.SLOT_SCALE),
                Size = new(SHUDConstants.SLOT_SIZE),
                Margin = margin,
            };

            SGUIImageElement iconElement = new(this.SGameInstance)
            {
                Texture = iconTexture,
                OriginPivot = SCardinalDirection.Center,
                Scale = new(1.5f),
                Size = new(SHUDConstants.SLOT_SIZE)
            };

            return new(backgroundElement, iconElement);
        }
    }
}
