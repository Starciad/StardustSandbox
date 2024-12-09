namespace StardustSandbox.Core.Constants.IO
{
    public static class SFileConstants
    {
        public const byte WORLD_SAVE_FILE_VERSION = 1;

        public const string SETTINGS_GENERAL = "general_settings" + SFileExtensionConstants.SETTINGS;
        public const string SETTINGS_VIDEO = "video_settings" + SFileExtensionConstants.SETTINGS;
        public const string SETTINGS_VOLUME = "volume_settings" + SFileExtensionConstants.SETTINGS;
        public const string SETTINGS_CURSOR = "cursor_settings" + SFileExtensionConstants.SETTINGS;
        public const string SETTINGS_LANGUAGE = "language_settings" + SFileExtensionConstants.SETTINGS;
    }
}