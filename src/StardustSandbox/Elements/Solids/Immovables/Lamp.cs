namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class Lamp : ImmovableSolid
    {
        protected override void OnTemperatureChanged(in ElementContext context, double currentValue)
        {
            if (currentValue >= 600)
            {
                context.DestroyElement();
            }
        }
    }
}