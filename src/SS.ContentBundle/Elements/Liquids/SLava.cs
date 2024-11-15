using StardustSandbox.ContentBundle.Elements.Solids.Movables;
using StardustSandbox.Core.Elements.Templates.Liquids;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Game.Resources.Elements.Rendering;

namespace StardustSandbox.ContentBundle.Elements.Liquids
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