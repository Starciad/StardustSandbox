using Microsoft.Xna.Framework;

using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
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

        private readonly Color toolbarContainerColor = new(Color.White, 32);

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
                PositionAnchor = SCardinalDirection.Southwest,
                OriginPivot = SCardinalDirection.Northeast
            };

            this.leftToolbarContainer.PositionRelativeToScreen();

            layoutBuilder.AddElement(this.leftToolbarContainer);
        }

        private void BuildRightToolbar(ISGUILayoutBuilder layoutBuilder)
        {
            this.rightToolbarContainer = new(this.SGameInstance)
            {
                Texture = this.particleTexture,
                Scale = new Vector2(96, 608),
                Size = SSize2.One,
                Color = this.toolbarContainerColor,
                PositionAnchor = SCardinalDirection.Southeast,
                OriginPivot = SCardinalDirection.Northwest
            };

            this.rightToolbarContainer.PositionRelativeToScreen();

            layoutBuilder.AddElement(this.rightToolbarContainer);
        }

        private void CreateTopToolbarSlots(ISGUILayoutBuilder layoutBuilder)
        {
            Vector2 slotMargin = new(SHUDConstants.SLOT_SPACING, 0);

            for (int i = 0; i < SHUDConstants.ELEMENT_BUTTONS_LENGTH; i++)
            {
                SItem selectedItem = this.SGameInstance.ItemDatabase.Items[i];

                SGUIImageElement slotBackground = new(this.SGameInstance)
                {
                    Texture = this.squareShapeTexture,
                    OriginPivot = SCardinalDirection.Center,
                    Scale = new Vector2(SHUDConstants.SLOT_SCALE),
                    PositionAnchor = SCardinalDirection.West,
                    Size = new SSize2(SHUDConstants.SLOT_SIZE),
                    Margin = slotMargin,
                };

                SGUIImageElement slotIcon = new(this.SGameInstance)
                {
                    Texture = selectedItem.IconTexture,
                    OriginPivot = SCardinalDirection.Center,
                    Scale = new Vector2(1.5f),
                    Size = new SSize2(SHUDConstants.SLOT_SIZE)
                };

                if (!slotBackground.ContainsData(SHUDConstants.DATA_FILED_ELEMENT_ID))
                {
                    slotBackground.AddData(SHUDConstants.DATA_FILED_ELEMENT_ID, selectedItem.Identifier);
                }

                // Update
                slotBackground.PositionRelativeToElement(this.topToolbarContainer);
                slotIcon.PositionRelativeToElement(slotBackground);

                // Save
                this.toolbarElementSlots[i] = new(slotBackground, slotIcon);

                // Spacing
                slotMargin.X += SHUDConstants.SLOT_SPACING + (SHUDConstants.SLOT_SIZE / 2);

                layoutBuilder.AddElement(slotBackground);
                layoutBuilder.AddElement(slotIcon);
            }
        }

        private void CreateSearchSlot(ISGUILayoutBuilder layoutBuilder)
        {
            SGUIImageElement slotSearchBackground = new(this.SGameInstance)
            {
                Texture = this.squareShapeTexture,
                OriginPivot = SCardinalDirection.Center,
                Scale = new Vector2(SHUDConstants.SLOT_SCALE + 0.45f),
                PositionAnchor = SCardinalDirection.East,
                Size = new SSize2(SHUDConstants.SLOT_SIZE),
                Margin = new Vector2(SHUDConstants.SLOT_SIZE * 2 * -1, 0),
            };

            SGUIImageElement slotIcon = new(this.SGameInstance)
            {
                Texture = this.magnifyingGlassIconTexture,
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
