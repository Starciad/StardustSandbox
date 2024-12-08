using MessagePack;

using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.IO;
using StardustSandbox.Core.IO.Files.World;
using StardustSandbox.Core.IO.Files.World.Data;
using StardustSandbox.Core.IO.Files.World.General;
using StardustSandbox.Core.Objects;

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace StardustSandbox.Core.Managers.IO
{
    public sealed class SWorldSaveFileManager : SGameObject
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

            [Path.Combine("general", "author.bin")] = (game, file, entry) =>
            {
                file.Author = MessagePackSerializer.Deserialize<SAuthorData>(entry.Open());
            },

            [Path.Combine("general", "security.bin")] = (game, file, entry) =>
            {
                file.Security = MessagePackSerializer.Deserialize<SSecurityData>(entry.Open());
            },

            [Path.Combine("data", "slots.bin")] = (game, file, entry) =>
            {
                file.World = MessagePackSerializer.Deserialize<SWorldData>(entry.Open());
            },
        };

        public SWorldSaveFileManager(ISGame gameInstance) : base(gameInstance)
        {

        }

        public void SaveFile(SWorldSaveFile saveFile)
        {
            // PATHS
            string directoryPath = Path.Combine(SDirectory.Worlds, saveFile.Metadata.Name);
            string saveFileFullname = string.Concat(directoryPath, SIOConstants.WORLD);

            // DIRECTORIES
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }

            Directory.CreateDirectory(directoryPath);

            // FILES
            // Texture
            using FileStream thumbnailFileStream = File.Create(Path.Combine(directoryPath, "thumbnail.png"));

        }

        public SWorldSaveFile LoadFile(string path)
        {
            SWorldSaveFile saveFile = new();

            using ZipArchive archive = ZipFile.OpenRead(path);

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
