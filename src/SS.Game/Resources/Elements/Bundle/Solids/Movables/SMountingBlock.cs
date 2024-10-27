using StardustSandbox.Game.Constants.Elements;
using StardustSandbox.Game.Elements.Templates.Solids.Movables;
using StardustSandbox.Game.Mathematics;
using StardustSandbox.Game.Resources.Elements.Rendering;
using StardustSandbox.Game.World.Data;

namespace StardustSandbox.Game.Resources.Elements.Bundle.Solids.Movables
{
    public sealed class SMountingBlock : SMovableSolid
    {
        public SMountingBlock(SGame gameInstance) : base(gameInstance)
        {
            this.Id = 022;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_23");
            this.Rendering.SetRenderingMechanism(new SElementSingleRenderingMechanism());
            this.DefaultTemperature = 20;
        }

        protected override void OnAwakeStep(SWorldSlot worldSlot)
        {
            worldSlot.SetColor(SElementConstants.COLORS_OF_MOUNTING_BLOCKS[SRandomMath.Range(0, SElementConstants.COLORS_OF_MOUNTING_BLOCKS.Length)]);
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue > 300)
            {
                this.Context.DestroyElement();
            }
        }
    }
}
