using StardustSandbox.Game.Elements.Templates.Solids.Movables;
using StardustSandbox.Game.Interfaces;
using StardustSandbox.Game.Resources.Elements.Bundle.Liquids;
using StardustSandbox.Game.Resources.Elements.Rendering;

namespace StardustSandbox.Game.Resources.Elements.Bundle.Solids.Movables
{
    public sealed class SStone : SMovableSolid
    {
        public SStone(ISGame gameInstance) : base(gameInstance)
        {
            this.Id = 003;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_4");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 20;
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
