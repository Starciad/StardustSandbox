using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Mathematics;
using PixelDust.Game.GUI.Elements.Common;
using PixelDust.Game.GUI.Interfaces;

using Microsoft.Xna.Framework;
using PixelDust.Game.Constants.GUI;

namespace PixelDust.Game.GUI.Common
{
    public sealed class PHudGUI : PGUISystem
    {
        private Texture2D particleTexture;

        protected override void OnAwake()
        {
            this.particleTexture = this.Game.AssetDatabase.GetTexture("particle_1");
            base.OnAwake();
        }

        protected override void OnBuild(IPGUILayoutBuilder layout)
        {

        }
    }
}
