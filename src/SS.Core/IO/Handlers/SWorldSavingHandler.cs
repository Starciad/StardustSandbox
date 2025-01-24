using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.IO;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.IO.Files.Saving;
using StardustSandbox.Core.IO.Files.Saving.World.Content;
using StardustSandbox.Core.IO.Files.Saving.World.Information;
using StardustSandbox.Core.Mathematics.Primitives;
using StardustSandbox.Core.World.Slots;

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace StardustSandbox.Core.IO.Handlers
{
    public static partial class SWorldSavingHandler
    {
        public static void Serialize(ISWorld world, GraphicsDevice graphicsDevice)
        {
            // Paths
            string filename = Path.Combine(SDirectory.Worlds, string.Concat(world.Infos.Name, SFileExtensionConstants.WORLD));

            // Streams
            using MemoryStream saveFileMemoryStream = new();
            using FileStream outputSaveFile = new(filename, FileMode.Create, FileAccess.Write, FileShare.Write);

            // Saving
            WriteZipFile(CreateWorldSaveFile(filename, world, graphicsDevice), saveFileMemoryStream);

            // Write
            _ = saveFileMemoryStream.Seek(0, SeekOrigin.Begin);
            saveFileMemoryStream.WriteTo(outputSaveFile);
        }

        public static SSaveFile CreateWorldSaveFile(string filename, ISWorld world, GraphicsDevice graphicsDevice)
        {
            DateTime currentDateTime = DateTime.Now;

            SSaveFileWorldResources resources = CreateWorldResources(world);

            return new()
            {
                Header = new()
                {
                    ThumbnailTexture = world.CreateThumbnail(graphicsDevice),

                    Metadata = new()
                    {
                        Filename = filename,
                        Identifier = world.Infos.Identifier,
                        Name = world.Infos.Name,
                        Description = world.Infos.Description,
                    },

                    Information = new()
                    {
                        SaveVersion = SFileConstants.WORLD_SAVE_FILE_VERSION,
                        GameVersion = SGameConstants.VERSION,
                        CreationTimestamp = currentDateTime,
                        LastUpdateTimestamp = currentDateTime,
                    },
                },

                World = new()
                {
                    Information = new()
                    {
                        Width = world.Infos.Size.Width,
                        Height = world.Infos.Size.Height,
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
                        Slots = CreateWorldSlotsData(resources, world, world.Infos.Size),
                    },
                },
            };
        }

        public static SSaveFile LoadSaveFile(string name, GraphicsDevice graphicsDevice)
        {
            // Paths
            string filename = Path.Combine(SDirectory.Worlds, string.Concat(name, SFileExtensionConstants.WORLD));

            // Streams
            using FileStream inputSaveFile = new(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            using ZipArchive saveFileZipArchive = new(inputSaveFile, ZipArchiveMode.Read);

            // Read
            return ReadZipFile(saveFileZipArchive, graphicsDevice);
        }

        // Utilities
        public static IEnumerable<SSaveFile> LoadAllSavedWorldData(GraphicsDevice graphicsDevice)
        {
            string[] files = Directory.GetFiles(SDirectory.Worlds, string.Concat('*', SFileExtensionConstants.WORLD), SearchOption.TopDirectoryOnly);
            SSaveFile saveFile;

            for (int i = 0; i < files.Length; i++)
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

        private static SSaveFileWorldResources CreateWorldResources(ISWorld world)
        {
            return new()
            {
                Elements = GetAllWorldDistinctElements(world),
            };
        }

        private static SSaveFileResourceContainer GetAllWorldDistinctElements(ISWorld world)
        {
            SSaveFileResourceContainer container = new();

            void TryAddElementIdentifier(string identifier)
            {
                if (!container.ContainsValue(identifier))
                {
                    container.Add(identifier);
                }
            }

            void ProcessWorldSlot(Point position, SWorldLayer layer)
            {
                if (!world.TryGetWorldSlot(position, out SWorldSlot slot))
                {
                    return;
                }

                SWorldSlotLayer worldSlotLayer = slot.GetLayer(layer);

                if (worldSlotLayer.IsEmpty)
                {
                    return;
                }

                TryAddElementIdentifier(worldSlotLayer.Element.Identifier);

                if (worldSlotLayer.StoredElement != null)
                {
                    TryAddElementIdentifier(worldSlotLayer.StoredElement.Identifier);
                }
            }

            for (int y = 0; y < world.Infos.Size.Height; y++)
            {
                for (int x = 0; x < world.Infos.Size.Width; x++)
                {
                    Point position = new(x, y);

                    if (!world.IsEmptyWorldSlot(position))
                    {
                        ProcessWorldSlot(position, SWorldLayer.Foreground);
                        ProcessWorldSlot(position, SWorldLayer.Background);
                    }
                }
            }

            return container;
        }

        private static IEnumerable<SSaveFileWorldSlot> CreateWorldSlotsData(SSaveFileWorldResources resources, ISWorld world, SSize2 worldSize)
        {
            for (int y = 0; y < worldSize.Height; y++)
            {
                for (int x = 0; x < worldSize.Width; x++)
                {
                    Point position = new(x, y);

                    if (!world.IsEmptyWorldSlot(position))
                    {
                        yield return new(resources, world.GetWorldSlot(position));
                    }
                }
            }
        }
    }
}
