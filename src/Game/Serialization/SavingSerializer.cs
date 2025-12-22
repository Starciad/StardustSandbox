using MessagePack;
using MessagePack.Resolvers;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Constants;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.Serialization;
using StardustSandbox.Enums.World;
using StardustSandbox.Extensions;
using StardustSandbox.IO;
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
        private static readonly MessagePackSerializerOptions options = MessagePackSerializerOptions.Standard.WithResolver(ContractlessStandardResolver.Instance);

        internal static void Save(World world, GraphicsDevice graphicsDevice)
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
            Write(zip, IOConstants.SAVE_ENTRY_RESOURCES, CreateResources(world));
            Write(zip, IOConstants.SAVE_ENTRY_ENVIRONMENT, CreateEnvironment(world));
            Write(zip, IOConstants.SAVE_ENTRY_CONTENT, CreateContent(world));
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
                Resources = flags.HasFlag(LoadFlags.Resources) ? LoadPart<ResourceData>(zip, IOConstants.SAVE_ENTRY_RESOURCES) : null,
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
            ZipArchiveEntry entry = zip.CreateEntry(entryName, CompressionLevel.Optimal);

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
                FormatVersion = IOConstants.SAVE_FILE_VERSION,
                Version = GameConstants.VERSION,
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

        private static ResourceData CreateResources(World world)
        {
            return new()
            {
                Elements = GetAllWorldDistinctElements(world)
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

        private static ContentData CreateContent(World world)
        {
            return new()
            {
                Slots = CreateSlotsData(world, world.Information.Size)
            };
        }

        private static HashSet<ElementIndex> GetAllWorldDistinctElements(World world)
        {
            HashSet<ElementIndex> elements = [];

            for (int y = 0; y < world.Information.Size.Y; y++)
            {
                for (int x = 0; x < world.Information.Size.X; x++)
                {
                    Point point = new(x, y);

                    if (!world.TryGetSlot(point, out Slot slot))
                    {
                        continue;
                    }

                    ProcessLayer(slot.GetLayer(Layer.Foreground));
                    ProcessLayer(slot.GetLayer(Layer.Background));
                }
            }

            return elements;

            void ProcessLayer(SlotLayer layer)
            {
                if (layer.HasState(ElementStates.IsEmpty))
                {
                    return;
                }

                _ = elements.Add(layer.Element.Index);

                if (layer.StoredElement != null)
                {
                    _ = elements.Add(layer.StoredElement.Index);
                }
            }
        }

        private static List<SlotData> CreateSlotsData(World world, in Point size)
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

            return slots;
        }
    }
}
