using StardustSandbox.Elements.Solids.Immovables;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Explosions;

namespace StardustSandbox.Elements.Solids.Movables
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

        protected override void OnDestroyed(in ElementContext context)
        {
            context.InstantiateExplosion(explosionBuilder);
        }

        protected override void OnNeighbors(in ElementContext context, in ElementNeighbors neighbors)
        {
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.HasNeighbor(i))
                {
                    continue;
                }

                switch (neighbors.GetSlotLayer(i, context.Layer).Element)
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

        protected override void OnTemperatureChanged(in ElementContext context, float currentValue)
        {
            if (currentValue > 100.0f)
            {
                context.DestroyElement();
            }
        }
    }
}
