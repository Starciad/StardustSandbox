using StardustSandbox.Game.Objects;

namespace StardustSandbox.Game.World.Components
{
    public abstract class SWorldComponent : SGameObject
    {
        protected SWorld SWorldInstance { get; private set; }

        public SWorldComponent(SGame gameInstance, SWorld worldInstance) : base(gameInstance)
        {
            this.SWorldInstance = worldInstance;
        }
    }
}
