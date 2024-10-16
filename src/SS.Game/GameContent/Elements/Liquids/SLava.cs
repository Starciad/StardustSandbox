using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements.Common.Solid.Movable;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.Elements.Common.Liquid
{
    public class SLava : SLiquid
    {
        public SLava(SGame gameInstance) : base(gameInstance)
        {
            this.Id = 009;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_10");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 1000;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue < 500)
            {
                this.Context.ReplaceElement<SStone>();
                this.Context.SetElementTemperature(550);
            }
        }
    }
}