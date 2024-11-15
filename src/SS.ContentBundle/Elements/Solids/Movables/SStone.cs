using StardustSandbox.ContentBundle.Elements.Liquids;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Game.Resources.Elements.Rendering;

namespace StardustSandbox.ContentBundle.Elements.Solids.Movables
{
    public sealed class SStone : SMovableSolid
    {
        public SStone(ISGame gameInstance) : base(gameInstance)
        {
            this.id = 003;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_4");
            this.rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
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
