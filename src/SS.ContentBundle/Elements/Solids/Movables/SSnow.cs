using StardustSandbox.ContentBundle.Elements.Liquids;
using StardustSandbox.ContentBundle.Enums.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.Elements.Solids.Movables
{
    public sealed class SSnow : SMovableSolid
    {
        public SSnow(ISGame gameInstance) : base(gameInstance)
        {
            this.identifier = (uint)SElementId.Snow;
            this.referenceColor = new(202, 242, 239, 255);
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
