using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Sand : MovableSolid
    {
        internal Sand() : base()
        {

        }

        protected override void OnTemperatureChanged(ElementContext context, double currentValue)
        {
            if (currentValue >= 1500)
            {
                context.ReplaceElement(ElementIndex.Glass);
            }
        }
    }
}
