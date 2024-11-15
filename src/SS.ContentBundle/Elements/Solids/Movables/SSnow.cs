using StardustSandbox.ContentBundle.Elements.Liquids;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Game.Resources.Elements.Rendering;

namespace StardustSandbox.ContentBundle.Elements.Solids.Movables
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
