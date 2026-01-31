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

        // Settings
        internal const string ACHIEVEMENT_SETTINGS_FILE = "achievement_settings.xml";
        internal const string CONTROL_SETTINGS_FILE = "control_settings.xml";
        internal const string CURSOR_SETTINGS_FILE = "cursor_settings.xml";
        internal const string GAMEPLAY_SETTINGS_FILE = "gameplay_settings.xml";
        internal const string GENERAL_SETTINGS_FILE = "general_settings.xml";
        internal const string STATUS_SETTINGS_FILE = "status_settings.xml";
        internal const string VIDEO_SETTINGS_FILE = "video_settings.xml";
        internal const string VOLUME_SETTINGS_FILE = "volume_settings.xml";

        // Save
        internal const string SAVE_FILE_EXTENSION = ".sf";
        internal const string SAVE_FILE_DATA = "data.bin";

        internal const string SAVE_ENTRY_THUMBNAIL = "thumbnail.bin";
        internal const string SAVE_ENTRY_METADATA = "metadata.bin";
        internal const string SAVE_ENTRY_MANIFEST = "manifest.bin";
        internal const string SAVE_ENTRY_PROPERTIES = "properties.bin";
        internal const string SAVE_ENTRY_ENVIRONMENT = "environment.bin";
        internal const string SAVE_ENTRY_CONTENT = "content.bin";

        // Others
        internal const string WARNING = "WARNING.txt";

        #endregion
    }
}
