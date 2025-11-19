using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Constants;
using StardustSandbox.Elements.Rendering;
using StardustSandbox.Elements.Utilities;
using StardustSandbox.Enums.Indexers;
using StardustSandbox.Enums.World;
using StardustSandbox.Interfaces.Elements;
using StardustSandbox.Randomness;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class IMCorruption : ImmovableSolid, ICorruptionElement
    {
        internal IMCorruption(Color referenceColor, ElementIndex index, Texture2D texture) : base(referenceColor, index, texture)
        {
            this.Rendering.SetRenderingMechanism(new ElementBlobRenderingMechanism());
            this.enableNeighborsAction = true;
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
