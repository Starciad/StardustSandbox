using StardustSandbox.Constants;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Explosions;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.Elements.Energies
{
    internal sealed class LightningBody : Energy
    {
        private static readonly ExplosionBuilder explosionBuilder = new()
        {
            Radius = 2.0f,
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

                SlotLayer slotLayer = neighbors.GetSlotLayer(i, context.Layer);
                Element element = slotLayer.Element;

                if (element.Category == ElementCategory.Gas)
                {
                    continue;
                }

                switch (element.Index)
                {
                    case ElementIndex.LightningBody:
                    case ElementIndex.LightningHead:
                    case ElementIndex.Clone:
                    case ElementIndex.Void:
                    case ElementIndex.Wall:
                    case ElementIndex.Fire:
                        continue;

                    case ElementIndex.Water:
                    case ElementIndex.Snow:
                    case ElementIndex.Ice:
                        if (slotLayer.States.HasFlag(ElementStates.IsFalling))
                        {
                            continue;
                        }

                        break;

                    default:
                        break;
                }

                context.InstantiateExplosion(explosionBuilder);
            }
        }
    }
}
