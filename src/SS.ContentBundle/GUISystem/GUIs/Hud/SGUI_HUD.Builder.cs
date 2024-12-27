using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Specials.General;
using StardustSandbox.ContentBundle.GUISystem.Specials.Interactive;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.GUI.Common;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Items;
using StardustSandbox.Core.Mathematics.Primitives;

using System.Linq;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud
{
    internal partial class SGUI_HUD
    {
        private SGUIImageElement topToolbarContainer;
        private SGUIImageElement leftToolbarContainer;
        private SGUIImageElement rightToolbarContainer;

        private SGUIImageElement toolbarElementSearchButton;
        private SGUIImageElement toolbarCurrentlySelectedToolIcon;

        private readonly SSlot[] toolbarElementSlots = new SSlot[SHUDConstants.ELEMENT_BUTTONS_LENGTH];
        private readonly SSlot[] leftPanelTopButtonElements;
        private readonly SSlot[] leftPanelBottomButtonElements;
        private readonly SSlot[] rightPanelTopButtonElements;
        private readonly SSlot[] rightPanelBottomButtonElements;

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
                Scale = new(SScreenConstants.DEFAULT_SCREEN_WIDTH, 96),
                Size = SSize2.One,
                Color = this.toolbarContainerColor,
                PositionAnchor = SCardinalDirection.Northwest
            };

            this.topToolbarContainer.PositionRelativeToScreen();

            layoutBuilder.AddElement(this.topToolbarContainer);

            CreateTopToolbatCurrentlySelectedToolSlot(layoutBuilder);
            CreateTopToolbarSlots(layoutBuilder);
            CreateTopToolbarSearchSlot(layoutBuilder);
        }

        private void BuildLeftToolbar(ISGUILayoutBuilder layoutBuilder)
        {
            this.leftToolbarContainer = new(this.SGameInstance)
            {
                Texture = this.particleTexture,
                Scale = new(96, 608),
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

                SSlot slot = CreateButtonSlot(baseMargin, button.IconTexture);

                slot.BackgroundElement.PositionAnchor = SCardinalDirection.North;
                slot.BackgroundElement.OriginPivot = SCardinalDirection.Center;

                slot.BackgroundElement.PositionRelativeToElement(this.leftToolbarContainer);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);

                this.leftPanelTopButtonElements[i] = slot;

                baseMargin.Y += SHUDConstants.SLOT_SPACING + (SHUDConstants.SLOT_SIZE / 2);

                layoutBuilder.AddElement(slot.BackgroundElement);
                layoutBuilder.AddElement(slot.IconElement);
            }

            baseMargin = new(0, -SHUDConstants.SLOT_SPACING);

            // Bottom
            for (int i = 0; i < this.leftPanelBottomButtons.Length; i++)
            {
                SButton button = this.leftPanelBottomButtons[i];

                SSlot slot = CreateButtonSlot(baseMargin, button.IconTexture);

                slot.BackgroundElement.PositionAnchor = SCardinalDirection.South;
                slot.BackgroundElement.OriginPivot = SCardinalDirection.Center;

                slot.BackgroundElement.PositionRelativeToElement(this.leftToolbarContainer);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);

                this.leftPanelBottomButtonElements[i] = slot;

                baseMargin.Y -= SHUDConstants.SLOT_SPACING + (SHUDConstants.SLOT_SIZE / 2);

                layoutBuilder.AddElement(slot.BackgroundElement);
                layoutBuilder.AddElement(slot.IconElement);
            }
            #endregion
        }

        private void BuildRightToolbar(ISGUILayoutBuilder layoutBuilder)
        {
            this.rightToolbarContainer = new(this.SGameInstance)
            {
                Texture = this.particleTexture,
                Scale = new(96, 608),
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

                SSlot slot = CreateButtonSlot(baseMargin, button.IconTexture);

                slot.BackgroundElement.PositionAnchor = SCardinalDirection.North;
                slot.BackgroundElement.OriginPivot = SCardinalDirection.Center;

                slot.BackgroundElement.PositionRelativeToElement(this.rightToolbarContainer);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);

                this.rightPanelTopButtonElements[i] = slot;

                baseMargin.Y += SHUDConstants.SLOT_SPACING + (SHUDConstants.SLOT_SIZE / 2);

                layoutBuilder.AddElement(slot.BackgroundElement);
                layoutBuilder.AddElement(slot.IconElement);
            }

            baseMargin = new(-96, -SHUDConstants.SLOT_SPACING);

            // Bottom
            for (int i = 0; i < this.rightPanelBottomButtons.Length; i++)
            {
                SButton button = this.rightPanelBottomButtons[i];

                SSlot slot = CreateButtonSlot(baseMargin, button.IconTexture);

                slot.BackgroundElement.PositionAnchor = SCardinalDirection.South;
                slot.BackgroundElement.OriginPivot = SCardinalDirection.Center;

                slot.BackgroundElement.PositionRelativeToElement(this.rightToolbarContainer);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);

                this.rightPanelBottomButtonElements[i] = slot;

                baseMargin.Y -= SHUDConstants.SLOT_SPACING + (SHUDConstants.SLOT_SIZE / 2);

                layoutBuilder.AddElement(slot.BackgroundElement);
                layoutBuilder.AddElement(slot.IconElement);
            }
            #endregion
        }

        // ======================================================================= //

        private void CreateTopToolbatCurrentlySelectedToolSlot(ISGUILayoutBuilder layoutBuilder)
        {
            SGUIImageElement slotSearchBackground = new(this.SGameInstance)
            {
                Texture = this.guiButtonTexture,
                OriginPivot = SCardinalDirection.Center,
                Scale = new(SHUDConstants.SLOT_SCALE + 0.45f),
                PositionAnchor = SCardinalDirection.West,
                Size = new(SHUDConstants.SLOT_SIZE),
                Margin = new(SHUDConstants.SLOT_SIZE * 2, 0),
            };

            SGUIImageElement slotIcon = new(this.SGameInstance)
            {
                Texture = this.iconTextures[00],
                OriginPivot = SCardinalDirection.Center,
                Scale = new(2f),
                Size = new(1),
            };

            slotSearchBackground.PositionRelativeToElement(this.topToolbarContainer);
            slotIcon.PositionRelativeToElement(slotSearchBackground);

            layoutBuilder.AddElement(slotSearchBackground);
            layoutBuilder.AddElement(slotIcon);

            this.toolbarCurrentlySelectedToolIcon = slotIcon;
        }

        private void CreateTopToolbarSlots(ISGUILayoutBuilder layoutBuilder)
        {
            Vector2 baseMargin = new(SHUDConstants.SLOT_SPACING * 2.5f, 0);

            for (int i = 0; i < SHUDConstants.ELEMENT_BUTTONS_LENGTH; i++)
            {
                SItem selectedItem = this.SGameInstance.ItemDatabase.Items.ElementAt(i);

                SSlot slot = CreateButtonSlot(baseMargin, selectedItem.IconTexture);

                slot.BackgroundElement.PositionAnchor = SCardinalDirection.West;
                slot.BackgroundElement.OriginPivot = SCardinalDirection.Center;

                if (!slot.BackgroundElement.ContainsData(SHUDConstants.DATA_FILED_ELEMENT_ID))
                {
                    slot.BackgroundElement.AddData(SHUDConstants.DATA_FILED_ELEMENT_ID, selectedItem.Identifier);
                }

                // Update
                slot.BackgroundElement.PositionRelativeToElement(this.topToolbarContainer);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);

                // Save
                this.toolbarElementSlots[i] = slot;

                // Spacing
                baseMargin.X += SHUDConstants.SLOT_SPACING + (SHUDConstants.SLOT_SIZE / 2);

                layoutBuilder.AddElement(slot.BackgroundElement);
                layoutBuilder.AddElement(slot.IconElement);
            }
        }

        private void CreateTopToolbarSearchSlot(ISGUILayoutBuilder layoutBuilder)
        {
            SGUIImageElement slotSearchBackground = new(this.SGameInstance)
            {
                Texture = this.guiButtonTexture,
                OriginPivot = SCardinalDirection.Center,
                Scale = new(SHUDConstants.SLOT_SCALE + 0.45f),
                PositionAnchor = SCardinalDirection.East,
                Size = new(SHUDConstants.SLOT_SIZE),
                Margin = new(SHUDConstants.SLOT_SIZE * 2 * -1, 0),
            };

            SGUIImageElement slotIcon = new(this.SGameInstance)
            {
                Texture = this.iconTextures[00],
                OriginPivot = SCardinalDirection.Center,
                Scale = new(2f),
                Size = new(1),
            };

            slotSearchBackground.PositionRelativeToElement(this.topToolbarContainer);
            slotIcon.PositionRelativeToElement(slotSearchBackground);

            layoutBuilder.AddElement(slotSearchBackground);
            layoutBuilder.AddElement(slotIcon);

            this.toolbarElementSearchButton = slotSearchBackground;
        }

        // ============================================================= //

        private SSlot CreateButtonSlot(Vector2 margin, Texture2D iconTexture)
        {
            SGUIImageElement backgroundElement = new(this.SGameInstance)
            {
                Texture = this.guiButtonTexture,
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
