using Microsoft.Xna.Framework;

using StardustSandbox.Game.Elements.Templates.Liquids;
using StardustSandbox.Game.GameContent.Elements.Rendering;
using StardustSandbox.Game.GameContent.Elements.Utilities;
using StardustSandbox.Game.World.Data;

using System;

namespace StardustSandbox.Game.GameContent.Elements.Liquids
{
    public class SLCorruption : SLiquid
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
            this.Context.InfectNeighboringElements(this.SGameInstance,neighbors, neighbors.Length);
        }
    }
}
