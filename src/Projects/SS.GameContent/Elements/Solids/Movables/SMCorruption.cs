using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Elements.Templates;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.World.Slots;

using System.Collections.Generic;

namespace StardustSandbox.GameContent.Elements.Solids.Movables
{
    internal sealed class SMCorruption : SMovableSolid, ISCorruption
    {
        internal SMCorruption(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = SColorPalette.PurpleGray;
            this.texture = gameInstance.AssetDatabase.GetTexture("texture_element_9");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.enableNeighborsAction = true;
            this.defaultDensity = 1400;
            this.defaultExplosionResistance = 0.8f;
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