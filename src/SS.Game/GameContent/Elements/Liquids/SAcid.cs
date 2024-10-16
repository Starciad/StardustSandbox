using Microsoft.Xna.Framework;

using StardustSandbox.Game.Elements.Rendering.Common;
using StardustSandbox.Game.Elements.Templates.Liquids;
using StardustSandbox.Game.GameContent.Elements.Solids.Immovables;
using StardustSandbox.Game.World.Data;

using System;

namespace StardustSandbox.Game.GameContent.Elements.Liquids
{
    public class SAcid : SLiquid
    {
        public SAcid(SGame gameInstance) : base(gameInstance)
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
                if (this.Context.ElementDatabase.GetElementById(neighbor.Item2.Id) is SAcid ||
                    this.Context.ElementDatabase.GetElementById(neighbor.Item2.Id) is SWall)
                {
                    continue;
                }

                this.Context.DestroyElement();
                this.Context.DestroyElement(neighbor.Item1);
            }
        }
    }
}