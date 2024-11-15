using StardustSandbox.Game.Constants.Elements;
using StardustSandbox.Game.Elements.Templates.Solids.Immovables;
using StardustSandbox.Game.Interfaces;
using StardustSandbox.Game.Mathematics;
using StardustSandbox.Game.Resources.Elements.Bundle.Energies;
using StardustSandbox.Game.Resources.Elements.Rendering;
using StardustSandbox.Game.World.Data;

namespace StardustSandbox.Game.Resources.Elements.Bundle.Solids.Immovables
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
