using StardustSandbox.ContentBundle.Enums.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.Elements.Solids.Immovables
{
    public sealed class SWall : SImmovableSolid
    {
        public SWall(ISGame gameInstance) : base(gameInstance)
        {
            this.id = (uint)SElementId.Wall;
            this.referenceColor = new(22, 99, 50, 255);
            this.texture = gameInstance.AssetDatabase.GetTexture("element_14");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.enableTemperature = false;
        }
    }
}
