using PixelDust.Game.Mathematics;

using System.Runtime.InteropServices;

namespace PixelDust.Game.Worlding.Components.Chunking
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PWorldChunk
    {
        public readonly Vector2Int Position => this._position;
        public readonly short Size => this._size;

        public readonly bool ShouldUpdate => this._shouldUpdate;

        private readonly Vector2Int _position;
        private readonly short _size;

        private bool _shouldUpdate;
        private bool _shouldUpdateNextFrame;

        public PWorldChunk(Vector2Int position, short size)
        {
            this._position = position;
            this._size = size;

            this._shouldUpdate = true;
            this._shouldUpdateNextFrame = true;
        }

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
