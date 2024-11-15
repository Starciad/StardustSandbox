using StardustSandbox.Game.Elements.Templates.Solids.Movables;
using StardustSandbox.Game.Interfaces;
using StardustSandbox.Game.Resources.Elements.Bundle.Liquids;
using StardustSandbox.Game.Resources.Elements.Rendering;

namespace StardustSandbox.Game.Resources.Elements.Bundle.Solids.Movables
{
    public sealed class SSnow : SMovableSolid
    {
        public SSnow(ISGame gameInstance) : base(gameInstance)
        {
            this.id = 007;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_8");
            this.rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = -15;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 8)
            {
                this.Context.ReplaceElement<SWater>();
            }
        }
    }
}
