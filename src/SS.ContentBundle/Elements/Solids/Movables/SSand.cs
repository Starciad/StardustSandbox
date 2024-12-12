using StardustSandbox.ContentBundle.Elements.Solids.Immovables;
using StardustSandbox.ContentBundle.Enums.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.Elements.Solids.Movables
{
    internal sealed class SSand : SMovableSolid
    {
        internal SSand(ISGame gameInstance) : base(gameInstance)
        {
            this.identifier = (uint)SElementId.Sand;
            this.referenceColor = new(248, 246, 68, 255);
            this.texture = gameInstance.AssetDatabase.GetTexture("element_7");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 22;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 1800)
            {
                this.Context.ReplaceElement<SGlass>();
            }
        }
    }
}
