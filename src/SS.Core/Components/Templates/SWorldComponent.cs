using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.World;

namespace StardustSandbox.Core.Components.Templates
{
    public abstract class SWorldComponent(ISGame gameInstance, ISWorld worldInstance) : SComponent(gameInstance)
    {
        protected ISWorld SWorldInstance { get; private set; } = worldInstance;
    }
}
