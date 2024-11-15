namespace StardustSandbox.Core.World.Data
{
    public sealed class SWorldState
    {
        public bool IsActive { get; set; }
        public bool IsPaused { get; set; }

        public SWorldState()
        {
            this.IsActive = false;
            this.IsPaused = false;
        }
    }
}
