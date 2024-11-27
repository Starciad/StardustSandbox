using StardustSandbox.ContentBundle.Elements.Energies;
using StardustSandbox.Core.Animations;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.World.Data;

namespace StardustSandbox.ContentBundle.Elements.Solids.Immovables
{
    public sealed class SMountingBlock : SImmovableSolid
    {
        public SMountingBlock(ISGame gameInstance) : base(gameInstance)
        {
            this.id = 022;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_23");
            this.Rendering.SetRenderingMechanism(new SElementSingleRenderingMechanism(new SAnimation(gameInstance, [new(new(new(0), new(SSpritesConstants.SPRITE_SCALE)), 0)])));
            this.defaultTemperature = 20;
            this.enableFlammability = true;
            this.defaultFlammabilityResistance = 150;
        }

        protected override void OnInstantiateStep(SWorldSlot worldSlot)
        {
            worldSlot.SetColor(SElementConstants.COLORS_OF_MOUNTING_BLOCKS.GetRandomItem());
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
