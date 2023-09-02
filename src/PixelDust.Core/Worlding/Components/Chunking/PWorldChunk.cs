using System.Runtime.InteropServices;

using Microsoft.Xna.Framework;

using PixelDust.Core.Mathematics;

namespace PixelDust.Core.Worlding
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct PWorldChunk
    {
        internal readonly Vector2Int Position => _position;
        internal readonly short Size => _size;

        internal readonly bool ShouldUpdate => _shouldUpdate;

        private readonly Vector2Int _position;
        private readonly short _size;

        private bool _shouldUpdate;
        private bool _shouldUpdateNextFrame;

        public PWorldChunk(Vector2Int position, short size)
        {
            _position = position;
            _size = size;

            _shouldUpdate = true;
            _shouldUpdateNextFrame = true;
        }

        internal void Update()
        {
            _shouldUpdate = _shouldUpdateNextFrame;
            _shouldUpdateNextFrame = false;
        }

        internal void Notify()
        {
            _shouldUpdateNextFrame = true;
        }
    }
}
