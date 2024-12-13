using StardustSandbox.ContentBundle.Elements.Utilities;
using StardustSandbox.ContentBundle.Enums.Elements;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Interfaces.Elements.Templates;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.Mathematics;

using System;

namespace StardustSandbox.ContentBundle.Elements.Solids.Movables
{
    internal sealed class SMCorruption : SMovableSolid, ISCorruption
    {
        internal SMCorruption(ISGame gameInstance) : base(gameInstance)
        {
            this.identifier = (uint)SElementId.MCorruption;
            this.referenceColor = SColorPalette.PurpleGray;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_9");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.enableNeighborsAction = true;
        }

        protected override void OnNeighbors(ReadOnlySpan<ISWorldSlot> neighbors)
        {
            if (SCorruptionUtilities.CheckIfNeighboringElementsAreCorrupted(neighbors, neighbors.Length))
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