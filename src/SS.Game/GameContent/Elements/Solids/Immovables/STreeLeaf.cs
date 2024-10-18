using StardustSandbox.Game.Elements.Templates.Solids.Immovables;
using StardustSandbox.Game.GameContent.Elements.Rendering;

namespace StardustSandbox.Game.GameContent.Elements.Solids.Immovables
{
    public sealed class STreeLeaf : SImmovableSolid
    {
        public STreeLeaf(SGame gameInstance) : base(gameInstance)
        {
            this.Id = 021;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_22");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 22;
        }
    }
}
