using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Ice : MovableSolid
    {
        protected override void OnTemperatureChanged(in ElementContext context, float currentValue)
        {
            if (currentValue >= 0.0f)
            {
                if (context.SlotLayer.StoredElement == null)
                {
                    context.ReplaceElement(ElementIndex.Water);
                }
                else
                {
                    context.ReplaceElement(context.SlotLayer.StoredElement);
                }

                context.SetElementTemperature(13.0f);
            }
        }
    }
}
