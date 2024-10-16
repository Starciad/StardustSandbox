using StardustSandbox.Game.Elements.Templates.Solids.Movables;
using StardustSandbox.Game.GameContent.Elements.Liquids;
using StardustSandbox.Game.GameContent.Elements.Rendering;

namespace StardustSandbox.Game.GameContent.Elements.Solids.Movables
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
