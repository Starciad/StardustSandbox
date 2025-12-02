using Microsoft.Xna.Framework;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.InputSystem.GameInput;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UI;
using StardustSandbox.Enums.World;
using StardustSandbox.InputSystem.GameInput;
using StardustSandbox.LocalizationSystem;
using StardustSandbox.Managers;
using StardustSandbox.UI.Elements;
using StardustSandbox.UI.Information;

namespace StardustSandbox.UI.Common.HUD
{
    internal sealed class PenSettingsUI : UIBase
    {
        private int toolButtonSelectedIndex;
        private int layerButtonSelectedIndex;
        private int shapeButtonSelectedIndex;

        private Image panelBackgroundElement;

        private Label menuTitleElement;
        private Label brushSectionTitleElement;
        private Label toolsSectionTitleElement;
        private Label layerSectionTitleElement;
        private Label shapeSectionTitleElement;

        private Image brushSizeSliderElement;

        private readonly TooltipBox tooltipBox;

        private readonly Rectangle[] brushSizeSliderClipTextures;

        private readonly ButtonInfo[] menuButtons;
        private readonly ButtonInfo[] toolButtons;
        private readonly ButtonInfo[] layerButtons;
        private readonly ButtonInfo[] shapeButtons;

        private readonly SlotInfo[] menuButtonSlots;
        private readonly SlotInfo[] toolButtonSlots;
        private readonly SlotInfo[] layerButtonSlots;
        private readonly SlotInfo[] shapeButtonSlots;

        private readonly HudUI hudUI;

        private readonly InputController inputController;
        private readonly GameManager gameManager;
        private readonly UIManager uiManager;

        internal PenSettingsUI(
            GameManager gameManager,
            UIIndex index,
            InputController inputController,
            HudUI hudUI,
            TooltipBox tooltipBox,
            UIManager uiManager
        ) : base(index)
        {
            this.inputController = inputController;
            this.gameManager = gameManager;
            this.hudUI = hudUI;
            this.tooltipBox = tooltipBox;
            this.uiManager = uiManager;

            this.toolButtonSelectedIndex = 0;
            this.layerButtonSelectedIndex = 0;

            this.menuButtons = [
                new(TextureIndex.UIButtons, new(224, 0, 32, 32), Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, ExitButtonAction),
            ];

            this.toolButtons = [
                new(TextureIndex.UIButtons, new(96, 64, 32, 32), Localization_WorldGizmos.Visualization_Name, Localization_WorldGizmos.Visualization_Description, () =>
                {
                    SelectToolButtonAction(PenTool.Visualization);
                    this.hudUI.SetToolIcon(new(96, 64, 32, 32));
                }),

                new(TextureIndex.UIButtons, new(64, 32, 32, 32), Localization_WorldGizmos.Pencil_Name, Localization_WorldGizmos.Pencil_Description, () =>
                {
                    SelectToolButtonAction(PenTool.Pencil);
                    this.hudUI.SetToolIcon(new(64, 32, 32, 32));
                }),
                new(TextureIndex.UIButtons, new(96, 32, 32, 32), Localization_WorldGizmos.Eraser_Name, Localization_WorldGizmos.Eraser_Description, () =>
                {
                    SelectToolButtonAction(PenTool.Eraser);
                    this.hudUI.SetToolIcon(new(96, 32, 32, 32));
                }),
                new(TextureIndex.UIButtons, new(128, 32, 32, 32), Localization_WorldGizmos.Fill_Name, Localization_WorldGizmos.Fill_Description, () =>
                {
                    SelectToolButtonAction(PenTool.Fill);
                    this.hudUI.SetToolIcon(new(128, 32, 32, 32));
                }),
                new(TextureIndex.UIButtons, new(160, 32, 32, 32), Localization_WorldGizmos.Replace_Name, Localization_WorldGizmos.Replace_Description, () =>
                {
                    SelectToolButtonAction(PenTool.Replace);
                    this.hudUI.SetToolIcon(new(160, 32, 32, 32));
                }),
            ];

            this.layerButtons = [
                new(TextureIndex.UIButtons, new(192, 32, 32, 32), Localization_GUIs.HUD_Complements_PenSettings_Section_Layer_Button_Front_Name, Localization_GUIs.HUD_Complements_PenSettings_Section_Layer_Button_Front_Description, () => SelectLayerButtonAction(LayerType.Foreground)),
                new(TextureIndex.UIButtons, new(224, 32, 32, 32), Localization_GUIs.HUD_Complements_PenSettings_Section_Layer_Button_Back_Name, Localization_GUIs.HUD_Complements_PenSettings_Section_Layer_Button_Back_Description, () => SelectLayerButtonAction(LayerType.Background)),
            ];

            this.shapeButtons = [
                new(TextureIndex.UIButtons, new(0, 64, 32, 32), Localization_GUIs.HUD_Complements_PenSettings_Section_Shape_Button_Circle_Name, Localization_GUIs.HUD_Complements_PenSettings_Section_Shape_Button_Circle_Description, () => SelectShapeButtonAction(PenShape.Circle)),
                new(TextureIndex.UIButtons, new(32, 64, 32, 32), Localization_GUIs.HUD_Complements_PenSettings_Section_Shape_Button_Square_Name, Localization_GUIs.HUD_Complements_PenSettings_Section_Shape_Button_Square_Description, () => SelectShapeButtonAction(PenShape.Square)),
                new(TextureIndex.UIButtons, new(64, 64, 32, 32), Localization_GUIs.HUD_Complements_PenSettings_Section_Shape_Button_Triangle_Name, Localization_GUIs.HUD_Complements_PenSettings_Section_Shape_Button_Triangle_Description, () => SelectShapeButtonAction(PenShape.Triangle)),
            ];

            this.menuButtonSlots = new SlotInfo[this.menuButtons.Length];
            this.toolButtonSlots = new SlotInfo[this.toolButtons.Length];
            this.layerButtonSlots = new SlotInfo[this.layerButtons.Length];
            this.shapeButtonSlots = new SlotInfo[this.shapeButtons.Length];

            this.brushSizeSliderClipTextures = [
                new(new(000, 000), new(326, 38)),
                new(new(000, 038), new(326, 38)),
                new(new(000, 076), new(326, 38)),
                new(new(000, 114), new(326, 38)),
                new(new(000, 152), new(326, 38)),
                new(new(000, 190), new(326, 38)),
                new(new(000, 228), new(326, 38)),
                new(new(000, 266), new(326, 38)),
                new(new(000, 304), new(326, 38)),
                new(new(000, 342), new(326, 38)),
            ];
        }

        #region ACTIONS

        // Menu
        private void ExitButtonAction()
        {
            this.uiManager.CloseGUI();
        }

        // Tools
        private void SelectToolButtonAction(PenTool tool)
        {
            this.inputController.Pen.Tool = tool;
        }

        // Layers
        private void SelectLayerButtonAction(LayerType layer)
        {
            this.inputController.Pen.Layer = layer;
        }

        // Shapes
        private void SelectShapeButtonAction(PenShape shape)
        {
            this.inputController.Pen.Shape = shape;
        }

        #endregion

        #region BUILDER

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildTitle();
            BuildMenuButtons();
            BuildBrushSizeSection();
            BuildToolSection();
            BuildLayerSection();
            BuildShapeSection();

            root.AddChild(this.tooltipBox);
        }

        private void BuildBackground(Container root)
        {
            Image backgroundShadowElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Scale = new(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT),
                Size = new(1),
                Color = new(AAP64ColorPalette.DarkGray, 160)
            };

            this.panelBackgroundElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.UIBackgroundPenSettings),
                Size = new(1084, 540),
                Margin = new(98, 90),
            };

            root.AddChild(backgroundShadowElement);
            root.AddChild(this.panelBackgroundElement);
        }

        private void BuildTitle()
        {
            this.menuTitleElement = new()
            {
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.12f),
                Alignment = CardinalDirection.Northwest,
                Margin = new(32, 40),
                Color = AAP64ColorPalette.White,
                TextContent = Localization_GUIs.HUD_Complements_PenSettings_Title,

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 3f,
                BorderThickness = 3f,
            };

            this.panelBackgroundElement.AddChild(this.menuTitleElement);
        }

        private void BuildMenuButtons()
        {
            Vector2 margin = new(-32f, -40f);

            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                ButtonInfo button = this.menuButtons[i];
                SlotInfo slot = CreateButtonSlot(margin, button);

                slot.Background.Alignment = CardinalDirection.Northeast;

                // Update
                this.panelBackgroundElement.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                // Save
                this.menuButtonSlots[i] = slot;

                // Spacing
                margin.X -= UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2);
            }
        }

        private void BuildBrushSizeSection()
        {
            this.brushSectionTitleElement = new()
            {
                Scale = new(0.1f),
                Margin = new(32, 112),
                Color = AAP64ColorPalette.White,
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                TextContent = Localization_GUIs.HUD_Complements_PenSettings_Section_BrushSize_Title,
            };

            this.brushSizeSliderElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.UISizeSlider),
                SourceRectangle = new(new(0, 0), new(326, 38)),
                Size = new(326, 38),
                Scale = new(2f),
                Margin = new(0, 48),
                Alignment = CardinalDirection.South,
            };

            this.panelBackgroundElement.AddChild(this.brushSectionTitleElement);
            this.brushSectionTitleElement.AddChild(this.brushSizeSliderElement);
        }

        private void BuildToolSection()
        {
            this.toolsSectionTitleElement = new()
            {
                Scale = new(0.1f),
                Margin = new(0, 144),
                Color = AAP64ColorPalette.White,
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                TextContent = Localization_GUIs.HUD_Complements_PenSettings_Section_Tool_Title
            };

            this.brushSectionTitleElement.AddChild(this.toolsSectionTitleElement);

            // Buttons
            Vector2 margin = new(32, 80);

            for (int i = 0; i < this.toolButtons.Length; i++)
            {
                ButtonInfo button = this.toolButtons[i];
                SlotInfo slot = CreateButtonSlot(margin, button);

                slot.Background.Alignment = CardinalDirection.South;

                // Update
                this.toolsSectionTitleElement.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                // Save
                this.toolButtonSlots[i] = slot;

                // Spacing
                margin.X += UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2);
            }
        }

        private void BuildLayerSection()
        {
            this.layerSectionTitleElement = new()
            {
                Scale = new(0.1f),
                Color = AAP64ColorPalette.White,
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Margin = new(this.toolsSectionTitleElement.Size.X + (UIConstants.HUD_GRID_SIZE * UIConstants.HUD_SLOT_SCALE * this.toolButtonSlots.Length) + 96, 0f),
                TextContent = Localization_GUIs.HUD_Complements_PenSettings_Section_Layer_Title,
            };

            this.toolsSectionTitleElement.AddChild(this.layerSectionTitleElement);

            // Buttons
            Vector2 margin = new(32, 80);

            for (int i = 0; i < this.layerButtons.Length; i++)
            {
                ButtonInfo button = this.layerButtons[i];
                SlotInfo slot = CreateButtonSlot(margin, button);

                slot.Background.Alignment = CardinalDirection.South;

                // Update
                this.layerSectionTitleElement.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                // Save
                this.layerButtonSlots[i] = slot;

                // Spacing
                margin.X += UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2);
            }
        }

        private void BuildShapeSection()
        {
            this.shapeSectionTitleElement = new()
            {
                Scale = new(0.1f),
                Color = AAP64ColorPalette.White,
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Margin = new(this.layerSectionTitleElement.Size.X + (UIConstants.HUD_GRID_SIZE * UIConstants.HUD_SLOT_SCALE * this.layerButtonSlots.Length) + 48, 0f),
                TextContent = Localization_GUIs.HUD_Complements_PenSettings_Section_Shape_Title
            };

            this.layerSectionTitleElement.AddChild(this.shapeSectionTitleElement);

            // Buttons
            Vector2 margin = new(32, 80);

            for (int i = 0; i < this.shapeButtons.Length; i++)
            {
                ButtonInfo button = this.shapeButtons[i];
                SlotInfo slot = CreateButtonSlot(margin, button);

                slot.Background.Alignment = CardinalDirection.South;

                // Update
                this.shapeSectionTitleElement.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                // Save
                this.shapeButtonSlots[i] = slot;

                // Spacing
                margin.X += UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2);
            }
        }

        // =============================================================== //

        private static SlotInfo CreateButtonSlot(Vector2 margin, ButtonInfo button)
        {
            Image backgroundElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                SourceRectangle = new(320, 140, 32, 32),
                Scale = new(UIConstants.HUD_SLOT_SCALE),
                Size = new(UIConstants.HUD_GRID_SIZE),
                Margin = margin,
            };

            Image iconElement = new()
            {
                Texture = button.IconTexture,
                SourceRectangle = button.IconTextureRectangle,
                Scale = new(1.5f),
                Size = new(UIConstants.HUD_GRID_SIZE)
            };

            return new(backgroundElement, iconElement);
        }

        #endregion

        #region UPDATING

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.tooltipBox.CanDraw = false;

            UpdateMenuButtons();
            UpdateBrushSizeSlider();
            UpdateToolBottons();
            UpdateLayerButtons();
            UpdateShapeButtons();
            SyncGUIElements();

            this.tooltipBox.RefreshDisplay();
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                SlotInfo slot = this.menuButtonSlots[i];

                Vector2 position = slot.Background.Position;
                Vector2 size = new(UIConstants.HUD_GRID_SIZE);

                if (Interaction.OnMouseLeftClick(position, size))
                {
                    this.menuButtons[i].ClickAction?.Invoke();
                }

                if (Interaction.OnMouseLeftOver(position, size))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(this.menuButtons[i].Name);
                    TooltipBoxContent.SetDescription(this.menuButtons[i].Description);

                    slot.Background.Color = AAP64ColorPalette.HoverColor;
                }
                else
                {
                    slot.Background.Color = AAP64ColorPalette.White;
                }
            }
        }

        private void UpdateBrushSizeSlider()
        {
            Vector2 basePosition = this.brushSizeSliderElement.Position;
            Vector2 offset = new(UIConstants.HUD_GRID_SIZE);
            Vector2 size = new(UIConstants.HUD_GRID_SIZE);

            for (int i = 0; i < this.brushSizeSliderClipTextures.Length; i++)
            {
                Vector2 position = basePosition + offset;

                if (Interaction.OnMouseLeftOver(position, size))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(Localization_GUIs.HUD_Complements_PenSettings_Section_BrushSize_Button_Slider_Name);
                    TooltipBoxContent.SetDescription(string.Format(Localization_GUIs.HUD_Complements_PenSettings_Section_BrushSize_Button_Slider_Description, i + 1));
                }

                if (Interaction.OnMouseLeftDown(position, size))
                {
                    this.inputController.Pen.Size = (sbyte)i;
                    break;
                }

                offset.X += UIConstants.HUD_SLOT_SPACING;
            }
        }

        private void UpdateToolBottons()
        {
            for (int i = 0; i < this.toolButtons.Length; i++)
            {
                SlotInfo slot = this.toolButtonSlots[i];
                bool isOver = Interaction.OnMouseLeftOver(slot.Background.Position, new(UIConstants.HUD_GRID_SIZE));

                Vector2 position = slot.Background.Position;
                Vector2 size = new(UIConstants.HUD_GRID_SIZE);

                if (Interaction.OnMouseLeftClick(position, size))
                {
                    this.toolButtons[i].ClickAction?.Invoke();
                }

                if (isOver)
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(this.toolButtons[i].Name);
                    TooltipBoxContent.SetDescription(this.toolButtons[i].Description);
                }

                slot.Background.Color = this.toolButtonSelectedIndex == i ? AAP64ColorPalette.SelectedColor : isOver ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        private void UpdateLayerButtons()
        {
            for (int i = 0; i < this.layerButtons.Length; i++)
            {
                SlotInfo slot = this.layerButtonSlots[i];
                bool isOver = Interaction.OnMouseLeftOver(slot.Background.Position, new(UIConstants.HUD_GRID_SIZE));

                if (Interaction.OnMouseLeftClick(slot.Background.Position, new(UIConstants.HUD_GRID_SIZE)))
                {
                    this.layerButtons[i].ClickAction?.Invoke();
                }

                if (isOver)
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(this.layerButtons[i].Name);
                    TooltipBoxContent.SetDescription(this.layerButtons[i].Description);
                }

                slot.Background.Color = this.layerButtonSelectedIndex == i ? AAP64ColorPalette.SelectedColor : isOver ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        private void UpdateShapeButtons()
        {
            for (int i = 0; i < this.shapeButtons.Length; i++)
            {
                SlotInfo slot = this.shapeButtonSlots[i];
                bool isOver = Interaction.OnMouseLeftOver(slot.Background.Position, new(UIConstants.HUD_GRID_SIZE));

                if (Interaction.OnMouseLeftClick(slot.Background.Position, new(UIConstants.HUD_GRID_SIZE)))
                {
                    this.shapeButtons[i].ClickAction?.Invoke();
                }

                if (isOver)
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(this.shapeButtons[i].Name);
                    TooltipBoxContent.SetDescription(this.shapeButtons[i].Description);
                }

                slot.Background.Color = this.shapeButtonSelectedIndex == i ? AAP64ColorPalette.SelectedColor : isOver ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        private void SyncGUIElements()
        {
            // Brush Size Slider
            this.brushSizeSliderElement.SourceRectangle = this.brushSizeSliderClipTextures[this.inputController.Pen.Size];

            // Tool
            this.toolButtonSelectedIndex = (byte)this.inputController.Pen.Tool;

            // Layer
            this.layerButtonSelectedIndex = (byte)this.inputController.Pen.Layer;

            // Shape
            this.shapeButtonSelectedIndex = (byte)this.inputController.Pen.Shape;
        }

        #endregion

        #region EVENTS

        protected override void OnOpened()
        {
            this.gameManager.SetState(GameStates.IsCriticalMenuOpen);
        }

        protected override void OnClosed()
        {
            this.gameManager.RemoveState(GameStates.IsCriticalMenuOpen);
        }

        #endregion
    }
}
