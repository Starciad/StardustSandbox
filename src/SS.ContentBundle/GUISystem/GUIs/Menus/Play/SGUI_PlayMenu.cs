using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants.Fonts;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.General;

using System;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    internal partial class SGUI_PlayMenu : SGUISystem
    {
        private readonly Texture2D guiBackgroundTexture;
        private readonly SpriteFont bigApple3PMSpriteFont;

        private readonly Action[] buttonActions;

        internal SGUI_PlayMenu(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance, identifier, guiEvents)
        {
            this.guiBackgroundTexture = gameInstance.AssetDatabase.GetTexture("gui_background_1");
            this.bigApple3PMSpriteFont = this.SGameInstance.AssetDatabase.GetSpriteFont(SFontFamilyConstants.BIG_APPLE_3PM);

            this.buttonActions = [
                WorldsButton
            ];
        }
    }
}
