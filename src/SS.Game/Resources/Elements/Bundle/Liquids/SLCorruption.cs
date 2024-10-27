using Microsoft.Xna.Framework;

using StardustSandbox.Game.Constants.Elements;
using StardustSandbox.Game.Elements.Templates.Liquids;
using StardustSandbox.Game.Interfaces.Elements.Templates;
using StardustSandbox.Game.Mathematics;
using StardustSandbox.Game.Resources.Elements.Rendering;
using StardustSandbox.Game.Resources.Elements.Utilities;
using StardustSandbox.Game.World.Data;

using System;

namespace StardustSandbox.Game.Resources.Elements.Bundle.Liquids
{
    public class SLCorruption : SLiquid, ISCorruption
    {
        public SLCorruption(SGame gameInstance) : base(gameInstance)
        {
            this.Id = 016;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_17");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.EnableNeighborsAction = true;
        }

        protected override void OnNeighbors(ReadOnlySpan<(Point, SWorldSlot)> neighbors, int length)
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
