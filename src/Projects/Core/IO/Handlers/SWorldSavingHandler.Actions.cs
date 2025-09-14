using MessagePack;

using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.IO;
using StardustSandbox.Core.IO.Files.Saving;
using StardustSandbox.Core.IO.Files.Saving.Header;
using StardustSandbox.Core.IO.Files.Saving.World.Content;
using StardustSandbox.Core.IO.Files.Saving.World.Environment;
using StardustSandbox.Core.IO.Files.Saving.World.Information;

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace StardustSandbox.Core.IO.Handlers
{
    public static partial class SWorldSavingHandler
    {
        private static string ThumbnailFilePath => Path.Combine(SDirectoryConstants.SAVE_FILE_HEADER, SFileConstants.SAVE_FILE_THUMBNAIL);
        private static string HeaderMetadataFilePath => Path.Combine(SDirectoryConstants.SAVE_FILE_HEADER, SFileConstants.SAVE_FILE_HEADER_METADATA);
        private static string HeaderInformationFilePath => Path.Combine(SDirectoryConstants.SAVE_FILE_HEADER, SFileConstants.SAVE_FILE_HEADER_INFORMATION);
        private static string WorldInformationFilePath => Path.Combine(SDirectoryConstants.SAVE_FILE_WORLD, SFileConstants.SAVE_FILE_WORLD_INFORMATION);
        private static string WorldResourceElementsFilePath => Path.Combine(SDirectoryConstants.SAVE_FILE_WORLD, SDirectoryConstants.SAVE_FILE_WORLD_RESOURCES, SFileConstants.SAVE_FILE_WORLD_RESOURCE_ELEMENTS);
        private static string WorldContentSlotsFilePath => Path.Combine(SDirectoryConstants.SAVE_FILE_WORLD, SDirectoryConstants.SAVE_FILE_WORLD_CONTENT, SFileConstants.SAVE_FILE_WORLD_CONTENT_SLOTS);
        private static string WorldContentEntitiesFilePath => Path.Combine(SDirectoryConstants.SAVE_FILE_WORLD, SDirectoryConstants.SAVE_FILE_WORLD_CONTENT, SFileConstants.SAVE_FILE_WORLD_CONTENT_ENTITIES);
        private static string WorldEnvironmentTimeFilePath => Path.Combine(SDirectoryConstants.SAVE_FILE_WORLD, SDirectoryConstants.SAVE_FILE_WORLD_ENVIRONMENT, SFileConstants.SAVE_FILE_WORLD_ENVIRONMENT_TIME);

        #region Readers
        private static readonly Dictionary<string, Action<ZipArchiveEntry, SSaveFile, GraphicsDevice>> fileReaders = new(StringComparer.InvariantCulture)
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
                    saveFile.Header.Metadata = MessagePackSerializer.Deserialize<SSaveFileMetadata>(stream, MessagePackSerializerOptions.Standard);
                }
            },
            {
                // ROOT/header/information.pdworlddata
                HeaderInformationFilePath,
                (entry, saveFile, _) =>
                {
                    using Stream stream = entry.Open();
                    saveFile.Header.Information = MessagePackSerializer.Deserialize<SSaveFileInformation>(stream, MessagePackSerializerOptions.Standard);
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
                    saveFile.World.Information = MessagePackSerializer.Deserialize<SSaveFileWorldInformation>(stream, MessagePackSerializerOptions.Standard);
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
                    saveFile.World.Resources.Elements = MessagePackSerializer.Deserialize<SSaveFileResourceContainer>(stream, MessagePackSerializerOptions.Standard);
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
                    saveFile.World.Content.Slots = MessagePackSerializer.Deserialize<IEnumerable<SSaveFileWorldSlot>>(stream, MessagePackSerializerOptions.Standard);
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
                    saveFile.World.Environment.Time = MessagePackSerializer.Deserialize<SSaveFileWorldTime>(stream, MessagePackSerializerOptions.Standard);
                }
            },
            #endregion
            
            #endregion
        };
        #endregion

        #region Writers
        private static void WriteZipFile(SSaveFile worldSaveFile, MemoryStream memoryStream)
        {
            using ZipArchive saveFileZipArchive = new(memoryStream, ZipArchiveMode.Create, true);

            #region Header
            // ROOT/header/thumbnail.png
            using (Stream thumbnailStreamWriter = saveFileZipArchive.CreateEntry(ThumbnailFilePath).Open())
            {
                worldSaveFile.Header.ThumbnailTexture.SaveAsPng(thumbnailStreamWriter, SWorldConstants.WORLD_THUMBNAIL_SIZE.Width, SWorldConstants.WORLD_THUMBNAIL_SIZE.Height);
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

        private static SSaveFile ReadZipFile(ZipArchive zipArchive, GraphicsDevice graphicsDevice)
        {
            SSaveFile saveFile = new();

            foreach (ZipArchiveEntry entry in zipArchive.Entries)
            {
                if (fileReaders.TryGetValue(entry.FullName, out Action<ZipArchiveEntry, SSaveFile, GraphicsDevice> handler))
                {
                    handler.Invoke(entry, saveFile, graphicsDevice);
                }
            }

            return saveFile;
        }
        #endregion
    }
}
