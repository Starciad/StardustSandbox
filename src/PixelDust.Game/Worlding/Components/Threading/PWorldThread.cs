using System.Runtime.InteropServices;

namespace PixelDust.Game.Worlding.Components.Threading
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PWorldThread
    {
        public readonly int Index => this._index;
        public readonly int StartPosition => this._startPosition;
        public int EndPosition { readonly get => this._endPosition; set => this._endPosition = value; }
        public readonly int Range => this._endPosition - this._startPosition;

        private readonly int _index;
        private readonly int _startPosition;
        private int _endPosition;

        public PWorldThread(int index, int startPos, int endPos)
        {
            this._index = index;
            this._startPosition = startPos;
            this._endPosition = endPos;
        }
    }
}
