using StardustSandbox.Constants;
using StardustSandbox.Elements.Utilities;
using StardustSandbox.Enums.World;
using StardustSandbox.Randomness;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Gases
{
    internal sealed class GasCorruption : Gas
    {
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
