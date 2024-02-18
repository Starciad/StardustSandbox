using PixelDust.Game.Mathematics;

using System.Runtime.InteropServices;

namespace PixelDust.Game.World.Components.Chunking
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PWorldChunk(Vector2Int position, short size)
    {
        public readonly Vector2Int Position => this._position;
        public readonly short Size => this._size;

        public readonly bool ShouldUpdate => this._shouldUpdate;

        private readonly Vector2Int _position = position;
        private readonly short _size = size;

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
