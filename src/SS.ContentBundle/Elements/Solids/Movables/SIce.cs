using StardustSandbox.ContentBundle.Elements.Liquids;
using StardustSandbox.ContentBundle.Enums.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.Elements.Solids.Movables
{
    internal sealed class SIce : SMovableSolid
    {
        internal SIce(ISGame gameInstance) : base(gameInstance)
        {
            this.identifier = (uint)SElementId.Ice;
            this.referenceColor = new(117, 215, 246, 255);
            this.texture = this.SGameInstance.AssetDatabase.GetTexture("element_6");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = -25;
            this.defaultDensity = 920;
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
