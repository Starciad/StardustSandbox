using PixelDust.Core.Managers;
using PixelDust.Core.Managers.Attributes;
using PixelDust.Core.Worlding;

namespace PixelDust.Game.Managers
{
    [PManagerRegister]
    internal sealed class WorldManager : PManager
    {
        public PWorld Instance => this._world;

        private readonly PWorld _world = new();

        protected override void OnAwake()
        {
            this._world.Initialize();
        }

        protected override void OnUpdate()
        {
            this._world.Update();
        }

        protected override void OnDraw()
        {
            this._world.Draw();
        }
    }
}
