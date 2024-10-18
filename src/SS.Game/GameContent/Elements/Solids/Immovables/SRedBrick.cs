using StardustSandbox.Game.Elements.Templates.Solids.Immovables;
using StardustSandbox.Game.GameContent.Elements.Liquids;
using StardustSandbox.Game.GameContent.Elements.Rendering;

namespace StardustSandbox.Game.GameContent.Elements.Solids.Immovables
{
    public sealed class SRedBrick : SImmovableSolid
    {
        public SRedBrick(SGame gameInstance) : base(gameInstance)
        {
            this.Id = 020;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_21");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 25;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 1727)
            {
                this.Context.ReplaceElement<SLava>();
            }
        }
    }
}
