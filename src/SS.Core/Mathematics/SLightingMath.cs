using StardustSandbox.Core.Constants;

namespace StardustSandbox.Core.Mathematics
{
    public sealed class SLightingMath
    {
        public static byte Clamp(byte value)
        {
            return byte.Clamp(value, SLightingConstants.MINIMUM_SLOT_LIGHT_LEVEL, SLightingConstants.MAXIMUM_SLOT_LIGHT_LEVEL);
        }
    }
}
