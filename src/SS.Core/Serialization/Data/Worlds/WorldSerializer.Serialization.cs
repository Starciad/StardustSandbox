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

using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Serialization.Data.Worlds.StorageModels;

using System;

namespace StardustSandbox.Core.Serialization
{
    internal sealed partial class WorldSerializer
    {
        private ContentStorageModel SerializeContent()
        {
            return new()
            {
                Actors = this.actorManager.Serialize(),
                Slots = this.world.SerializationHelper.Serialize(),
            };
        }

        private EnvironmentStorageModel SerializeEnvironment()
        {
            return new()
            {
                CurrentTime = this.world.Time.CurrentTime,
                IsFrozen = this.world.Time.IsFrozen,
            };
        }

        private ManifestStorageModel SerializeManifest()
        {
            return new()
            {
                CreationTimestamp = DateTime.Now,
                Description = this.world.Description,
                LastModifiedTimestamp = DateTime.Now,
                Name = this.world.Name,
            };
        }

        private PropertyStorageModel SerializeProperties()
        {
            return new()
            {
                Width = this.world.TileMap.Width,
                Height = this.world.TileMap.Height,
            };
        }

        private Texture2DStorageModel SerializeThumbnail()
        {
            return new(this.world.TileMap.CreateThumbnail(this.graphicsDeviceManager.GraphicsDevice));
        }
    }
}
