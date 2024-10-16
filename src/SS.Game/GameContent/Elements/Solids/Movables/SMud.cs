using StardustSandbox.Game.Elements.Templates.Solids.Movables;
using StardustSandbox.Game.GameContent.Elements.Rendering;

namespace StardustSandbox.Game.GameContent.Elements.Solids.Movables
{
    public sealed class SMud : SMovableSolid
    {
        public SMud(SGame gameInstance) : base(gameInstance)
        {
            this.Id = 001;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_2");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 18;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 100)
            {
                this.Context.ReplaceElement<SDirt>();
            }
        }
    }
}
