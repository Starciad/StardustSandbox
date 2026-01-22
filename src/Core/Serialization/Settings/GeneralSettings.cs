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

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Localization;

using System;
using System.Globalization;
using System.Xml.Serialization;

namespace StardustSandbox.Core.Serialization.Settings
{
    [Serializable]
    [XmlRoot("GeneralSettings")]
    public sealed class GeneralSettings : ISettingsModule
    {
        [XmlElement("Language", typeof(string))]
        public string Language { get; set; }

        [XmlElement("Region", typeof(string))]
        public string Region { get; set; }

        [XmlIgnore]
        public string Name => string.Concat(this.Language, '-', this.Region);

        public GeneralSettings()
        {
            GameCulture gameCulture = LocalizationConstants.DEFAULT_GAME_CULTURE;

            if (TryGetAvailableGameCulture(out GameCulture value))
            {
                gameCulture = value;
            }

            this.Language = gameCulture.Language;
            this.Region = gameCulture.Region;
        }

        public GameCulture GetGameCulture()
        {
            return LocalizationConstants.GetGameCulture(this.Name) ?? LocalizationConstants.DEFAULT_GAME_CULTURE;
        }

        private static bool TryGetAvailableGameCulture(out GameCulture value)
        {
            value = LocalizationConstants.GetGameCulture(CultureInfo.CurrentCulture.Name);

            return value != null;
        }
    }
}

