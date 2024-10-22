using Microsoft.Xna.Framework;

using StardustSandbox.Game.Elements.Templates.Gases;
using StardustSandbox.Game.GameContent.Elements.Rendering;
using StardustSandbox.Game.GameContent.Elements.Utilities;
using StardustSandbox.Game.World.Data;

using System;

namespace StardustSandbox.Game.GameContent.Elements.Gases
{
    public sealed class SGCorruption : SGas
    {
        public SGCorruption(SGame gameInstance) : base(gameInstance)
        {
            this.Id = 015;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_16");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.EnableNeighborsAction = true;
        }

        protected override void OnNeighbors(ReadOnlySpan<(Point, SWorldSlot)> neighbors, int length)
        {
            this.Context.InfectNeighboringElements(neighbors, neighbors.Length);
        }
    }
}
