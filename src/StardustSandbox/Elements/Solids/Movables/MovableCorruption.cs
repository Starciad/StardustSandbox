using StardustSandbox.Constants;
using StardustSandbox.Elements.Utilities;
using StardustSandbox.Enums.World;
using StardustSandbox.Randomness;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class MovableCorruption : MovableSolid
    {
        internal MovableCorruption() : base()
        {

        }

        protected override void OnNeighbors(ElementContext context, IEnumerable<Slot> neighbors)
        {
            if (CorruptionUtilities.CheckIfNeighboringElementsAreCorrupted(LayerType.Foreground, neighbors) &&
                CorruptionUtilities.CheckIfNeighboringElementsAreCorrupted(LayerType.Background, neighbors))
            {
                return;
            }

            context.NotifyChunk();

            if (SSRandom.Chance(ElementConstants.CHANCE_OF_CORRUPTION_TO_SPREAD))
            {
                context.InfectNeighboringElements(neighbors);
            }
        }
    }
}