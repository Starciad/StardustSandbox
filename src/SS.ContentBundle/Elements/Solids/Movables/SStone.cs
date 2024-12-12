using StardustSandbox.ContentBundle.Elements.Liquids;
using StardustSandbox.ContentBundle.Enums.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.Elements.Solids.Movables
{
    internal sealed class SStone : SMovableSolid
    {
        internal SStone(ISGame gameInstance) : base(gameInstance)
        {
            this.identifier = (uint)SElementId.Stone;
            this.referenceColor = new(66, 65, 65, 255);
            this.texture = gameInstance.AssetDatabase.GetTexture("element_4");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 20;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue > 600)
            {
                this.Context.ReplaceElement<SLava>();
            }
        }
    }
}
