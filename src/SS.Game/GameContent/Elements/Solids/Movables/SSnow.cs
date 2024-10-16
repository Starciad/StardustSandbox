using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Elements.Templates.Solids.Movables;

namespace StardustSandbox.Game.GameContent.Elements.Solids.Movables
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
