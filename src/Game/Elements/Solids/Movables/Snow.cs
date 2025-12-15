using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Snow : MovableSolid
    {
        protected override void OnTemperatureChanged(ElementContext context, in float currentValue)
        {
            if (currentValue >= 8.0f)
            {
                if (context.SlotLayer.StoredElement == null)
                {
                    context.ReplaceElement(ElementIndex.Water);
                }
                else
                {
                    context.ReplaceElement(context.SlotLayer.StoredElement);
                }

                context.SetElementTemperature(12.0f);
            }
        }
    }
}
