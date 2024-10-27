using StardustSandbox.Game.Elements.Templates.Solids.Immovables;
using StardustSandbox.Game.Resources.Elements.Rendering;

namespace StardustSandbox.Game.Resources.Elements.Bundle.Solids.Immovables
{
    public sealed class SWood : SImmovableSolid
    {
        public SWood(SGame gameInstance) : base(gameInstance)
        {
            this.Id = 014;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_15");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 20;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 300)
            {
                this.Context.DestroyElement();
            }
        }
    }
}
