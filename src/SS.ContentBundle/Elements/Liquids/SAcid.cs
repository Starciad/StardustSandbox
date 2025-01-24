using StardustSandbox.ContentBundle.Elements.Solids.Immovables;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Liquids;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.World.Slots;

using System.Collections.Generic;

namespace StardustSandbox.ContentBundle.Elements.Liquids
{
    internal sealed class SAcid : SLiquid
    {
        internal SAcid(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = new(059, 167, 005, 255);
            this.texture = gameInstance.AssetDatabase.GetTexture("element_11");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 10;
            this.enableNeighborsAction = true;
            this.defaultDensity = 1100;
        }

        protected override void OnNeighbors(IEnumerable<SWorldSlot> neighbors)
        {
            foreach (SWorldSlot neighbor in neighbors)
            {
                SWorldSlotLayer slotLayer = neighbor.GetLayer(this.Context.Layer);

                if (slotLayer.IsEmpty)
                {
                    continue;
                }

                switch (slotLayer.Element)
                {
                    case SAcid:
                    case SWall:
                    case SClone:
                    case SVoid:
                        continue;

                    default:
                        break;
                }

                this.Context.DestroyElement();
                this.Context.DestroyElement(neighbor.Position, this.Context.Layer);
            }
        }
    }
}