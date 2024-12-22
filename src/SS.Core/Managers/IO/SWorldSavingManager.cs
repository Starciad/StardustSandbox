using MessagePack;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.IO;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.IO;
using StardustSandbox.Core.IO.Files.World;
using StardustSandbox.Core.IO.Files.World.Data;
using StardustSandbox.Core.Mathematics.Primitives;
using StardustSandbox.Core.World.Data;

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace StardustSandbox.Core.Managers.IO
{
    public static class SWorldSavingManager
    {
        private static readonly Dictionary<string, Action<ZipArchiveEntry, SWorldSaveFile, GraphicsDevice>> fileHandlers = new(StringComparer.InvariantCulture)
        {
            {
                SFileConstants.WORLD_SAVE_FILE_THUMBNAIL,
                (entry, saveFile, graphicsDevice) =>
                {
                    using Stream stream = entry.Open();
                    saveFile.ThumbnailTexture = Texture2D.FromStream(graphicsDevice, stream);
                }
            },
            {
                SFileConstants.WORLD_SAVE_FILE_METADATA,
                (entry, saveFile, _) =>
                {
                    using Stream stream = entry.Open();
                    saveFile.Metadata = MessagePackSerializer.Deserialize<SWorldSaveFileMetadata>(stream, MessagePackSerializerOptions.Standard);
                }
            },
            {
                Path.Combine(SDirectoryConstants.WORLD_SAVE_FILE_DATA, SFileConstants.WORLD_SAVE_FILE_DATA_WORLD),
                (entry, saveFile, _) =>
                {
                    using Stream stream = entry.Open();
                    saveFile.World = MessagePackSerializer.Deserialize<SWorldData>(stream, MessagePackSerializer.DefaultOptions);
                }
            }
        };

        public static void Serialize(ISWorld world, GraphicsDevice graphicsDevice)
        {
            // Paths
            string filename = Path.Combine(SDirectory.Worlds, string.Concat(world.Infos.Name, SFileExtensionConstants.WORLD));

            // Streams
            using MemoryStream saveFileMemoryStream = new();
            using FileStream outputSaveFile = new(filename, FileMode.Create, FileAccess.Write, FileShare.Write);

            // Saving
            CreateZipFile(CreateWorldSaveFile(world, graphicsDevice), saveFileMemoryStream);

            // Write
            _ = saveFileMemoryStream.Seek(0, SeekOrigin.Begin);
            saveFileMemoryStream.WriteTo(outputSaveFile);
        }

        public static void Deserialize(string name, ISWorld world, GraphicsDevice graphicsDevice)
        {
            // Read
            SWorldSaveFile worldSaveFile = LoadWorldSaveData(name, graphicsDevice);

            // Apply
            world.LoadFromWorldSaveFile(worldSaveFile);
        }

        public static SWorldSaveFile CreateWorldSaveFile(ISWorld world, GraphicsDevice graphicsDevice)
        {
            DateTime currentDateTime = DateTime.Now;

            return new()
            {
                ThumbnailTexture = world.CreateThumbnail(graphicsDevice),

                Metadata = new()
                {
                    Identifier = world.Infos.Identifier,
                    Name = world.Infos.Name,
                    Description = world.Infos.Description,
                    Version = SFileConstants.WORLD_SAVE_FILE_VERSION,
                    CreationTimestamp = currentDateTime,
                },

                World = new()
                {
                    Width = world.Infos.Size.Width,
                    Height = world.Infos.Size.Height,
                    Slots = CreateWorldSlotsData(world, world.Infos.Size),
                },
            };
        }

        public static SWorldSaveFile LoadWorldSaveData(string name, GraphicsDevice graphicsDevice)
        {
            // Paths
            string filename = Path.Combine(SDirectory.Worlds, string.Concat(name, SFileExtensionConstants.WORLD));

            // Streams
            using FileStream inputSaveFile = new(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            using ZipArchive saveFileZipArchive = new(inputSaveFile, ZipArchiveMode.Read);

            // Read
            return ReadZipFile(saveFileZipArchive, graphicsDevice);
        }

        public static SWorldSaveFile[] LoadAllSavedWorldData(GraphicsDevice graphicsDevice)
        {
            string[] files = Directory.GetFiles(SDirectory.Worlds, string.Concat('*', SFileExtensionConstants.WORLD), SearchOption.TopDirectoryOnly);
            SWorldSaveFile[] saveFiles = new SWorldSaveFile[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                saveFiles[i] = LoadWorldSaveData(Path.GetFileNameWithoutExtension(files[i]), graphicsDevice);
            }

            return saveFiles;
        }

        public static void DeleteSavedFile(string name)
        {
            string filename = Path.Combine(SDirectory.Worlds, string.Concat(name, SFileExtensionConstants.WORLD));

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
        }

        // ============================================== //
        // Utilities

        private static SWorldSlotData[] CreateWorldSlotsData(ISWorld world, SSize2 worldSize)
        {
            List<SWorldSlotData> slotData = [];

            for (int y = 0; y < worldSize.Height; y++)
            {
                for (int x = 0; x < worldSize.Width; x++)
                {
                    Point position = new(x, y);

                    if (!world.IsEmptyWorldSlot(position))
                    {
                        slotData.Add(new(world.GetWorldSlot(position)));
                    }
                }
            }

            return [.. slotData];
        }

        private static void CreateZipFile(SWorldSaveFile worldSaveFile, MemoryStream memoryStream)
        {
            using ZipArchive saveFileZipArchive = new(memoryStream, ZipArchiveMode.Create, true);

            // ROOT/thumbnail
            using (Stream thumbnailStreamWriter = saveFileZipArchive.CreateEntry(SFileConstants.WORLD_SAVE_FILE_THUMBNAIL).Open())
            {
                worldSaveFile.ThumbnailTexture.SaveAsPng(thumbnailStreamWriter, SWorldConstants.WORLD_THUMBNAIL_SIZE.Width, SWorldConstants.WORLD_THUMBNAIL_SIZE.Height);
            }

            // ROOT/metadata
            using (Stream metadataStreamWriter = saveFileZipArchive.CreateEntry(SFileConstants.WORLD_SAVE_FILE_METADATA).Open())
            {
                metadataStreamWriter.Write(MessagePackSerializer.Serialize(worldSaveFile.Metadata));
            }

            // ROOT/data/world
            using Stream worldDataStreamWriter = saveFileZipArchive.CreateEntry(Path.Combine(SDirectoryConstants.WORLD_SAVE_FILE_DATA, SFileConstants.WORLD_SAVE_FILE_DATA_WORLD)).Open();
            worldDataStreamWriter.Write(MessagePackSerializer.Serialize(worldSaveFile.World));
        }

        private static SWorldSaveFile ReadZipFile(ZipArchive zipArchive, GraphicsDevice graphicsDevice)
        {
            SWorldSaveFile saveFile = new();

            foreach (ZipArchiveEntry entry in zipArchive.Entries)
            {
                if (fileHandlers.TryGetValue(entry.FullName, out Action<ZipArchiveEntry, SWorldSaveFile, GraphicsDevice> handler))
                {
                    handler.Invoke(entry, saveFile, graphicsDevice);
                }
            }

            return saveFile;
        }
    }
}
