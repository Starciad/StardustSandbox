using Microsoft.Xna.Framework;

namespace StardustSandbox.Game.World.Data
{
    public sealed class SWorldChunk(Point position)
    {
        public Point Position => this._position;
        public bool ShouldUpdate => this._shouldUpdate;

        private readonly Point _position = position;

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
