using StardustSandbox.ContentBundle.Elements.Energies;
using StardustSandbox.ContentBundle.Enums.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.Elements.Solids.Immovables
{
    public sealed class SWood : SImmovableSolid
    {
        public SWood(ISGame gameInstance) : base(gameInstance)
        {
            this.id = (uint)SElementId.Wood;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_15");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
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
