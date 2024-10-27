using Microsoft.Xna.Framework;

using StardustSandbox.Game.Constants.Elements;
using StardustSandbox.Game.Elements.Templates.Solids.Movables;
using StardustSandbox.Game.Interfaces.Elements.Templates;
using StardustSandbox.Game.Mathematics;
using StardustSandbox.Game.Resources.Elements.Rendering;
using StardustSandbox.Game.Resources.Elements.Utilities;
using StardustSandbox.Game.World.Data;

using System;

namespace StardustSandbox.Game.Resources.Elements.Bundle.Solids.Movables
{
    public sealed class SMCorruption : SMovableSolid, ISCorruption
    {
        public SMCorruption(SGame gameInstance) : base(gameInstance)
        {
            this.Id = 008;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_9");
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