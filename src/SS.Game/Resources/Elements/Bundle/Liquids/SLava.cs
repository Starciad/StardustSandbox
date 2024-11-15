using StardustSandbox.Game.Elements.Templates.Liquids;
using StardustSandbox.Game.Interfaces;
using StardustSandbox.Game.Resources.Elements.Bundle.Solids.Movables;
using StardustSandbox.Game.Resources.Elements.Rendering;

namespace StardustSandbox.Game.Resources.Elements.Bundle.Liquids
{
    public class SLava : SLiquid
    {
        public SLava(ISGame gameInstance) : base(gameInstance)
        {
            this.id = 009;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_10");
            this.rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 1000;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue <= 500)
            {
                this.Context.ReplaceElement<SStone>();
                this.Context.SetElementTemperature(500);
            }
        }
    }
}