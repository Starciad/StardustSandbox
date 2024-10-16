using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.Elements.Common.Solid.Movable
{
    public sealed class SDirt : SMovableSolid
    {
        public SDirt(SGame gameInstance) : base(gameInstance)
        {
            this.Id = 000;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_1");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 20;
            this.EnableNeighborsAction = true;
        }
    }
}
