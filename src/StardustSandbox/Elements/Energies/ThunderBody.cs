using StardustSandbox.Constants;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Explosions;

namespace StardustSandbox.Elements.Energies
{
    internal sealed class ThunderBody : Energy
    {
        private static readonly ExplosionBuilder explosionBuilder = new()
        {
            Radius = 3.0f,
            Power = 5.0f,
            Heat = TemperatureConstants.MAX_CELSIUS_VALUE,

            AffectsWater = true,
            AffectsSolids = true,
            AffectsGases = true,

            ExplosionResidues =
            [
                ElementIndex.Fire,
                ElementIndex.Smoke
            ]
        };

        protected override void OnAfterStep(in ElementContext context)
        {
            context.RemoveElement();
        }

        protected override void OnNeighbors(in ElementContext context, in ElementNeighbors neighbors)
        {
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.HasNeighbor(i) || neighbors.GetSlotLayer(i, context.Layer).HasState(ElementStates.IsEmpty))
                {
                    continue;
                }

                Element element = neighbors.GetSlotLayer(i, context.Layer).Element;

                if (element.Index == ElementIndex.ThunderHead ||
                    element.Index == ElementIndex.ThunderBody)
                {
                    continue;
                }

                context.InstantiateExplosion(explosionBuilder);
            }
        }
    }
}
