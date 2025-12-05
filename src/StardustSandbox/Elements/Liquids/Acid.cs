using StardustSandbox.Elements.Solids.Immovables;

namespace StardustSandbox.Elements.Liquids
{
    internal sealed class Acid : Liquid
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
                    case Acid:
                    case Wall:
                    case Clone:
                    case Void:
                        continue;

                    default:
                        break;
                }

                context.DestroyElement();
                context.DestroyElement(neighbors.GetSlot(i).Position, context.Layer);
            }
        }
    }
}