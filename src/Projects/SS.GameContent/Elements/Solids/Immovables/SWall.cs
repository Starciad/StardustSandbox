using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.ContentBundle.Elements.Solids.Immovables
{
    internal sealed class SWall : SImmovableSolid
    {
        internal SWall(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = new(22, 99, 50, 255);
            this.texture = gameInstance.AssetDatabase.GetTexture("texture_element_14");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.enableTemperature = false;
            this.isExplosionImmune = true;
            this.defaultDensity = 2200;
        }
    }
}
