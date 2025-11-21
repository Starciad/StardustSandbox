using Microsoft.Xna.Framework;

using System;

namespace StardustSandbox.Constants
{
    internal static class BackgroundConstants
    {
        internal static Vector2 CELESTIAL_BODY_CENTER_PIVOT => new(ScreenConstants.SCREEN_WIDTH / 2, ScreenConstants.SCREEN_HEIGHT);

        internal const byte ACTIVE_CLOUDS_LIMIT = 32;

        internal const short CHANCE_OF_CLOUD_SPAWNING_TOTAL = 300;
        internal const byte CHANCE_OF_CLOUD_SPAWNING = 1;

        internal const double CELESTIAL_BODY_MAX_ARC_ANGLE = MathF.PI;
        internal const double CELESTIAL_BODY_ARC_OFFSET = MathF.PI * 2.0;
        internal const double CELESTIAL_BODY_ARC_RADIUS = 500.0;
    }
}
