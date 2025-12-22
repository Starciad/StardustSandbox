using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Mud : MovableSolid
    {
        protected override void OnTemperatureChanged(ElementContext context, in float currentValue)
        {
            if (currentValue >= 100.0f)
            {
                context.ReplaceElement(ElementIndex.Dirt);
            }
        }
    }
}
