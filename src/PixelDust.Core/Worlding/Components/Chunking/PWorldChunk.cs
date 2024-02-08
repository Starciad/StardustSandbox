using PixelDust.Mathematics;

using System.Runtime.InteropServices;

namespace PixelDust.Core.Worlding.Components.Chunking
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct PWorldChunk
    {
        internal readonly Vector2Int Position => this._position;
        internal readonly short Size => this._size;

        internal readonly bool ShouldUpdate => this._shouldUpdate;

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

        internal void Update()
        {
            this._shouldUpdate = this._shouldUpdateNextFrame;
            this._shouldUpdateNextFrame = false;
        }

        internal void Notify()
        {
            this._shouldUpdateNextFrame = true;
        }
    }
}
