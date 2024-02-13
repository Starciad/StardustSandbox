using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.GUI.Interfaces;

namespace PixelDust.Game.GUI.Common
{
    public sealed class PGUI_HUD : PGUISystem
    {
        private Texture2D menuBackgroundTexture;

        protected override void OnAwake()
        {
            this.menuBackgroundTexture = this.Game.AssetDatabase.GetTexture("particle_1");
            base.OnAwake();
        }

        protected override void OnBuild(IPGUILayoutBuilder layout)
        {

        }
    }
}
