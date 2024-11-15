using StardustSandbox.ContentBundle.Elements.Energies;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Game.Resources.Elements.Rendering;

namespace StardustSandbox.ContentBundle.Elements.Solids.Movables
{
    public sealed class SGrass : SMovableSolid
    {
        public SGrass(ISGame gameInstance) : base(gameInstance)
        {
            this.id = 004;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_5");
            this.rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 22;
            this.enableFlammability = true;
            this.defaultFlammabilityResistance = 10;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue > 200)
            {
                this.Context.ReplaceElement<SFire>();
            }
        }
    }
}
