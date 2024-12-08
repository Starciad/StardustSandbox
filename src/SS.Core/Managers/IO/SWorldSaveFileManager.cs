using MessagePack;

using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.IO;
using StardustSandbox.Core.IO.Files.World;
using StardustSandbox.Core.IO.Files.World.Data;
using StardustSandbox.Core.Objects;

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace StardustSandbox.Core.Managers.IO
{
    public sealed class SWorldSaveFileManager(ISGame gameInstance) : SGameObject(gameInstance)
    {
        private static readonly Dictionary<string, Action<ISGame, SWorldSaveFile, ZipArchiveEntry>> handlers = new()
        {
            ["thumbnail.png"] = (game, file, entry) =>
            {
                file.Texture = Texture2D.FromStream(game.GraphicsManager.GraphicsDevice, entry.Open());
            },

            ["metadata.bin"] = (game, file, entry) =>
            {
                file.Metadata = MessagePackSerializer.Deserialize<SWorldSaveFileMetadata>(entry.Open());
            },

            [Path.Combine("data", "slots.bin")] = (game, file, entry) =>
            {
                file.World = MessagePackSerializer.Deserialize<SWorldData>(entry.Open());
            },
        };

        public void SaveFile(SWorldSaveFile saveFile)
        {
            // PATHS
            string directoryPath = Path.Combine(SDirectory.Worlds, saveFile.Metadata.Name);
            string saveFilePath = string.Concat(directoryPath, SIOConstants.WORLD);

            // DIRECTORIES
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }

            _ = Directory.CreateDirectory(directoryPath);

            // FILES
            // ROOT/thumbnail.png
            using FileStream thumbnailFileStream = File.Create(Path.Combine(directoryPath, "thumbnail.png"));
            saveFile.Texture.SaveAsPng(thumbnailFileStream, saveFile.Texture.Width, saveFile.Texture.Height);

            // ROOT/metadata.bin
            File.WriteAllBytes(Path.Combine(directoryPath, "metadata.bin"), MessagePackSerializer.Serialize(saveFile.Metadata));

            // ROOT/data/world.bin
            File.WriteAllBytes(Path.Combine(directoryPath, "data", "world.bin"), MessagePackSerializer.Serialize(saveFile.World));

            // COMPRESS
            ZipFile.CreateFromDirectory(directoryPath, saveFilePath, CompressionLevel.SmallestSize, false);
        }

        public SWorldSaveFile LoadFile(string name)
        {
            SWorldSaveFile saveFile = new();

            using ZipArchive archive = ZipFile.OpenRead(Path.Combine(SDirectory.Worlds, name, SIOConstants.WORLD));

            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                if (handlers.TryGetValue(entry.FullName, out Action<ISGame, SWorldSaveFile, ZipArchiveEntry> handler))
                {
                    handler.Invoke(this.SGameInstance, saveFile, entry);
                }
            }

            return saveFile;
        }

        public SWorldSaveFile[] LoadFiles()
        {
            string[] paths = Directory.GetFiles(SDirectory.Worlds, string.Concat("*.", SIOConstants.WORLD), SearchOption.TopDirectoryOnly);
            SWorldSaveFile[] saveFiles = new SWorldSaveFile[paths.Length];

            for (int i = 0; i < paths.Length; i++)
            {
                string path = paths[i];
                saveFiles[i] = LoadFile(path);
            }

            return saveFiles;
        }
    }
}
