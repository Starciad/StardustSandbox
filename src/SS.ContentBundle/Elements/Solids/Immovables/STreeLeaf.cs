using StardustSandbox.ContentBundle.Elements.Energies;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.Elements.Solids.Immovables
{
    public sealed class STreeLeaf : SImmovableSolid
    {
        public STreeLeaf(ISGame gameInstance) : base(gameInstance)
        {
            this.id = 021;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_22");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 22;
            this.enableFlammability = true;
            this.defaultFlammabilityResistance = 5;
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
