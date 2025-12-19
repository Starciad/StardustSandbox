using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Ice : MovableSolid
    {
        protected override void OnTemperatureChanged(ElementContext context, in float currentValue)
        {
            if (currentValue >= 0.0f)
            {
                if (context.GetStoredElement() == null)
                {
                    context.ReplaceElement(ElementIndex.Water);
                }
                else
                {
                    context.ReplaceElement(context.GetStoredElement());
                }

                context.SetElementTemperature(13.0f);
            }
        }
    }
}
