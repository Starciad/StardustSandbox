using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.System;
using StardustSandbox.Core.Objects;

namespace StardustSandbox.Core.Components
{
    public abstract class SComponent(ISGame gameInstance) : SGameObject(gameInstance), ISResettable
    {
        public virtual void Reset()
        {
            return;
        }
    }
}
