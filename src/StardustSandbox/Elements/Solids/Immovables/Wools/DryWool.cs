using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Immovables.Wools
{
    internal abstract class DryWool : ImmovableSolid
    {
        internal required ElementIndex WetWoolIndex { get; init; }

        protected override void OnNeighbors(in ElementContext context, in ElementNeighbors neighbors)
        {
            bool shouldBecomeWet = false;

            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.HasNeighbor(i) || neighbors.GetSlotLayer(i, context.Layer).HasState(ElementStates.IsEmpty))
                {
                    continue;
                }

                switch (neighbors.GetSlotLayer(i, context.Layer).Element.Index)
                {
                    case ElementIndex.Water:
                    case ElementIndex.Saltwater:
                        context.RemoveElement(neighbors.GetSlot(i).Position);
                        shouldBecomeWet = true;
                        break;

                    default:
                        break;
                }
            }

            if (shouldBecomeWet)
            {
                context.ReplaceElement(this.WetWoolIndex);
            }
        }

        protected override void OnTemperatureChanged(in ElementContext context, float currentValue)
        {
            if (currentValue >= 580.0f)
            {
                context.ReplaceElement(ElementIndex.Fire);
            }
        }
    }
}
