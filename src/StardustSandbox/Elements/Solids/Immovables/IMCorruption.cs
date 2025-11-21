using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Constants;
using StardustSandbox.Elements.Utilities;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.World;
using StardustSandbox.Randomness;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class IMCorruption : ImmovableSolid
    {
        internal IMCorruption(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.renderingType = ElementRenderingType.Blob;
            this.characteristics = ElementCharacteristics.AffectsNeighbors | ElementCharacteristics.HasTemperature | ElementCharacteristics.IsCorruption;

            this.defaultDensity = 1600;
            this.defaultExplosionResistance = 1.2f;
        }

        protected override void OnNeighbors(IEnumerable<Slot> neighbors)
        {
            if (CorruptionUtilities.CheckIfNeighboringElementsAreCorrupted(LayerType.Foreground, neighbors) &&
                CorruptionUtilities.CheckIfNeighboringElementsAreCorrupted(LayerType.Background, neighbors))
            {
                return;
            }

            this.Context.NotifyChunk();

            if (SSRandom.Chance(ElementConstants.CHANCE_OF_CORRUPTION_TO_SPREAD))
            {
                this.Context.InfectNeighboringElements(neighbors);
            }
        }
    }
}
