namespace PixelDust.Core
{
    public class PWorldStates
    {
        public bool IsPaused { get; private set; }
        public bool IsUnloaded { get; private set; }

        public PWorldStates()
        {
            IsPaused = false;
            IsUnloaded = false;
        }

        internal void SetPaused(bool value)
        {
            IsPaused = value;
        }

        internal void SetUnloaded(bool value)
        {
            IsUnloaded = value;
        }
    }
}
