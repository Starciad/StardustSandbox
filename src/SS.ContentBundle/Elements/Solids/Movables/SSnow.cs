using StardustSandbox.ContentBundle.Elements.Liquids;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.Elements.Solids.Movables
{
    public sealed class SSnow : SMovableSolid
    {
        public SSnow(ISGame gameInstance) : base(gameInstance)
        {
            this.id = 007;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_8");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
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
