using Microsoft.Xna.Framework;

using StardustSandbox.Game.Elements.Templates.Liquids;
using StardustSandbox.Game.Interfaces;
using StardustSandbox.Game.Resources.Elements.Bundle.Solids.Immovables;
using StardustSandbox.Game.Resources.Elements.Rendering;
using StardustSandbox.Game.World.Data;

using System;

namespace StardustSandbox.Game.Resources.Elements.Bundle.Liquids
{
    public class SAcid : SLiquid
    {
        public SAcid(ISGame gameInstance) : base(gameInstance)
        {
            this.Id = 010;
            this.Texture = gameInstance.AssetDatabase.GetTexture("element_11");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.DefaultTemperature = 10;
            this.EnableNeighborsAction = true;
        }

        protected override void OnNeighbors(ReadOnlySpan<(Point, SWorldSlot)> neighbors, int length)
        {
            foreach ((Point, SWorldSlot) neighbor in neighbors)
            {
                if (neighbor.Item2.Element is SAcid ||
                    neighbor.Item2.Element is SWall)
                {
                    continue;
                }

                this.Context.DestroyElement();
                this.Context.DestroyElement(neighbor.Item1);
            }
        }
    }
}