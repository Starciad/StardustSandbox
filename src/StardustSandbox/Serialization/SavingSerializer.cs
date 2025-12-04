using MessagePack;
using MessagePack.Resolvers;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Constants;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.World;
using StardustSandbox.Extensions;
using StardustSandbox.IO;
using StardustSandbox.Serialization.Saving;
using StardustSandbox.Serialization.Saving.Data;
using StardustSandbox.World;

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace StardustSandbox.Serialization
{
    internal static class SavingSerializer
    {
        private static readonly MessagePackSerializerOptions MapOptions =
            MessagePackSerializerOptions.Standard.WithResolver(ContractlessStandardResolver.Instance);

        internal static void Serialize(GameWorld world, GraphicsDevice graphicsDevice)
        {
            string filename = Path.Combine(SSDirectory.Worlds, string.Concat(world.Information.Name, IOConstants.SAVE_FILE_EXTENSION));

            using MemoryStream saveFileMemoryStream = new();
            using FileStream outputSaveFile = new(filename, FileMode.Create, FileAccess.Write, FileShare.Write);

            WriteZipFile(CreateWorldSaveFile(filename, world, graphicsDevice), saveFileMemoryStream);

            _ = saveFileMemoryStream.Seek(0, SeekOrigin.Begin);
            saveFileMemoryStream.WriteTo(outputSaveFile);
        }

        internal static SaveFile CreateWorldSaveFile(string filename, GameWorld world, GraphicsDevice graphicsDevice)
        {
            return new()
            {
                ThumbnailTexture = world.CreateThumbnail(graphicsDevice),

                Metadata = new()
                {
                    Filename = filename,
                    Identifier = world.Information.Identifier,
                    Name = world.Information.Name,
                    Description = world.Information.Description,
                },

                Manifest = new()
                {
                    FormatVersion = IOConstants.SAVE_FILE_VERSION,
                    GameVersion = GameConstants.VERSION,
                    CreationTimestamp = DateTime.Now,
                },

                Properties = new()
                {
                    Width = world.Information.Size.X,
                    Height = world.Information.Size.Y,
                },

                Resources = new()
                {
                    Elements = GetAllWorldDistinctElements(world),
                },

                Environment = new()
                {
                    Time = new()
                    {
                        CurrentTime = world.Time.CurrentTime,
                        IsFrozen = world.Time.IsFrozen,
                    }
                },

                Content = new()
                {
                    Slots = CreateSlotsData(world, world.Information.Size),
                },
            };
        }

        internal static SaveFile LoadSaveFile(string name, GraphicsDevice graphicsDevice)
        {
            string filename = Path.Combine(SSDirectory.Worlds, string.Concat(name, IOConstants.SAVE_FILE_EXTENSION));
            
            using FileStream inputSaveFile = new(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            using ZipArchive saveFileZipArchive = new(inputSaveFile, ZipArchiveMode.Read);

            SaveFile saveFile = ReadZipFile(saveFileZipArchive, graphicsDevice);

            saveFile.Metadata.Filename = filename;

            return saveFile;
        }

        internal static SaveFile[] LoadAllSavedWorldData(GraphicsDevice graphicsDevice)
        {
            string[] filenames = Directory.GetFiles(SSDirectory.Worlds, string.Concat('*', IOConstants.SAVE_FILE_EXTENSION), SearchOption.TopDirectoryOnly);
            int length = filenames.Length;

            SaveFile[] saveFiles = new SaveFile[length];

            for (int i = 0; i < length; i++)
            {
                saveFiles[i] = LoadSaveFile(Path.GetFileNameWithoutExtension(filenames[i]), graphicsDevice);
            }

            return saveFiles;
        }

        internal static void DeleteSavedFile(SaveFile saveFile)
        {
            if (File.Exists(saveFile.Metadata.Filename))
            {
                File.Delete(saveFile.Metadata.Filename);
            }
        }

        private static ElementIndex[] GetAllWorldDistinctElements(GameWorld world)
        {
            HashSet<ElementIndex> distinctElements = [];

            void TryAddElementIdentifier(ElementIndex index)
            {
                _ = distinctElements.Add(index);
            }

            void ProcessSlot(Point position, LayerType layer)
            {
                if (!world.TryGetSlot(position, out Slot slot))
                {
                    return;
                }

                SlotLayer worldSlotLayer = slot.GetLayer(layer);

                if (worldSlotLayer.HasState(ElementStates.IsEmpty))
                {
                    return;
                }

                TryAddElementIdentifier(worldSlotLayer.Element.Index);

                if (worldSlotLayer.StoredElement != null)
                {
                    TryAddElementIdentifier(worldSlotLayer.StoredElement.Index);
                }
            }

            for (int y = 0; y < world.Information.Size.Y; y++)
            {
                for (int x = 0; x < world.Information.Size.X; x++)
                {
                    Point position = new(x, y);

                    if (!world.IsEmptySlot(position))
                    {
                        ProcessSlot(position, LayerType.Foreground);
                        ProcessSlot(position, LayerType.Background);
                    }
                }
            }

            return [.. distinctElements];
        }

        private static SlotData[] CreateSlotsData(GameWorld world, Point worldSize)
        {
            List<SlotData> slots = [];

            for (int y = 0; y < worldSize.Y; y++)
            {
                for (int x = 0; x < worldSize.X; x++)
                {
                    Point position = new(x, y);

                    if (!world.IsEmptySlot(position))
                    {
                        slots.Add(new(world.GetSlot(position)));
                    }
                }
            }

            return [.. slots];
        }

        private static void WriteZipFile(SaveFile saveFile, MemoryStream memoryStream)
        {
            using ZipArchive saveFileZipArchive = new(memoryStream, ZipArchiveMode.Create, true);

            // Write thumbnail
            using (Stream entryStream = saveFileZipArchive.CreateEntry(IOConstants.SAVE_FILE_THUMBNAIL).Open())
            {
                saveFile.ThumbnailTexture.SaveAsPng(entryStream, saveFile.ThumbnailTexture.Width, saveFile.ThumbnailTexture.Height);
            }

            // Write save file data
            using (Stream entryStream = saveFileZipArchive.CreateEntry(IOConstants.SAVE_FILE_DATA).Open())
            {
                MessagePackSerializer.Serialize(entryStream, saveFile, MapOptions);
            }
        }

        private static SaveFile ReadZipFile(ZipArchive zipArchive, GraphicsDevice graphicsDevice)
        {
            SaveFile saveFile = null;
            Texture2D thumbnailTexture = null;

            foreach (ZipArchiveEntry entry in zipArchive.Entries)
            {
                switch (entry.FullName)
                {
                    case IOConstants.SAVE_FILE_THUMBNAIL:
                        using (Stream stream = entry.Open())
                        {
                            thumbnailTexture = Texture2D.FromStream(graphicsDevice, stream);
                        }
                        break;

                    case IOConstants.SAVE_FILE_DATA:
                        using (Stream stream = entry.Open())
                        {
                            saveFile = MessagePackSerializer.Deserialize<SaveFile>(stream, MessagePackSerializerOptions.Standard);
                        }
                        break;

                    default:
                        break;
                }
            }

            saveFile ??= new();

            if (thumbnailTexture != null)
            {
                saveFile.ThumbnailTexture = thumbnailTexture;
            }

            return saveFile;
        }
    }
}
