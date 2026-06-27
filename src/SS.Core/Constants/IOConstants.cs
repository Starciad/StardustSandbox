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
        #region DIRECTORIES

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

        #endregion

        #endregion

        #region FILE

        // Progress
        internal const string ACHIEVEMENT_PROGRESS_FILE = "achievement_progress.bin";

        // Settings
        internal const string CONTROL_SETTINGS_FILE = "control_settings.xml";
        internal const string CURSOR_SETTINGS_FILE = "cursor_settings.xml";
        internal const string GAMEPLAY_SETTINGS_FILE = "gameplay_settings.xml";
        internal const string GENERAL_SETTINGS_FILE = "general_settings.xml";
        internal const string STATUS_SETTINGS_FILE = "status_settings.xml";
        internal const string INTERFACE_SETTINGS_FILE = "interface_settings.xml";
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
