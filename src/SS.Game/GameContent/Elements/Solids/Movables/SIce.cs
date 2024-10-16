using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Elements.Templates.Solids.Movables;

namespace StardustSandbox.Game.GameContent.Elements.Solids.Movables
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
