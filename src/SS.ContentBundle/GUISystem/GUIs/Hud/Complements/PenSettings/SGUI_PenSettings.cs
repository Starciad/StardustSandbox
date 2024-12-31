using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements.Informational;
using StardustSandbox.ContentBundle.GUISystem.Global;
using StardustSandbox.ContentBundle.GUISystem.Specials.General;
using StardustSandbox.ContentBundle.GUISystem.Specials.Interactive;
using StardustSandbox.ContentBundle.Localization.GUIs;
using StardustSandbox.ContentBundle.Localization.Tools;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.GUI.Common;
using StardustSandbox.Core.Controllers.GameInput;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements
{
    internal sealed partial class SGUI_PenSettings : SGUISystem
    {
        private int toolButtonSelectedIndex;
        private int layerButtonSelectedIndex;
        private int shapeButtonSelectedIndex;

        private readonly Texture2D particleTexture;
        private readonly Texture2D guiBackgroundTexture;
        private readonly Texture2D guiButton1Texture;
        private readonly Texture2D guiSliderTexture;
        private readonly Texture2D[] iconTextures;
        private readonly SpriteFont bigApple3PMSpriteFont;

        private readonly SButton[] menuButtons;
        private readonly SButton[] toolButtons;
        private readonly SButton[] layerButtons;
        private readonly SButton[] shapeButtons;

        private readonly Rectangle[] brushSizeSliderClipTextures;

        private readonly SGameInputController gameInputController;

        private readonly SGUITooltipBoxElement tooltipBoxElement;

        internal SGUI_PenSettings(ISGame gameInstance, string identifier, SGUIEvents guiEvents, SGUITooltipBoxElement tooltipBoxElement) : base(gameInstance, identifier, guiEvents)
        {
            this.toolButtonSelectedIndex = 0;
            this.layerButtonSelectedIndex = 0;

            this.particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
            this.guiBackgroundTexture = gameInstance.AssetDatabase.GetTexture("gui_background_1");
            this.guiButton1Texture = gameInstance.AssetDatabase.GetTexture("gui_button_1");
            this.guiSliderTexture = gameInstance.AssetDatabase.GetTexture("gui_slider_1");
            this.bigApple3PMSpriteFont = gameInstance.AssetDatabase.GetSpriteFont("font_2");

            this.iconTextures = [
                gameInstance.AssetDatabase.GetTexture("icon_gui_16"),
                gameInstance.AssetDatabase.GetTexture("icon_gui_19"),
                gameInstance.AssetDatabase.GetTexture("icon_gui_20"),
                gameInstance.AssetDatabase.GetTexture("icon_gui_21"),
                gameInstance.AssetDatabase.GetTexture("icon_gui_22"),
                gameInstance.AssetDatabase.GetTexture("icon_gui_23"),
                gameInstance.AssetDatabase.GetTexture("icon_gui_25"),
                gameInstance.AssetDatabase.GetTexture("icon_gui_24"),
                gameInstance.AssetDatabase.GetTexture("icon_gui_26"),
                gameInstance.AssetDatabase.GetTexture("icon_gui_27"),
                gameInstance.AssetDatabase.GetTexture("icon_gui_4"),
            ];

            this.menuButtons = [
                new(this.iconTextures[0], SLocalization_GUIs.Button_Exit_Name, SLocalization_GUIs.Button_Exit_Description, ExitButtonAction),
            ];

            this.toolButtons = [
                new(this.iconTextures[9], SLocalization_Tools.Visualization_Name, SLocalization_Tools.Visualization_Description, SelectVisualizationToolButtonAction),
                new(this.iconTextures[1], SLocalization_Tools.Pencil_Name, SLocalization_Tools.Pencil_Description, SelectPencilToolButtonAction),
                new(this.iconTextures[10], SLocalization_Tools.Eraser_Name, SLocalization_Tools.Eraser_Description, SelectEraserToolButtonAction),
                new(this.iconTextures[2], SLocalization_Tools.Fill_Name, SLocalization_Tools.Fill_Description, SelectFillToolButtonAction),
                new(this.iconTextures[3], SLocalization_Tools.Replace_Name, SLocalization_Tools.Replace_Description, SelectReplaceToolButtonAction),
            ];

            this.layerButtons = [
                new(this.iconTextures[4], SLocalization_GUIs.HUD_Complements_PenSettings_Section_Layer_Button_Front_Name, SLocalization_GUIs.HUD_Complements_PenSettings_Section_Layer_Button_Front_Description, SelectForegroundLayerButtonAction),
                new(this.iconTextures[5], SLocalization_GUIs.HUD_Complements_PenSettings_Section_Layer_Button_Back_Name, SLocalization_GUIs.HUD_Complements_PenSettings_Section_Layer_Button_Back_Description, SelectBackgroundLayerButtonAction),
            ];

            this.shapeButtons = [
                new(this.iconTextures[6], SLocalization_GUIs.HUD_Complements_PenSettings_Section_Shape_Button_Circle_Name, SLocalization_GUIs.HUD_Complements_PenSettings_Section_Shape_Button_Circle_Description, SelectCircleShapeButtonAction),
                new(this.iconTextures[7], SLocalization_GUIs.HUD_Complements_PenSettings_Section_Shape_Button_Square_Name, SLocalization_GUIs.HUD_Complements_PenSettings_Section_Shape_Button_Square_Description, SelectSquareShapeButtonAction),
                new(this.iconTextures[8], SLocalization_GUIs.HUD_Complements_PenSettings_Section_Shape_Button_Triangle_Name, SLocalization_GUIs.HUD_Complements_PenSettings_Section_Shape_Button_Triangle_Description, SelectTriangleShapeButtonAction),
            ];

            this.menuButtonSlots = new SSlot[this.menuButtons.Length];
            this.toolButtonSlots = new SSlot[this.toolButtons.Length];
            this.layerButtonSlots = new SSlot[this.layerButtons.Length];
            this.shapeButtonSlots = new SSlot[this.shapeButtons.Length];

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

            this.gameInputController = gameInstance.GameInputController;
            this.tooltipBoxElement = tooltipBoxElement;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.tooltipBoxElement.IsVisible = false;

            UpdateMenuButtons();
            UpdateBrushSizeSlider();
            UpdateToolBottons();
            UpdateLayerButtons();
            UpdateShapeButtons();
            SyncGUIElements();

            this.tooltipBoxElement.RefreshDisplay(SGUIGlobalTooltip.Title, SGUIGlobalTooltip.Description);
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                SSlot slot = this.menuButtonSlots[i];

                Vector2 position = slot.BackgroundElement.Position;
                SSize2 size = new(SHUDConstants.SLOT_SIZE);

                if (this.GUIEvents.OnMouseClick(position, size))
                {
                    this.menuButtons[i].ClickAction.Invoke();
                }

                if (this.GUIEvents.OnMouseOver(position, size))
                {
                    this.tooltipBoxElement.IsVisible = true;

                    SGUIGlobalTooltip.Title = this.menuButtons[i].DisplayName;
                    SGUIGlobalTooltip.Description = this.menuButtons[i].Description;

                    slot.BackgroundElement.Color = SColorPalette.HoverColor;
                }
                else
                {
                    slot.BackgroundElement.Color = SColorPalette.White;
                }
            }
        }

        private void UpdateBrushSizeSlider()
        {
            Vector2 basePosition = this.brushSizeSliderElement.Position;
            Vector2 offset = new(SHUDConstants.SLOT_SIZE);
            SSize2 size = new(SHUDConstants.SLOT_SIZE);

            for (int i = 0; i < this.brushSizeSliderClipTextures.Length; i++)
            {
                Vector2 position = basePosition + offset;

                if (this.GUIEvents.OnMouseOver(position, size))
                {
                    this.tooltipBoxElement.IsVisible = true;

                    SGUIGlobalTooltip.Title = SLocalization_GUIs.HUD_Complements_PenSettings_Section_BrushSize_Button_Slider_Name;
                    SGUIGlobalTooltip.Description = string.Format(SLocalization_GUIs.HUD_Complements_PenSettings_Section_BrushSize_Button_Slider_Description, i + 1);
                }

                if (this.GUIEvents.OnMouseDown(position, size))
                {
                    this.SGameInstance.GameInputController.Pen.Size = (sbyte)i;
                    break;
                }

                offset.X += SHUDConstants.SLOT_SPACING;
            }
        }

        private void UpdateToolBottons()
        {
            for (int i = 0; i < this.toolButtons.Length; i++)
            {
                SSlot slot = this.toolButtonSlots[i];
                bool isOver = this.GUIEvents.OnMouseOver(slot.BackgroundElement.Position, new(SHUDConstants.SLOT_SIZE));

                Vector2 position = slot.BackgroundElement.Position;
                SSize2 size = new(SHUDConstants.SLOT_SIZE);

                if (this.GUIEvents.OnMouseClick(position, size))
                {
                    this.toolButtons[i].ClickAction.Invoke();
                }

                if (isOver)
                {
                    this.tooltipBoxElement.IsVisible = true;

                    SGUIGlobalTooltip.Title = this.toolButtons[i].DisplayName;
                    SGUIGlobalTooltip.Description = this.toolButtons[i].Description;
                }

                if (this.toolButtonSelectedIndex == i)
                {
                    slot.BackgroundElement.Color = SColorPalette.SelectedColor;
                }
                else if (isOver)
                {
                    slot.BackgroundElement.Color = SColorPalette.HoverColor;
                }
                else
                {
                    slot.BackgroundElement.Color = SColorPalette.White;
                }
            }
        }

        private void UpdateLayerButtons()
        {
            for (int i = 0; i < this.layerButtons.Length; i++)
            {
                SSlot slot = this.layerButtonSlots[i];
                bool isOver = this.GUIEvents.OnMouseOver(slot.BackgroundElement.Position, new(SHUDConstants.SLOT_SIZE));

                if (this.GUIEvents.OnMouseClick(slot.BackgroundElement.Position, new(SHUDConstants.SLOT_SIZE)))
                {
                    this.layerButtons[i].ClickAction.Invoke();
                }

                if (isOver)
                {
                    this.tooltipBoxElement.IsVisible = true;

                    SGUIGlobalTooltip.Title = this.layerButtons[i].DisplayName;
                    SGUIGlobalTooltip.Description = this.layerButtons[i].Description;
                }

                if (this.layerButtonSelectedIndex == i)
                {
                    slot.BackgroundElement.Color = SColorPalette.SelectedColor;
                }
                else if (isOver)
                {
                    slot.BackgroundElement.Color = SColorPalette.HoverColor;
                }
                else
                {
                    slot.BackgroundElement.Color = SColorPalette.White;
                }
            }
        }

        private void UpdateShapeButtons()
        {
            for (int i = 0; i < this.shapeButtons.Length; i++)
            {
                SSlot slot = this.shapeButtonSlots[i];
                bool isOver = this.GUIEvents.OnMouseOver(slot.BackgroundElement.Position, new(SHUDConstants.SLOT_SIZE));

                if (this.GUIEvents.OnMouseClick(slot.BackgroundElement.Position, new(SHUDConstants.SLOT_SIZE)))
                {
                    this.shapeButtons[i].ClickAction.Invoke();
                }

                if (isOver)
                {
                    this.tooltipBoxElement.IsVisible = true;

                    SGUIGlobalTooltip.Title = this.shapeButtons[i].DisplayName;
                    SGUIGlobalTooltip.Description = this.shapeButtons[i].Description;
                }

                if (this.shapeButtonSelectedIndex == i)
                {
                    slot.BackgroundElement.Color = SColorPalette.SelectedColor;
                }
                else if (isOver)
                {
                    slot.BackgroundElement.Color = SColorPalette.HoverColor;
                }
                else
                {
                    slot.BackgroundElement.Color = SColorPalette.White;
                }
            }
        }

        private void SyncGUIElements()
        {
            // Brush Size Slider
            this.brushSizeSliderElement.TextureClipArea = this.brushSizeSliderClipTextures[this.gameInputController.Pen.Size];

            // Tool
            this.toolButtonSelectedIndex = (byte)this.gameInputController.Pen.Tool;

            // Layer
            this.layerButtonSelectedIndex = (byte)this.gameInputController.Pen.Layer;

            // Shape
            this.shapeButtonSelectedIndex = (byte)this.gameInputController.Pen.Shape;
        }
    }
}
