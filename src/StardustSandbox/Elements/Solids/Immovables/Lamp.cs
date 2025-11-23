namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class Lamp : ImmovableSolid
    {
        protected override void OnTemperatureChanged(double currentValue)
        {
            if (currentValue >= 600)
            {
                this.Context.DestroyElement();
            }
        }
    }
}