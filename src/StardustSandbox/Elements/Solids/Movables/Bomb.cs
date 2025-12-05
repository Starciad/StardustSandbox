using StardustSandbox.Elements.Solids.Immovables;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Explosions;
using StardustSandbox.World;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Bomb : MovableSolid
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
                new(ElementIndex.Fire, 65),
                new(ElementIndex.Smoke, 70)
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
                    case Bomb:
                    case Wall:
                    case Clone:
                    case Void:
                        break;

                    default:
                        context.DestroyElement();
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(in ElementContext context, double currentValue)
        {
            if (currentValue > 100)
            {
                context.DestroyElement();
            }
        }
    }
}
