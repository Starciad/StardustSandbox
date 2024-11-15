using StardustSandbox.Game.Elements.Templates.Solids.Immovables;
using StardustSandbox.Game.Interfaces;
using StardustSandbox.Game.Resources.Elements.Rendering;

namespace StardustSandbox.Game.Resources.Elements.Bundle.Solids.Immovables
{
    public sealed class SGlass : SImmovableSolid
    {
        public SGlass(ISGame gameInstance) : base(gameInstance)
        {
            this.id = 011;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_12");
            this.rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 25;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 620)
            {
                this.Context.DestroyElement();
            }
        }
    }
}