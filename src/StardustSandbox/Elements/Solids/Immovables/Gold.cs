using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class Gold : ImmovableSolid
    {
        protected override void OnTemperatureChanged(in ElementContext context, double currentValue)
        {
            if (currentValue > 1060)
            {
                context.ReplaceElement(ElementIndex.Lava);
                context.SetStoredElement(ElementIndex.Gold);
            }
        }
    }
}
