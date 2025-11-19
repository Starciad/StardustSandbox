using System;

namespace StardustSandbox.Constants
{
    internal static class IOConstants
    {
        #region DIRECTORY

        #region LOCAL

        internal const string LOCAL_LOGS_DIRECTORY = "logs";

        #endregion

        #region APPDATA
        internal const string APPDATA_SETTINGS_DIRECTORY = "settings";
        internal const string APPDATA_WORLDS_DIRECTORY = "worlds";
        #endregion

        #region ASSETS

        internal const string ASSETS_DIRECTORY = "assets";

        internal const string ASSETS_EFFECTS_DIRECTORY = "effects";
        internal const string ASSETS_FONTS_DIRECTORY = "fonts";
        internal const string ASSETS_TEXTURES_DIRECTORY = "textures";
        internal const string ASSETS_SONGS_DIRECTORY = "songs";

        #region SOUNDS

        internal const string ASSETS_SOUNDS_DIRECTORY = "sounds";
        internal const string ASSETS_SOUNDS_EXPLOSIONS_DIRECTORY = "explosions";

        #endregion

        #region TEXTURES

        // Backgrounds
        internal const string ASSETS_TEXTURES_BACKGROUNDS_DIRECTORY = "backgrounds";

        // Bgos
        internal const string ASSETS_TEXTURES_BGOS_DIRECTORY = "bgos";
        internal const string ASSETS_TEXTURES_BGOS_CELESTIAL_BODIES_DIRECTORY = "celestial_bodies";
        internal const string ASSETS_TEXTURES_BGOS_CLOUDS_DIRECTORY = "clouds";

        // Characters
        internal const string ASSETS_TEXTURES_CHARACTERS_DIRECTORY = "characters";

        // Cursors
        internal const string ASSETS_TEXTURES_CURSORS_DIRECTORY = "cursors";

        // Effects
        internal const string ASSETS_TEXTURES_EFFECTS_DIRECTORY = "effects";

        // Elements
        internal const string ASSETS_TEXTURES_ELEMENTS_DIRECTORY = "elements";

        // Entities
        internal const string ASSETS_TEXTURES_ENTITIES_DIRECTORY = "entities";

        // Game
        internal const string ASSETS_TEXTURES_GAME_DIRECTORY = "game";
        internal const string ASSETS_TEXTURES_GAME_ICONS_DIRECTORY = "icons";
        internal const string ASSETS_TEXTURES_GAME_TITLES_DIRECTORY = "titles";

        // Gui
        internal const string ASSETS_TEXTURES_GUI_DIRECTORY = "gui";
        internal const string ASSETS_TEXTURES_GUI_BACKGROUNDS_DIRECTORY = "backgrounds";
        internal const string ASSETS_TEXTURES_GUI_BUTTONS_DIRECTORY = "buttons";
        internal const string ASSETS_TEXTURES_GUI_SLIDERS_DIRECTORY = "sliders";
        internal const string ASSETS_TEXTURES_GUI_FIELDS_DIRECTORY = "fields";

        // Icons
        internal const string ASSETS_TEXTURES_ICONS_DIRECTORY = "icons";
        internal const string ASSETS_TEXTURES_ICONS_ELEMENTS_DIRECTORY = "elements";
        internal const string ASSETS_TEXTURES_ICONS_ENTITIES_DIRECTORY = "entities";
        internal const string ASSETS_TEXTURES_ICONS_GUI_DIRECTORY = "gui";
        internal const string ASSETS_TEXTURES_ICONS_CONTROLLERS_DIRECTORY = "controllers";
        internal const string ASSETS_TEXTURES_ICONS_TOOLS_DIRECTORY = "tools";

        // Miscellaneous
        internal const string ASSETS_TEXTURES_MISCELLANEOUS_DIRECTORY = "miscellaneous";

        // Particles
        internal const string ASSETS_TEXTURES_PARTICLES_DIRECTORY = "particles";

        // Shapes
        internal const string ASSETS_TEXTURES_SHAPES_DIRECTORY = "shapes";
        internal const string ASSETS_TEXTURES_SHAPES_SQUARES_DIRECTORY = "squares";

        // Third Parties
        internal const string ASSETS_TEXTURES_THIRD_PARTIES_DIRECTORY = "third_parties";

        #endregion

        #endregion

        #region WORLD SAVE FILE

        internal const string SAVE_FILE_HEADER_DIRECTORY = "header";
        internal const string SAVE_FILE_WORLD_DIRECTORY = "world";
        internal const string SAVE_FILE_WORLD_RESOURCES_DIRECTORY = "resources";
        internal const string SAVE_FILE_WORLD_CONTENT_DIRECTORY = "content";
        internal const string SAVE_FILE_WORLD_ENVIRONMENT_DIRECTORY = "environment";

        #endregion

        #endregion

        #region FILE EXTENSIONS

        // General
        internal const string PREFIX_IDENTIFIER_FILE_EXTENSION = ".pd";

        // Core
        internal const string SETTINGS_FILE_EXTENSION = PREFIX_IDENTIFIER_FILE_EXTENSION + "settings";
        internal const string WORLD_FILE_EXTENSION = PREFIX_IDENTIFIER_FILE_EXTENSION + "world";
        internal const string WORLD_DATA_FILE_EXTENSION = PREFIX_IDENTIFIER_FILE_EXTENSION + "worlddata";

        // System
        internal const string BACKUP_FILE_EXTENSION = ".backup";

        // Normal
        internal const string PNG_FILE_EXTENSION = ".png";
        internal const string TXT_FILE_EXTENSION = ".txt";

        #endregion

        #region FILE

        // Versions
        internal static Version WORLD_SAVE_FILE_VERSION => new(2, 0, 0, 0);

        // Settings
        internal const string VIDEO_SETTINGS_FILE = "video_settings" + SETTINGS_FILE_EXTENSION;
        internal const string VOLUME_SETTINGS_FILE = "volume_settings" + SETTINGS_FILE_EXTENSION;
        internal const string CURSOR_SETTINGS_FILE = "cursor_settings" + SETTINGS_FILE_EXTENSION;
        internal const string GENERAL_SETTINGS_FILE = "general_settings" + SETTINGS_FILE_EXTENSION;
        internal const string GAMEPLAY_SETTINGS_FILE = "gameplay_settings" + SETTINGS_FILE_EXTENSION;
        internal const string GRAPHICS_SETTINGS_FILE = "graphics_settings" + SETTINGS_FILE_EXTENSION;

        // World
        internal const string SAVE_FILE_THUMBNAIL = "thumbnail" + PNG_FILE_EXTENSION;

        internal const string SAVE_FILE_HEADER_METADATA = "metadata" + WORLD_DATA_FILE_EXTENSION;
        internal const string SAVE_FILE_HEADER_INFORMATION = "information" + WORLD_DATA_FILE_EXTENSION;

        internal const string SAVE_FILE_WORLD_INFORMATION = "information" + WORLD_DATA_FILE_EXTENSION;
        internal const string SAVE_FILE_WORLD_RESOURCE_ELEMENTS = "elements" + WORLD_DATA_FILE_EXTENSION;
        internal const string SAVE_FILE_WORLD_CONTENT_SLOTS = "slots" + WORLD_DATA_FILE_EXTENSION;
        internal const string SAVE_FILE_WORLD_CONTENT_ENTITIES = "entities" + WORLD_DATA_FILE_EXTENSION;
        internal const string SAVE_FILE_WORLD_ENVIRONMENT_TIME = "time" + WORLD_DATA_FILE_EXTENSION;

        // Others
        internal const string WARNING = "WARNING" + TXT_FILE_EXTENSION;

        #endregion

    }
}
