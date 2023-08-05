namespace PixelDust.Core
{
    public sealed partial class World
    {
        // Const
        public const uint GridScale = 12;

        // System
        public bool IsPaused { get; private set; }

        // Infos
        public float Temperature { get; private set; } = 20;
        public uint TotalElements { get; private set; }

        public uint Width { get; private set; }
        public uint Height { get; private set; }

        // World
        private WorldSlot[,] _slots;
        private readonly PElementContext _EContext;

        public World()
        {
            _EContext = new(this);
            Restart();
        }
    }
}