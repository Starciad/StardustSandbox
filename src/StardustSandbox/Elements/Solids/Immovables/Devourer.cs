using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Elements;
using StardustSandbox.Explosions;
using StardustSandbox.Extensions;
using StardustSandbox.Randomness;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class Devourer : ImmovableSolid
    {
        private static readonly ExplosionBuilder explosionBuilder = new()
        {
            Radius = 4,
            Power = 2.5f,
            Heat = 180,

            AffectsWater = false,
            AffectsSolids = true,
            AffectsGases = true,

            ExplosionResidues =
            [
                ElementIndex.Fire,
                ElementIndex.Smoke,
            ]
        };

        private static readonly List<Slot> cachedNeighborSlots = [];

        protected override void OnDestroyed(in ElementContext context)
        {
            context.InstantiateExplosion(explosionBuilder);
        }

        protected override void OnNeighbors(in ElementContext context, in ElementNeighbors neighbors)
        {
            cachedNeighborSlots.Clear();

            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.HasNeighbor(i))
                {
                    continue;
                }

                Slot slot = neighbors.GetSlot(i);
                SlotLayer slotLayer = slot.GetLayer(context.Layer);

                if (!slotLayer.HasState(ElementStates.IsEmpty) &&
                    slotLayer.Element.Index != ElementIndex.Devourer &&
                    slotLayer.Element.Index != ElementIndex.Void &&
                    slotLayer.Element.Index != ElementIndex.Clone &&
                    slotLayer.Element.Index != ElementIndex.Wall
                )
                {
                    cachedNeighborSlots.Add(slot);
                }
            }

            if (cachedNeighborSlots.Count > 0)
            {
                Slot neighborSlot = cachedNeighborSlots.GetRandomItem();

                Point oldPosition = context.Slot.Position;
                Point newPosition = neighborSlot.Position;

                context.SwappingElements(oldPosition, newPosition, context.Layer);
                context.RemoveElement(oldPosition);
            }
            else if (SSRandom.Chance(15))
            {
                context.DestroyElement();
            }
            else
            {
                context.NotifyChunk();
            }
        }
    }
}
