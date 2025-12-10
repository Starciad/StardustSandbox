using StardustSandbox.Constants;
using StardustSandbox.Elements.Utilities;
using StardustSandbox.Enums.World;
using StardustSandbox.Randomness;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class ImmovableCorruption : ImmovableSolid
    {
        protected override void OnNeighbors(in ElementContext context, in ElementNeighbors neighbors)
        {
            if (CorruptionUtility.CheckIfNeighboringElementsAreCorrupted(Layer.Foreground, neighbors) &&
                CorruptionUtility.CheckIfNeighboringElementsAreCorrupted(Layer.Background, neighbors))
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
