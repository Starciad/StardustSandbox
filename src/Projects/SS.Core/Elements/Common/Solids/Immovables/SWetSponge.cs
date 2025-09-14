using Microsoft.Xna.Framework;

using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Elements.Utilities;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Elements;

namespace StardustSandbox.Core.Elements.Common.Solids.Immovables
{
    internal sealed class SWetSponge : SImmovableSolid
    {
        internal SWetSponge(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = SColorPalette.Amber.Darken(0.25f);
            this.texture = gameInstance.AssetDatabase.GetTexture("texture_element_35");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 20;
            this.defaultDensity = 1200;
            this.defaultExplosionResistance = 0.8f;
        }

        protected override void OnStep()
        {
            foreach (Point belowPosition in SElementUtility.GetRandomSidePositions(this.Context.Slot.Position, SDirection.Down))
            {
                if (!this.Context.TryGetElement(belowPosition, this.Context.Layer, out ISElement element))
                {
                    return;
                }

                switch (element)
                {
                    case SDrySponge:
                        this.Context.SwappingElements(belowPosition, this.Context.Layer);
                        break;

                    default:
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 60)
            {
                this.Context.ReplaceElement(SElementConstants.DRY_SPONGE_IDENTIFIER);
            }
        }
    }
}
