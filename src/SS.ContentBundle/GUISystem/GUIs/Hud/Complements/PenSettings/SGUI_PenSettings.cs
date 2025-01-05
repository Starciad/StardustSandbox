using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements.Informational;
using StardustSandbox.ContentBundle.GUISystem.Global;
using StardustSandbox.ContentBundle.GUISystem.Specials.General;
using StardustSandbox.ContentBundle.GUISystem.Specials.Interactive;
using StardustSandbox.ContentBundle.Localization.GUIs;
using StardustSandbox.ContentBundle.Localization.Statements;
using StardustSandbox.ContentBundle.Localization.Tools;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.GUISystem.GUIs.Hud;
using StardustSandbox.Core.Enums.GameInput.Pen;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Controllers.GameInput;
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

        private readonly SButton[] menuButtons;
        private readonly SButton[] toolButtons;
        private readonly SButton[] layerButtons;
        private readonly SButton[] shapeButtons;

        private readonly Rectangle[] brushSizeSliderClipTextures;

        private readonly ISGameInputController gameInputController;

        private readonly SGUI_HUD guiHUD;
        private readonly SGUITooltipBoxElement tooltipBoxElement;

        internal SGUI_PenSettings(ISGame gameInstance, string identifier, SGUIEvents guiEvents, SGUI_HUD guiHUD, SGUITooltipBoxElement tooltipBoxElement) : base(gameInstance, identifier, guiEvents)
        {
            this.toolButtonSelectedIndex = 0;
            this.layerButtonSelectedIndex = 0;

            this.particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
            this.guiBackgroundTexture = gameInstance.AssetDatabase.GetTexture("gui_background_1");
            this.guiButton1Texture = gameInstance.AssetDatabase.GetTexture("gui_button_1");
            this.guiSliderTexture = gameInstance.AssetDatabase.GetTexture("gui_slider_1");
            this.bigApple3PMSpriteFont = gameInstance.AssetDatabase.GetSpriteFont("font_2");

            this.guiHUD = guiHUD;

            this.eraserIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_4");
            this.exitIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_16");
            this.penIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_19");
            this.bucketIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_20");
            this.replacementIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_21");
            this.frontLayerIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_22");
            this.backLayerIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_23");
            this.squareShapeIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_24");
            this.circleShapeIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_25");
            this.triangleShapeIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_26");
            this.eyeIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_27");

            this.menuButtons = [
                new(this.exitIconTexture, SLocalization_Statements.Exit, SLocalization_GUIs.Button_Exit_Description, ExitButtonAction),
            ];

            this.toolButtons = [
                new(this.eyeIconTexture, SLocalization_Tools.Visualization_Name, SLocalization_Tools.Visualization_Description, () =>
                {
                    SelectToolButtonAction(SPenTool.Visualization);
                    this.guiHUD.SetToolIcon(this.eyeIconTexture);
                }),

                new(this.penIconTexture, SLocalization_Tools.Pencil_Name, SLocalization_Tools.Pencil_Description, () =>
                {
                    SelectToolButtonAction(SPenTool.Pencil);
                    this.guiHUD.SetToolIcon(this.penIconTexture);
                }),
                new(this.eraserIconTexture, SLocalization_Tools.Eraser_Name, SLocalization_Tools.Eraser_Description, () =>
                {
                    SelectToolButtonAction(SPenTool.Eraser);
                    this.guiHUD.SetToolIcon(this.eraserIconTexture);
                }),
                new(this.bucketIconTexture, SLocalization_Tools.Fill_Name, SLocalization_Tools.Fill_Description, () =>
                {
                    SelectToolButtonAction(SPenTool.Fill);
                    this.guiHUD.SetToolIcon(this.bucketIconTexture);
                }),
                new(this.replacementIconTexture, SLocalization_Tools.Replace_Name, SLocalization_Tools.Replace_Description, () =>
                {
                    SelectToolButtonAction(SPenTool.Replace);
                    this.guiHUD.SetToolIcon(this.replacementIconTexture);
                }),
            ];

            this.layerButtons = [
                new(this.frontLayerIconTexture, SLocalization_GUIs.HUD_Complements_PenSettings_Section_Layer_Button_Front_Name, SLocalization_GUIs.HUD_Complements_PenSettings_Section_Layer_Button_Front_Description, () => SelectLayerButtonAction(SWorldLayer.Foreground)),
                new(this.backLayerIconTexture, SLocalization_GUIs.HUD_Complements_PenSettings_Section_Layer_Button_Back_Name, SLocalization_GUIs.HUD_Complements_PenSettings_Section_Layer_Button_Back_Description, () => SelectLayerButtonAction(SWorldLayer.Background)),
            ];

            this.shapeButtons = [
                new(this.squareShapeIconTexture, SLocalization_GUIs.HUD_Complements_PenSettings_Section_Shape_Button_Circle_Name, SLocalization_GUIs.HUD_Complements_PenSettings_Section_Shape_Button_Circle_Description, () => SelectShapeButtonAction(SPenShape.Circle)),
                new(this.circleShapeIconTexture, SLocalization_GUIs.HUD_Complements_PenSettings_Section_Shape_Button_Square_Name, SLocalization_GUIs.HUD_Complements_PenSettings_Section_Shape_Button_Square_Description, () => SelectShapeButtonAction(SPenShape.Square)),
                new(this.triangleShapeIconTexture, SLocalization_GUIs.HUD_Complements_PenSettings_Section_Shape_Button_Triangle_Name, SLocalization_GUIs.HUD_Complements_PenSettings_Section_Shape_Button_Triangle_Description, () => SelectShapeButtonAction(SPenShape.Triangle)),
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
                SSize2 size = new(SGUI_HUDConstants.SLOT_SIZE);

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
            Vector2 offset = new(SGUI_HUDConstants.SLOT_SIZE);
            SSize2 size = new(SGUI_HUDConstants.SLOT_SIZE);

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

                offset.X += SGUI_HUDConstants.SLOT_SPACING;
            }
        }

        private void UpdateToolBottons()
        {
            for (int i = 0; i < this.toolButtons.Length; i++)
            {
                SSlot slot = this.toolButtonSlots[i];
                bool isOver = this.GUIEvents.OnMouseOver(slot.BackgroundElement.Position, new(SGUI_HUDConstants.SLOT_SIZE));

                Vector2 position = slot.BackgroundElement.Position;
                SSize2 size = new(SGUI_HUDConstants.SLOT_SIZE);

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

                slot.BackgroundElement.Color = this.toolButtonSelectedIndex == i ? SColorPalette.SelectedColor : isOver ? SColorPalette.HoverColor : SColorPalette.White;
            }
        }

        private void UpdateLayerButtons()
        {
            for (int i = 0; i < this.layerButtons.Length; i++)
            {
                SSlot slot = this.layerButtonSlots[i];
                bool isOver = this.GUIEvents.OnMouseOver(slot.BackgroundElement.Position, new(SGUI_HUDConstants.SLOT_SIZE));

                if (this.GUIEvents.OnMouseClick(slot.BackgroundElement.Position, new(SGUI_HUDConstants.SLOT_SIZE)))
                {
                    this.layerButtons[i].ClickAction.Invoke();
                }

                if (isOver)
                {
                    this.tooltipBoxElement.IsVisible = true;

                    SGUIGlobalTooltip.Title = this.layerButtons[i].DisplayName;
                    SGUIGlobalTooltip.Description = this.layerButtons[i].Description;
                }

                slot.BackgroundElement.Color = this.layerButtonSelectedIndex == i ? SColorPalette.SelectedColor : isOver ? SColorPalette.HoverColor : SColorPalette.White;
            }
        }

        private void UpdateShapeButtons()
        {
            for (int i = 0; i < this.shapeButtons.Length; i++)
            {
                SSlot slot = this.shapeButtonSlots[i];
                bool isOver = this.GUIEvents.OnMouseOver(slot.BackgroundElement.Position, new(SGUI_HUDConstants.SLOT_SIZE));

                if (this.GUIEvents.OnMouseClick(slot.BackgroundElement.Position, new(SGUI_HUDConstants.SLOT_SIZE)))
                {
                    this.shapeButtons[i].ClickAction.Invoke();
                }

                if (isOver)
                {
                    this.tooltipBoxElement.IsVisible = true;

                    SGUIGlobalTooltip.Title = this.shapeButtons[i].DisplayName;
                    SGUIGlobalTooltip.Description = this.shapeButtons[i].Description;
                }

                slot.BackgroundElement.Color = this.shapeButtonSelectedIndex == i ? SColorPalette.SelectedColor : isOver ? SColorPalette.HoverColor : SColorPalette.White;
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
