using StardustSandbox.ContentBundle.Enums.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.ContentBundle.Elements.Solids.Immovables
{
    internal sealed class SWall : SImmovableSolid
    {
        internal SWall(ISGame gameInstance) : base(gameInstance)
        {
            this.identifier = (uint)SElementId.Wall;
            this.referenceColor = new(22, 99, 50, 255);
            this.texture = gameInstance.AssetDatabase.GetTexture("element_14");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.enableTemperature = false;
            this.defaultDensity = 2200;
        }
    }
}
