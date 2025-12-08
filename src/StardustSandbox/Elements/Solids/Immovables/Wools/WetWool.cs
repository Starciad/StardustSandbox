using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Immovables.Wools
{
    internal abstract class WetWool : ImmovableSolid
    {
        internal required ElementIndex DryWoolIndex { get; init; }

        protected override void OnTemperatureChanged(in ElementContext context, float currentValue)
        {
            if (currentValue >= 55.0f)
            {
                context.ReplaceElement(this.DryWoolIndex);
            }
        }
    }
}
