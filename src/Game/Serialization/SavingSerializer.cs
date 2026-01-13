/*
 * Copyright (C) 2026  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
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
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Constants;
using StardustSandbox.Enums.Serialization;
using StardustSandbox.Extensions;
using StardustSandbox.IO;
using StardustSandbox.Managers;
using StardustSandbox.Serialization.Saving;
using StardustSandbox.Serialization.Saving.Data;
using StardustSandbox.WorldSystem;

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace StardustSandbox.Serialization
{
    internal static class SavingSerializer
    {
        private static readonly MessagePackSerializerOptions options =
            MessagePackSerializerOptions.Standard
                .WithResolver(StandardResolver.Instance)
                .WithSecurity(MessagePackSecurity.UntrustedData)
                .WithCompression(MessagePackCompression.Lz4BlockArray);

        internal static void Save(ActorManager actorManager, World world, GraphicsDevice graphicsDevice)
        {
            string filename = Path.Combine(SSDirectory.Worlds, string.Concat(world.Information.Name, IOConstants.SAVE_FILE_EXTENSION));

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            using FileStream fs = new(filename, FileMode.Create, FileAccess.Write);
            using ZipArchive zip = new(fs, ZipArchiveMode.Create);

            Write(zip, IOConstants.SAVE_ENTRY_THUMBNAIL, new Texture2DData(world.CreateThumbnail(graphicsDevice)));
            Write(zip, IOConstants.SAVE_ENTRY_METADATA, CreateMetadata(world));
            Write(zip, IOConstants.SAVE_ENTRY_MANIFEST, CreateManifest());
            Write(zip, IOConstants.SAVE_ENTRY_PROPERTIES, CreateProperties(world));
            Write(zip, IOConstants.SAVE_ENTRY_ENVIRONMENT, CreateEnvironment(world));
            Write(zip, IOConstants.SAVE_ENTRY_CONTENT, CreateContent(actorManager, world));
        }

        internal static SaveFile Load(string name, LoadFlags flags)
        {
            string filename = Path.Combine(SSDirectory.Worlds, string.Concat(name, IOConstants.SAVE_FILE_EXTENSION));

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

        internal static IEnumerable<SaveFile> LoadAll(LoadFlags flags)
        {
            foreach (string filename in Directory.EnumerateFiles(SSDirectory.Worlds, string.Concat("*", IOConstants.SAVE_FILE_EXTENSION), SearchOption.TopDirectoryOnly))
            {
                yield return Load(Path.GetFileNameWithoutExtension(filename), flags);
            }
        }

        internal static void Delete(string name)
        {
            string filename = Path.Combine(SSDirectory.Worlds, string.Concat(name, IOConstants.SAVE_FILE_EXTENSION));

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
        }

        private static void Write<T>(ZipArchive zip, string entryName, T data)
        {
            ZipArchiveEntry entry = zip.CreateEntry(entryName, CompressionLevel.SmallestSize);

            using Stream stream = entry.Open();
            MessagePackSerializer.Serialize(stream, data, options);
        }

        private static T LoadPart<T>(ZipArchive zip, string entryName)
        {
            ZipArchiveEntry entry = zip.GetEntry(entryName);
            using Stream stream = entry.Open();

            T value = MessagePackSerializer.Deserialize<T>(stream, options);

            return value;
        }

        private static Metadata CreateMetadata(World world)
        {
            return new()
            {
                Name = world.Information.Name,
                Description = world.Information.Description
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

        private static PropertyData CreateProperties(World world)
        {
            return new()
            {
                Width = world.Information.Size.X,
                Height = world.Information.Size.Y
            };
        }

        private static EnvironmentData CreateEnvironment(World world)
        {
            return new()
            {
                CurrentTime = world.Time.CurrentTime,
                IsFrozen = world.Time.IsFrozen
            };
        }

        private static ContentData CreateContent(ActorManager actorManager, World world)
        {
            return new()
            {
                Slots = CreateSlotData(world, world.Information.Size),
                Actors = actorManager.Serialize(),
            };
        }

        private static SlotData[] CreateSlotData(World world, in Point size)
        {
            List<SlotData> slots = [];

            for (int y = 0; y < size.Y; y++)
            {
                for (int x = 0; x < size.X; x++)
                {
                    Point point = new(x, y);

                    if (world.IsEmptySlot(point))
                    {
                        continue;
                    }

                    slots.Add(new SlotData(world.GetSlot(point)));
                }
            }

            return [.. slots];
        }
    }
}

