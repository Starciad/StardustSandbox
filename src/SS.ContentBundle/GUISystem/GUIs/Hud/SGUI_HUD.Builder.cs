using Microsoft.Xna.Framework;

using SharpDX.Direct2D1.Effects;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.GUI.Common;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.GUISystem.Elements.Graphics;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Items;
using StardustSandbox.Core.Mathematics.Primitives;

using System;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud
{
    public partial class SGUI_HUD
    {
        private ISGUILayoutBuilder layout;

        private SGUIImageElement topToolbarContainer;
        private SGUIImageElement leftToolbarContainer;
        private SGUIImageElement rightToolbarContainer;

        private SGUIImageElement toolbarElementSearchButton;
        private readonly SToolbarSlot[] toolbarElementSlots = new SToolbarSlot[SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_LENGTH];

        private SToolbarButton[] leftToolbarTopButtons;
        private SToolbarButton[] leftToolbarBottomButtons;
        private SToolbarButton[] rightToolbarTopButtons;
        private SToolbarButton[] rightToolbarBottomButtons;

        private const int SLOT_SIZE = SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE;
        private const int SLOT_SCALE = SHUDConstants.SLOT_SCALE;
        private const int SLOT_SPACING = SLOT_SIZE * 2;

        private readonly Color toolbarContainerColor = new(Color.White, 32);

        protected override void OnBuild(ISGUILayoutBuilder layout)
        {
            this.layout = layout;
            BuildToolbars();
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

            this.leftToolbarTopButtons = CreateToolbarButtons(new (string, Action)[] {
                ("WeatherSettings", WeatherSettingsButton),
                ("PenSettings", PenSettingsButton),
                ("Screenshot", ScreenshotButton),
                ("WorldSettings", WorldSettingsButton)
            }, SCardinalDirection.North, this.leftToolbarContainer);

            this.leftToolbarBottomButtons = CreateToolbarButtons(new (string, Action)[] {
                ("PauseSimulation", PauseSimulationButton)
            }, SCardinalDirection.South, this.leftToolbarContainer);
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

            this.rightToolbarTopButtons = CreateToolbarButtons(new (string, Action)[] {
                ("GameMenu", GameMenuButton),
                ("Save", SaveButton)
            }, SCardinalDirection.North, this.rightToolbarContainer);

            this.rightToolbarBottomButtons = CreateToolbarButtons(new (string, Action)[] {
                ("Eraser", EraserButton),
                ("Undo", UndoButton),
                ("ClearWorld", ClearWorld)
            }, SCardinalDirection.South, this.rightToolbarContainer);
        }


        private void CreateTopToolbarSlots()
        {
            int slotSize = SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE;
            int slotScale = SHUDConstants.SLOT_SCALE;
            int slotSpacing = slotSize * 2;

            Vector2 slotMargin = new(slotSpacing, 0);

            for (int i = 0; i < SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_LENGTH; i++)
            {
                SItem selectedItem = this.SGameInstance.ItemDatabase.Items[i];

                SGUIImageElement slotBackground = new(this.SGameInstance)
                {
                    Texture = this.squareShapeTexture,
                    OriginPivot = SCardinalDirection.Center,
                    Scale = new Vector2(slotScale),
                    PositionAnchor = SCardinalDirection.West,
                    Size = new SSize2(slotSize),
                    Margin = slotMargin,
                };

                SGUIImageElement slotIcon = new(this.SGameInstance)
                {
                    Texture = selectedItem.IconTexture,
                    OriginPivot = SCardinalDirection.Center,
                    Scale = new Vector2(1.5f),
                    Size = new SSize2(slotSize)
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
                slotMargin.X += slotSpacing + (slotSize / 2);

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
                Size = new SSize2(SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE),
                Margin = new Vector2(SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE * 2 * -1, 0),
            };

            SGUIImageElement slotIcon = new(this.SGameInstance)
            {
                Texture = this.SGameInstance.AssetDatabase.GetTexture("icon_gui_1"),
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

        private SToolbarButton[] CreateToolbarButtons((string name, Action action)[] buttons, SCardinalDirection anchor, SGUIImageElement container)
        {
            SToolbarButton[] toolbarButtons = new SToolbarButton[buttons.Length];

            for (int i = 0; i < buttons.Length; i++)
            {
                toolbarButtons[i] = new SToolbarButton(this.SGameInstance, this.layout, buttons[i].name, buttons[i].action, container, anchor, i);
            }

            return toolbarButtons;
        }
    }
}
