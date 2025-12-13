using StardustSandbox.Elements.Energies;
using StardustSandbox.Elements.Liquids;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Explosions;

namespace StardustSandbox.Elements.Solids.Movables.Explosives
{
    internal sealed class Tnt : MovableSolid
    {
        private static readonly ExplosionBuilder explosionBuilder = new()
        {
            Radius = 6.0f,
            Power = 5.0f,
            Heat = 450.0f,

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

                switch (neighbors.GetSlotLayer(i, context.Layer).Element.Index)
                {
                    case ElementIndex.Fire:
                    case ElementIndex.Lava:
                        context.DestroyElement();
                        break;

                    default:
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(in ElementContext context, float currentValue)
        {
            if (currentValue > 120.0f)
            {
                context.DestroyElement();
            }
        }
    }
}
