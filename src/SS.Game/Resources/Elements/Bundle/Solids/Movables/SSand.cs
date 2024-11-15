using StardustSandbox.Game.Elements.Templates.Solids.Movables;
using StardustSandbox.Game.Interfaces;
using StardustSandbox.Game.Resources.Elements.Bundle.Solids.Immovables;
using StardustSandbox.Game.Resources.Elements.Rendering;

namespace StardustSandbox.Game.Resources.Elements.Bundle.Solids.Movables
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
