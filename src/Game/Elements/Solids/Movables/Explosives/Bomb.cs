using StardustSandbox.Enums.Elements;
using StardustSandbox.Explosions;

namespace StardustSandbox.Elements.Solids.Movables.Explosives
{
    internal sealed class Bomb : MovableSolid
    {
        private static readonly ExplosionBuilder explosionBuilder = new()
        {
            Radius = 4.0f,
            Power = 2.5f,
            Heat = 180.0f,

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
                    case ElementIndex.Bomb:
                    case ElementIndex.Wall:
                    case ElementIndex.Clone:
                    case ElementIndex.Void:
                    case ElementIndex.DownwardPusher:
                    case ElementIndex.UpwardPusher:
                    case ElementIndex.RightwardPusher:
                    case ElementIndex.LeftwardPusher:
                        break;

                    default:
                        context.DestroyElement();
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(ElementContext context, in float currentValue)
        {
            if (currentValue > 100.0f)
            {
                context.DestroyElement();
            }
        }
    }
}
