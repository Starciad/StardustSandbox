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

using StardustSandbox.Core.Serialization.Worlds.StorageModels;

namespace StardustSandbox.Core.Serialization
{
    internal sealed partial class WorldSerializer
    {
        private Worlds.Formats.V1.ContentData CreateContent()
        {
            ContentStorageModel contentStorageModel = new()
            {
                Slots = this.world.SerializationHelper.Serialize(),
                Actors = this.actorManager.Serialize(),
            };

            return this.contentMapper.ToData(contentStorageModel);
        }

        private Worlds.Formats.V1.EnvironmentData CreateEnvironment()
        {
            EnvironmentStorageModel environmentStorageModel = new()
            {
                CurrentTime = this.world.Time.CurrentTime,
                IsFrozen = this.world.Time.IsFrozen,
            };

            return this.environmentMapper.ToData(environmentStorageModel);
        }

        private Worlds.Formats.V1.ManifestData CreateManifest()
        {
            ManifestStorageModel manifestStorageModel = new()
            {

            };

            return this.manifestMapper.ToData(manifestStorageModel);
        }

        private Worlds.Formats.V1.PropertyData CreateProperties()
        {
            PropertyStorageModel propertyStorageModel = new()
            {
                Width = this.world.TileMap.Width,
                Height = this.world.TileMap.Height,
            };

            return this.propertyMapper.ToData(propertyStorageModel);
        }
    }
}
