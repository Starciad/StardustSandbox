using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Objects;
using StardustSandbox.Core.World;

namespace StardustSandbox.Core.World.Components
{
    public abstract class SWorldComponent(ISGame gameInstance, SWorld worldInstance) : SGameObject(gameInstance)
    {
        protected SWorld SWorldInstance { get; private set; } = worldInstance;
    }
}
