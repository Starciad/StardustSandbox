using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Stone : MovableSolid
    {
        protected override void OnTemperatureChanged(ElementContext context, in float currentValue)
        {
            if (currentValue > 600.0f)
            {
                context.ReplaceElement(ElementIndex.Lava);
            }
        }
    }
}
