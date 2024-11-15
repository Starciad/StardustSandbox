using StardustSandbox.ContentBundle.Elements.Solids.Immovables;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.Elements.Solids.Movables
{
    public sealed class SSand : SMovableSolid
    {
        public SSand(ISGame gameInstance) : base(gameInstance)
        {
            this.id = 006;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_7");
            this.rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 22;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 1800)
            {
                this.Context.ReplaceElement<SGlass>();
            }
        }
    }
}
