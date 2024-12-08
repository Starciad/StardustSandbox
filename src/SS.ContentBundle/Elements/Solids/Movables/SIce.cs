using StardustSandbox.ContentBundle.Elements.Liquids;
using StardustSandbox.ContentBundle.Enums.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.Elements.Solids.Movables
{
    public sealed class SIce : SMovableSolid
    {
        public SIce(ISGame gameInstance) : base(gameInstance)
        {
            this.id = (uint)SElementId.Ice;
            this.texture = this.SGameInstance.AssetDatabase.GetTexture("element_6");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
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
