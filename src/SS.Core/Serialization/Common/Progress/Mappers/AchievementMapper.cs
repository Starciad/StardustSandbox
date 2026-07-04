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
using StardustSandbox.Core.Interfaces.Serialization.Morph;
using StardustSandbox.Core.Serialization.Common.Progress.Data.V1;
using StardustSandbox.Core.Serialization.Common.Progress.StorageModels;

using System.Collections.Generic;

namespace StardustSandbox.Core.Serialization.Common.Progress.Mappers
{
    internal sealed class AchievementMapper : IMapper
    {
        public IData ToData(IStorageModel value)
        {
            AchievementStorageModel storageModel = (AchievementStorageModel)value;
            Dictionary<byte, bool> statuses = [];

            foreach (KeyValuePair<AchievementIndex, bool> pair in storageModel.AchievementStatuses)
            {
                statuses.Add((byte)pair.Key, pair.Value);
            }

            return new AchievementData()
            {
                AchievementStatuses = statuses
            };
        }

        public IStorageModel ToStorageModel(IData value)
        {
            AchievementData data = (AchievementData)value;
            Dictionary<AchievementIndex, bool> statuses = [];

            foreach (KeyValuePair<byte, bool> pair in data.AchievementStatuses)
            {
                statuses.Add((AchievementIndex)pair.Key, pair.Value);
            }

            return new AchievementStorageModel()
            {
                AchievementStatuses = statuses
            };
        }
    }
}
