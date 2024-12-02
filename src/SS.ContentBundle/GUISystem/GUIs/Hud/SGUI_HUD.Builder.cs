using Microsoft.Xna.Framework;

using StardustSandbox.ContentBundle.GUISystem.Elements;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.GUI.Common;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.GUISystem.Elements.Graphics;
using StardustSandbox.Core.Interfaces.General;
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

        internal const int SLOT_SIZE = SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE;
        internal const int SLOT_SCALE = SHUDConstants.SLOT_SCALE;
        internal const int SLOT_SPACING = SLOT_SIZE * 2;

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
            this.topToolbarContainer = CreateToolbarContainer(
                new Vector2(SScreenConstants.DEFAULT_SCREEN_WIDTH, 96),
                SCardinalDirection.Center,
                Color.White * 32
            );

            this.layout.AddElement(this.topToolbarContainer);
            CreateTopToolbarSlots();
            CreateSearchSlot();
        }

        private void CreateTopToolbarSlots()
        {
            Vector2 slotMargin = new(SLOT_SPACING, 0);
            for (int i = 0; i < this.toolbarElementSlots.Length; i++)
            {
                SItem selectedItem = this.SGameInstance.ItemDatabase.Items[i];
                this.toolbarElementSlots[i] = new SToolbarSlot(
                    this.SGameInstance,
                    this.topToolbarContainer,
                    this.layout,
                    selectedItem,
                    slotMargin
                );

                slotMargin.X += SLOT_SPACING + (SLOT_SIZE / 2);
            }
        }

        private void CreateSearchSlot()
        {
            this.toolbarElementSearchButton = CreateSlot(this.SGameInstance, this.topToolbarContainer,
                SCardinalDirection.East, "icon_gui_1", new Vector2(2f),
                SHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_SIZE * -2);

            this.layout.AddElement(this.toolbarElementSearchButton);
        }

        private void BuildLeftToolbar()
        {
            this.leftToolbarContainer = CreateToolbarContainer(
                new Vector2(96, 608),
                SCardinalDirection.Southwest,
                Color.White * 32
            );

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
            this.rightToolbarContainer = CreateToolbarContainer(
                new Vector2(96, 608),
                SCardinalDirection.Southeast,
                Color.White * 32
            );

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

        // Helper method to create toolbar container
        private SGUIImageElement CreateToolbarContainer(Vector2 scale, SCardinalDirection anchor, Color color)
        {
            SGUIImageElement container = new(this.SGameInstance)
            {
                Texture = this.particleTexture,
                Scale = scale,
                Color = color,
                PositionAnchor = anchor
            };

            container.PositionRelativeToScreen();
            return container;
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

        private SGUIImageElement CreateSlot(ISGame gameInstance, SGUIImageElement parentContainer, SCardinalDirection anchor, string iconName, Vector2 iconScale, int marginX)
        {
            SGUIImageElement slotBackground = new(gameInstance)
            {
                Texture = this.squareShapeTexture,
                OriginPivot = SCardinalDirection.Center,
                Scale = new Vector2(SHUDConstants.SLOT_SCALE + 0.45f),
                PositionAnchor = anchor,
                Size = new SSize2(SLOT_SIZE),
                Margin = new Vector2(marginX, 0),
            };

            slotBackground.PositionRelativeToElement(parentContainer);

            return slotBackground;
        }
    }
}
