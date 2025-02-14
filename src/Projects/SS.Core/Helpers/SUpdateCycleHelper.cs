using StardustSandbox.Core.Enums.General;

namespace StardustSandbox.Core.Helpers
{
    public static class SUpdateCycleHelper
    {
        public static SUpdateCycleFlag GetNextCycle(this SUpdateCycleFlag currentCycle)
        {
            return currentCycle switch
            {
                SUpdateCycleFlag.None => SUpdateCycleFlag.Primary,
                SUpdateCycleFlag.Primary => SUpdateCycleFlag.Secondary,
                SUpdateCycleFlag.Secondary => SUpdateCycleFlag.Primary,
                _ => SUpdateCycleFlag.None,
            };
        }
    }
}
