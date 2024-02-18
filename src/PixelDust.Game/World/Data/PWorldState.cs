namespace PixelDust.Game.World.Data
{
    public sealed class PWorldState
    {
        public bool IsActive { get; set; }
        public bool IsPaused { get; set; }

        public PWorldState()
        {
            this.IsActive = false;
            this.IsPaused = false;
        }
    }
}
