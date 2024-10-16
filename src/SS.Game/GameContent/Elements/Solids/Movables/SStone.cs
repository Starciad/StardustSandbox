using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements.Common.Liquid;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.Elements.Common.Solid.Movable
{
    public sealed class SStone : SMovableSolid
    {
        public SStone(SGame gameInstance) : base(gameInstance)
        {
            this.Id = 003;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_4");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 20;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue > 500)
            {
                this.Context.ReplaceElement<SLava>();
                this.Context.SetElementTemperature(600);
            }
        }
    }
}
