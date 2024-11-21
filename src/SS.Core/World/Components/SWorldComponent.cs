using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Objects;

namespace StardustSandbox.Core.World.Components
{
    public abstract class SWorldComponent(ISGame gameInstance, SWorld worldInstance) : SGameObject(gameInstance)
    {
        protected SWorld SWorldInstance { get; private set; } = worldInstance;
    }
}
