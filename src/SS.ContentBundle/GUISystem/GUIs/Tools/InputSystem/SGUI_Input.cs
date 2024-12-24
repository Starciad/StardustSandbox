using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants.Fonts;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Specials
{
    internal sealed partial class SGUI_Input : SGUISystem
    {
        private readonly Texture2D particleTexture;
        private readonly Texture2D typingFieldTexture;
        private readonly SpriteFont pixelOperatorSpriteFont;

        internal SGUI_Input(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance, identifier, guiEvents)
        {
            this.particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
            this.typingFieldTexture = gameInstance.AssetDatabase.GetTexture("gui_field_2");
            this.pixelOperatorSpriteFont = gameInstance.AssetDatabase.GetSpriteFont(SFontFamilyConstants.PIXEL_OPERATOR);
        }

        internal void Configure(string synopsis)
        {

        }
    }
}
