using StardustSandbox.ContentBundle.Elements.Liquids;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Game.Resources.Elements.Rendering;

namespace StardustSandbox.ContentBundle.Elements.Solids.Movables
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
