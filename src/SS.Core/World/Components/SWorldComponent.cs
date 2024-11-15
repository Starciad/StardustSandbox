using StardustSandbox.Game.Interfaces;
using StardustSandbox.Game.Objects;

namespace StardustSandbox.Game.World.Components
{
    public abstract class SWorldComponent(ISGame gameInstance, SWorld worldInstance) : SGameObject(gameInstance)
    {
        protected SWorld SWorldInstance { get; private set; } = worldInstance;
    }
}
