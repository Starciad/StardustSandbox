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

using MessagePack;

using StardustSandbox.Core.Enums.Indexers;
using StardustSandbox.Core.Interfaces.Serialization.Modules;

using System;

namespace StardustSandbox.Core.Serialization.Progress.Common
{
    [Serializable]
    [MessagePackObject]
    public sealed class AchievementProgress : IProgressModule
    {
        [Key("Datas")]
        public AchievementProgressData[] Datas { get => this.datas; set => this.datas = value; }

        private AchievementProgressData[] datas = [];

        public AchievementProgress() { }

        private void EnsureCapacity(int index)
        {
            if (index >= this.Datas.Length)
            {
                Array.Resize(ref this.datas, index + 1);
            }
        }

        private AchievementProgressData AddData(AchievementIndex index)
        {
            int idx = (int)index;
            EnsureCapacity(idx);

            AchievementProgressData data = new(index);
            this.Datas[idx] = data;
            return data;
        }

        private AchievementProgressData GetData(AchievementIndex index)
        {
            int idx = (int)index;
            return idx >= this.Datas.Length ? null : this.Datas[idx];
        }

        public bool IsUnlocked(AchievementIndex index)
        {
            return GetData(index)?.IsUnlocked ?? false;
        }

        public void Unlock(AchievementIndex index)
        {
            AchievementProgressData data = GetData(index) ?? AddData(index);
            data.IsUnlocked = true;
        }

        public void Lock(AchievementIndex index)
        {
            AchievementProgressData data = GetData(index) ?? AddData(index);
            data.IsUnlocked = false;
        }

        public uint GetUnlockedCount()
        {
            uint count = 0;

            for (int i = 0; i < this.Datas.Length; i++)
            {
                if (this.Datas[i] != null && this.Datas[i].IsUnlocked)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
