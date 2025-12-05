using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class Glass : ImmovableSolid
    {
        protected override void OnTemperatureChanged(in ElementContext context, double currentValue)
        {
            if (currentValue >= 620)
            {
                context.ReplaceElement(ElementIndex.Lava);
                context.SetStoredElement(ElementIndex.Glass);
            }
        }
    }
}