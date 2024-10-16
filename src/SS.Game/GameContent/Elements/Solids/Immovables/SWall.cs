using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.Elements.Common.Solid.Immovable
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
