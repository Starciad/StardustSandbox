using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Sand : MovableSolid
    {
        protected override void OnTemperatureChanged(ElementContext context, in float currentValue)
        {
            if (currentValue >= 1500.0f)
            {
                context.ReplaceElement(ElementIndex.Glass);
            }
        }
    }
}
