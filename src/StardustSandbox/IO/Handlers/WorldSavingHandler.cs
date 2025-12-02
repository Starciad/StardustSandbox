using MessagePack;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Constants;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.World;
using StardustSandbox.Extensions;
using StardustSandbox.IO.Saving;
using StardustSandbox.IO.Saving.Header;
using StardustSandbox.IO.Saving.World.Content;
using StardustSandbox.IO.Saving.World.Environment;
using StardustSandbox.IO.Saving.World.Information;
using StardustSandbox.WorldSystem;

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace StardustSandbox.IO.Handlers
{
    internal static class WorldSavingHandler
    {
        private static string ThumbnailFilePath => Path.Combine(IOConstants.SAVE_FILE_HEADER_DIRECTORY, IOConstants.SAVE_FILE_THUMBNAIL);
        private static string HeaderMetadataFilePath => Path.Combine(IOConstants.SAVE_FILE_HEADER_DIRECTORY, IOConstants.SAVE_FILE_HEADER_METADATA);
        private static string HeaderInformationFilePath => Path.Combine(IOConstants.SAVE_FILE_HEADER_DIRECTORY, IOConstants.SAVE_FILE_HEADER_INFORMATION);
        private static string WorldInformationFilePath => Path.Combine(IOConstants.SAVE_FILE_WORLD_DIRECTORY, IOConstants.SAVE_FILE_WORLD_INFORMATION);
        private static string WorldResourceElementsFilePath => Path.Combine(IOConstants.SAVE_FILE_WORLD_DIRECTORY, IOConstants.SAVE_FILE_WORLD_RESOURCES_DIRECTORY, IOConstants.SAVE_FILE_WORLD_RESOURCE_ELEMENTS);
        private static string WorldContentSlotsFilePath => Path.Combine(IOConstants.SAVE_FILE_WORLD_DIRECTORY, IOConstants.SAVE_FILE_WORLD_CONTENT_DIRECTORY, IOConstants.SAVE_FILE_WORLD_CONTENT_SLOTS);
        private static string WorldContentEntitiesFilePath => Path.Combine(IOConstants.SAVE_FILE_WORLD_DIRECTORY, IOConstants.SAVE_FILE_WORLD_CONTENT_DIRECTORY, IOConstants.SAVE_FILE_WORLD_CONTENT_ENTITIES);
        private static string WorldEnvironmentTimeFilePath => Path.Combine(IOConstants.SAVE_FILE_WORLD_DIRECTORY, IOConstants.SAVE_FILE_WORLD_ENVIRONMENT_DIRECTORY, IOConstants.SAVE_FILE_WORLD_ENVIRONMENT_TIME);

        internal static void Serialize(World world, GraphicsDevice graphicsDevice)
        {
            // Paths
            string filename = Path.Combine(SSDirectory.Worlds, string.Concat(world.Information.Name, IOConstants.WORLD_FILE_EXTENSION));

            // Streams
            using MemoryStream saveFileMemoryStream = new();
            using FileStream outputSaveFile = new(filename, FileMode.Create, FileAccess.Write, FileShare.Write);

            // Saving
            WriteZipFile(CreateWorldSaveFile(filename, world, graphicsDevice), saveFileMemoryStream);

            // Write
            _ = saveFileMemoryStream.Seek(0, SeekOrigin.Begin);
            saveFileMemoryStream.WriteTo(outputSaveFile);
        }

        internal static SaveFile CreateWorldSaveFile(string filename, World world, GraphicsDevice graphicsDevice)
        {
            DateTime currentDateTime = DateTime.Now;

            SaveFileWorldResources resources = CreateWorldResources(world);

            return new()
            {
                Header = new()
                {
                    ThumbnailTexture = world.CreateThumbnail(graphicsDevice),

                    Metadata = new()
                    {
                        Filename = filename,
                        Identifier = world.Information.Identifier,
                        Name = world.Information.Name,
                        Description = world.Information.Description,
                    },

                    Information = new()
                    {
                        SaveVersion = IOConstants.WORLD_SAVE_FILE_VERSION,
                        GameVersion = GameConstants.VERSION,
                        CreationTimestamp = currentDateTime,
                        LastUpdateTimestamp = currentDateTime,
                    },
                },

                World = new()
                {
                    Information = new()
                    {
                        Width = world.Information.Size.X,
                        Height = world.Information.Size.Y,
                    },

                    Resources = resources,

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
                        Slots = CreateSlotsData(resources, world, world.Information.Size),
                    },
                },
            };
        }

        internal static SaveFile LoadSaveFile(string name, GraphicsDevice graphicsDevice)
        {
            // Paths
            string filename = Path.Combine(SSDirectory.Worlds, string.Concat(name, IOConstants.WORLD_FILE_EXTENSION));

            // Streams
            using FileStream inputSaveFile = new(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            using ZipArchive saveFileZipArchive = new(inputSaveFile, ZipArchiveMode.Read);

            // Read
            return ReadZipFile(saveFileZipArchive, graphicsDevice);
        }

        // Utilities
        internal static IEnumerable<SaveFile> LoadAllSavedWorldData(GraphicsDevice graphicsDevice)
        {
            string[] files = Directory.GetFiles(SSDirectory.Worlds, string.Concat('*', IOConstants.WORLD_FILE_EXTENSION), SearchOption.TopDirectoryOnly);
            SaveFile saveFile;

            for (byte i = 0; i < files.Length; i++)
            {
                try
                {
                    saveFile = LoadSaveFile(Path.GetFileNameWithoutExtension(files[i]), graphicsDevice);
                }
                catch (Exception)
                {
                    continue;
                }

                yield return saveFile;
            }
        }

        internal static void DeleteSavedFile(string name)
        {
            string filename = Path.Combine(SSDirectory.Worlds, string.Concat(name, IOConstants.WORLD_FILE_EXTENSION));

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
        }

        // ============================================== //
        // Utilities

        private static SaveFileWorldResources CreateWorldResources(World world)
        {
            return new()
            {
                Elements = GetAllWorldDistinctElements(world),
            };
        }

        private static SaveFileResourceContainer GetAllWorldDistinctElements(World world)
        {
            SaveFileResourceContainer container = new();

            void TryAddElementIdentifier(ElementIndex index)
            {
                if (!container.ContainsValue(index))
                {
                    container.Add(index);
                }
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

            return container;
        }

        private static IEnumerable<SaveFileSlot> CreateSlotsData(SaveFileWorldResources resources, World world, Point worldSize)
        {
            for (int y = 0; y < worldSize.Y; y++)
            {
                for (int x = 0; x < worldSize.X; x++)
                {
                    Point position = new(x, y);

                    if (!world.IsEmptySlot(position))
                    {
                        yield return new(resources, world.GetSlot(position));
                    }
                }
            }
        }

        #region Readers
        private static readonly Dictionary<string, Action<ZipArchiveEntry, SaveFile, GraphicsDevice>> fileReaders = new(StringComparer.InvariantCulture)
        {
            {
                // ROOT/header/thumbnail.png
                ThumbnailFilePath,
                (entry, saveFile, graphicsDevice) =>
                {
                    using Stream stream = entry.Open();
                    saveFile.Header.ThumbnailTexture = Texture2D.FromStream(graphicsDevice, stream);
                }
            },

            #region Header
            {
                // ROOT/header/metadata.pdworlddata
                HeaderMetadataFilePath,
                (entry, saveFile, _) =>
                {
                    using Stream stream = entry.Open();
                    saveFile.Header.Metadata = MessagePackSerializer.Deserialize<SaveFileMetadata>(stream, MessagePackSerializerOptions.Standard);
                }
            },
            {
                // ROOT/header/information.pdworlddata
                HeaderInformationFilePath,
                (entry, saveFile, _) =>
                {
                    using Stream stream = entry.Open();
                    saveFile.Header.Information = MessagePackSerializer.Deserialize<SaveFileInformation>(stream, MessagePackSerializerOptions.Standard);
                }
            },
            #endregion

            #region World

            #region General
            {
                // ROOT/world/information.pdworlddata
                WorldInformationFilePath,
                (entry, saveFile, _) =>
                {
                    using Stream stream = entry.Open();
                    saveFile.World.Information = MessagePackSerializer.Deserialize<SaveFileWorldInformation>(stream, MessagePackSerializerOptions.Standard);
                }
            },
            #endregion
            
            #region Resources
            {
                // ROOT/world/resources/elements.pdworlddata
                WorldResourceElementsFilePath,
                (entry, saveFile, _) =>
                {
                    using Stream stream = entry.Open();
                    saveFile.World.Resources.Elements = MessagePackSerializer.Deserialize<SaveFileResourceContainer>(stream, MessagePackSerializerOptions.Standard);
                }
            },
            #endregion

            #region Content
            {
                // ROOT/world/content/slots.pdworlddata
                WorldContentSlotsFilePath,
                (entry, saveFile, _) =>
                {
                    using Stream stream = entry.Open();
                    saveFile.World.Content.Slots = MessagePackSerializer.Deserialize<IEnumerable<SaveFileSlot>>(stream, MessagePackSerializerOptions.Standard);
                }
            },
            {
                // ROOT/world/content/entities.pdworlddata
                WorldContentEntitiesFilePath,
                (entry, saveFile, _) =>
                {

                }
            },
            #endregion

            #region Environment
            {
                // ROOT/world/environment/time.pdworlddata
                WorldEnvironmentTimeFilePath,
                (entry, saveFile, _) =>
                {
                    using Stream stream = entry.Open();
                    saveFile.World.Environment.Time = MessagePackSerializer.Deserialize<SaveFileWorldTime>(stream, MessagePackSerializerOptions.Standard);
                }
            },
            #endregion
            
            #endregion
        };

        #endregion

        #region Writers

        private static void WriteZipFile(SaveFile worldSaveFile, MemoryStream memoryStream)
        {
            using ZipArchive saveFileZipArchive = new(memoryStream, ZipArchiveMode.Create, true);

            #region Header
            // ROOT/header/thumbnail.png
            using (Stream thumbnailStreamWriter = saveFileZipArchive.CreateEntry(ThumbnailFilePath).Open())
            {
                worldSaveFile.Header.ThumbnailTexture.SaveAsPng(thumbnailStreamWriter, WorldConstants.WORLD_THUMBNAIL_SIZE.X, WorldConstants.WORLD_THUMBNAIL_SIZE.Y);
            }

            // ROOT/header/metadata.pdworlddata
            using (Stream headerMetadataStreamWriter = saveFileZipArchive.CreateEntry(HeaderMetadataFilePath).Open())
            {
                headerMetadataStreamWriter.Write(MessagePackSerializer.Serialize(worldSaveFile.Header.Metadata));
            }

            // ROOT/header/information.pdworlddata
            using (Stream headerInformationStreamWriter = saveFileZipArchive.CreateEntry(HeaderInformationFilePath).Open())
            {
                headerInformationStreamWriter.Write(MessagePackSerializer.Serialize(worldSaveFile.Header.Information));
            }
            #endregion

            #region World
            // ROOT/world/information.pdworlddata
            using (Stream worldInformationStreamWriter = saveFileZipArchive.CreateEntry(WorldInformationFilePath).Open())
            {
                worldInformationStreamWriter.Write(MessagePackSerializer.Serialize(worldSaveFile.World.Information));
            }

            #region Resources
            // ROOT/world/resources/elements.pdworlddata
            using (Stream worldResourcesStreamWriter = saveFileZipArchive.CreateEntry(WorldResourceElementsFilePath).Open())
            {
                worldResourcesStreamWriter.Write(MessagePackSerializer.Serialize(worldSaveFile.World.Resources.Elements));
            }
            #endregion

            #region Content
            // ROOT/world/content/slots.pdworlddata
            using (Stream worldContentSlotsStreamWriter = saveFileZipArchive.CreateEntry(WorldContentSlotsFilePath).Open())
            {
                worldContentSlotsStreamWriter.Write(MessagePackSerializer.Serialize(worldSaveFile.World.Content.Slots));
            }

            // ROOT/world/content/entities.pdworlddata
            // using (Stream worldContentEntitiesStreamWriter = saveFileZipArchive.CreateEntry(WorldContentEntitiesFilePath).Open())
            // {
            // 
            // }
            #endregion

            #region Environment
            // ROOT/world/environment/time.pdworlddata
            using Stream worldEnvironmentTimeStreamWriter = saveFileZipArchive.CreateEntry(WorldEnvironmentTimeFilePath).Open();
            worldEnvironmentTimeStreamWriter.Write(MessagePackSerializer.Serialize(worldSaveFile.World.Environment.Time));
            #endregion

            #endregion
        }

        private static SaveFile ReadZipFile(ZipArchive zipArchive, GraphicsDevice graphicsDevice)
        {
            SaveFile saveFile = new();

            foreach (ZipArchiveEntry entry in zipArchive.Entries)
            {
                if (fileReaders.TryGetValue(entry.FullName, out Action<ZipArchiveEntry, SaveFile, GraphicsDevice> handler))
                {
                    handler.Invoke(entry, saveFile, graphicsDevice);
                }
            }

            return saveFile;
        }

        #endregion
    }
}
