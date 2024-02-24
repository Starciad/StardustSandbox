using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Constants.GUI;
using PixelDust.Game.GUI.Events;

namespace PixelDust.Game.GUI.Common.Menus.ElementExplorer
{
    public sealed partial class PGUI_ElementExplorer(PGUIEvents events, PGUILayoutPool layoutPool) : PGUISystem(events, layoutPool)
    {
        private Texture2D particleTexture;
        private Texture2D guiBackgroundTexture;

        protected override void OnAwake()
        {
            base.OnAwake();

            this.Name = PGUIConstants.ELEMENT_EXPLORER;

            this.particleTexture = this.Game.AssetDatabase.GetTexture("particle_1");
            this.guiBackgroundTexture = this.Game.AssetDatabase.GetTexture("gui_background_1");
        }
    }
}
