using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Mud : MovableSolid
    {
        internal Mud() : base()
        {

        }

        protected override void OnTemperatureChanged(double currentValue)
        {
            if (currentValue >= 100)
            {
                this.Context.ReplaceElement(ElementIndex.Dirt);
            }
        }
    }
}
