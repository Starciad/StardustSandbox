using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class Iron : ImmovableSolid
    {
        protected override void OnTemperatureChanged(in ElementContext context, float currentValue)
        {
            if (currentValue > 1200.0f)
            {
                context.ReplaceElement(ElementIndex.Lava);
                context.SetStoredElement(ElementIndex.Iron);
            }
        }
    }
}
