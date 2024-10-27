using StardustSandbox.Game.Elements.Templates.Liquids;
using StardustSandbox.Game.Resources.Elements.Bundle.Solids.Movables;
using StardustSandbox.Game.Resources.Elements.Rendering;

namespace StardustSandbox.Game.Resources.Elements.Bundle.Liquids
{
    public class SLava : SLiquid
    {
        public SLava(SGame gameInstance) : base(gameInstance)
        {
            this.Id = 009;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_10");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 1000;
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