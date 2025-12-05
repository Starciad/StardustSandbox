using StardustSandbox.Constants;
using StardustSandbox.Elements.Utilities;
using StardustSandbox.Enums.World;
using StardustSandbox.Randomness;
using StardustSandbox.World;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Gases
{
    internal sealed class GasCorruption : Gas
    {
        protected override void OnNeighbors(in ElementContext context, IEnumerable<Slot> neighbors)
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
