using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Sand : MovableSolid
    {
        internal Sand() : base()
        {

        }

        protected override void OnTemperatureChanged(double currentValue)
        {
            if (currentValue >= 1500)
            {
                this.Context.ReplaceElement(ElementIndex.Glass);
            }
        }
    }
}
