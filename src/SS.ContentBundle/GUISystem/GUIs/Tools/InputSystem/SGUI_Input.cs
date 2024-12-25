using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants.Fonts;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.General;

using System.Text;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Specials
{
    internal sealed partial class SGUI_Input : SGUISystem
    {
        private int cursorPosition = 0;

        private readonly Texture2D particleTexture;
        private readonly Texture2D typingFieldTexture;
        private readonly SpriteFont pixelOperatorSpriteFont;

        private readonly StringBuilder userInputStringBuilder = new();

        internal SGUI_Input(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance, identifier, guiEvents)
        {
            this.particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
            this.typingFieldTexture = gameInstance.AssetDatabase.GetTexture("gui_field_2");
            this.pixelOperatorSpriteFont = gameInstance.AssetDatabase.GetSpriteFont(SFontFamilyConstants.PIXEL_OPERATOR);
        }

        internal void Setup()
        {

        }
    }
}
