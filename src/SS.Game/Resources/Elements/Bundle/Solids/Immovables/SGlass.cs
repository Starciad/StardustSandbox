using StardustSandbox.Game.Elements.Templates.Solids.Immovables;
using StardustSandbox.Game.Resources.Elements.Rendering;

namespace StardustSandbox.Game.Resources.Elements.Bundle.Solids.Immovables
{
    public sealed class SGlass : SImmovableSolid
    {
        public SGlass(SGame gameInstance) : base(gameInstance)
        {
            this.Id = 011;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_12");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 25;
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