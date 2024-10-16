using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Elements.Templates.Solids.Immovables;

namespace StardustSandbox.Game.GameContent.Elements.Solids.Immovables
{
    public sealed class SGlass : SImmovableSolid
    {
        public SGlass(SGame gameInstance) : base(gameInstance)
        {
            this.Id = 011;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_12");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 25;
        }
    }
}