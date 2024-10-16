using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.Elements.Common.Solid.Movable
{
    public sealed class SSnow : SMovableSolid
    {
        public SSnow(SGame gameInstance) : base(gameInstance)
        {
            this.Id = 007;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_8");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = -5;
        }
    }
}
