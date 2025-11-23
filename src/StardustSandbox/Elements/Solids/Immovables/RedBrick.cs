using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class RedBrick : ImmovableSolid
    {
        protected override void OnTemperatureChanged(double currentValue)
        {
            if (currentValue >= 1727)
            {
                this.Context.ReplaceElement(ElementIndex.Lava);
                this.Context.SetStoredElement(ElementIndex.RedBrick);
            }
        }
    }
}
