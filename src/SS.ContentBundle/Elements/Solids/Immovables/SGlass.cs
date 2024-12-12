using StardustSandbox.ContentBundle.Enums.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.Elements.Solids.Immovables
{
    internal sealed class SGlass : SImmovableSolid
    {
        internal SGlass(ISGame gameInstance) : base(gameInstance)
        {
            this.identifier = (uint)SElementId.Glass;
            this.referenceColor = new(249, 253, 254, 21);
            this.texture = gameInstance.AssetDatabase.GetTexture("element_12");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
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