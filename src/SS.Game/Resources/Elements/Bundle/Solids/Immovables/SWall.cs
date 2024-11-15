using StardustSandbox.Game.Elements.Templates.Solids.Immovables;
using StardustSandbox.Game.Interfaces;
using StardustSandbox.Game.Resources.Elements.Rendering;

namespace StardustSandbox.Game.Resources.Elements.Bundle.Solids.Immovables
{
    public sealed class SWall : SImmovableSolid
    {
        public SWall(ISGame gameInstance) : base(gameInstance)
        {
            this.id = 013;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_14");
            this.rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.enableTemperature = false;
        }
    }
}
