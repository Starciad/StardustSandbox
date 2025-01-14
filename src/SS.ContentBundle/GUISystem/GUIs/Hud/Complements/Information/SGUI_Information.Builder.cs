using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.ContentBundle.GUISystem.Helpers.General;
using StardustSandbox.ContentBundle.GUISystem.Helpers.Interactive;
using StardustSandbox.ContentBundle.Localization.GUIs;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.GUISystem.GUIs.Hud;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces.GUI;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements.Information
{
    internal sealed partial class SGUI_Information
    {
        private SGUIImageElement panelBackgroundElement;

        private SGUILabelElement menuTitleElement;

        private readonly SGUILabelElement[] infoElements;
        private readonly SSlot[] menuButtonSlots;

        protected override void OnBuild(ISGUILayoutBuilder layoutBuilder)
        {
            BuildBackground(layoutBuilder);
            BuildTitle(layoutBuilder);
            BuildMenuButtons(layoutBuilder);
            BuildInfoFields(layoutBuilder);
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

            this.menuTitleElement.SetTextualContent(SLocalization_GUIs.HUD_Complements_Information_Title);
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
                margin.X -= SGUI_HUDConstants.SLOT_SPACING + (SGUI_HUDConstants.GRID_SIZE / 2);

                layoutBuilder.AddElement(slot.BackgroundElement);
                layoutBuilder.AddElement(slot.IconElement);
            }
        }

        private void BuildInfoFields(ISGUILayoutBuilder layoutBuilder)
        {
            Vector2 margin = new(32, 144);

            for (int i = 0; i < this.infoElements.Length; i++)
            {
                SGUILabelElement labelElement = new(this.SGameInstance)
                {
                    SpriteFont = this.bigApple3PMSpriteFont,
                    Scale = new(0.1f),
                    PositionAnchor = SCardinalDirection.Northwest,
                    OriginPivot = SCardinalDirection.East,
                    Margin = margin,
                    Color = SColorPalette.White,
                };

                labelElement.SetTextualContent(string.Concat("Info ", i));
                labelElement.PositionRelativeToElement(this.panelBackgroundElement);

                // Save
                this.infoElements[i] = labelElement;

                // Spacing
                margin.Y += labelElement.GetStringSize().Height + 8;

                layoutBuilder.AddElement(labelElement);
            }
        }

        // =============================================================== //

        private SSlot CreateButtonSlot(Vector2 margin, Texture2D iconTexture)
        {
            SGUIImageElement backgroundElement = new(this.SGameInstance)
            {
                Texture = this.guiSmallButtonTexture,
                Scale = new(SGUI_HUDConstants.SLOT_SCALE),
                Size = new(SGUI_HUDConstants.GRID_SIZE),
                Margin = margin,
            };

            SGUIImageElement iconElement = new(this.SGameInstance)
            {
                Texture = iconTexture,
                OriginPivot = SCardinalDirection.Center,
                Scale = new(1.5f),
                Size = new(SGUI_HUDConstants.GRID_SIZE)
            };

            return new(backgroundElement, iconElement);
        }
    }
}
