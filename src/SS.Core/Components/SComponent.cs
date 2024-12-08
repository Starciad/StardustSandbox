using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Objects;

namespace StardustSandbox.Core.Components
{
    public abstract class SComponent(ISGame gameInstance) : SGameObject(gameInstance), ISReset
    {
        public virtual void Reset()
        {
            return;
        }
    }
}
