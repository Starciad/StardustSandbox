using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class FertileSoil : MovableSolid
    {
        protected override void OnTemperatureChanged(in ElementContext context, float currentValue)
        {
            if (currentValue <= 0.0f || currentValue >= 100.0f)
            {
                context.ReplaceElement(ElementIndex.Dirt);
            }
        }
    }
}
