using StardustSandbox.ContentBundle.Enums.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.Elements.Solids.Movables
{
    public sealed class SMud : SMovableSolid
    {
        public SMud(ISGame gameInstance) : base(gameInstance)
        {
            this.identifier = (uint)SElementId.Mud;
            this.referenceColor = new(75, 36, 38, 255);
            this.texture = gameInstance.AssetDatabase.GetTexture("element_2");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 18;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 100)
            {
                this.Context.ReplaceElement<SDirt>();
            }
        }
    }
}
