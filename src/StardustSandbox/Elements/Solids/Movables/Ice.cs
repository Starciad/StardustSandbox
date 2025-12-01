using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Ice : MovableSolid
    {
        internal Ice() : base()
        {

        }

        protected override void OnTemperatureChanged(ElementContext context, double currentValue)
        {
            if (currentValue >= 0)
            {
                if (context.SlotLayer.StoredElement == null)
                {
                    context.ReplaceElement(ElementIndex.Water);
                }
                else
                {
                    context.ReplaceElement(context.SlotLayer.StoredElement);
                }

                context.SetElementTemperature(13);
            }
        }
    }
}
