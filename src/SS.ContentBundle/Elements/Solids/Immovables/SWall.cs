using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.Elements.Solids.Immovables
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
