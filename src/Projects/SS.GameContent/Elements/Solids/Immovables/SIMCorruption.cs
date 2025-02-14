using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Elements.Templates;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.World.Slots;
using StardustSandbox.GameContent.Elements.Utilities;

using System.Collections.Generic;

namespace StardustSandbox.GameContent.Elements.Solids.Immovables
{
    internal sealed class SIMCorruption : SImmovableSolid, ISCorruption
    {
        internal SIMCorruption(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = SColorPalette.PurpleGray;
            this.texture = gameInstance.AssetDatabase.GetTexture("texture_element_18");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.enableNeighborsAction = true;
            this.defaultDensity = 1600;
            this.defaultExplosionResistance = 1.2f;
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
