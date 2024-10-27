using StardustSandbox.Game.Elements.Templates.Solids.Immovables;
using StardustSandbox.Game.Resources.Elements.Rendering;

namespace StardustSandbox.Game.Resources.Elements.Bundle.Solids.Immovables
{
    public sealed class SWall : SImmovableSolid
    {
        public SWall(SGame gameInstance) : base(gameInstance)
        {
            this.Id = 013;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_14");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.EnableTemperature = false;
        }
    }
}
