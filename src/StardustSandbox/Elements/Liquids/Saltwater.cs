using StardustSandbox.Elements.Energies;
using StardustSandbox.Elements.Solids.Movables;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Randomness;

namespace StardustSandbox.Elements.Liquids
{
    internal sealed class Saltwater : Liquid
    {
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
                    case Dirt:
                        context.DestroyElement();
                        context.ReplaceElement(neighbors.GetSlot(i).Position, context.Layer, ElementIndex.Mud);
                        break;

                    case Stone:
                        if (SSRandom.Range(0, 150) == 0)
                        {
                            context.DestroyElement();
                            context.ReplaceElement(neighbors.GetSlot(i).Position, context.Layer, ElementIndex.Sand);
                        }

                        break;

                    case Fire:
                        context.DestroyElement(neighbors.GetSlot(i).Position, context.Layer);
                        break;

                    default:
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(in ElementContext context, float currentValue)
        {
            if (currentValue <= 21.0f)
            {
                context.ReplaceElement(ElementIndex.Ice);
                context.SetStoredElement(ElementIndex.Saltwater);
                return;
            }

            if (currentValue >= 110.0f)
            {
                if (SSRandom.RandomBool())
                {
                    context.ReplaceElement(ElementIndex.Steam);
                }
                else
                {
                    context.ReplaceElement(ElementIndex.Saltwater);
                }

                return;
            }
        }
    }
}
