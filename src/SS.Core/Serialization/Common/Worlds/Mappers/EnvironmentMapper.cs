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

using StardustSandbox.Core.Interfaces.Serialization.Morph;
using StardustSandbox.Core.Serialization.Common.Worlds.Data.V1;
using StardustSandbox.Core.Serialization.Common.Worlds.StorageModels;

namespace StardustSandbox.Core.Serialization.Common.Worlds.Mappers
{
    internal sealed class EnvironmentMapper : IMapper
    {
        public IData ToData(IStorageModel value)
        {
            EnvironmentStorageModel environmentStorageModel = (EnvironmentStorageModel)value;

            return new EnvironmentData()
            {
                CurrentTime = environmentStorageModel.CurrentTime,
                IsFrozen = environmentStorageModel.IsFrozen,
            };
        }

        public IStorageModel ToStorageModel(IData value)
        {
            EnvironmentData environmentData = (EnvironmentData)value;

            return new EnvironmentStorageModel()
            {
                CurrentTime = environmentData.CurrentTime,
                IsFrozen = environmentData.IsFrozen,
            };
        }
    }
}
