namespace PixelDust.Game.Worlding.World.Data
{
    public class PWorldStates
    {
        public bool IsActive { get; internal set; }
        public bool IsPaused { get; internal set; }

        public PWorldStates()
        {
            this.IsActive = false;
            this.IsPaused = false;
        }
    }
}
