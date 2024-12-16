using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Specials.Interactive;
using StardustSandbox.Core.Constants.Fonts;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    internal sealed partial class SGUI_WorldsExplorerMenu : SGUISystem
    {
        private readonly Texture2D particleTexture;
        private readonly Texture2D guiButtonTexture;
        private readonly Texture2D reloadIconTexture;
        private readonly Texture2D exitIconTexture;
        private readonly SpriteFont bigApple3PMSpriteFont;

        private readonly SButton[] headerButtons;
        private readonly SButton[] footerButtons;

        public SGUI_WorldsExplorerMenu(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance, identifier, guiEvents)
        {
            this.particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
            this.guiButtonTexture = this.SGameInstance.AssetDatabase.GetTexture("gui_button_1");
            this.reloadIconTexture = this.SGameInstance.AssetDatabase.GetTexture("icon_gui_5");
            this.exitIconTexture = this.SGameInstance.AssetDatabase.GetTexture("icon_gui_15");
            this.bigApple3PMSpriteFont = gameInstance.AssetDatabase.GetSpriteFont(SFontFamilyConstants.BIG_APPLE_3PM);

            this.headerButtons = [
                new(this.reloadIconTexture, "Reload", ReloadButtonAction),
                new(this.exitIconTexture, "Exit", ExitButtonAction),
            ];

            this.footerButtons = [
                new(this.reloadIconTexture, "Previous", PreviousButtonAction),
                new(this.exitIconTexture, "Next", NextButtonAction),
            ];
        }
    }
}
