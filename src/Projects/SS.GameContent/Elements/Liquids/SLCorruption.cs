using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Liquids;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Elements.Templates;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.World.Slots;
using StardustSandbox.GameContent.Elements.Utilities;

using System.Collections.Generic;

namespace StardustSandbox.GameContent.Elements.Liquids
{
    internal sealed class SLCorruption : SLiquid, ISCorruption
    {
        internal SLCorruption(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = SColorPalette.PurpleGray;
            this.texture = gameInstance.AssetDatabase.GetTexture("texture_element_17");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.enableNeighborsAction = true;
            this.defaultDensity = 1050;
            this.defaultExplosionResistance = 0.1f;
        }

        protected override void OnNeighbors(IEnumerable<SWorldSlot> neighbors)
        {
            if (SCorruptionUtilities.CheckIfNeighboringElementsAreCorrupted(SWorldLayer.Foreground, neighbors) &&
                SCorruptionUtilities.CheckIfNeighboringElementsAreCorrupted(SWorldLayer.Background, neighbors))
            {
                return;
            }

            this.Context.NotifyChunk();

            if (SRandomMath.Chance(SElementConstants.CHANCE_OF_CORRUPTION_TO_SPREAD))
            {
                this.Context.InfectNeighboringElements(neighbors);
            }
        }
    }
}
