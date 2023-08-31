namespace PixelDust.Core
{
    public class PWorldStates
    {
        public bool IsActive { get; internal set; }
        public bool IsPaused { get; internal set; }

        public PWorldStates()
        {
            IsActive = false;
            IsPaused = false;
        }
    }
}
