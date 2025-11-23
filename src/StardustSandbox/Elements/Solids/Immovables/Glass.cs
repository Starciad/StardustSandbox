using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class Glass : ImmovableSolid
    {
        protected override void OnTemperatureChanged(double currentValue)
        {
            if (currentValue >= 620)
            {
                this.Context.ReplaceElement(ElementIndex.Lava);
                this.Context.SetStoredElement(ElementIndex.Glass);
            }
        }
    }
}