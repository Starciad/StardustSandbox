using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Constants;
using PixelDust.Game.Mathematics;

namespace PixelDust.Game.Managers
{
    public sealed class PScreenManager
    {
        public Size2Int DefaultResolution => PScreenConstants.RESOLUTIONS[3];
        public Size2Int CurrentResolution { get; private set; }

        public Viewport Viewport => this._viewport;

        private Viewport _viewport;

        public void Build(Viewport viewport)
        {
            this._viewport = viewport;
        }
    }
}
