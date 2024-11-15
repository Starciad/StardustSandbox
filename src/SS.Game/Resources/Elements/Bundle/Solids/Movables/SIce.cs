using StardustSandbox.Game.Elements.Templates.Solids.Movables;
using StardustSandbox.Game.Interfaces;
using StardustSandbox.Game.Resources.Elements.Bundle.Liquids;
using StardustSandbox.Game.Resources.Elements.Rendering;

namespace StardustSandbox.Game.Resources.Elements.Bundle.Solids.Movables
{
    public sealed class SIce : SMovableSolid
    {
        public SIce(ISGame gameInstance) : base(gameInstance)
        {
            this.id = 005;
            this.texture = this.SGameInstance.AssetDatabase.GetTexture("element_6");
            this.rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = -25;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 0)
            {
                this.Context.ReplaceElement<SWater>();
            }
        }
    }
}
