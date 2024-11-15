using Microsoft.Xna.Framework;

using StardustSandbox.ContentBundle.Elements.Solids.Immovables;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Liquids;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.World;

using System;

namespace StardustSandbox.ContentBundle.Elements.Liquids
{
    public class SAcid : SLiquid
    {
        public SAcid(ISGame gameInstance) : base(gameInstance)
        {
            this.id = 010;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_11");
            this.rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 10;
            this.enableNeighborsAction = true;
        }

        protected override void OnNeighbors(ReadOnlySpan<(Point, ISWorldSlot)> neighbors, int length)
        {
            for (int i = 0; i < length; i++)
            {
                (Point position, ISWorldSlot slot) = neighbors[i];

                if (slot.Element is SAcid ||
                    slot.Element is SWall)
                {
                    continue;
                }

                this.Context.DestroyElement();
                this.Context.DestroyElement(position);
            }
        }
    }
}