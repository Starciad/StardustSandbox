using Microsoft.Xna.Framework;

using System;

namespace StardustSandbox.Core.Constants
{
    public static class SBackgroundConstants
    {
        public static Vector2 CELESTIAL_BODY_CENTER_PIVOT => new(SScreenConstants.DEFAULT_SCREEN_WIDTH / 2, SScreenConstants.DEFAULT_SCREEN_HEIGHT);

        public const byte ACTIVE_CLOUDS_LIMIT = 32;

        public const short CHANCE_OF_CLOUD_SPAWNING_TOTAL = 300;
        public const byte CHANCE_OF_CLOUD_SPAWNING = 1;

        public const float CELESTIAL_BODY_MAX_ARC_ANGLE = MathF.PI;
        public const float CELESTIAL_BODY_ARC_OFFSET = MathF.PI * 2f;
        public const float CELESTIAL_BODY_ARC_RADIUS = 500f;
    }
}
