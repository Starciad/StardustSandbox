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
using MessagePack.Resolvers;

using Microsoft.Xna.Framework;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.Serialization;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Serialization.Worlds;
using StardustSandbox.Core.WorldSystem;

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace StardustSandbox.Core.Serialization
{
    internal sealed class WorldSerializer
    {
        private readonly MessagePackSerializerOptions options = MessagePackSerializerOptions.Standard
            .WithResolver(CompositeResolver.Create(StandardResolver.Instance, ContractlessStandardResolver.Instance))
            .WithSecurity(MessagePackSecurity.UntrustedData)
            .WithCompression(MessagePackCompression.Lz4BlockArray)
            .WithAllowAssemblyVersionMismatch(true);

        private readonly ActorManager actorManager;
        private readonly GraphicsDeviceManager graphicsDeviceManager;
        private readonly World world;

        internal WorldSerializer(ActorManager actorManager, GraphicsDeviceManager graphicsDeviceManager, World world)
        {
            this.actorManager = actorManager;
            this.graphicsDeviceManager = graphicsDeviceManager;
            this.world = world;
        }

        internal void Save()
        {
            string filename = Path.Combine(IO.Directory.Worlds, string.Concat(this.world.Name, IOConstants.SAVE_FILE_EXTENSION));

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            using FileStream fs = new(filename, FileMode.Create, FileAccess.Write);
            using ZipArchive zip = new(fs, ZipArchiveMode.Create);

            Write(zip, IOConstants.SAVE_ENTRY_THUMBNAIL, new Texture2DData(this.world.TileMap.CreateThumbnail(this.graphicsDeviceManager.GraphicsDevice)));
            Write(zip, IOConstants.SAVE_ENTRY_METADATA, CreateMetadata());
            Write(zip, IOConstants.SAVE_ENTRY_MANIFEST, CreateManifest());
            Write(zip, IOConstants.SAVE_ENTRY_PROPERTIES, CreateProperties());
            Write(zip, IOConstants.SAVE_ENTRY_ENVIRONMENT, CreateEnvironment());
            Write(zip, IOConstants.SAVE_ENTRY_CONTENT, CreateContent());
        }

        internal WorldSaveFile Load(string name, LoadFlags flags)
        {
            string filename = Path.Combine(IO.Directory.Worlds, string.Concat(name, IOConstants.SAVE_FILE_EXTENSION));

            using FileStream fs = new(filename, FileMode.Open, FileAccess.Read);
            using ZipArchive zip = new(fs, ZipArchiveMode.Read);

            return new()
            {
                ThumbnailTextureData = flags.HasFlag(LoadFlags.Thumbnail) ? LoadPart<Texture2DData>(zip, IOConstants.SAVE_ENTRY_THUMBNAIL) : null,
                Metadata = flags.HasFlag(LoadFlags.Metadata) ? LoadPart<Metadata>(zip, IOConstants.SAVE_ENTRY_METADATA) : null,
                Manifest = flags.HasFlag(LoadFlags.Manifest) ? LoadPart<ManifestData>(zip, IOConstants.SAVE_ENTRY_MANIFEST) : null,
                Properties = flags.HasFlag(LoadFlags.Properties) ? LoadPart<PropertyData>(zip, IOConstants.SAVE_ENTRY_PROPERTIES) : null,
                Environment = flags.HasFlag(LoadFlags.Environment) ? LoadPart<EnvironmentData>(zip, IOConstants.SAVE_ENTRY_ENVIRONMENT) : null,
                Content = flags.HasFlag(LoadFlags.Content) ? LoadPart<ContentData>(zip, IOConstants.SAVE_ENTRY_CONTENT) : null
            };
        }

        internal IEnumerable<WorldSaveFile> LoadAll(LoadFlags flags)
        {
            foreach (string filename in Directory.EnumerateFiles(IO.Directory.Worlds, string.Concat("*", IOConstants.SAVE_FILE_EXTENSION), SearchOption.TopDirectoryOnly))
            {
                yield return Load(Path.GetFileNameWithoutExtension(filename), flags);
            }
        }

        internal void Delete(string name)
        {
            string filename = Path.Combine(IO.Directory.Worlds, string.Concat(name, IOConstants.SAVE_FILE_EXTENSION));

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
        }

        private void Write<T>(ZipArchive zip, string entryName, T data)
        {
            ZipArchiveEntry entry = zip.CreateEntry(entryName, CompressionLevel.SmallestSize);

            using Stream stream = entry.Open();
            MessagePackSerializer.Serialize(stream, data, this.options);
        }

        private T LoadPart<T>(ZipArchive zip, string entryName)
        {
            try
            {
                ZipArchiveEntry entry = zip.GetEntry(entryName);

                if (entry == null)
                {
                    return default;
                }

                using Stream stream = entry.Open();
                return MessagePackSerializer.Deserialize<T>(stream, this.options);
            }
            catch (MessagePackSerializationException)
            {
                return default;
            }
            catch (Exception)
            {
                return default;
            }
        }

        private Metadata CreateMetadata()
        {
            return new()
            {
                Name = this.world.Name,
                Description = this.world.Description
            };
        }

        private static ManifestData CreateManifest()
        {
            return new()
            {
                GameVersion = GameConstants.VERSION,
                CreationTimestamp = DateTime.Now
            };
        }

        private PropertyData CreateProperties()
        {
            return new()
            {
                Width = this.world.TileMap.Width,
                Height = this.world.TileMap.Height
            };
        }

        private EnvironmentData CreateEnvironment()
        {
            return new()
            {
                CurrentTime = this.world.Time.CurrentTime,
                IsFrozen = this.world.Time.IsFrozen,
                Temperatures = this.world.Temperature.Serialize(),
            };
        }

        private ContentData CreateContent()
        {
            return new()
            {
                Slots = this.world.SerializationHelper.Serialize(),
                Actors = this.actorManager.Serialize(),
            };
        }
    }
}

