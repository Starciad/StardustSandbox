using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Specials.General;
using StardustSandbox.ContentBundle.GUISystem.Specials.Interactive;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.Fonts;
using StardustSandbox.Core.Constants.GUI.Common;
using StardustSandbox.Core.Controllers.GameInput;
using StardustSandbox.Core.Enums.GameInput.Pen;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Items;
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

        internal SGUI_PenSettings(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance, identifier, guiEvents)
        {
            this.toolButtonSelectedIndex = 0;
            this.layerButtonSelectedIndex = 0;

            this.particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
            this.guiBackgroundTexture = gameInstance.AssetDatabase.GetTexture("gui_background_1");
            this.guiButton1Texture = gameInstance.AssetDatabase.GetTexture("gui_button_1");
            this.guiSliderTexture = gameInstance.AssetDatabase.GetTexture("gui_slider_1");
            this.bigApple3PMSpriteFont = gameInstance.AssetDatabase.GetSpriteFont(SFontFamilyConstants.BIG_APPLE_3PM);

            this.iconTextures = [
                gameInstance.AssetDatabase.GetTexture("icon_gui_16"),
                gameInstance.AssetDatabase.GetTexture("icon_gui_19"),
                gameInstance.AssetDatabase.GetTexture("icon_gui_20"),
                gameInstance.AssetDatabase.GetTexture("icon_gui_21"),
                gameInstance.AssetDatabase.GetTexture("icon_gui_22"),
                gameInstance.AssetDatabase.GetTexture("icon_gui_23"),
                gameInstance.AssetDatabase.GetTexture("icon_gui_24"),
                gameInstance.AssetDatabase.GetTexture("icon_gui_25"),
                gameInstance.AssetDatabase.GetTexture("icon_gui_26"),
            ];

            this.menuButtons = [
                new(this.iconTextures[0], "Exit", string.Empty, ExitButtonAction),
            ];

            this.toolButtons = [
                new(this.iconTextures[1], "Pencil", string.Empty, SelectPencilToolButtonAction),
                new(this.iconTextures[2], "Fill", string.Empty, SelectFillToolButtonAction),
                new(this.iconTextures[3], "Replace", string.Empty, SelectReplaceToolButtonAction),
            ];

            this.layerButtons = [
                new(this.iconTextures[4], "Front", string.Empty, SelectFrontLayerButtonAction),
                new(this.iconTextures[5], "Back", string.Empty, SelectBackLayerButtonAction),
            ];

            this.shapeButtons = [
                new(this.iconTextures[6], "Circle", string.Empty, SelectCircleShapeButtonAction),
                new(this.iconTextures[7], "Square", string.Empty, SelectSquareShapeButtonAction),
                new(this.iconTextures[8], "Triangle", string.Empty, SelectTriangleShapeButtonAction),
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
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateBrushSizeSlider();
            UpdateMenuButtons();
            UpdateToolBottons();
            UpdateLayerButtons();
            UpdateShapeButtons();
            SyncGUIElements();
        }

        private void UpdateBrushSizeSlider()
        {
            Vector2 position = this.brushSizeSliderElement.Position;
            Vector2 offset = new(SHUDConstants.SLOT_SIZE);

            for (int i = 0; i < this.brushSizeSliderClipTextures.Length; i++)
            {
                if (this.GUIEvents.OnMouseDown(position + offset, new(SHUDConstants.SLOT_SIZE)))
                {
                    this.SGameInstance.GameInputController.Pen.SetSize((byte)(i + 1));
                    break;
                }

                offset.X += SHUDConstants.SLOT_SPACING;
            }
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtonSlots.Length; i++)
            {
                SSlot slot = this.menuButtonSlots[i];

                if (this.GUIEvents.OnMouseClick(slot.BackgroundElement.Position, new SSize2(SHUDConstants.SLOT_SIZE)))
                {
                    this.menuButtons[i].ClickAction.Invoke();
                }

                slot.BackgroundElement.Color = this.GUIEvents.OnMouseOver(slot.BackgroundElement.Position, new SSize2(SHUDConstants.SLOT_SIZE)) ? SColorPalette.HoverColor : SColorPalette.White;
            }
        }

        private void UpdateToolBottons()
        {
            for (int i = 0; i < this.toolButtonSlots.Length; i++)
            {
                SSlot slot = this.toolButtonSlots[i];
                bool isOver = this.GUIEvents.OnMouseOver(slot.BackgroundElement.Position, new SSize2(SHUDConstants.SLOT_SIZE));

                if (this.GUIEvents.OnMouseClick(slot.BackgroundElement.Position, new SSize2(SHUDConstants.SLOT_SIZE)))
                {
                    this.toolButtons[i].ClickAction.Invoke();
                }

                slot.BackgroundElement.Color = this.toolButtonSelectedIndex == i ? SColorPalette.SelectedColor : (isOver ? SColorPalette.HoverColor : SColorPalette.White);
            }
        }

        private void UpdateLayerButtons()
        {
            for (int i = 0; i < this.layerButtons.Length; i++)
            {
                SSlot slot = this.layerButtonSlots[i];
                bool isOver = this.GUIEvents.OnMouseOver(slot.BackgroundElement.Position, new SSize2(SHUDConstants.SLOT_SIZE));

                if (this.GUIEvents.OnMouseClick(slot.BackgroundElement.Position, new SSize2(SHUDConstants.SLOT_SIZE)))
                {
                    this.layerButtons[i].ClickAction.Invoke();
                }

                slot.BackgroundElement.Color = this.layerButtonSelectedIndex == i ? SColorPalette.SelectedColor : (isOver ? SColorPalette.HoverColor : SColorPalette.White);
            }
        }

        private void UpdateShapeButtons()
        {
            for (int i = 0; i < this.shapeButtons.Length; i++)
            {
                SSlot slot = this.shapeButtonSlots[i];
                bool isOver = this.GUIEvents.OnMouseOver(slot.BackgroundElement.Position, new SSize2(SHUDConstants.SLOT_SIZE));

                if (this.GUIEvents.OnMouseClick(slot.BackgroundElement.Position, new SSize2(SHUDConstants.SLOT_SIZE)))
                {
                    this.shapeButtons[i].ClickAction.Invoke();
                }

                slot.BackgroundElement.Color = this.shapeButtonSelectedIndex == i ? SColorPalette.SelectedColor : (isOver ? SColorPalette.HoverColor : SColorPalette.White);
            }
        }
        
        private void SyncGUIElements()
        {
            // Brush Size Slider
            this.brushSizeSliderElement.TextureClipArea = this.brushSizeSliderClipTextures[this.gameInputController.Pen.Size - 1];

            // Tool
            this.toolButtonSelectedIndex = (byte)this.gameInputController.Pen.Tool;

            // Layer
            this.layerButtonSelectedIndex = (byte)this.gameInputController.Pen.Layer;

            // Shape
            this.shapeButtonSelectedIndex = (byte)this.gameInputController.Pen.Shape;
        }
    }
}
