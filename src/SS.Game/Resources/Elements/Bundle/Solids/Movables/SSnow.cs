using StardustSandbox.Game.Elements.Templates.Solids.Movables;
using StardustSandbox.Game.Resources.Elements.Bundle.Liquids;
using StardustSandbox.Game.Resources.Elements.Rendering;

namespace StardustSandbox.Game.Resources.Elements.Bundle.Solids.Movables
{
    public sealed class SSnow : SMovableSolid
    {
        public SSnow(SGame gameInstance) : base(gameInstance)
        {
            this.Id = 007;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_8");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = -15;
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
