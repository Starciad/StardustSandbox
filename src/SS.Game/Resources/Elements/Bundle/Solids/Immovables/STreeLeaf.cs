using StardustSandbox.Game.Elements.Templates.Solids.Immovables;
using StardustSandbox.Game.Interfaces;
using StardustSandbox.Game.Resources.Elements.Bundle.Energies;
using StardustSandbox.Game.Resources.Elements.Rendering;

namespace StardustSandbox.Game.Resources.Elements.Bundle.Solids.Immovables
{
    public sealed class STreeLeaf : SImmovableSolid
    {
        public STreeLeaf(ISGame gameInstance) : base(gameInstance)
        {
            this.Id = 021;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_22");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 22;
            this.EnableFlammability = true;
            this.DefaultFlammabilityResistance = 5;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 250)
            {
                this.Context.ReplaceElement<SFire>();
            }
        }
    }
}
