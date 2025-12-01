using StardustSandbox.Elements.Energies;
using StardustSandbox.Elements.Liquids;
using StardustSandbox.Enums.Elements;
using StardustSandbox.ExplosionSystem;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Dynamite : MovableSolid
    {
        private static readonly ExplosionBuilder explosionBuilder = new()
        {
            Radius = 10,
            Power = 5f,
            Heat = 850,

            AffectsWater = true,
            AffectsSolids = true,
            AffectsGases = true,

            ExplosionResidues =
            [
                new(ElementIndex.Fire, 80),
                new(ElementIndex.Smoke, 90),
                new(ElementIndex.Stone, 90)
            ]
        };

        protected override void OnDestroyed(ElementContext context)
        {
            context.InstantiateExplosion(explosionBuilder);
        }

        protected override void OnNeighbors(ElementContext context, IEnumerable<Slot> neighbors)
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

        protected override void OnTemperatureChanged(ElementContext context, double currentValue)
        {
            if (currentValue > 150)
            {
                context.DestroyElement();
            }
        }
    }
}
