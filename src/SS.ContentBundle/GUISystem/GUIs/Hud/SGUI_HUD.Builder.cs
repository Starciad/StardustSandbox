using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements;
using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Helpers.General;
using StardustSandbox.ContentBundle.GUISystem.Helpers.Interactive;
using StardustSandbox.Core.Catalog;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.GUISystem;
using StardustSandbox.Core.Constants.GUISystem.GUIs.Hud;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces.GUI;

using System.Linq;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud
{
    internal partial class SGUI_HUD
    {
        private SGUIContainerElement topToolbarContainerElement;
        private SGUIContainerElement leftToolbarContainerElement;
        private SGUIContainerElement rightToolbarContainerElement;

        private SGUIImageElement topToolbarBackgroundElement;
        private SGUIImageElement leftToolbarBackgroundElement;
        private SGUIImageElement rightToolbarBackgroundElement;

        private SGUIImageElement topDrawerButtonElement;
        private SGUIImageElement leftDrawerButtonElement;
        private SGUIImageElement rightDrawerButtonElement;

        private SGUIImageElement toolbarElementSearchButtonElement;
        private SGUIImageElement toolbarCurrentlySelectedToolIconElement;

        private readonly SSlot[] toolbarElementSlots = new SSlot[SGUI_HUDConstants.ELEMENT_BUTTONS_LENGTH];
        private readonly SSlot[] leftPanelTopButtonElements;
        private readonly SSlot[] leftPanelBottomButtonElements;
        private readonly SSlot[] rightPanelTopButtonElements;
        private readonly SSlot[] rightPanelBottomButtonElements;

        protected override void OnBuild(ISGUILayoutBuilder layoutBuilder)
        {
            BuildDrawerButtons(layoutBuilder);
            BuildToolbars(layoutBuilder);

            layoutBuilder.AddElement(this.tooltipBoxElement);
        }

        private void BuildToolbars(ISGUILayoutBuilder layoutBuilder)
        {
            this.topToolbarContainerElement = new(this.SGameInstance)
            {
                Size = new(SScreenConstants.DEFAULT_SCREEN_WIDTH, 96),
            };

            this.leftToolbarContainerElement = new(this.SGameInstance)
            {
                Size = new(96, 608),
                PositionAnchor = SCardinalDirection.Northwest,
                Margin = new(0, 112),
            };

            this.rightToolbarContainerElement = new(this.SGameInstance)
            {
                Size = new(96, 608),
                Margin = new(-16, 112),
                PositionAnchor = SCardinalDirection.Northeast,
            };

            this.topToolbarContainerElement.PositionRelativeToScreen();
            this.leftToolbarContainerElement.PositionRelativeToScreen();
            this.rightToolbarContainerElement.PositionRelativeToScreen();

            layoutBuilder.AddElement(this.topToolbarContainerElement);
            layoutBuilder.AddElement(this.leftToolbarContainerElement);
            layoutBuilder.AddElement(this.rightToolbarContainerElement);

            BuildTopToolbar(this.topToolbarContainerElement);
            BuildLeftToolbar(this.leftToolbarContainerElement);
            BuildRightToolbar(this.rightToolbarContainerElement);
        }

        #region Top Toolbar
        private void BuildTopToolbar(SGUIContainerElement containerElement)
        {
            this.topToolbarBackgroundElement = new(this.SGameInstance)
            {
                Texture = this.guiHorizontalBackgroundTexture,
                TextureClipArea = new(new(0, 0), new(SScreenConstants.DEFAULT_SCREEN_WIDTH, 96)),
                Size = new(SScreenConstants.DEFAULT_SCREEN_WIDTH, 96),
            };

            this.topToolbarBackgroundElement.PositionRelativeToScreen();

            containerElement.AddElement(this.topToolbarBackgroundElement);

            CreateTopToolbatCurrentlySelectedToolSlot(containerElement);
            CreateTopToolbarSlots(containerElement);
            CreateTopToolbarSearchSlot(containerElement);
        }

        private void CreateTopToolbatCurrentlySelectedToolSlot(SGUIContainerElement containerElement)
        {
            SGUIImageElement slotSearchBackground = new(this.SGameInstance)
            {
                Texture = this.guiButtonTexture,
                OriginPivot = SCardinalDirection.Center,
                Scale = new(SGUI_HUDConstants.SLOT_SCALE + 0.45f),
                PositionAnchor = SCardinalDirection.West,
                Size = new(SGUI_HUDConstants.GRID_SIZE),
                Margin = new(SGUI_HUDConstants.GRID_SIZE * 2, 0),
            };

            SGUIImageElement slotIcon = new(this.SGameInstance)
            {
                Texture = this.penIconTexture,
                OriginPivot = SCardinalDirection.Center,
                Scale = new(2f),
                Size = new(1),
            };

            slotSearchBackground.PositionRelativeToElement(this.topToolbarBackgroundElement);
            slotIcon.PositionRelativeToElement(slotSearchBackground);

            containerElement.AddElement(slotSearchBackground);
            containerElement.AddElement(slotIcon);

            this.toolbarCurrentlySelectedToolIconElement = slotIcon;
        }

        private void CreateTopToolbarSlots(SGUIContainerElement containerElement)
        {
            Vector2 margin = new(SGUI_HUDConstants.SLOT_SPACING * 2.5f, 0);

            for (int i = 0; i < SGUI_HUDConstants.ELEMENT_BUTTONS_LENGTH; i++)
            {
                SItem selectedItem = this.SGameInstance.CatalogDatabase.Items.ElementAt(i);

                SSlot slot = CreateButtonSlot(margin, selectedItem.IconTexture);

                slot.BackgroundElement.PositionAnchor = SCardinalDirection.West;
                slot.BackgroundElement.OriginPivot = SCardinalDirection.Center;

                if (!slot.BackgroundElement.ContainsData(SGUIConstants.DATA_ITEM))
                {
                    slot.BackgroundElement.AddData(SGUIConstants.DATA_ITEM, selectedItem);
                }

                // Update
                slot.BackgroundElement.PositionRelativeToElement(this.topToolbarBackgroundElement);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);

                // Save
                this.toolbarElementSlots[i] = slot;

                // Spacing
                margin.X += SGUI_HUDConstants.SLOT_SPACING + (SGUI_HUDConstants.GRID_SIZE / 2);

                containerElement.AddElement(slot.BackgroundElement);
                containerElement.AddElement(slot.IconElement);
            }
        }

        private void CreateTopToolbarSearchSlot(SGUIContainerElement containerElement)
        {
            SGUIImageElement slotSearchBackground = new(this.SGameInstance)
            {
                Texture = this.guiButtonTexture,
                OriginPivot = SCardinalDirection.Center,
                Scale = new(SGUI_HUDConstants.SLOT_SCALE + 0.45f),
                PositionAnchor = SCardinalDirection.East,
                Size = new(SGUI_HUDConstants.GRID_SIZE),
                Margin = new(SGUI_HUDConstants.GRID_SIZE * 2 * -1, 0),
            };

            SGUIImageElement slotIcon = new(this.SGameInstance)
            {
                Texture = this.magnifyingGlassIconTexture,
                OriginPivot = SCardinalDirection.Center,
                Scale = new(2f),
                Size = new(1),
            };

            slotSearchBackground.PositionRelativeToElement(this.topToolbarBackgroundElement);
            slotIcon.PositionRelativeToElement(slotSearchBackground);

            containerElement.AddElement(slotSearchBackground);
            containerElement.AddElement(slotIcon);

            this.toolbarElementSearchButtonElement = slotSearchBackground;
        }
        #endregion

        private void BuildLeftToolbar(SGUIContainerElement containerElement)
        {
            this.leftToolbarBackgroundElement = new(this.SGameInstance)
            {
                Texture = this.guiVerticalBackgroundTexture,
                TextureClipArea = new(new(0, 0), new(96, 608)),
                Size = new(96, 608),
                PositionAnchor = SCardinalDirection.Northwest,
                Margin = new(0, 112),
            };

            this.leftToolbarBackgroundElement.PositionRelativeToScreen();

            containerElement.AddElement(this.leftToolbarBackgroundElement);

            #region BUTTONS
            Vector2 margin = new(0, SGUI_HUDConstants.SLOT_SPACING);

            // Top
            for (int i = 0; i < this.leftPanelTopButtons.Length; i++)
            {
                SButton button = this.leftPanelTopButtons[i];

                SSlot slot = CreateButtonSlot(margin, button.IconTexture);

                slot.BackgroundElement.PositionAnchor = SCardinalDirection.North;
                slot.BackgroundElement.OriginPivot = SCardinalDirection.Center;

                slot.BackgroundElement.PositionRelativeToElement(this.leftToolbarBackgroundElement);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);

                this.leftPanelTopButtonElements[i] = slot;

                margin.Y += SGUI_HUDConstants.SLOT_SPACING + (SGUI_HUDConstants.GRID_SIZE / 2);

                containerElement.AddElement(slot.BackgroundElement);
                containerElement.AddElement(slot.IconElement);
            }

            margin = new(0, -SGUI_HUDConstants.SLOT_SPACING);

            // Bottom
            for (int i = 0; i < this.leftPanelBottomButtons.Length; i++)
            {
                SButton button = this.leftPanelBottomButtons[i];

                SSlot slot = CreateButtonSlot(margin, button.IconTexture);

                slot.BackgroundElement.PositionAnchor = SCardinalDirection.South;
                slot.BackgroundElement.OriginPivot = SCardinalDirection.Center;

                slot.BackgroundElement.PositionRelativeToElement(this.leftToolbarBackgroundElement);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);

                this.leftPanelBottomButtonElements[i] = slot;

                margin.Y -= SGUI_HUDConstants.SLOT_SPACING + (SGUI_HUDConstants.GRID_SIZE / 2);

                containerElement.AddElement(slot.BackgroundElement);
                containerElement.AddElement(slot.IconElement);
            }
            #endregion
        }

        private void BuildRightToolbar(SGUIContainerElement containerElement)
        {
            this.rightToolbarBackgroundElement = new(this.SGameInstance)
            {
                Texture = this.guiVerticalBackgroundTexture,
                TextureClipArea = new(new(96, 0), new(96, 608)),
                Size = new(96, 608),
                Margin = new(96, 112),
                PositionAnchor = SCardinalDirection.Northeast,
                OriginPivot = SCardinalDirection.Southwest,
            };

            this.rightToolbarBackgroundElement.PositionRelativeToScreen();

            containerElement.AddElement(this.rightToolbarBackgroundElement);

            #region BUTTONS
            Vector2 margin = new(-192, SGUI_HUDConstants.SLOT_SPACING);

            // Top
            for (int i = 0; i < this.rightPanelTopButtons.Length; i++)
            {
                SButton button = this.rightPanelTopButtons[i];

                SSlot slot = CreateButtonSlot(margin, button.IconTexture);

                slot.BackgroundElement.PositionAnchor = SCardinalDirection.North;
                slot.BackgroundElement.OriginPivot = SCardinalDirection.Center;

                slot.BackgroundElement.PositionRelativeToElement(this.rightToolbarBackgroundElement);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);

                this.rightPanelTopButtonElements[i] = slot;

                margin.Y += SGUI_HUDConstants.SLOT_SPACING + (SGUI_HUDConstants.GRID_SIZE / 2);

                containerElement.AddElement(slot.BackgroundElement);
                containerElement.AddElement(slot.IconElement);
            }

            margin = new(margin.X, -SGUI_HUDConstants.SLOT_SPACING);

            // Bottom
            for (int i = 0; i < this.rightPanelBottomButtons.Length; i++)
            {
                SButton button = this.rightPanelBottomButtons[i];

                SSlot slot = CreateButtonSlot(margin, button.IconTexture);

                slot.BackgroundElement.PositionAnchor = SCardinalDirection.South;
                slot.BackgroundElement.OriginPivot = SCardinalDirection.Center;

                slot.BackgroundElement.PositionRelativeToElement(this.rightToolbarBackgroundElement);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);

                this.rightPanelBottomButtonElements[i] = slot;

                margin.Y -= SGUI_HUDConstants.SLOT_SPACING + (SGUI_HUDConstants.GRID_SIZE / 2);

                containerElement.AddElement(slot.BackgroundElement);
                containerElement.AddElement(slot.IconElement);
            }
            #endregion
        }

        // ============================================================= //

        private void BuildDrawerButtons(ISGUILayoutBuilder layoutBuilder)
        {
            BuildTopDrawerButton(layoutBuilder);
            BuildLeftDrawerButton(layoutBuilder);
            BuildRightDrawerButton(layoutBuilder);
        }

        private void BuildTopDrawerButton(ISGUILayoutBuilder layoutBuilder)
        {
            this.topDrawerButtonElement = new(this.SGameInstance)
            {
                Texture = this.guiHorizontalDrawerButtonsTexture,
                TextureClipArea = new(new(0, 0), new(80, 24)),
                Size = new(80, 24),
                Scale = new(2f),
                PositionAnchor = SCardinalDirection.North,
                OriginPivot = SCardinalDirection.Center,
                Margin = new(0, 128),
            };

            this.topDrawerButtonElement.PositionRelativeToScreen();

            layoutBuilder.AddElement(this.topDrawerButtonElement);
        }

        private void BuildLeftDrawerButton(ISGUILayoutBuilder layoutBuilder)
        {
            this.leftDrawerButtonElement = new(this.SGameInstance)
            {
                Texture = this.guiVerticalDrawerButtonsTexture,
                TextureClipArea = new(new(0, 0), new(24, 80)),
                Size = new(24, 80),
                Scale = new(2f),
                PositionAnchor = SCardinalDirection.West,
                OriginPivot = SCardinalDirection.Center,
                Margin = new(128, 0),
            };

            this.leftDrawerButtonElement.PositionRelativeToScreen();

            layoutBuilder.AddElement(this.leftDrawerButtonElement);
        }

        private void BuildRightDrawerButton(ISGUILayoutBuilder layoutBuilder)
        {
            this.rightDrawerButtonElement = new(this.SGameInstance)
            {
                Texture = this.guiVerticalDrawerButtonsTexture,
                TextureClipArea = new(new(24, 0), new(24, 80)),
                Size = new(24, 80),
                Scale = new(2f),
                PositionAnchor = SCardinalDirection.East,
                OriginPivot = SCardinalDirection.Center,
                Margin = new(-80, 0),
            };

            this.rightDrawerButtonElement.PositionRelativeToScreen();

            layoutBuilder.AddElement(this.rightDrawerButtonElement);
        }

        // ============================================================= //

        private SSlot CreateButtonSlot(Vector2 margin, Texture2D iconTexture)
        {
            SGUIImageElement backgroundElement = new(this.SGameInstance)
            {
                Texture = this.guiButtonTexture,
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
