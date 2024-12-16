using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants.Fonts;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    internal sealed partial class SGUI_WorldsExplorerMenu(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : SGUISystem(gameInstance, identifier, guiEvents)
    {
        private enum SWorldLoadingType
        {
            Local,
        }

        private SWorldLoadingType worldLoadingType;

        private readonly Texture2D particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
        private readonly Texture2D guiBackgroundTexture = gameInstance.AssetDatabase.GetTexture("gui_background_1");
        private readonly Texture2D squareShapeTexture = gameInstance.AssetDatabase.GetTexture("shape_square_1");
        private readonly SpriteFont bigApple3PMSpriteFont = gameInstance.AssetDatabase.GetSpriteFont(SFontFamilyConstants.BIG_APPLE_3PM);
    }
}
