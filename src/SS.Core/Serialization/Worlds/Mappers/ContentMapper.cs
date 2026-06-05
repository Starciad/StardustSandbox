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

using StardustSandbox.Core.Serialization.Worlds.Formats.V1;
using StardustSandbox.Core.Serialization.Worlds.StorageModels;

namespace StardustSandbox.Core.Serialization.Worlds.Mappers
{
    internal sealed class ContentMapper : Mapper<ContentStorageModel, ContentData>
    {
        private readonly ActorMapper actorMapper;
        private readonly SlotMapper slotMapper;

        internal ContentMapper(ActorMapper actorMapper, SlotMapper slotMapper)
        {
            this.actorMapper = actorMapper;
            this.slotMapper = slotMapper;
        }

        internal override ContentData ToData(ContentStorageModel storageModel)
        {
            ActorData[] actors = new ActorData[storageModel.Actors.Length];
            SlotData[] slots = new SlotData[storageModel.Slots.Length];

            for (int i = 0; i < actors.Length; i++)
            {
                actors[i] = this.actorMapper.ToData(storageModel.Actors[i]);
            }

            for (int i = 0; i < slots.Length; i++)
            {
                slots[i] = this.slotMapper.ToData(storageModel.Slots[i]);
            }

            return new()
            {
                Actors = actors,
                Slots = slots
            };
        }

        internal override ContentStorageModel ToStorageModel(ContentData data)
        {
            ActorStorageModel[] actors = new ActorStorageModel[data.Actors.Length];
            SlotStorageModel[] slots = new SlotStorageModel[data.Slots.Length];

            for (int i = 0; i < actors.Length; i++)
            {
                actors[i] = this.actorMapper.ToStorageModel(data.Actors[i]);
            }

            for (int i = 0; i < slots.Length; i++)
            {
                slots[i] = this.slotMapper.ToStorageModel(data.Slots[i]);
            }

            return new()
            {
                Actors = actors,
                Slots = slots
            };
        }
    }
}
