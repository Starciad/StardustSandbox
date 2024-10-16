using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Elements.Templates.Solids.Immovables;

namespace StardustSandbox.Game.GameContent.Elements.Solids.Immovables
{
    public sealed class SMetal : SImmovableSolid
    {
        public SMetal(SGame gameInstance) : base(gameInstance)
        {
            this.Id = 012;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_13");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 30;
        }
    }
}
