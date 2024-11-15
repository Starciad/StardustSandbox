using StardustSandbox.ContentBundle.Elements.Energies;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Game.Resources.Elements.Rendering;

namespace StardustSandbox.ContentBundle.Elements.Solids.Immovables
{
    public sealed class SWood : SImmovableSolid
    {
        public SWood(ISGame gameInstance) : base(gameInstance)
        {
            this.id = 014;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_15");
            this.rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 20;
            this.enableFlammability = true;
            this.defaultFlammabilityResistance = 35;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 300)
            {
                this.Context.ReplaceElement<SFire>();
            }
        }
    }
}
