using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    public sealed partial class SGUI_OptionsMenu : SGUISystem
    {
        private readonly Texture2D particleTexture;
        private readonly Texture2D guiBackgroundTexture;

        public SGUI_OptionsMenu(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance, identifier, guiEvents)
        {
            this.particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
            this.guiBackgroundTexture = gameInstance.AssetDatabase.GetTexture("gui_background_1");
        }
    }
}
