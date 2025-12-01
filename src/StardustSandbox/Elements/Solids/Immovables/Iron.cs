using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class Iron : ImmovableSolid
    {
        protected override void OnTemperatureChanged(ElementContext context, double currentValue)
        {
            if (currentValue > 1200)
            {
                context.ReplaceElement(ElementIndex.Lava);
                context.SetStoredElement(ElementIndex.Iron);
            }
        }
    }
}
