using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Constants;
using PixelDust.Game.Mathematics;

namespace PixelDust.Game.Engine
{
    public sealed class PScreen
    {
        public Size2Int DefaultResolution => PScreenConstants.RESOLUTIONS[3];
        public Size2Int CurrentResolution { get; private set; }

        public Viewport Viewport => _viewport;

        private Viewport _viewport;

        public void Build(Viewport viewport)
        {
            _viewport = viewport;
        }
    }
}
