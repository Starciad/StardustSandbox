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

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Serialization.Data.Worlds.Formats;
using StardustSandbox.Core.Serialization.Data.Worlds.Formats.V1;
using StardustSandbox.Core.Serialization.Data.Worlds.StorageModels;

using System.Collections.Generic;

namespace StardustSandbox.Core.Serialization
{
    internal sealed partial class WorldSerializer
    {
        private ContentData SerializeContent()
        {
            ContentStorageModel contentStorageModel = new()
            {
                Actors = this.actorManager.Serialize(),
                Slots = this.world.SerializationHelper.Serialize(),
            };

            return this.contentMapper.ToData(contentStorageModel);
        }

        private EnvironmentData SerializeEnvironment()
        {
            EnvironmentStorageModel environmentStorageModel = new()
            {
                CurrentTime = this.world.Time.CurrentTime,
                IsFrozen = this.world.Time.IsFrozen,
            };

            return this.environmentMapper.ToData(environmentStorageModel);
        }

        private ManifestData SerializeManifest()
        {
            ManifestStorageModel manifestStorageModel = new()
            {

            };

            return this.manifestMapper.ToData(manifestStorageModel);
        }

        private PropertyData SerializeProperties()
        {
            PropertyStorageModel propertyStorageModel = new()
            {
                Width = this.world.TileMap.Width,
                Height = this.world.TileMap.Height,
            };

            return this.propertyMapper.ToData(propertyStorageModel);
        }

        private Texture2DData SerializeThumbnail()
        {
            Texture2DStorageModel texture2DStorageModel = new(this.world.TileMap.CreateThumbnail(this.graphicsDeviceManager.GraphicsDevice));

            return this.texture2DMapper.ToData(texture2DStorageModel);
        }

        private VersionData SerializeVersion()
        {
            VersionStorageModel versionStorageModel = new()
            {
                ComponentVersions = new Dictionary<string, int>()
                {
                    [IOConstants.SAVE_ENTRY_CONTENT_ID] = IOConstants.SAVE_CONTENT_COMPONENT_VERSION,
                    [IOConstants.SAVE_ENTRY_ENVIRONMENT_ID] = IOConstants.SAVE_ENVIRONMENT_COMPONENT_VERSION,
                    [IOConstants.SAVE_ENTRY_MANIFEST_ID] = IOConstants.SAVE_MANIFEST_COMPONENT_VERSION,
                    [IOConstants.SAVE_ENTRY_PROPERTIES_ID] = IOConstants.SAVE_PROPERTIES_COMPONENT_VERSION,
                    [IOConstants.SAVE_ENTRY_THUMBNAIL_ID] = IOConstants.SAVE_THUMBNAIL_COMPONENT_VERSION,
                }
            };

            return this.versionMapper.ToData(versionStorageModel);
        }
    }
}
