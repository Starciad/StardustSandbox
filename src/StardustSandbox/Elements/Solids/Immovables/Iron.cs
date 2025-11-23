using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class Iron : ImmovableSolid
    {
        protected override void OnTemperatureChanged(double currentValue)
        {
            if (currentValue > 1200)
            {
                this.Context.ReplaceElement(ElementIndex.Lava);
                this.Context.SetStoredElement(ElementIndex.Iron);
            }
        }
    }
}
