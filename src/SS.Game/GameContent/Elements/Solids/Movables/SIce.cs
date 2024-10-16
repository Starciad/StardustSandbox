using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.Elements.Common.Solid.Movable
{
    public sealed class SIce : SMovableSolid
    {
        public SIce(SGame gameInstance) : base(gameInstance)
        {
            this.Id = 005;
            this.Texture = this.SGameInstance.AssetDatabase.GetTexture("element_6");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 0;
        }
    }
}
