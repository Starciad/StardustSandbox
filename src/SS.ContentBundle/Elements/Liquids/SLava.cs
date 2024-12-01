using StardustSandbox.ContentBundle.Elements.Solids.Movables;
using StardustSandbox.ContentBundle.Enums.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Liquids;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.Elements.Liquids
{
    public class SLava : SLiquid
    {
        public SLava(ISGame gameInstance) : base(gameInstance)
        {
            this.id = (uint)SElementId.Lava;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_10");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
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