using MessagePack;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.IO;
using StardustSandbox.Core.IO;
using StardustSandbox.Core.IO.Files.World;
using StardustSandbox.Core.IO.Files.World.Data;
using StardustSandbox.Core.Mathematics.Primitives;
using StardustSandbox.Core.World;

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace StardustSandbox.Core.Managers.IO
{
    public static class SWorldSavingManager
    {
        public static void Serialize(string identifier, string description, SWorld world, GraphicsDevice graphicsDevice)
        {
            // Streams
            using FileStream outputSaveFile = new(Path.Combine(SDirectory.Worlds, string.Concat(identifier, SFileExtensionConstants.WORLD)), FileMode.Create, FileAccess.Write, FileShare.Write);
            using MemoryStream saveFileMemoryStream = new();
            using ZipArchive saveFileZipArchive = CreateZipFile(CreateWorldSaveFile(identifier, description, world, graphicsDevice), saveFileMemoryStream);

            // Saving
            saveFileMemoryStream.WriteTo(outputSaveFile);
        }

        private static SWorldSaveFile CreateWorldSaveFile(string name, string description, SWorld world, GraphicsDevice graphicsDevice)
        {
            DateTime currentDateTime = DateTime.Now;

            return new()
            {
                ThumbnailTexture = CreateWorldThumbnail(graphicsDevice, world),

                Metadata = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = name,
                    Description = description,
                    Version = SFileConstants.WORLD_SAVE_FILE_VERSION,
                    CreationTimestamp = currentDateTime,
                    LastUpdateTimestamp = currentDateTime,
                },

                World = new()
                {
                    Width = world.Infos.Size.Width,
                    Height = world.Infos.Size.Height,
                    Slots = CreateWorldSlotsData(world, world.Infos.Size),
                },
            };
        }

        private static Texture2D CreateWorldThumbnail(GraphicsDevice graphicsDevice, SWorld world)
        {
            return new Texture2D(graphicsDevice, 1, 1, true, SurfaceFormat.Color, 1);
        }

        private static SWorldSlotData[] CreateWorldSlotsData(SWorld world, SSize2 worldSize)
        {
            List<SWorldSlotData> slotData = [];

            for (int y = 0; y < worldSize.Height; y++)
            {
                for (int x = 0; x < worldSize.Width; x++)
                {
                    Point position = new(x, y);

                    if (!world.IsEmptyElementSlot(position))
                    {
                        slotData.Add(new(world.GetElementSlot(position)));
                    }
                }
            }

            return [.. slotData];
        }

        private static ZipArchive CreateZipFile(SWorldSaveFile worldSaveFile, MemoryStream memoryStream)
        {
            ZipArchive saveFileZipArchive = new(memoryStream, ZipArchiveMode.Create);

            // ROOT/thumbnail.png
            using (Stream thumbnailStreamWriter = saveFileZipArchive.CreateEntry(string.Concat("thumbnail", SFileExtensionConstants.PNG)).Open())
            {
                worldSaveFile.ThumbnailTexture.SaveAsPng(thumbnailStreamWriter, SWorldConstants.WORLD_THUMBNAIL_SIZE.Width, SWorldConstants.WORLD_THUMBNAIL_SIZE.Height);
            }

            // ROOT/metadata.pdworlddata
            using (Stream metadataStreamWriter = saveFileZipArchive.CreateEntry(string.Concat("metadata", SFileExtensionConstants.WORLD_DATA)).Open())
            {
                metadataStreamWriter.Write(MessagePackSerializer.Serialize(worldSaveFile.Metadata));
            }

            // ROOT/data/world.pdworlddata
            using (Stream worldDataStreamWriter = saveFileZipArchive.CreateEntry(Path.Combine("data", string.Concat("world", SFileExtensionConstants.WORLD_DATA))).Open())
            {
                worldDataStreamWriter.Write(MessagePackSerializer.Serialize(worldSaveFile.World));
            }

            return saveFileZipArchive;
        }
    }
}
