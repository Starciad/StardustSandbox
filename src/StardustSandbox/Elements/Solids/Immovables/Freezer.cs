using StardustSandbox.Elements.Utilities;
using StardustSandbox.Enums.Elements;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class Freezer : ImmovableSolid
    {
        protected override void OnNeighbors(ElementContext context, IEnumerable<Slot> neighbors)
        {
            TemperatureUtilities.ModifyNeighborsTemperature(context, neighbors, TemperatureModifierMode.Cooling);
        }
    }
}
