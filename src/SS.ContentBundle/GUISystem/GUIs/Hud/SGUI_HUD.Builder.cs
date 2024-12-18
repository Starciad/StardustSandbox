using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Specials.Interactive;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.GUI.Common;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Items;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud
{
    internal partial class SGUI_HUD
    {
        private SGUIImageElement topToolbarContainer;
        private SGUIImageElement leftToolbarContainer;
        private SGUIImageElement rightToolbarContainer;

        private SGUIImageElement toolbarElementSearchButton;

        private readonly SToolbarSlot[] toolbarElementSlots = new SToolbarSlot[SHUDConstants.ELEMENT_BUTTONS_LENGTH];
        private readonly SToolbarSlot[] leftPanelTopButtonElements;
        private readonly SToolbarSlot[] leftPanelBottomButtonElements;
        private readonly SToolbarSlot[] rightPanelTopButtonElements;
        private readonly SToolbarSlot[] rightPanelBottomButtonElements;

        private readonly Color toolbarContainerColor = new(SColorPalette.White, 32);

        protected override void OnBuild(ISGUILayoutBuilder layoutBuilder)
        {
            BuildToolbars(layoutBuilder);

            layoutBuilder.AddElement(this.tooltipBoxElement);
        }

        private void BuildToolbars(ISGUILayoutBuilder layoutBuilder)
        {
            BuildTopToolbar(layoutBuilder);
            BuildLeftToolbar(layoutBuilder);
            BuildRightToolbar(layoutBuilder);
        }

        private void BuildTopToolbar(ISGUILayoutBuilder layoutBuilder)
        {
            this.topToolbarContainer = new(this.SGameInstance)
            {
                Texture = this.particleTexture,
                Scale = new Vector2(SScreenConstants.DEFAULT_SCREEN_WIDTH, 96),
                Size = SSize2.One,
                Color = this.toolbarContainerColor,
                PositionAnchor = SCardinalDirection.Northwest
            };

            this.topToolbarContainer.PositionRelativeToScreen();

            layoutBuilder.AddElement(this.topToolbarContainer);

            CreateTopToolbarSlots(layoutBuilder);
            CreateSearchSlot(layoutBuilder);
        }

        private void BuildLeftToolbar(ISGUILayoutBuilder layoutBuilder)
        {
            this.leftToolbarContainer = new(this.SGameInstance)
            {
                Texture = this.particleTexture,
                Scale = new Vector2(96, 608),
                Size = SSize2.One,
                Color = this.toolbarContainerColor,
                PositionAnchor = SCardinalDirection.Northwest,
                Margin = new(0, 112),
            };

            this.leftToolbarContainer.PositionRelativeToScreen();

            layoutBuilder.AddElement(this.leftToolbarContainer);

            #region BUTTONS
            Vector2 baseMargin = new(0, SHUDConstants.SLOT_SPACING);

            // Top
            for (int i = 0; i < this.leftPanelTopButtons.Length; i++)
            {
                SButton button = this.leftPanelTopButtons[i];

                (SGUIImageElement buttonBackgroundElement, SGUIImageElement buttonIconElement) = CreateButtonSlot(baseMargin, button.IconTexture);

                buttonBackgroundElement.PositionAnchor = SCardinalDirection.North;
                buttonBackgroundElement.OriginPivot = SCardinalDirection.Center;

                buttonBackgroundElement.PositionRelativeToElement(this.leftToolbarContainer);
                buttonIconElement.PositionRelativeToElement(buttonBackgroundElement);

                this.leftPanelTopButtonElements[i] = new(buttonBackgroundElement, buttonIconElement);

                baseMargin.Y += SHUDConstants.SLOT_SPACING + (SHUDConstants.SLOT_SIZE / 2);

                layoutBuilder.AddElement(buttonBackgroundElement);
                layoutBuilder.AddElement(buttonIconElement);
            }

            baseMargin = new(0, -SHUDConstants.SLOT_SPACING);

            // Bottom
            for (int i = 0; i < this.leftPanelBottomButtons.Length; i++)
            {
                SButton button = this.leftPanelBottomButtons[i];

                (SGUIImageElement buttonBackgroundElement, SGUIImageElement buttonIconElement) = CreateButtonSlot(baseMargin, button.IconTexture);

                buttonBackgroundElement.PositionAnchor = SCardinalDirection.South;
                buttonBackgroundElement.OriginPivot = SCardinalDirection.Center;

                buttonBackgroundElement.PositionRelativeToElement(this.leftToolbarContainer);
                buttonIconElement.PositionRelativeToElement(buttonBackgroundElement);

                this.leftPanelBottomButtonElements[i] = new(buttonBackgroundElement, buttonIconElement);

                baseMargin.Y -= SHUDConstants.SLOT_SPACING + (SHUDConstants.SLOT_SIZE / 2);

                layoutBuilder.AddElement(buttonBackgroundElement);
                layoutBuilder.AddElement(buttonIconElement);
            }
            #endregion
        }

        private void BuildRightToolbar(ISGUILayoutBuilder layoutBuilder)
        {
            this.rightToolbarContainer = new(this.SGameInstance)
            {
                Texture = this.particleTexture,
                Scale = new Vector2(96, 608),
                Size = SSize2.One,
                Color = this.toolbarContainerColor,
                PositionAnchor = SCardinalDirection.Northeast,
                OriginPivot = SCardinalDirection.Southwest,
                Margin = new(0, 112),
            };

            this.rightToolbarContainer.PositionRelativeToScreen();

            layoutBuilder.AddElement(this.rightToolbarContainer);

            #region BUTTONS
            Vector2 baseMargin = new(-96, SHUDConstants.SLOT_SPACING);

            // Top
            for (int i = 0; i < this.rightPanelTopButtons.Length; i++)
            {
                SButton button = this.rightPanelTopButtons[i];

                (SGUIImageElement buttonBackgroundElement, SGUIImageElement buttonIconElement) = CreateButtonSlot(baseMargin, button.IconTexture);

                buttonBackgroundElement.PositionAnchor = SCardinalDirection.North;
                buttonBackgroundElement.OriginPivot = SCardinalDirection.Center;

                buttonBackgroundElement.PositionRelativeToElement(this.rightToolbarContainer);
                buttonIconElement.PositionRelativeToElement(buttonBackgroundElement);

                this.rightPanelTopButtonElements[i] = new(buttonBackgroundElement, buttonIconElement);

                baseMargin.Y += SHUDConstants.SLOT_SPACING + (SHUDConstants.SLOT_SIZE / 2);

                layoutBuilder.AddElement(buttonBackgroundElement);
                layoutBuilder.AddElement(buttonIconElement);
            }

            baseMargin = new(-96, -SHUDConstants.SLOT_SPACING);

            // Bottom
            for (int i = 0; i < this.rightPanelBottomButtons.Length; i++)
            {
                SButton button = this.rightPanelBottomButtons[i];

                (SGUIImageElement buttonBackgroundElement, SGUIImageElement buttonIconElement) = CreateButtonSlot(baseMargin, button.IconTexture);

                buttonBackgroundElement.PositionAnchor = SCardinalDirection.South;
                buttonBackgroundElement.OriginPivot = SCardinalDirection.Center;

                buttonBackgroundElement.PositionRelativeToElement(this.rightToolbarContainer);
                buttonIconElement.PositionRelativeToElement(buttonBackgroundElement);

                this.rightPanelBottomButtonElements[i] = new(buttonBackgroundElement, buttonIconElement);

                baseMargin.Y -= SHUDConstants.SLOT_SPACING + (SHUDConstants.SLOT_SIZE / 2);

                layoutBuilder.AddElement(buttonBackgroundElement);
                layoutBuilder.AddElement(buttonIconElement);
            }
            #endregion
        }

        // ======================================================================= //

        private void CreateTopToolbarSlots(ISGUILayoutBuilder layoutBuilder)
        {
            Vector2 baseMargin = new(SHUDConstants.SLOT_SPACING, 0);

            for (int i = 0; i < SHUDConstants.ELEMENT_BUTTONS_LENGTH; i++)
            {
                SItem selectedItem = this.SGameInstance.ItemDatabase.Items[i];

                (SGUIImageElement buttonBackgroundElement, SGUIImageElement buttonIconElement) = CreateButtonSlot(baseMargin, selectedItem.IconTexture);

                buttonBackgroundElement.PositionAnchor = SCardinalDirection.West;
                buttonBackgroundElement.OriginPivot = SCardinalDirection.Center;

                if (!buttonBackgroundElement.ContainsData(SHUDConstants.DATA_FILED_ELEMENT_ID))
                {
                    buttonBackgroundElement.AddData(SHUDConstants.DATA_FILED_ELEMENT_ID, selectedItem.Identifier);
                }

                // Update
                buttonBackgroundElement.PositionRelativeToElement(this.topToolbarContainer);
                buttonIconElement.PositionRelativeToElement(buttonBackgroundElement);

                // Save
                this.toolbarElementSlots[i] = new(buttonBackgroundElement, buttonIconElement);

                // Spacing
                baseMargin.X += SHUDConstants.SLOT_SPACING + (SHUDConstants.SLOT_SIZE / 2);

                layoutBuilder.AddElement(buttonBackgroundElement);
                layoutBuilder.AddElement(buttonIconElement);
            }
        }

        private (SGUIImageElement buttonBackgroundElement, SGUIImageElement buttonIconElement) CreateButtonSlot(Vector2 margin, Texture2D iconTexture)
        {
            SGUIImageElement backgroundElement = new(this.SGameInstance)
            {
                Texture = this.guiButtonTexture,
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

            return (backgroundElement, iconElement);
        }

        private void CreateSearchSlot(ISGUILayoutBuilder layoutBuilder)
        {
            SGUIImageElement slotSearchBackground = new(this.SGameInstance)
            {
                Texture = this.guiButtonTexture,
                OriginPivot = SCardinalDirection.Center,
                Scale = new Vector2(SHUDConstants.SLOT_SCALE + 0.45f),
                PositionAnchor = SCardinalDirection.East,
                Size = new SSize2(SHUDConstants.SLOT_SIZE),
                Margin = new Vector2(SHUDConstants.SLOT_SIZE * 2 * -1, 0),
            };

            SGUIImageElement slotIcon = new(this.SGameInstance)
            {
                Texture = this.iconTextures[(byte)SIconIndex.MagnifyingGlass],
                OriginPivot = SCardinalDirection.Center,
                Scale = new Vector2(2f),
                Size = new SSize2(1),
            };

            slotSearchBackground.PositionRelativeToElement(this.topToolbarContainer);
            slotIcon.PositionRelativeToElement(slotSearchBackground);

            layoutBuilder.AddElement(slotSearchBackground);
            layoutBuilder.AddElement(slotIcon);

            this.toolbarElementSearchButton = slotSearchBackground;
        }
    }
}
