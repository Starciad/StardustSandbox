using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Elements.Templates.Solids.Movables;

namespace StardustSandbox.Game.GameContent.Elements.Solids.Movables
{
    public sealed class SSand : SMovableSolid
    {
        public SSand(SGame gameInstance) : base(gameInstance)
        {
            this.Id = 006;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_7");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 22;
        }
    }
}
