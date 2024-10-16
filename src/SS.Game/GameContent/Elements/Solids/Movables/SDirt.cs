using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Elements.Templates.Solids.Movables;

namespace StardustSandbox.Game.GameContent.Elements.Solids.Movables
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
