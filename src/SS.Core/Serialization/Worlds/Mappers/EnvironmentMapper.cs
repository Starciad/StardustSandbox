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

using StardustSandbox.Core.Serialization.Mappers;
using StardustSandbox.Core.Serialization.Worlds.Formats.V1;
using StardustSandbox.Core.Serialization.Worlds.StorageModels;

namespace StardustSandbox.Core.Serialization.Worlds.Mappers
{
    internal sealed class EnvironmentMapper : Mapper<EnvironmentStorageModel, EnvironmentData>
    {
        internal override EnvironmentData ToData(EnvironmentStorageModel storageModel)
        {
            return new()
            {
                CurrentTime = storageModel.CurrentTime,
                IsFrozen = storageModel.IsFrozen,
            };
        }

        internal override EnvironmentStorageModel ToStorageModel(EnvironmentData data)
        {
            return new()
            {
                CurrentTime = data.CurrentTime,
                IsFrozen = data.IsFrozen,
            };
        }
    }
}
