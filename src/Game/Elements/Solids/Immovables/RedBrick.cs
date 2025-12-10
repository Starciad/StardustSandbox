using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class RedBrick : ImmovableSolid
    {
        protected override void OnTemperatureChanged(in ElementContext context, float currentValue)
        {
            if (currentValue >= 1727.0f)
            {
                context.ReplaceElement(ElementIndex.Lava);
                context.SetStoredElement(ElementIndex.RedBrick);
            }
        }
    }
}
