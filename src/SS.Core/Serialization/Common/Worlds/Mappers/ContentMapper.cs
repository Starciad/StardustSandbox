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
    internal sealed class ContentMapper : IMapper
    {
        private readonly ActorMapper actorMapper;
        private readonly SlotMapper slotMapper;

        internal ContentMapper(ActorMapper actorMapper, SlotMapper slotMapper)
        {
            this.actorMapper = actorMapper;
            this.slotMapper = slotMapper;
        }

        public IData ToData(IStorageModel value)
        {
            ContentStorageModel storageModel = (ContentStorageModel)value;

            ActorData[] actors = new ActorData[storageModel.Actors.Length];
            SlotData[] slots = new SlotData[storageModel.Slots.Length];

            for (int i = 0; i < actors.Length; i++)
            {
                actors[i] = (ActorData)this.actorMapper.ToData(storageModel.Actors[i]);
            }

            for (int i = 0; i < slots.Length; i++)
            {
                slots[i] = (SlotData)this.slotMapper.ToData(storageModel.Slots[i]);
            }

            return new ContentData()
            {
                Actors = actors,
                Slots = slots
            };
        }

        public IStorageModel ToStorageModel(IData value)
        {
            ContentData data = (ContentData)value;

            ActorStorageModel[] actors = new ActorStorageModel[data.Actors.Length];
            SlotStorageModel[] slots = new SlotStorageModel[data.Slots.Length];

            for (int i = 0; i < actors.Length; i++)
            {
                actors[i] = (ActorStorageModel)this.actorMapper.ToStorageModel(data.Actors[i]);
            }

            for (int i = 0; i < slots.Length; i++)
            {
                slots[i] = (SlotStorageModel)this.slotMapper.ToStorageModel(data.Slots[i]);
            }

            return new ContentStorageModel()
            {
                Actors = actors,
                Slots = slots
            };
        }
    }
}
