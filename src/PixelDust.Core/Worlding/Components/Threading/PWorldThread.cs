using System.Runtime.InteropServices;

namespace PixelDust.Core.Worlding
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct PWorldThread
    {
        public readonly int Index => _index;
        public readonly int StartPosition => _startPosition;
        public int EndPosition { readonly get => _endPosition; set => _endPosition = value; }
        public readonly int Range => _endPosition - _startPosition;

        private readonly int _index;
        private readonly int _startPosition;
        private int _endPosition;

        public PWorldThread(int index, int startPos, int endPos)
        {
            _index = index;
            _startPosition = startPos;
            _endPosition = endPos;
        }
    }
}
