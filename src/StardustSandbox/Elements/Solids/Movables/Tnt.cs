using StardustSandbox.Elements.Energies;
using StardustSandbox.Elements.Liquids;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Explosions;
using StardustSandbox.World;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Tnt : MovableSolid
    {
        private static readonly ExplosionBuilder explosionBuilder = new()
        {
            Radius = 6,
            Power = 5f,
            Heat = 450,

            AffectsWater = false,
            AffectsSolids = true,
            AffectsGases = true,

            ExplosionResidues =
            [
                new(ElementIndex.Fire, 65),
                new(ElementIndex.Smoke, 65)
            ]
        };

        protected override void OnDestroyed(in ElementContext context)
        {
            context.InstantiateExplosion(explosionBuilder);
        }

        protected override void OnNeighbors(in ElementContext context, IEnumerable<Slot> neighbors)
        {
            foreach (Slot neighbor in neighbors)
            {
                SlotLayer worldSlotLayer = neighbor.GetLayer(context.Layer);

                switch (worldSlotLayer.Element)
                {
                    case Fire:
                    case Lava:
                        context.DestroyElement();
                        break;

                    default:
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(in ElementContext context, double currentValue)
        {
            if (currentValue > 120)
            {
                context.DestroyElement();
            }
        }
    }
}
