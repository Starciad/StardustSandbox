using Microsoft.Xna.Framework;

using StardustSandbox.ContentBundle.Elements.Utilities;
using StardustSandbox.ContentBundle.Enums.Elements;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Gases;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Interfaces.Elements.Templates;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.Mathematics;

using System;

namespace StardustSandbox.ContentBundle.Elements.Gases
{
    public sealed class SGCorruption : SGas, ISCorruption
    {
        public SGCorruption(ISGame gameInstance) : base(gameInstance)
        {
            this.id = (uint)SElementId.GCorruption;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_16");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.enableNeighborsAction = true;
            this.movementType = SGasMovementType.Spread;
        }

        protected override void OnNeighbors(ReadOnlySpan<(Point, ISWorldSlot)> neighbors, int length)
        {
            if (this.Context.CheckIfNeighboringElementsAreCorrupted(neighbors, neighbors.Length))
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
