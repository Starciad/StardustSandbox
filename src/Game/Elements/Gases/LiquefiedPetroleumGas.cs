using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Gases
{
    internal sealed class LiquefiedPetroleumGas : Gas
    {
        protected override void OnTemperatureChanged(in ElementContext context, in float currentValue)
        {
            if (currentValue >= 400.0f)
            {
                context.ReplaceElement(ElementIndex.Fire);
            }
        }
    }
}
