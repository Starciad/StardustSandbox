using StardustSandbox.Game.Elements.Templates.Solids.Movables;
using StardustSandbox.Game.GameContent.Elements.Rendering;

namespace StardustSandbox.Game.GameContent.Elements.Solids.Movables
{
    public sealed class SMountingBlock : SMovableSolid
    {
        public SMountingBlock(SGame gameInstance) : base(gameInstance)
        {
            this.Id = 022;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_23");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 20;
        }
    }
}
