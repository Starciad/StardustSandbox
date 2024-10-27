using StardustSandbox.Game.Elements.Templates.Solids.Movables;
using StardustSandbox.Game.Resources.Elements.Bundle.Solids.Immovables;
using StardustSandbox.Game.Resources.Elements.Rendering;

namespace StardustSandbox.Game.Resources.Elements.Bundle.Solids.Movables
{
    public sealed class SSand : SMovableSolid
    {
        public SSand(SGame gameInstance) : base(gameInstance)
        {
            this.Id = 006;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_7");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 22;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 1800)
            {
                this.Context.ReplaceElement<SGlass>();
            }
        }
    }
}
