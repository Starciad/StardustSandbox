using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Elements.Templates.Solids.Immovables;

namespace StardustSandbox.Game.GameContent.Elements.Solids.Immovables
{
    public sealed class SWood : SImmovableSolid
    {
        public SWood(SGame gameInstance) : base(gameInstance)
        {
            this.Id = 014;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_15");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 20;
        }
    }
}
