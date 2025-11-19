using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.Indexers;
using StardustSandbox.Enums.InputSystem.GameInput;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.World;
using StardustSandbox.InputSystem.GameInput;
using StardustSandbox.LocalizationSystem;
using StardustSandbox.Managers;
using StardustSandbox.UISystem.Elements;
using StardustSandbox.UISystem.Elements.Graphics;
using StardustSandbox.UISystem.Elements.Textual;
using StardustSandbox.UISystem.Utilities;

namespace StardustSandbox.UISystem.UIs.HUD
{
    internal sealed class PenSettingsUI : UI
    {
        private int toolButtonSelectedIndex;
        private int layerButtonSelectedIndex;
        private int shapeButtonSelectedIndex;

        private ImageUIElement panelBackgroundElement;

        private LabelUIElement menuTitleElement;
        private LabelUIElement brushSectionTitleElement;
        private LabelUIElement toolsSectionTitleElement;
        private LabelUIElement layerSectionTitleElement;
        private LabelUIElement shapeSectionTitleElement;

        private ImageUIElement brushSizeSliderElement;

        private readonly TooltipBoxUIElement tooltipBoxElement;

        private readonly Rectangle[] brushSizeSliderClipTextures;

        private readonly UIButton[] menuButtons;
        private readonly UIButton[] toolButtons;
        private readonly UIButton[] layerButtons;
        private readonly UIButton[] shapeButtons;

        private readonly UISlot[] menuButtonSlots;
        private readonly UISlot[] toolButtonSlots;
        private readonly UISlot[] layerButtonSlots;
        private readonly UISlot[] shapeButtonSlots;

        private readonly Texture2D particleTexture;
        private readonly Texture2D panelBackgroundTexture;
        private readonly Texture2D guiSmallButtonTexture;
        private readonly Texture2D guiSliderTexture;
        private readonly Texture2D eraserIconTexture;
        private readonly Texture2D exitIconTexture;
        private readonly Texture2D penIconTexture;
        private readonly Texture2D bucketIconTexture;
        private readonly Texture2D replacementIconTexture;
        private readonly Texture2D frontLayerIconTexture;
        private readonly Texture2D backLayerIconTexture;
        private readonly Texture2D squareShapeIconTexture;
        private readonly Texture2D circleShapeIconTexture;
        private readonly Texture2D triangleShapeIconTexture;
        private readonly Texture2D eyeIconTexture;

        private readonly SpriteFont bigApple3PMSpriteFont;

        private readonly HudUI hudUI;

        private readonly InputController inputController;
        private readonly GameManager gameManager;
        private readonly UIManager uiManager;

        internal PenSettingsUI(
            GameManager gameManager,
            UIIndex index,
            InputController inputController,
            HudUI hudUI,
            TooltipBoxUIElement tooltipBoxElement,
            UIManager uiManager
        ) : base(index)
        {
            this.inputController = inputController;
            this.gameManager = gameManager;
            this.hudUI = hudUI;
            this.tooltipBoxElement = tooltipBoxElement;
            this.uiManager = uiManager;

            this.toolButtonSelectedIndex = 0;
            this.layerButtonSelectedIndex = 0;

            this.particleTexture = AssetDatabase.GetTexture("texture_particle_1");
            this.panelBackgroundTexture = AssetDatabase.GetTexture("texture_gui_background_8");
            this.guiSmallButtonTexture = AssetDatabase.GetTexture("texture_gui_button_1");
            this.guiSliderTexture = AssetDatabase.GetTexture("texture_gui_slider_1");
            this.bigApple3PMSpriteFont = AssetDatabase.GetSpriteFont("font_2");

            this.eraserIconTexture = AssetDatabase.GetTexture("texture_icon_gui_4");
            this.exitIconTexture = AssetDatabase.GetTexture("texture_icon_gui_16");
            this.penIconTexture = AssetDatabase.GetTexture("texture_icon_gui_19");
            this.bucketIconTexture = AssetDatabase.GetTexture("texture_icon_gui_20");
            this.replacementIconTexture = AssetDatabase.GetTexture("texture_icon_gui_21");
            this.frontLayerIconTexture = AssetDatabase.GetTexture("texture_icon_gui_22");
            this.backLayerIconTexture = AssetDatabase.GetTexture("texture_icon_gui_23");
            this.squareShapeIconTexture = AssetDatabase.GetTexture("texture_icon_gui_24");
            this.circleShapeIconTexture = AssetDatabase.GetTexture("texture_icon_gui_25");
            this.triangleShapeIconTexture = AssetDatabase.GetTexture("texture_icon_gui_26");
            this.eyeIconTexture = AssetDatabase.GetTexture("texture_icon_gui_27");

            this.menuButtons = [
                new(this.exitIconTexture, Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, ExitButtonAction),
            ];

            this.toolButtons = [
                new(this.eyeIconTexture, Localization_WorldGizmos.Visualization_Name, Localization_WorldGizmos.Visualization_Description, () =>
                {
                    SelectToolButtonAction(PenTool.Visualization);
                    this.hudUI.SetToolIcon(this.eyeIconTexture);
                }),

                new(this.penIconTexture, Localization_WorldGizmos.Pencil_Name, Localization_WorldGizmos.Pencil_Description, () =>
                {
                    SelectToolButtonAction(PenTool.Pencil);
                    this.hudUI.SetToolIcon(this.penIconTexture);
                }),
                new(this.eraserIconTexture, Localization_WorldGizmos.Eraser_Name, Localization_WorldGizmos.Eraser_Description, () =>
                {
                    SelectToolButtonAction(PenTool.Eraser);
                    this.hudUI.SetToolIcon(this.eraserIconTexture);
                }),
                new(this.bucketIconTexture, Localization_WorldGizmos.Fill_Name, Localization_WorldGizmos.Fill_Description, () =>
                {
                    SelectToolButtonAction(PenTool.Fill);
                    this.hudUI.SetToolIcon(this.bucketIconTexture);
                }),
                new(this.replacementIconTexture, Localization_WorldGizmos.Replace_Name, Localization_WorldGizmos.Replace_Description, () =>
                {
                    SelectToolButtonAction(PenTool.Replace);
                    this.hudUI.SetToolIcon(this.replacementIconTexture);
                }),
            ];

            this.layerButtons = [
                new(this.frontLayerIconTexture, Localization_GUIs.HUD_Complements_PenSettings_Section_Layer_Button_Front_Name, Localization_GUIs.HUD_Complements_PenSettings_Section_Layer_Button_Front_Description, () => SelectLayerButtonAction(LayerType.Foreground)),
                new(this.backLayerIconTexture, Localization_GUIs.HUD_Complements_PenSettings_Section_Layer_Button_Back_Name, Localization_GUIs.HUD_Complements_PenSettings_Section_Layer_Button_Back_Description, () => SelectLayerButtonAction(LayerType.Background)),
            ];

            this.shapeButtons = [
                new(this.circleShapeIconTexture, Localization_GUIs.HUD_Complements_PenSettings_Section_Shape_Button_Circle_Name, Localization_GUIs.HUD_Complements_PenSettings_Section_Shape_Button_Circle_Description, () => SelectShapeButtonAction(PenShape.Circle)),
                new(this.squareShapeIconTexture, Localization_GUIs.HUD_Complements_PenSettings_Section_Shape_Button_Square_Name, Localization_GUIs.HUD_Complements_PenSettings_Section_Shape_Button_Square_Description, () => SelectShapeButtonAction(PenShape.Square)),
                new(this.triangleShapeIconTexture, Localization_GUIs.HUD_Complements_PenSettings_Section_Shape_Button_Triangle_Name, Localization_GUIs.HUD_Complements_PenSettings_Section_Shape_Button_Triangle_Description, () => SelectShapeButtonAction(PenShape.Triangle)),
            ];

            this.menuButtonSlots = new UISlot[this.menuButtons.Length];
            this.toolButtonSlots = new UISlot[this.toolButtons.Length];
            this.layerButtonSlots = new UISlot[this.layerButtons.Length];
            this.shapeButtonSlots = new UISlot[this.shapeButtons.Length];

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

        protected override void OnBuild(Layout layout)
        {
            BuildBackground(layout);
            BuildTitle(layout);
            BuildMenuButtons(layout);
            BuildBrushSizeSection(layout);
            BuildToolSection(layout);
            BuildLayerSection(layout);
            BuildShapeSection(layout);

            layout.AddElement(this.tooltipBoxElement);
        }

        private void BuildBackground(Layout layout)
        {
            ImageUIElement backgroundShadowElement = new()
            {
                Texture = this.particleTexture,
                Scale = new(ScreenConstants.DEFAULT_SCREEN_WIDTH, ScreenConstants.DEFAULT_SCREEN_HEIGHT),
                Size = new(1),
                Color = new(AAP64ColorPalette.DarkGray, 160)
            };

            this.panelBackgroundElement = new()
            {
                Texture = this.panelBackgroundTexture,
                Size = new(1084, 540),
                Margin = new(98, 90),
            };

            this.panelBackgroundElement.PositionRelativeToScreen();

            layout.AddElement(backgroundShadowElement);
            layout.AddElement(this.panelBackgroundElement);
        }

        private void BuildTitle(Layout layout)
        {
            this.menuTitleElement = new()
            {
                SpriteFont = this.bigApple3PMSpriteFont,
                Scale = new(0.12f),
                PositionAnchor = CardinalDirection.Northwest,
                OriginPivot = CardinalDirection.East,
                Margin = new(32, 40),
                Color = AAP64ColorPalette.White,
            };

            this.menuTitleElement.SetTextualContent(Localization_GUIs.HUD_Complements_PenSettings_Title);
            this.menuTitleElement.SetAllBorders(true, AAP64ColorPalette.DarkGray, new(3f));
            this.menuTitleElement.PositionRelativeToElement(this.panelBackgroundElement);

            layout.AddElement(this.menuTitleElement);
        }

        private void BuildMenuButtons(Layout layout)
        {
            Vector2 margin = new(-32f, -40f);

            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                UIButton button = this.menuButtons[i];
                UISlot slot = CreateButtonSlot(margin, button.IconTexture);

                slot.BackgroundElement.PositionAnchor = CardinalDirection.Northeast;
                slot.BackgroundElement.OriginPivot = CardinalDirection.Center;

                // Update
                slot.BackgroundElement.PositionRelativeToElement(this.panelBackgroundElement);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);

                // Save
                this.menuButtonSlots[i] = slot;

                // Spacing
                margin.X -= UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2);

                layout.AddElement(slot.BackgroundElement);
                layout.AddElement(slot.IconElement);
            }
        }

        private void BuildBrushSizeSection(Layout layout)
        {
            this.brushSectionTitleElement = new()
            {
                Scale = new(0.1f),
                Margin = new(32, 112),
                Color = AAP64ColorPalette.White,
                SpriteFont = this.bigApple3PMSpriteFont,
            };

            this.brushSizeSliderElement = new()
            {
                Texture = this.guiSliderTexture,
                TextureClipArea = new(new(0, 0), new(326, 38)),
                Size = new(326, 38),
                Scale = new(2f),
                Margin = new(0, 48),
                PositionAnchor = CardinalDirection.South,
            };

            this.brushSectionTitleElement.SetTextualContent(Localization_GUIs.HUD_Complements_PenSettings_Section_BrushSize_Title);

            this.brushSectionTitleElement.PositionRelativeToElement(this.panelBackgroundElement);
            this.brushSizeSliderElement.PositionRelativeToElement(this.brushSectionTitleElement);

            layout.AddElement(this.brushSectionTitleElement);
            layout.AddElement(this.brushSizeSliderElement);
        }

        private void BuildToolSection(Layout layout)
        {
            this.toolsSectionTitleElement = new()
            {
                Scale = new(0.1f),
                Margin = new(0, 144),
                Color = AAP64ColorPalette.White,
                SpriteFont = this.bigApple3PMSpriteFont,
            };

            this.toolsSectionTitleElement.SetTextualContent(Localization_GUIs.HUD_Complements_PenSettings_Section_Tool_Title);
            this.toolsSectionTitleElement.PositionRelativeToElement(this.brushSectionTitleElement);

            layout.AddElement(this.toolsSectionTitleElement);

            // Buttons
            Vector2 margin = new(32, 80);

            for (int i = 0; i < this.toolButtons.Length; i++)
            {
                UIButton button = this.toolButtons[i];
                UISlot slot = CreateButtonSlot(margin, button.IconTexture);

                slot.BackgroundElement.PositionAnchor = CardinalDirection.South;
                slot.BackgroundElement.OriginPivot = CardinalDirection.Center;

                // Update
                slot.BackgroundElement.PositionRelativeToElement(this.toolsSectionTitleElement);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);

                // Save
                this.toolButtonSlots[i] = slot;

                // Spacing
                margin.X += UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2);

                layout.AddElement(slot.BackgroundElement);
                layout.AddElement(slot.IconElement);
            }
        }

        private void BuildLayerSection(Layout layout)
        {
            this.layerSectionTitleElement = new()
            {
                Scale = new(0.1f),
                Color = AAP64ColorPalette.White,
                SpriteFont = this.bigApple3PMSpriteFont,
                Margin = new(this.toolsSectionTitleElement.Size.X + (UIConstants.HUD_GRID_SIZE * UIConstants.HUD_SLOT_SCALE * this.toolButtonSlots.Length) + 96, 0f)
            };

            this.layerSectionTitleElement.SetTextualContent(Localization_GUIs.HUD_Complements_PenSettings_Section_Layer_Title);
            this.layerSectionTitleElement.PositionRelativeToElement(this.toolsSectionTitleElement);

            layout.AddElement(this.layerSectionTitleElement);

            // Buttons
            Vector2 margin = new(32, 80);

            for (int i = 0; i < this.layerButtons.Length; i++)
            {
                UIButton button = this.layerButtons[i];
                UISlot slot = CreateButtonSlot(margin, button.IconTexture);

                slot.BackgroundElement.PositionAnchor = CardinalDirection.South;
                slot.BackgroundElement.OriginPivot = CardinalDirection.Center;

                // Update
                slot.BackgroundElement.PositionRelativeToElement(this.layerSectionTitleElement);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);

                // Save
                this.layerButtonSlots[i] = slot;

                // Spacing
                margin.X += UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2);

                layout.AddElement(slot.BackgroundElement);
                layout.AddElement(slot.IconElement);
            }
        }

        private void BuildShapeSection(Layout layout)
        {
            this.shapeSectionTitleElement = new()
            {
                Scale = new(0.1f),
                Color = AAP64ColorPalette.White,
                SpriteFont = this.bigApple3PMSpriteFont,
                Margin = new(this.layerSectionTitleElement.Size.X + (UIConstants.HUD_GRID_SIZE * UIConstants.HUD_SLOT_SCALE * this.layerButtonSlots.Length) + 48, 0f)
            };

            this.shapeSectionTitleElement.SetTextualContent(Localization_GUIs.HUD_Complements_PenSettings_Section_Shape_Title);
            this.shapeSectionTitleElement.PositionRelativeToElement(this.layerSectionTitleElement);

            layout.AddElement(this.shapeSectionTitleElement);

            // Buttons
            Vector2 margin = new(32, 80);

            for (int i = 0; i < this.shapeButtons.Length; i++)
            {
                UIButton button = this.shapeButtons[i];
                UISlot slot = CreateButtonSlot(margin, button.IconTexture);

                slot.BackgroundElement.PositionAnchor = CardinalDirection.South;
                slot.BackgroundElement.OriginPivot = CardinalDirection.Center;

                // Update
                slot.BackgroundElement.PositionRelativeToElement(this.shapeSectionTitleElement);
                slot.IconElement.PositionRelativeToElement(slot.BackgroundElement);

                // Save
                this.shapeButtonSlots[i] = slot;

                // Spacing
                margin.X += UIConstants.HUD_SLOT_SPACING + (UIConstants.HUD_GRID_SIZE / 2);

                layout.AddElement(slot.BackgroundElement);
                layout.AddElement(slot.IconElement);
            }
        }

        // =============================================================== //

        private UISlot CreateButtonSlot(Vector2 margin, Texture2D iconTexture)
        {
            ImageUIElement backgroundElement = new()
            {
                Texture = this.guiSmallButtonTexture,
                Scale = new(UIConstants.HUD_SLOT_SCALE),
                Size = new(UIConstants.HUD_GRID_SIZE),
                Margin = margin,
            };

            ImageUIElement iconElement = new()
            {
                Texture = iconTexture,
                OriginPivot = CardinalDirection.Center,
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

            this.tooltipBoxElement.IsVisible = false;

            UpdateMenuButtons();
            UpdateBrushSizeSlider();
            UpdateToolBottons();
            UpdateLayerButtons();
            UpdateShapeButtons();
            SyncGUIElements();

            this.tooltipBoxElement.RefreshDisplay(TooltipContent.Title, TooltipContent.Description);
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                UISlot slot = this.menuButtonSlots[i];

                Vector2 position = slot.BackgroundElement.Position;
                Vector2 size = new(UIConstants.HUD_GRID_SIZE);

                if (UIInteraction.OnMouseClick(position, size))
                {
                    this.menuButtons[i].ClickAction?.Invoke();
                }

                if (UIInteraction.OnMouseOver(position, size))
                {
                    this.tooltipBoxElement.IsVisible = true;

                    TooltipContent.Title = this.menuButtons[i].Name;
                    TooltipContent.Description = this.menuButtons[i].Description;

                    slot.BackgroundElement.Color = AAP64ColorPalette.HoverColor;
                }
                else
                {
                    slot.BackgroundElement.Color = AAP64ColorPalette.White;
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

                if (UIInteraction.OnMouseOver(position, size))
                {
                    this.tooltipBoxElement.IsVisible = true;

                    TooltipContent.Title = Localization_GUIs.HUD_Complements_PenSettings_Section_BrushSize_Button_Slider_Name;
                    TooltipContent.Description = string.Format(Localization_GUIs.HUD_Complements_PenSettings_Section_BrushSize_Button_Slider_Description, i + 1);
                }

                if (UIInteraction.OnMouseDown(position, size))
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
                UISlot slot = this.toolButtonSlots[i];
                bool isOver = UIInteraction.OnMouseOver(slot.BackgroundElement.Position, new(UIConstants.HUD_GRID_SIZE));

                Vector2 position = slot.BackgroundElement.Position;
                Vector2 size = new(UIConstants.HUD_GRID_SIZE);

                if (UIInteraction.OnMouseClick(position, size))
                {
                    this.toolButtons[i].ClickAction?.Invoke();
                }

                if (isOver)
                {
                    this.tooltipBoxElement.IsVisible = true;

                    TooltipContent.Title = this.toolButtons[i].Name;
                    TooltipContent.Description = this.toolButtons[i].Description;
                }

                slot.BackgroundElement.Color = this.toolButtonSelectedIndex == i ? AAP64ColorPalette.SelectedColor : isOver ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        private void UpdateLayerButtons()
        {
            for (int i = 0; i < this.layerButtons.Length; i++)
            {
                UISlot slot = this.layerButtonSlots[i];
                bool isOver = UIInteraction.OnMouseOver(slot.BackgroundElement.Position, new(UIConstants.HUD_GRID_SIZE));

                if (UIInteraction.OnMouseClick(slot.BackgroundElement.Position, new(UIConstants.HUD_GRID_SIZE)))
                {
                    this.layerButtons[i].ClickAction?.Invoke();
                }

                if (isOver)
                {
                    this.tooltipBoxElement.IsVisible = true;

                    TooltipContent.Title = this.layerButtons[i].Name;
                    TooltipContent.Description = this.layerButtons[i].Description;
                }

                slot.BackgroundElement.Color = this.layerButtonSelectedIndex == i ? AAP64ColorPalette.SelectedColor : isOver ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        private void UpdateShapeButtons()
        {
            for (int i = 0; i < this.shapeButtons.Length; i++)
            {
                UISlot slot = this.shapeButtonSlots[i];
                bool isOver = UIInteraction.OnMouseOver(slot.BackgroundElement.Position, new(UIConstants.HUD_GRID_SIZE));

                if (UIInteraction.OnMouseClick(slot.BackgroundElement.Position, new(UIConstants.HUD_GRID_SIZE)))
                {
                    this.shapeButtons[i].ClickAction?.Invoke();
                }

                if (isOver)
                {
                    this.tooltipBoxElement.IsVisible = true;

                    TooltipContent.Title = this.shapeButtons[i].Name;
                    TooltipContent.Description = this.shapeButtons[i].Description;
                }

                slot.BackgroundElement.Color = this.shapeButtonSelectedIndex == i ? AAP64ColorPalette.SelectedColor : isOver ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        private void SyncGUIElements()
        {
            // Brush Size Slider
            this.brushSizeSliderElement.TextureClipArea = this.brushSizeSliderClipTextures[this.inputController.Pen.Size];

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
