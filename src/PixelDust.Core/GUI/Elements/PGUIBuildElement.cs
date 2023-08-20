using System;

namespace PixelDust.Core.GUI
{
    public sealed class PGUIBuildElement : IDisposable
    {
        public bool IsClosed => closed;
        private bool closed;

        public void Dispose()
        {
            closed = true;
        }
    }
}
