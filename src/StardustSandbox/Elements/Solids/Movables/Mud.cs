using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Mud : MovableSolid
    {
        internal Mud() : base()
        {

        }

        protected override void OnTemperatureChanged(in ElementContext context, double currentValue)
        {
            if (currentValue >= 100)
            {
                context.ReplaceElement(ElementIndex.Dirt);
            }
        }
    }
}
