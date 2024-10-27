using StardustSandbox.Game.Elements.Templates.Solids.Movables;
using StardustSandbox.Game.Interfaces;
using StardustSandbox.Game.Resources.Elements.Rendering;

namespace StardustSandbox.Game.Resources.Elements.Bundle.Solids.Movables
{
    public sealed class SMud : SMovableSolid
    {
        public SMud(ISGame gameInstance) : base(gameInstance)
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
