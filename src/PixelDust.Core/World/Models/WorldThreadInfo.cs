using System.Runtime.InteropServices;

namespace PixelDust.Core
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct WorldThreadInfo
    {
        internal readonly int Index => _index;
        internal readonly int StartPosition => _startPosition;
        internal int EndPosition { readonly get => _endPosition; set => _endPosition = value; }
        internal readonly int Range => _endPosition - _startPosition;

        private readonly int _index;
        private readonly int _startPosition;
        private int _endPosition;

        public WorldThreadInfo(int index, int startPos, int endPos)
        {
            _index = index;
            _startPosition = startPos;
            _endPosition = endPos;
        }
    }
}
