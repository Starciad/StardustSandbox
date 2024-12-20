using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.Core.Elements.Templates.Solids
{
    public abstract class SSolid : SElement
    {
        public SSolid(ISGame gameInstance) : base(gameInstance)
        {
            this.allowedLayers = SWorldLayer.Foreground | SWorldLayer.Background;
        }
    }
}
