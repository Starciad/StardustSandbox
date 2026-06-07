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

using StardustSandbox.Core.Enums.Indexers;
using StardustSandbox.Core.Serialization.Data.Progress.Formats.V1;
using StardustSandbox.Core.Serialization.Data.Progress.StorageModels;
using StardustSandbox.Core.Serialization.Mappers;

using System.Collections.Generic;

namespace StardustSandbox.Core.Serialization.Data.Progress.Mappers
{
    internal sealed class AchievementMapper : Mapper<AchievementStorageModel, AchievementsData>
    {
        internal override AchievementsData ToData(AchievementStorageModel storageModel)
        {
            Dictionary<byte, bool> datas = [];

            foreach (KeyValuePair<AchievementIndex, bool> pair in storageModel.Datas)
            {
                datas.Add((byte)pair.Key, pair.Value);
            }

            return new()
            {
                Datas = datas
            };
        }

        internal override AchievementStorageModel ToStorageModel(AchievementsData data)
        {
            Dictionary<AchievementIndex, bool> datas = [];

            foreach (KeyValuePair<byte, bool> pair in data.Datas)
            {
                datas.Add((AchievementIndex)pair.Key, pair.Value);
            }

            return new()
            {
                Datas = datas
            };
        }
    }
}
