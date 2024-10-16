using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.Elements.Common.Solid.Immovable
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