using StardustSandbox.ContentBundle.Elements.Energies;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.World.Data;

namespace StardustSandbox.ContentBundle.Elements.Solids.Immovables
{
    public sealed class SMountingBlock : SImmovableSolid
    {
        public SMountingBlock(ISGame gameInstance) : base(gameInstance)
        {
            this.id = 022;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_23");
            this.rendering.SetRenderingMechanism(new SElementSingleRenderingMechanism());
            this.defaultTemperature = 20;
            this.enableFlammability = true;
            this.defaultFlammabilityResistance = 150;
        }

        protected override void OnAwakeStep(SWorldSlot worldSlot)
        {
            worldSlot.SetColor(SElementConstants.COLORS_OF_MOUNTING_BLOCKS[SRandomMath.Range(0, SElementConstants.COLORS_OF_MOUNTING_BLOCKS.Length)]);
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue > 300)
            {
                this.Context.ReplaceElement<SFire>();
            }
        }
    }
}
