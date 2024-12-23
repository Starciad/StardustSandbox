using StardustSandbox.ContentBundle.Elements.Utilities;
using StardustSandbox.ContentBundle.Enums.Elements;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces.Elements.Templates;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.World.Data;

namespace StardustSandbox.ContentBundle.Elements.Solids.Immovables
{
    internal sealed class SIMCorruption : SImmovableSolid, ISCorruption
    {
        internal SIMCorruption(ISGame gameInstance) : base(gameInstance)
        {
            this.identifier = (uint)SElementId.IMCorruption;
            this.referenceColor = SColorPalette.PurpleGray;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_18");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.enableNeighborsAction = true;
            this.defaultDensity = 1600;
        }

        protected override void OnNeighbors(SWorldSlot[] neighbors)
        {
            if (SCorruptionUtilities.CheckIfNeighboringElementsAreCorrupted(SWorldLayer.Foreground, neighbors, neighbors.Length) &&
                SCorruptionUtilities.CheckIfNeighboringElementsAreCorrupted(SWorldLayer.Background, neighbors, neighbors.Length))
            {
                return;
            }

            this.Context.NotifyChunk();

            if (SRandomMath.Chance(SElementConstants.CHANCE_OF_CORRUPTION_TO_SPREAD, SElementConstants.CHANCE_OF_CORRUPTION_TO_SPREAD_TOTAL))
            {
                this.Context.InfectNeighboringElements(neighbors, neighbors.Length);
            }
        }
    }
}
