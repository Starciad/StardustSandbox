/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using Microsoft.Xna.Framework;

using StardustSandbox.Audio;
using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
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
        private SlotInfo[] menuButtonSlotInfos, toolButtonSlotInfos, layerButtonSlotInfos, layerVisibilitySlotInfos, shapeButtonSlotInfos;

        private readonly TooltipBox tooltipBox;

        private readonly Rectangle[] brushSizeSliderSourceRectangles;

        private readonly ButtonInfo[] menuButtonInfos, toolButtonInfos, layerButtonInfos, layerVisibility, shapeButtonInfos;

        private readonly HudUI hudUI;

        private readonly InputController inputController;
        private readonly World world;

        internal PenSettingsUI(
            InputController inputController,
            HudUI hudUI,
            TooltipBox tooltipBox,
            UIManager uiManager,
            World world
        ) : base()
        {
            this.inputController = inputController;
            this.hudUI = hudUI;
            this.tooltipBox = tooltipBox;
            this.world = world;

            this.toolButtonSelectedIndex = 0;
            this.layerButtonSelectedIndex = 0;

            this.menuButtonInfos = [
                new(TextureIndex.IconUI, new(224, 0, 32, 32), Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, uiManager.CloseUI),
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
                new(TextureIndex.IconUI, new(192, 32, 32, 32), Localization_GUIs.PenSettings_Layer_Front_Name, Localization_GUIs.PenSettings_Layer_Front_Description, () => this.inputController.Pen.Layer = Layer.Foreground),
                new(TextureIndex.IconUI, new(224, 32, 32, 32), Localization_GUIs.PenSettings_Layer_Back_Name, Localization_GUIs.PenSettings_Layer_Back_Description, () => this.inputController.Pen.Layer = Layer.Background),
            ];

            this.layerVisibility = [
                new(TextureIndex.IconUI, new(), Localization_GUIs.PenSettings_LayerVisibility_ShowOrHideForegroundLayer_Name, Localization_GUIs.PenSettings_LayerVisibility_ShowOrHideForegroundLayer_Description, () => world.Rendering.DrawForegroundElements = !world.Rendering.DrawForegroundElements),
                new(TextureIndex.IconUI, new(), Localization_GUIs.PenSettings_LayerVisibility_ShowOrHideBackgroundLayer_Name, Localization_GUIs.PenSettings_LayerVisibility_ShowOrHideBackgroundLayer_Description, () => world.Rendering.DrawBackgroundElements = !world.Rendering.DrawBackgroundElements),
            ];

            this.shapeButtonInfos = [
                new(TextureIndex.IconUI, new(32, 64, 32, 32), Localization_GUIs.PenSettings_Shape_Circle_Name, Localization_GUIs.PenSettings_Shape_Circle_Description, () => this.inputController.Pen.Shape = PenShape.Circle),
                new(TextureIndex.IconUI, new(00, 64, 32, 32), Localization_GUIs.PenSettings_Shape_Square_Name, Localization_GUIs.PenSettings_Shape_Square_Description, () => this.inputController.Pen.Shape = PenShape.Square),
                new(TextureIndex.IconUI, new(64, 64, 32, 32), Localization_GUIs.PenSettings_Shape_Triangle_Name, Localization_GUIs.PenSettings_Shape_Triangle_Description, () => this.inputController.Pen.Shape = PenShape.Triangle),
            ];

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
                TextureIndex = TextureIndex.Pixel,
                Scale = new(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT),
                Color = new(AAP64ColorPalette.DarkGray, 160),
                Size = Vector2.One,
            };

            this.background = new()
            {
                Alignment = UIDirection.Center,
                TextureIndex = TextureIndex.UIBackgroundPenSettings,
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
                TextContent = Localization_GUIs.PenSettings_Title,

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 3.0f,
                BorderThickness = 3.0f,
            };

            this.background.AddChild(this.menuTitle);
        }

        private void BuildMenuButtons()
        {
            this.menuButtonSlotInfos = UIBuilderUtility.BuildHorizontalButtonLine(
                this.background,
                this.menuButtonInfos,
                new(-32.0f, -72.0f),
                -80.0f,
                UIDirection.Northeast
            );
        }

        private void BuildBrushSizeSection()
        {
            this.brushSectionTitle = new()
            {
                Scale = new(0.1f),
                Margin = new(32.0f, 128.0f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                TextContent = Localization_GUIs.PenSettings_BrushSize_Title,
            };

            this.brushSizeSlider = new()
            {
                TextureIndex = TextureIndex.UISizeSlider,
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
                TextContent = Localization_GUIs.PenSettings_Tool_Title
            };

            this.brushSectionTitle.AddChild(this.toolsSectionTitle);

            this.toolButtonSlotInfos = UIBuilderUtility.BuildHorizontalButtonLine(
                this.toolsSectionTitle,
                this.toolButtonInfos,
                new(0.0f, 52.0f),
                80.0f,
                UIDirection.Southwest
            );
        }

        private void BuildLayerSection()
        {
            this.layerSectionTitle = new()
            {
                Scale = new(0.1f),
                Color = AAP64ColorPalette.White,
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Margin = new(this.toolsSectionTitle.GetLayoutBounds().Size.X + 32.0f, 0.0f),
                TextContent = Localization_GUIs.PenSettings_Layer_Title,
            };

            this.toolsSectionTitle.AddChild(this.layerSectionTitle);

            this.layerButtonSlotInfos = UIBuilderUtility.BuildHorizontalButtonLine(
                this.layerSectionTitle,
                this.layerButtonInfos,
                new(0.0f, 52.0f),
                80.0f,
                UIDirection.Southwest
            );

            this.layerVisibilitySlotInfos = UIBuilderUtility.BuildHorizontalButtonLine(
                this.layerSectionTitle,
                this.layerVisibility,
                new(0.0f, 132.0f),
                80.0f,
                UIDirection.Southwest
            );
        }

        private void BuildShapeSection()
        {
            this.shapeSectionTitle = new()
            {
                Scale = new(0.1f),
                Color = AAP64ColorPalette.White,
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Margin = new(this.layerSectionTitle.GetLayoutBounds().Size.X + 32.0f, 0.0f),
                TextContent = Localization_GUIs.PenSettings_Shape_Title
            };

            this.layerSectionTitle.AddChild(this.shapeSectionTitle);

            this.shapeButtonSlotInfos = UIBuilderUtility.BuildHorizontalButtonLine(
                this.shapeSectionTitle,
                this.shapeButtonInfos,
                new(0.0f, 52.0f),
                80.0f,
                UIDirection.Southwest
            );
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            this.tooltipBox.CanDraw = false;

            UpdateMenuButtons();
            UpdateBrushSizeSlider();
            UpdateToolBottons();
            UpdateLayerButtons();
            UpdateShapeButtons();
            SyncGUIElements();
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtonInfos.Length; i++)
            {
                SlotInfo slot = this.menuButtonSlotInfos[i];

                if (Interaction.OnMouseEnter(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Click);
                    this.menuButtonInfos[i].ClickAction?.Invoke();
                    break;
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
            if (Interaction.OnMouseEnter(this.brushSizeSlider))
            {
                SoundEngine.Play(SoundEffectIndex.GUI_Hover);
            }

            for (int i = 0; i < this.brushSizeSliderSourceRectangles.Length; i++)
            {
                Vector2 position = new(
                    this.brushSizeSlider.Position.X + (i * 64.0f),
                    this.brushSizeSlider.Position.Y
                );

                if (Interaction.OnMouseLeftDown(position, new(64.0f)))
                {
                    this.inputController.Pen.Size = (sbyte)i;
                    break;
                }

                if (Interaction.OnMouseOver(position, new(64.0f)))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(Localization_GUIs.PenSettings_BrushSize_Slider_Name);
                    TooltipBoxContent.SetDescription(string.Format(Localization_GUIs.PenSettings_BrushSize_Slider_Description, i + 1));
                }
            }
        }

        private void UpdateToolBottons()
        {
            for (int i = 0; i < this.toolButtonInfos.Length; i++)
            {
                SlotInfo slot = this.toolButtonSlotInfos[i];

                if (Interaction.OnMouseEnter(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Accepted);
                    this.toolButtonInfos[i].ClickAction?.Invoke();
                    break;
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

                if (Interaction.OnMouseEnter(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Accepted);
                    this.layerButtonInfos[i].ClickAction?.Invoke();
                    break;
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

                if (Interaction.OnMouseEnter(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Accepted);
                    this.layerVisibility[i].ClickAction?.Invoke();
                    break;
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

                if (Interaction.OnMouseEnter(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Accepted);
                    this.shapeButtonInfos[i].ClickAction?.Invoke();
                    break;
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

        protected override void OnOpened()
        {
            GameHandler.SetState(GameStates.IsCriticalMenuOpen);
        }

        protected override void OnClosed()
        {
            GameHandler.RemoveState(GameStates.IsCriticalMenuOpen);
        }
    }
}

