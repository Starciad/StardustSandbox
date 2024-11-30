using StardustSandbox.Core.Components.Common.Entities;
using StardustSandbox.Core.Components.Templates;
using StardustSandbox.Core.Entities;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.Components.Entities.AI
{
    internal sealed class SMagicCursorEntityAIComponent : SEntityComponent
    {
        public SMagicCursorEntityAIComponent(ISGame gameInstance, SEntity entityInstance, SEntityTransformComponent transformComponent) : base(gameInstance, entityInstance)
        {

        }
    }
}
