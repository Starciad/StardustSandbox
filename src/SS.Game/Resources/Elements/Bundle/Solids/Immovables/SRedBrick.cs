using StardustSandbox.Game.Elements.Templates.Solids.Immovables;
using StardustSandbox.Game.Interfaces;
using StardustSandbox.Game.Resources.Elements.Bundle.Liquids;
using StardustSandbox.Game.Resources.Elements.Rendering;

namespace StardustSandbox.Game.Resources.Elements.Bundle.Solids.Immovables
{
    public sealed class SRedBrick : SImmovableSolid
    {
        public SRedBrick(ISGame gameInstance) : base(gameInstance)
        {
            this.id = 020;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_21");
            this.rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 25;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 1727)
            {
                this.Context.ReplaceElement<SLava>();
            }
        }
    }
}
