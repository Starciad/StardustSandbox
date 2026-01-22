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

using System;
using System.Xml.Serialization;

namespace StardustSandbox.Core.Serialization.Settings
{
    public readonly struct AchievementProgress(bool isUnlocked)
    {
        [XmlElement("IsUnlocked", typeof(bool))]
        public readonly bool IsUnlocked => isUnlocked;
    }

    [Serializable]
    [XmlRoot("ControlSettings")]
    public readonly struct AchievementSettings : ISettingsModule
    {
        [XmlElement("AchievementProgresses", typeof(AchievementProgress[]))]
        public readonly AchievementProgress[] AchievementProgresses { get; init; }

        public AchievementSettings()
        {

        }
    }
}
