using StardustSandbox.Enums.Elements;
using StardustSandbox.Generators;
using StardustSandbox.Randomness;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Sapling : MovableSolid
    {
        protected override void OnNeighbors(in ElementContext context, in ElementNeighbors neighbors)
        {
            bool hasWater = false, hasFertileSoil = false;

            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.HasNeighbor(i) || neighbors.GetSlotLayer(i, context.Layer).HasState(ElementStates.IsEmpty))
                {
                    continue;
                }

                switch (neighbors.GetSlotLayer(i, context.Layer).Element.Index)
                {
                    case ElementIndex.Water:
                        hasWater = true;
                        context.DestroyElement(neighbors.GetSlot(i).Position);
                        break;

                    case ElementIndex.FertileSoil:
                        hasFertileSoil = true;
                        break;

                    default:
                        break;
                }
            }

            if (hasWater && hasFertileSoil && SSRandom.Chance(25, 350))
            {
                context.DestroyElement();
                TreeGenerator.Start(context, context.Position, SSRandom.Range(5, 8), 1, 2);
            }
        }

        protected override void OnTemperatureChanged(in ElementContext context, in float currentValue)
        {
            if (currentValue >= 100.0f)
            {
                context.ReplaceElement(ElementIndex.Fire);
            }
        }
    }
}
