using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.Core.Elements.Templates.Solids
{
    public abstract class SSolid : SElement
    {
        public SSolid(ISGame gameInstance) : base(gameInstance)
        {
            this.defaultDensity = 2000;
        }
    }
}
