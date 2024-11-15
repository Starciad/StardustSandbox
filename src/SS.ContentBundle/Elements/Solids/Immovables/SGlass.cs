using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.Elements.Solids.Immovables
{
    public sealed class SGlass : SImmovableSolid
    {
        public SGlass(ISGame gameInstance) : base(gameInstance)
        {
            this.id = 011;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_12");
            this.rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 25;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 620)
            {
                this.Context.DestroyElement();
            }
        }
    }
}