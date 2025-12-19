using Microsoft.Xna.Framework;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.Inputs.Game;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UI;
using StardustSandbox.Enums.World;
using StardustSandbox.InputSystem.Game;
using StardustSandbox.Localization;
using StardustSandbox.Managers;
using StardustSandbox.UI.Elements;
using StardustSandbox.UI.Information;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.UI.Common.HUD
{
    internal sealed class PenSettingsUI : UIBase
    {
        private int toolButtonSelectedIndex, layerButtonSelectedIndex, shapeButtonSelectedIndex;

        private Image background, brushSizeSlider;
        private Label menuTitle, brushSectionTitle, toolsSectionTitle, layerSectionTitle, shapeSectionTitle;

        private readonly TooltipBox tooltipBox;

        private readonly Rectangle[] brushSizeSliderSourceRectangles;

        private readonly ButtonInfo[] menuButtonInfos, toolButtonInfos, layerButtonInfos, layerVisibility, shapeButtonInfos;
        private readonly SlotInfo[] menuButtonSlotInfos, toolButtonSlotInfos, layerButtonSlotInfos, layerVisibilitySlotInfos, shapeButtonSlotInfos;

        private readonly HudUI hudUI;

        private readonly InputController inputController;
        private readonly GameManager gameManager;
        private readonly UIManager uiManager;
        private readonly World world;

        internal PenSettingsUI(
            GameManager gameManager,
            UIIndex index,
            InputController inputController,
            HudUI hudUI,
            TooltipBox tooltipBox,
            UIManager uiManager,
            World world
        ) : base(index)
        {
            this.inputController = inputController;
            this.gameManager = gameManager;
            this.hudUI = hudUI;
            this.tooltipBox = tooltipBox;
            this.uiManager = uiManager;
            this.world = world;

            this.toolButtonSelectedIndex = 0;
            this.layerButtonSelectedIndex = 0;

            this.menuButtonInfos = [
                new(TextureIndex.IconUI, new(224, 0, 32, 32), Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, this.uiManager.CloseGUI),
            ];

            this.toolButtonInfos = [
                new(TextureIndex.IconUI, new(96, 64, 32, 32), Localization_WorldGizmos.Visualization_Name, Localization_WorldGizmos.Visualization_Description, () =>
                {
                    this.inputController.Pen.Tool = PenTool.Visualization;
                    this.hudUI.SetToolIcon(new(96, 64, 32, 32));
                }),

                new(TextureIndex.IconUI, new(64, 32, 32, 32), Localization_WorldGizmos.Pencil_Name, Localization_WorldGizmos.Pencil_Description, () =>
                {
                    this.inputController.Pen.Tool = PenTool.Pencil;
                    this.hudUI.SetToolIcon(new(64, 32, 32, 32));
                }),
                new(TextureIndex.IconUI, new(96, 32, 32, 32), Localization_WorldGizmos.Eraser_Name, Localization_WorldGizmos.Eraser_Description, () =>
                {
                    this.inputController.Pen.Tool = PenTool.Eraser;
                    this.hudUI.SetToolIcon(new(96, 32, 32, 32));
                }),
                new(TextureIndex.IconUI, new(128, 32, 32, 32), Localization_WorldGizmos.Fill_Name, Localization_WorldGizmos.Fill_Description, () =>
                {
                    this.inputController.Pen.Tool = PenTool.Fill;
                    this.hudUI.SetToolIcon(new(128, 32, 32, 32));
                }),
                new(TextureIndex.IconUI, new(160, 32, 32, 32), Localization_WorldGizmos.Replace_Name, Localization_WorldGizmos.Replace_Description, () =>
                {
                    this.inputController.Pen.Tool = PenTool.Replace;
                    this.hudUI.SetToolIcon(new(160, 32, 32, 32));
                }),
            ];

            this.layerButtonInfos = [
                new(TextureIndex.IconUI, new(192, 32, 32, 32), Localization_GUIs.HUD_Complements_PenSettings_Section_Layer_Button_Front_Name, Localization_GUIs.HUD_Complements_PenSettings_Section_Layer_Button_Front_Description, () => this.inputController.Pen.Layer = Layer.Foreground),
                new(TextureIndex.IconUI, new(224, 32, 32, 32), Localization_GUIs.HUD_Complements_PenSettings_Section_Layer_Button_Back_Name, Localization_GUIs.HUD_Complements_PenSettings_Section_Layer_Button_Back_Description, () => this.inputController.Pen.Layer = Layer.Background),
            ];

            this.layerVisibility = [
                new(TextureIndex.IconUI, new(), "Show/Hide Foreground Layer", string.Empty, () => world.Rendering.DrawForegroundElements = !world.Rendering.DrawForegroundElements),
                new(TextureIndex.IconUI, new(), "Show/Hide Background Layer", string.Empty, () => world.Rendering.DrawBackgroundElements = !world.Rendering.DrawBackgroundElements),
            ];

            this.shapeButtonInfos = [
                new(TextureIndex.IconUI, new(32, 64, 32, 32), Localization_GUIs.HUD_Complements_PenSettings_Section_Shape_Button_Circle_Name, Localization_GUIs.HUD_Complements_PenSettings_Section_Shape_Button_Circle_Description, () => this.inputController.Pen.Shape = PenShape.Circle),
                new(TextureIndex.IconUI, new(00, 64, 32, 32), Localization_GUIs.HUD_Complements_PenSettings_Section_Shape_Button_Square_Name, Localization_GUIs.HUD_Complements_PenSettings_Section_Shape_Button_Square_Description, () => this.inputController.Pen.Shape = PenShape.Square),
                new(TextureIndex.IconUI, new(64, 64, 32, 32), Localization_GUIs.HUD_Complements_PenSettings_Section_Shape_Button_Triangle_Name, Localization_GUIs.HUD_Complements_PenSettings_Section_Shape_Button_Triangle_Description, () => this.inputController.Pen.Shape = PenShape.Triangle),
            ];

            this.menuButtonSlotInfos = new SlotInfo[this.menuButtonInfos.Length];
            this.toolButtonSlotInfos = new SlotInfo[this.toolButtonInfos.Length];
            this.layerButtonSlotInfos = new SlotInfo[this.layerButtonInfos.Length];
            this.layerVisibilitySlotInfos = new SlotInfo[this.layerVisibility.Length];
            this.shapeButtonSlotInfos = new SlotInfo[this.shapeButtonInfos.Length];

            this.brushSizeSliderSourceRectangles = [
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
            Image shadow = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Scale = new(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT),
                Color = new(AAP64ColorPalette.DarkGray, 160),
                Size = Vector2.One,
            };

            this.background = new()
            {
                Alignment = UIDirection.Center,
                Texture = AssetDatabase.GetTexture(TextureIndex.UIBackgroundPenSettings),
                Size = new(1084.0f, 540.0f),
            };

            root.AddChild(shadow);
            root.AddChild(this.background);
        }

        private void BuildTitle()
        {
            this.menuTitle = new()
            {
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.12f),
                Margin = new(24.0f, 10.0f),
                TextContent = Localization_GUIs.HUD_Complements_PenSettings_Title,

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 3.0f,
                BorderThickness = 3.0f,
            };

            this.background.AddChild(this.menuTitle);
        }

        private void BuildMenuButtons()
        {
            float marginX = -32.0f;

            for (int i = 0; i < this.menuButtonInfos.Length; i++)
            {
                ButtonInfo button = this.menuButtonInfos[i];
                SlotInfo slot = CreateButtonSlot(new(marginX, -72.0f), button);

                slot.Background.Alignment = UIDirection.Northeast;
                slot.Icon.Alignment = UIDirection.Center;

                // Update
                this.background.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                // Save
                this.menuButtonSlotInfos[i] = slot;

                // Spacing
                marginX -= 80.0f;
            }
        }

        private void BuildBrushSizeSection()
        {
            this.brushSectionTitle = new()
            {
                Scale = new(0.1f),
                Margin = new(32.0f, 128.0f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                TextContent = Localization_GUIs.HUD_Complements_PenSettings_Section_BrushSize_Title,
            };

            this.brushSizeSlider = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.UISizeSlider),
                SourceRectangle = new(new(0, 0), new(326, 38)),
                Size = new(326.0f, 38.0f),
                Scale = new(2.0f),
                Margin = new(0.0f, 48.0f),
                Alignment = UIDirection.Southwest,
            };

            this.background.AddChild(this.brushSectionTitle);
            this.brushSectionTitle.AddChild(this.brushSizeSlider);
        }

        private void BuildToolSection()
        {
            this.toolsSectionTitle = new()
            {
                Scale = new(0.1f),
                Margin = new(0.0f, 144.0f),
                Color = AAP64ColorPalette.White,
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                TextContent = Localization_GUIs.HUD_Complements_PenSettings_Section_Tool_Title
            };

            this.brushSectionTitle.AddChild(this.toolsSectionTitle);

            // Buttons
            float marginX = 0.0f;

            for (int i = 0; i < this.toolButtonInfos.Length; i++)
            {
                ButtonInfo button = this.toolButtonInfos[i];
                SlotInfo slot = CreateButtonSlot(new(marginX, 52.0f), button);

                slot.Background.Alignment = UIDirection.Southwest;
                slot.Icon.Alignment = UIDirection.Center;

                // Update
                this.toolsSectionTitle.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                // Save
                this.toolButtonSlotInfos[i] = slot;

                // Spacing
                marginX += 80.0f;
            }
        }

        private void BuildLayerSection()
        {
            this.layerSectionTitle = new()
            {
                Scale = new(0.1f),
                Color = AAP64ColorPalette.White,
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Margin = new(this.toolButtonSlotInfos[^1].Background.Position.X + 96.0f, 0.0f),
                TextContent = Localization_GUIs.HUD_Complements_PenSettings_Section_Layer_Title,
            };

            this.toolsSectionTitle.AddChild(this.layerSectionTitle);

            // Buttons
            float marginX = 0.0f;
            float marginY = 52.0f;

            // Layer Buttons
            for (int i = 0; i < this.layerButtonInfos.Length; i++)
            {
                ButtonInfo button = this.layerButtonInfos[i];
                SlotInfo slot = CreateButtonSlot(new(marginX, marginY), button);

                slot.Background.Alignment = UIDirection.Southwest;
                slot.Icon.Alignment = UIDirection.Center;

                // Update
                this.layerSectionTitle.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                // Save
                this.layerButtonSlotInfos[i] = slot;

                // Spacing
                marginX += 80.0f;
            }

            marginY += 80.0f;
            marginX = 0.0f;

            // Layer Visibility Buttons
            for (int i = 0; i < this.layerVisibility.Length; i++)
            {
                ButtonInfo button = this.layerVisibility[i];
                SlotInfo slot = CreateButtonSlot(new(marginX, marginY), button);

                slot.Background.Alignment = UIDirection.Southwest;
                slot.Icon.Alignment = UIDirection.Center;

                // Update
                this.layerSectionTitle.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                // Save
                this.layerVisibilitySlotInfos[i] = slot;

                // Spacing
                marginX += 80.0f;
            }
        }

        private void BuildShapeSection()
        {
            this.shapeSectionTitle = new()
            {
                Scale = new(0.1f),
                Color = AAP64ColorPalette.White,
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Margin = new(this.layerButtonSlotInfos[^1].Background.Position.X + 96.0f, 0.0f),
                TextContent = Localization_GUIs.HUD_Complements_PenSettings_Section_Shape_Title
            };

            this.layerSectionTitle.AddChild(this.shapeSectionTitle);

            // Buttons
            float marginX = 0.0f;

            for (int i = 0; i < this.shapeButtonInfos.Length; i++)
            {
                ButtonInfo button = this.shapeButtonInfos[i];
                SlotInfo slot = CreateButtonSlot(new(marginX, 52.0f), button);

                slot.Background.Alignment = UIDirection.Southwest;
                slot.Icon.Alignment = UIDirection.Center;

                // Update
                this.shapeSectionTitle.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                // Save
                this.shapeButtonSlotInfos[i] = slot;

                // Spacing
                marginX += 80.0f;
            }
        }

        // =============================================================== //

        private static SlotInfo CreateButtonSlot(Vector2 margin, ButtonInfo button)
        {
            Image background = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                SourceRectangle = new(320, 140, 32, 32),
                Scale = new(2.0f),
                Size = new(32.0f),
                Margin = margin,
            };

            Image icon = new()
            {
                Texture = button.Texture,
                SourceRectangle = button.TextureSourceRectangle,
                Scale = new(1.5f),
                Size = new(32.0f)
            };

            return new(background, icon);
        }

        #endregion

        #region UPDATING

        internal override void Update(GameTime gameTime)
        {
            this.tooltipBox.CanDraw = false;

            UpdateMenuButtons();
            UpdateBrushSizeSlider();
            UpdateToolBottons();
            UpdateLayerButtons();
            UpdateShapeButtons();
            SyncGUIElements();

            base.Update(gameTime);
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtonInfos.Length; i++)
            {
                SlotInfo slot = this.menuButtonSlotInfos[i];

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    this.menuButtonInfos[i].ClickAction?.Invoke();
                }

                if (Interaction.OnMouseOver(slot.Background))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(this.menuButtonInfos[i].Name);
                    TooltipBoxContent.SetDescription(this.menuButtonInfos[i].Description);

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
            for (int i = 0; i < this.brushSizeSliderSourceRectangles.Length; i++)
            {
                Vector2 position = new(
                    this.brushSizeSlider.Position.X + (i * 64.0f),
                    this.brushSizeSlider.Position.Y
                );

                if (Interaction.OnMouseOver(position, new(64.0f)))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(Localization_GUIs.HUD_Complements_PenSettings_Section_BrushSize_Button_Slider_Name);
                    TooltipBoxContent.SetDescription(string.Format(Localization_GUIs.HUD_Complements_PenSettings_Section_BrushSize_Button_Slider_Description, i + 1));
                }

                if (Interaction.OnMouseLeftDown(position, new(64.0f)))
                {
                    this.inputController.Pen.Size = (sbyte)i;
                    break;
                }
            }
        }

        private void UpdateToolBottons()
        {
            for (int i = 0; i < this.toolButtonInfos.Length; i++)
            {
                SlotInfo slot = this.toolButtonSlotInfos[i];

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    this.toolButtonInfos[i].ClickAction?.Invoke();
                }

                bool isOver = Interaction.OnMouseOver(slot.Background);

                if (isOver)
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(this.toolButtonInfos[i].Name);
                    TooltipBoxContent.SetDescription(this.toolButtonInfos[i].Description);
                }

                slot.Background.Color = this.toolButtonSelectedIndex == i ? AAP64ColorPalette.SelectedColor : isOver ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        private void UpdateLayerButtons()
        {
            // Update Layer Buttons
            for (int i = 0; i < this.layerButtonInfos.Length; i++)
            {
                SlotInfo slot = this.layerButtonSlotInfos[i];

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    this.layerButtonInfos[i].ClickAction?.Invoke();
                }

                bool isOver = Interaction.OnMouseOver(slot.Background);

                if (isOver)
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(this.layerButtonInfos[i].Name);
                    TooltipBoxContent.SetDescription(this.layerButtonInfos[i].Description);
                }

                slot.Background.Color = this.layerButtonSelectedIndex == i ? AAP64ColorPalette.SelectedColor : isOver ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }

            // Update Layer Visibility Buttons
            for (int i = 0; i < this.layerVisibility.Length; i++)
            {
                SlotInfo slot = this.layerVisibilitySlotInfos[i];

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    this.layerVisibility[i].ClickAction?.Invoke();
                }

                bool isOver = Interaction.OnMouseOver(slot.Background);

                if (isOver)
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(this.layerVisibility[i].Name);
                    TooltipBoxContent.SetDescription(this.layerVisibility[i].Description);
                }

                slot.Background.Color = isOver ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }

            // Update Layer Visibility Icons
            this.layerVisibilitySlotInfos[0].Icon.SourceRectangle = this.world.Rendering.DrawForegroundElements
                ? new(192, 32, 32, 32)
                : new(192, 192, 32, 32);

            this.layerVisibilitySlotInfos[1].Icon.SourceRectangle = this.world.Rendering.DrawBackgroundElements
                ? new(224, 32, 32, 32)
                : new(192, 192, 32, 32);
        }

        private void UpdateShapeButtons()
        {
            for (int i = 0; i < this.shapeButtonInfos.Length; i++)
            {
                SlotInfo slot = this.shapeButtonSlotInfos[i];

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    this.shapeButtonInfos[i].ClickAction?.Invoke();
                }

                bool isOver = Interaction.OnMouseOver(slot.Background);

                if (isOver)
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(this.shapeButtonInfos[i].Name);
                    TooltipBoxContent.SetDescription(this.shapeButtonInfos[i].Description);
                }

                slot.Background.Color = this.shapeButtonSelectedIndex == i ? AAP64ColorPalette.SelectedColor : isOver ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        private void SyncGUIElements()
        {
            // Brush Size Slider
            this.brushSizeSlider.SourceRectangle = this.brushSizeSliderSourceRectangles[this.inputController.Pen.Size];

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
