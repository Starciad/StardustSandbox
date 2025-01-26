namespace StardustSandbox.Core.Constants
{
    public static class SEntityConstants
    {
        // Identifiers
        public const string PREFIX_IDENTIFIER = "entity_";

        public const string MAGIC_CURSOR_IDENTIFIER = PREFIX_IDENTIFIER + "magic_cursor";
        public const string ANT_IDENTIFIER = PREFIX_IDENTIFIER + "ant";

        // Settings
        public const byte ACTIVE_ENTITIES_LIMIT = byte.MaxValue;
    }
}
