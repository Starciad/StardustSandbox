/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

namespace StardustSandbox.Core.Constants
{
    internal static class IOConstants
    {
        #region DIRECTORY

        #region LOCAL

        internal const string LOCAL_LOGS_DIRECTORY = "logs";
        internal const string LOCAL_SCREENSHOTS_DIRECTORY = "screenshots";

        #endregion

        #region APPDATA

        internal const string APPDATA_PROGRESS_DIRECTORY = "progress";
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

        // Versioning Header
        internal const string VERSIONING_HEADER_FILE = "versioning_header.bin";

        #region PROGRESS

        // Filenames
        internal const string PROGRESS_ACHIEVEMENT_COMPONENT_FILE = "achievement_progress.bin";

        // Identifiers
        internal const string PROGRESS_ACHIEVEMENT_COMPONENT_ID = "achievement_progress";

        // Versions
        internal const int PROGRESS_ACHIEVEMENT_COMPONENT_VERSION = 1;

        #endregion

        #region SETTINGS

        // Filenames
        internal const string SETTINGS_CONTROL_COMPONENT_FILE = "control_settings.xml";
        internal const string SETTINGS_CURSOR_COMPONENT_FILE = "cursor_settings.xml";
        internal const string SETTINGS_GAMEPLAY_COMPONENT_FILE = "gameplay_settings.xml";
        internal const string SETTINGS_GENERAL_COMPONENT_FILE = "general_settings.xml";
        internal const string SETTINGS_INTERFACE_COMPONENT_FILE = "interface_settings.xml";
        internal const string SETTINGS_VIDEO_COMPONENT_FILE = "video_settings.xml";
        internal const string SETTINGS_VOLUME_COMPONENT_FILE = "volume_settings.xml";

        // Identifiers
        internal const string SETTINGS_CONTROL_COMPONENT_ID = "control_settings";
        internal const string SETTINGS_CURSOR_COMPONENT_ID = "cursor_settings";
        internal const string SETTINGS_GAMEPLAY_COMPONENT_ID = "gameplay_settings";
        internal const string SETTINGS_GENERAL_COMPONENT_ID = "general_settings";
        internal const string SETTINGS_INTERFACE_COMPONENT_ID = "interface_settings";
        internal const string SETTINGS_VIDEO_COMPONENT_ID = "video_settings";
        internal const string SETTINGS_VOLUME_COMPONENT_ID = "volume_settings";

        // Versions
        internal const int SETTINGS_CONTROL_COMPONENT_VERSION = 1;
        internal const int SETTINGS_CURSOR_COMPONENT_VERSION = 1;
        internal const int SETTINGS_GAMEPLAY_COMPONENT_VERSION = 1;
        internal const int SETTINGS_GENERAL_COMPONENT_VERSION = 1;
        internal const int SETTINGS_INTERFACE_COMPONENT_VERSION = 1;
        internal const int SETTINGS_VIDEO_COMPONENT_VERSION = 1;
        internal const int SETTINGS_VOLUME_COMPONENT_VERSION = 1;

        #endregion

        #region WORLD

        // Filenames
        internal const string WORLD_CONTENT_COMPONENT_FILE = "content.bin";
        internal const string WORLD_ENVIRONMENT_COMPONENT_FILE = "environment.bin";
        internal const string WORLD_MANIFEST_COMPONENT_FILE = "manifest.bin";
        internal const string WORLD_PROPERTIES_COMPONENT_FILE = "properties.bin";
        internal const string WORLD_THUMBNAIL_COMPONENT_FILE = "thumbnail.bin";

        // Identifiers
        internal const string WORLD_CONTENT_COMPONENT_ID = "CONTENT";
        internal const string WORLD_ENVIRONMENT_COMPONENT_ID = "ENVIRONMENT";
        internal const string WORLD_MANIFEST_COMPONENT_ID = "MANIFEST";
        internal const string WORLD_PROPERTIES_COMPONENT_ID = "PROPERTIES";
        internal const string WORLD_THUMBNAIL_COMPONENT_ID = "THUMBNAIL";

        // Metadata
        internal const string WORLD_FILE_EXTENSION = ".sf2";
        internal const string WORLD_FILE_DATA = "data.bin";

        // Versions
        internal const int WORLD_CONTENT_COMPONENT_VERSION = 1;
        internal const int WORLD_ENVIRONMENT_COMPONENT_VERSION = 1;
        internal const int WORLD_MANIFEST_COMPONENT_VERSION = 1;
        internal const int WORLD_PROPERTIES_COMPONENT_VERSION = 1;
        internal const int WORLD_THUMBNAIL_COMPONENT_VERSION = 1;

        #endregion

        // Others
        internal const string WARNING = "WARNING.txt";

        #endregion
    }
}
