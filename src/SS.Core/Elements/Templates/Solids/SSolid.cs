using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.Core.Elements.Templates.Solids
{
    public abstract class SSolid : SElement
    {
        public SSolid(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.defaultDensity = 2000;
        }
    }
}
