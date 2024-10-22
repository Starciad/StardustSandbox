using StardustSandbox.Game.Objects;

namespace StardustSandbox.Game.World.Components
{
    public abstract class SWorldComponent(SGame gameInstance, SWorld worldInstance) : SGameObject(gameInstance)
    {
        protected SWorld SWorldInstance { get; private set; } = worldInstance;
    }
}
