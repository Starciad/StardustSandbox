using StardustSandbox.Enums.Elements;
using StardustSandbox.Explosions;

namespace StardustSandbox.Elements.Solids.Movables.Explosives
{
    internal sealed class Gunpowder : MovableSolid
    {
        private static readonly ExplosionBuilder explosionBuilder = new()
        {
            Radius = 4.0f,
            Power = 3.0f,
            Heat = 300.0f,

            AffectsWater = false,
            AffectsSolids = true,
            AffectsGases = true,

            ExplosionResidues =
            [
                ElementIndex.Fire,
                ElementIndex.Smoke,
            ]
        };

        protected override void OnDestroyed(ElementContext context)
        {
            context.InstantiateExplosion(explosionBuilder);
        }

        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, context.Layer))
                {
                    continue;
                }

                switch (neighbors.GetSlotLayer(i, context.Layer).Element.Index)
                {
                    case ElementIndex.Fire:
                    case ElementIndex.Lava:
                        context.DestroyElement();
                        return;

                    case ElementIndex.Water:
                    case ElementIndex.Saltwater:
                        context.RemoveElement();
                        return;

                    default:
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(ElementContext context, in float currentValue)
        {
            if (currentValue >= 300.0f)
            {
                context.DestroyElement();
            }
        }
    }
}
