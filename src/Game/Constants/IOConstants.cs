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

        #endregion

        #region FILE

        // Versions
        internal static Version SAVE_FILE_VERSION => new("3.0.0.0");

        // Settings
        internal const string CONTROL_SETTINGS_FILE = "control_settings.xml";
        internal const string CURSOR_SETTINGS_FILE = "cursor_settings.xml";
        internal const string GAMEPLAY_SETTINGS_FILE = "gameplay_settings.xml";
        internal const string GENERAL_SETTINGS_FILE = "general_settings.xml";
        internal const string VIDEO_SETTINGS_FILE = "video_settings.xml";
        internal const string VOLUME_SETTINGS_FILE = "volume_settings.xml";

        // Save
        internal const string SAVE_FILE_EXTENSION = ".sf";
        internal const string SAVE_FILE_DATA = "data.bin";
        internal const string SAVE_FILE_THUMBNAIL = "thumbnail.png";

        // Others
        internal const string WARNING = "WARNING.txt";

        #endregion

    }
}
