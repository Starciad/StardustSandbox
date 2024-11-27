using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.World;

namespace StardustSandbox.Core.Components.Templates
{
    public abstract class SWorldComponent(ISGame gameInstance, SWorld worldInstance) : SComponent(gameInstance)
    {
        protected SWorld SWorldInstance { get; private set; } = worldInstance;
    }
}
