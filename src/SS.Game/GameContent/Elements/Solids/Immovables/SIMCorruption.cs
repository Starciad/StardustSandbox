using Microsoft.Xna.Framework;

using StardustSandbox.Game.Elements.Templates.Solids.Immovables;
using StardustSandbox.Game.GameContent.Elements.Rendering;
using StardustSandbox.Game.GameContent.Elements.Utilities;
using StardustSandbox.Game.World.Data;

using System;

namespace StardustSandbox.Game.GameContent.Elements.Solids.Immovables
{
    public class SIMCorruption : SImmovableSolid
    {
        public SIMCorruption(SGame gameInstance) : base(gameInstance)
        {
            this.Id = 017;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_18");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.EnableNeighborsAction = true;
        }

        protected override void OnNeighbors(ReadOnlySpan<(Point, SWorldSlot)> neighbors, int length)
        {
            this.Context.InfectNeighboringElements(neighbors, neighbors.Length);
        }
    }
}
