using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Snow : MovableSolid
    {
        protected override void OnTemperatureChanged(ElementContext context, in float currentValue)
        {
            if (currentValue >= 8.0f)
            {
                if (context.GetStoredElement() == null)
                {
                    context.ReplaceElement(ElementIndex.Water);
                }
                else
                {
                    context.ReplaceElement(context.GetStoredElement());
                }

                context.SetElementTemperature(12.0f);
            }
        }
    }
}
