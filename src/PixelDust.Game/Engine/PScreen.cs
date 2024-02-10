using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PixelDust.Game.Engine
{
    public sealed class PScreen
    {
        public Vector2 DefaultResolution => resolutions[3];
        public Vector2 CurrentResolution { get; private set; }

        public Viewport Viewport => _viewport;

        private Viewport _viewport;


        public void Build(Viewport viewport)
        {
            _viewport = viewport;
        }
    }
}
