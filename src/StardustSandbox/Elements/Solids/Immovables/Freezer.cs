using StardustSandbox.Elements.Utilities;
using StardustSandbox.Enums.Elements;
using StardustSandbox.World;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class Freezer : ImmovableSolid
    {
        protected override void OnNeighbors(in ElementContext context, IEnumerable<Slot> neighbors)
        {
            TemperatureUtilities.ModifyNeighborsTemperature(context, neighbors, TemperatureModifierMode.Cooling);
        }
    }
}
