using PixelDust.Game.Mathematics;

namespace PixelDust.Game.World.Data
{
    public sealed class PWorldChunk(Vector2Int position)
    {
        public Vector2Int Position => this._position;
        public bool ShouldUpdate => this._shouldUpdate;

        private readonly Vector2Int _position = position;

        private bool _shouldUpdate = true;
        private bool _shouldUpdateNextFrame = true;

        public void Update()
        {
            this._shouldUpdate = this._shouldUpdateNextFrame;
            this._shouldUpdateNextFrame = false;
        }

        public void Notify()
        {
            this._shouldUpdateNextFrame = true;
        }
    }
}
