using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Specials.General;
using StardustSandbox.ContentBundle.GUISystem.Specials.Interactive;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.Fonts;
using StardustSandbox.Core.Constants.GUI.Common;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Items;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements
{
    internal sealed partial class SGUI_PenSettings : SGUISystem
    {
        private int brushSizeSliderValue;
        private int toolBottomSelectedIndex;
        private int layerBottomSelectedIndex;

        private readonly Texture2D particleTexture;
        private readonly Texture2D guiBackgroundTexture;
        private readonly Texture2D guiButton1Texture;
        private readonly Texture2D guiSliderTexture;
        private readonly Texture2D[] iconTextures;
        private readonly SpriteFont bigApple3PMSpriteFont;

        private readonly SButton[] toolButtons;
        private readonly SButton[] layerButtons;

        private readonly Rectangle[] brushSizeSliderClipTextures;

        internal SGUI_PenSettings(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance, identifier, guiEvents)
        {
            this.brushSizeSliderValue = 0;
            this.toolBottomSelectedIndex = 0;
            this.layerBottomSelectedIndex = 0;

            this.particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
            this.guiBackgroundTexture = gameInstance.AssetDatabase.GetTexture("gui_background_1");
            this.guiButton1Texture = gameInstance.AssetDatabase.GetTexture("gui_button_1");
            this.guiSliderTexture = gameInstance.AssetDatabase.GetTexture("gui_slider_1");
            this.bigApple3PMSpriteFont = gameInstance.AssetDatabase.GetSpriteFont(SFontFamilyConstants.BIG_APPLE_3PM);

            this.iconTextures = [
                gameInstance.AssetDatabase.GetTexture("icon_gui_19"),
                gameInstance.AssetDatabase.GetTexture("icon_gui_20"),
                gameInstance.AssetDatabase.GetTexture("icon_gui_21"),
                gameInstance.AssetDatabase.GetTexture("icon_gui_22"),
                gameInstance.AssetDatabase.GetTexture("icon_gui_23"),
            ];

            this.toolButtons = [
                new(this.iconTextures[0], "Pencil", string.Empty, () => { }),
                new(this.iconTextures[1], "Fill", string.Empty, () => { }),
                new(this.iconTextures[2], "Replace", string.Empty, () => { }),
            ];

            this.layerButtons = [
                new(this.iconTextures[3], "Front", string.Empty, () => { }),
                new(this.iconTextures[4], "Back", string.Empty, () => { }),
            ];

            this.toolButtonSlots = new SSlot[this.toolButtons.Length];
            this.layerButtonSlots = new SSlot[this.layerButtons.Length];

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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateBrushSizeSlider();
            UpdateToolBottons();
            UpdateLayerButtons();
        }

        private void UpdateBrushSizeSlider()
        {
            Vector2 position = this.brushSizeSliderElement.Position;
            Vector2 offset = new(SHUDConstants.SLOT_SIZE);

            for (int i = 0; i < this.brushSizeSliderClipTextures.Length; i++)
            {
                if (this.GUIEvents.OnMouseDown(position + offset, new(SHUDConstants.SLOT_SIZE)))
                {
                    this.brushSizeSliderValue = i;
                    this.brushSizeSliderElement.TextureClipArea = this.brushSizeSliderClipTextures[i];
                    break;
                }

                offset.X += SHUDConstants.SLOT_SPACING;
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
                    this.toolBottomSelectedIndex = (byte)i;
                }

                slot.BackgroundElement.Color = this.toolBottomSelectedIndex == i ?
                                        SColorPalette.SelectedColor :
                                        (isOver ? SColorPalette.HoverColor : SColorPalette.White);
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
                    this.layerBottomSelectedIndex = (byte)i;
                }

                slot.BackgroundElement.Color = this.layerBottomSelectedIndex == i ?
                                        SColorPalette.SelectedColor :
                                        (isOver ? SColorPalette.HoverColor : SColorPalette.White);
            }
        }
    }
}
