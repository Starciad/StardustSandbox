using StardustSandbox.Elements.Utilities;
using StardustSandbox.Enums.Elements;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class Heater : ImmovableSolid
    {
        protected override void OnNeighbors(IEnumerable<Slot> neighbors)
        {
            TemperatureUtilities.ModifyNeighborsTemperature(this.Context, neighbors, TemperatureModifierMode.Warming);
        }
    }
}
