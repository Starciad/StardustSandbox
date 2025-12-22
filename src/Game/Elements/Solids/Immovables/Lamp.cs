namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class Lamp : ImmovableSolid
    {
        protected override void OnTemperatureChanged(ElementContext context, in float currentValue)
        {
            if (currentValue >= 600.0f)
            {
                context.DestroyElement();
            }
        }
    }
}