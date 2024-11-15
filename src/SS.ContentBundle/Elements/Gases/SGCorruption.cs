using Microsoft.Xna.Framework;

using StardustSandbox.ContentBundle.Elements.Utilities;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Templates.Gases;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Elements.Templates;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Game.Resources.Elements.Rendering;

using System;

namespace StardustSandbox.ContentBundle.Elements.Gases
{
    public sealed class SGCorruption : SGas, ISCorruption
    {
        public SGCorruption(ISGame gameInstance) : base(gameInstance)
        {
            this.id = 015;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_16");
            this.rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.enableNeighborsAction = true;
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
