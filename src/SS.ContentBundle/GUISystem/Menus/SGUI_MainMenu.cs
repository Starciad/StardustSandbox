using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Elements.Graphics;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.GUISystem.Menus
{
    public sealed partial class SGUI_MainMenu : SGUISystem
    {
        private readonly Texture2D gameTitleTexture;
        private readonly Texture2D buttonBackgroundTexture;
        private readonly Texture2D particleTexture;

        public SGUI_MainMenu(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance, identifier, guiEvents)
        {
            this.gameTitleTexture = gameInstance.AssetDatabase.GetTexture("game_title_1");
            this.buttonBackgroundTexture = gameInstance.AssetDatabase.GetTexture("gui_background_2");
            this.particleTexture = this.SGameInstance.AssetDatabase.GetTexture("particle_1");
        }
    }
}
