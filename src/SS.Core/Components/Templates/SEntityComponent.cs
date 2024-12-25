using StardustSandbox.Core.Entities;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.Core.Components.Templates
{
    public abstract class SEntityComponent(ISGame gameInstance, SEntity entityInstance) : SComponent(gameInstance)
    {
        protected SEntity SEntityInstance => entityInstance;
    }
}