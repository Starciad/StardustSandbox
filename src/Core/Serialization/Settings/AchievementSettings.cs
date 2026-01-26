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

using StardustSandbox.Core.Enums.Achievements;

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace StardustSandbox.Core.Serialization.Settings
{
    [Serializable]
    [XmlRoot("Data")]
    public sealed class AchievementProgressData
    {
        [XmlElement("Index", typeof(AchievementIndex))]
        public AchievementIndex Index { get; set; } = AchievementIndex.None;

        [XmlElement("IsUnlocked", typeof(bool))]
        public bool IsUnlocked { get; set; } = false;

        public AchievementProgressData()
        {

        }

        public AchievementProgressData(AchievementIndex index)
        {
            this.Index = index;
        }
    }

    [Serializable]
    [XmlRoot("AchievementSettings")]
    public sealed class AchievementSettings : ISettingsModule
    {
        [XmlArray("Datas")]
        [XmlArrayItem("Data", typeof(AchievementProgressData))]
        public List<AchievementProgressData> Datas { get; set; } = [];

        public AchievementSettings()
        {

        }

        private AchievementProgressData AddData(AchievementIndex index)
        {
            AchievementProgressData data = new(index);
            this.Datas.Add(data);
            return data;
        }

        private AchievementProgressData GetData(AchievementIndex index)
        {
            return this.Datas.Find(d => d.Index == index);
        }

        public bool IsUnlocked(AchievementIndex index)
        {
            return GetData(index)?.IsUnlocked ?? false;
        }

        public void Unlock(AchievementIndex index)
        {
            AchievementProgressData data = GetData(index);

            if (data is null)
            {
                data = AddData(index);
                data.IsUnlocked = true;
            }
            else
            {
                data.IsUnlocked = true;
            }
        }
    }
}
