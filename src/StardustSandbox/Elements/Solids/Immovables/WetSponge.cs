using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Elements.Utilities;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class WetSponge : ImmovableSolid
    {
        internal WetSponge(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.renderingType = ElementRenderingType.Blob;
            this.characteristics = ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruptible;

            this.defaultTemperature = 20;
            this.defaultDensity = 1200;
            this.defaultExplosionResistance = 0.8f;
        }

        protected override void OnStep()
        {
            foreach (Point belowPosition in ElementUtility.GetRandomSidePositions(this.Context.Slot.Position, Direction.Down))
            {
                if (!this.Context.TryGetElement(belowPosition, this.Context.Layer, out Element element))
                {
                    return;
                }

                switch (element)
                {
                    case DrySponge:
                        this.Context.SwappingElements(this.Context.Position, belowPosition);
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
                this.Context.ReplaceElement(ElementIndex.DrySponge);
            }
        }
    }
}
