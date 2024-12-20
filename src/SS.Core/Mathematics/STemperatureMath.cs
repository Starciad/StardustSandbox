namespace StardustSandbox.Core.Mathematics
{
    public static class STemperatureMath
    {
        public const short MinCelsiusValue = -275;
        public const short MaxCelsiusValue = 9725;

        public const short EquilibriumThreshold = 1;

        public static short Clamp(short value)
        {
            return short.Clamp(value, MinCelsiusValue, MaxCelsiusValue);
        }
    }
}
