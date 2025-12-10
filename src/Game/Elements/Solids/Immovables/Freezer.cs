using StardustSandbox.Elements.Utilities;
using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class Freezer : ImmovableSolid
    {
        protected override void OnNeighbors(in ElementContext context, in ElementNeighbors neighbors)
        {
            TemperatureUtilities.ModifyNeighborsTemperature(context, neighbors, TemperatureModifierMode.Cooling);
        }
    }
}
