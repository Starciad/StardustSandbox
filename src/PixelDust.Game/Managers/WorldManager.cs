using PixelDust.Core.Managers;
using PixelDust.Core.Worlding;

namespace PixelDust.Game.Managers
{
    [PManagerRegister]
    internal sealed class WorldManager : PManager
    {
        public PWorld Instance => _world;

        private readonly PWorld _world = new();

        protected override void OnAwake()
        {
            _world.Initialize();
        }

        protected override void OnUpdate()
        {
            _world.Update();
        }

        protected override void OnDraw()
        {
            _world.Draw();
        }
    }
}
