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
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements
{
    internal sealed partial class SGUI_SaveSettings : SGUISystem
    {
        private readonly Texture2D particleTexture;
        private readonly Texture2D guiBackgroundTexture;
        private readonly Texture2D guiButton1Texture;
        private readonly Texture2D guiField1Texture;
        private readonly Texture2D[] iconTextures;
        private readonly SpriteFont bigApple3PMSpriteFont;

        private readonly SButton[] menuButtons;

        internal SGUI_SaveSettings(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance, identifier, guiEvents)
        {
            this.particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
            this.guiBackgroundTexture = gameInstance.AssetDatabase.GetTexture("gui_background_1");
            this.guiButton1Texture = gameInstance.AssetDatabase.GetTexture("gui_button_1");
            this.guiField1Texture = gameInstance.AssetDatabase.GetTexture("gui_field_1");
            this.bigApple3PMSpriteFont = gameInstance.AssetDatabase.GetSpriteFont(SFontFamilyConstants.BIG_APPLE_3PM);

            this.iconTextures = [
                gameInstance.AssetDatabase.GetTexture("icon_gui_16"),
            ];

            this.menuButtons = [
                new(this.iconTextures[0], "Exit", string.Empty, ExitButtonAction),
            ];

            this.menuButtonSlots = new SSlot[this.menuButtons.Length];
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateMenuButtons();
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
    }
}
