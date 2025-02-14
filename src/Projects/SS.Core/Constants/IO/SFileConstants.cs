using System;

namespace StardustSandbox.Core.Constants.IO
{
    public static class SFileConstants
    {
        // Versions
        public static Version WORLD_SAVE_FILE_VERSION => new(2, 0, 0, 0);

        // Settings
        public const string VIDEO_SETTINGS = "video_settings" + SFileExtensionConstants.SETTINGS;
        public const string VOLUME_SETTINGS = "volume_settings" + SFileExtensionConstants.SETTINGS;
        public const string CURSOR_SETTINGS = "cursor_settings" + SFileExtensionConstants.SETTINGS;
        public const string GENERAL_SETTINGS = "general_settings" + SFileExtensionConstants.SETTINGS;
        public const string GAMEPLAY_SETTINGS = "gameplay_settings" + SFileExtensionConstants.SETTINGS;
        public const string GRAPHICS_SETTINGS = "graphics_settings" + SFileExtensionConstants.SETTINGS;

        // World
        public const string SAVE_FILE_THUMBNAIL = "thumbnail" + SFileExtensionConstants.PNG;

        public const string SAVE_FILE_HEADER_METADATA = "metadata" + SFileExtensionConstants.WORLD_DATA;
        public const string SAVE_FILE_HEADER_INFORMATION = "information" + SFileExtensionConstants.WORLD_DATA;

        public const string SAVE_FILE_WORLD_INFORMATION = "information" + SFileExtensionConstants.WORLD_DATA;
        public const string SAVE_FILE_WORLD_RESOURCE_ELEMENTS = "elements" + SFileExtensionConstants.WORLD_DATA;
        public const string SAVE_FILE_WORLD_CONTENT_SLOTS = "slots" + SFileExtensionConstants.WORLD_DATA;
        public const string SAVE_FILE_WORLD_CONTENT_ENTITIES = "entities" + SFileExtensionConstants.WORLD_DATA;
        public const string SAVE_FILE_WORLD_ENVIRONMENT_TIME = "time" + SFileExtensionConstants.WORLD_DATA;

        // Others
        public const string WARNING = "WARNING.txt";
    }
}