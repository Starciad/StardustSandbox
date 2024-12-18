using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Specials.General;
using StardustSandbox.ContentBundle.GUISystem.Specials.Interactive;
using StardustSandbox.Core.Constants.Fonts;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements
{
    internal sealed partial class SGUI_PenSettings : SGUISystem
    {
        private readonly Texture2D particleTexture;
        private readonly Texture2D guiBackgroundTexture;
        private readonly Texture2D guiButton1Texture;
        private readonly Texture2D guiSliderTexture;
        private readonly Texture2D[] iconTextures;
        private readonly SpriteFont bigApple3PMSpriteFont;

        private readonly SButton[] toolButtons;
        private readonly SButton[] layerButtons;

        internal SGUI_PenSettings(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance, identifier, guiEvents)
        {
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
        }
    }
}
