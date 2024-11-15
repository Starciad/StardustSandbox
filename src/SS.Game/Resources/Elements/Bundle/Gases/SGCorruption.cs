using Microsoft.Xna.Framework;

using StardustSandbox.Game.Constants.Elements;
using StardustSandbox.Game.Elements.Templates.Gases;
using StardustSandbox.Game.Interfaces;
using StardustSandbox.Game.Interfaces.Elements.Templates;
using StardustSandbox.Game.Interfaces.World;
using StardustSandbox.Game.Mathematics;
using StardustSandbox.Game.Resources.Elements.Rendering;
using StardustSandbox.Game.Resources.Elements.Utilities;

using System;

namespace StardustSandbox.Game.Resources.Elements.Bundle.Gases
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
