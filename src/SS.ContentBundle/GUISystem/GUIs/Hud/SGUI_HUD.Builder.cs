using Microsoft.Xna.Framework;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.GUI.Common;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.GUISystem.Elements.Graphics;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Items;
using StardustSandbox.Core.Mathematics.Primitives;

using System;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud
{
    public partial class SGUI_HUD
    {
        // WeatherSettings
        // PenSettings
        // Screenshot
        // WorldSettings
        // PauseSimulation
        // GameMenu
        // Save
        // Eraser
        // Undo
        // ClearWorld

        private ISGUILayoutBuilder layout;

        private SGUIImageElement topToolbarContainer;
        private SGUIImageElement leftToolbarContainer;
        private SGUIImageElement rightToolbarContainer;

        private SGUIImageElement toolbarElementSearchButton;
        private readonly SToolbarSlot[] toolbarElementSlots = new SToolbarSlot[SHUDConstants.ELEMENT_BUTTONS_LENGTH];
        private readonly SToolbarSlot[] toolbarToolButtons = new SToolbarSlot[SHUDConstants.TOOL_BUTTONS_LENGTH];

        private readonly Color toolbarContainerColor = new(Color.White, 32);

        protected override void OnBuild(ISGUILayoutBuilder layout)
        {
            this.layout = layout;

            BuildGeneralButtons();
            BuildToolbars();
        }

        private void BuildGeneralButtons()
        {
            for (int i = 0; i < SHUDConstants.TOOL_BUTTONS_LENGTH; i++)
            {
                SGUIImageElement slotBackground = new(this.SGameInstance)
                {
                    Texture = this.squareShapeTexture,
                    OriginPivot = SCardinalDirection.Center,
                    Scale = new Vector2(SHUDConstants.SLOT_SCALE),
                    PositionAnchor = SCardinalDirection.West,
                    Size = new SSize2(SHUDConstants.SLOT_SIZE),
                };

                SGUIImageElement slotIcon = new(this.SGameInstance)
                {
                    OriginPivot = SCardinalDirection.Center,
                    Scale = new Vector2(1.5f),
                    Size = new SSize2(SHUDConstants.SLOT_SIZE)
                };

                // Update
                slotBackground.PositionRelativeToScreen();
                slotIcon.PositionRelativeToElement(slotBackground);

                // Save
                this.toolbarElementSlots[i] = new(slotBackground, slotIcon);

                this.layout.AddElement(slotBackground);
                this.layout.AddElement(slotIcon);
            }
        }
        private void BuildToolbars()
        {
            BuildTopToolbar();
            BuildLeftToolbar();
            BuildRightToolbar();
        }

        private void BuildTopToolbar()
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
            this.layout.AddElement(this.topToolbarContainer);

            CreateTopToolbarSlots();
            CreateSearchSlot();
        }

        private void BuildLeftToolbar()
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

            this.layout.AddElement(this.leftToolbarContainer);
        }

        private void BuildRightToolbar()
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

            this.layout.AddElement(this.rightToolbarContainer);

        }

        private void CreateTopToolbarSlots()
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

                this.layout.AddElement(slotBackground);
                this.layout.AddElement(slotIcon);
            }
        }

        private void CreateSearchSlot()
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

            this.layout.AddElement(slotSearchBackground);
            this.layout.AddElement(slotIcon);

            this.toolbarElementSearchButton = slotSearchBackground;
        }
    }
}
