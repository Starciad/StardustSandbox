namespace StardustSandbox.Core.Constants
{
    public static class SEntityConstants
    {
        // Identifiers
        public const string IDENTIFIER_PREFIX = "entity_";

        public const string IDENTIFIER_MAGIC_CURSOR = IDENTIFIER_PREFIX + "magic_cursor";
        public const string IDENTIFIER_ANT = IDENTIFIER_PREFIX + "ant";

        // Settings
        public const byte ACTIVE_ENTITIES_LIMIT = byte.MaxValue;
    }
}
