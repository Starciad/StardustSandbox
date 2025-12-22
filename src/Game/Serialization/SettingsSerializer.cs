using StardustSandbox.Constants;
using StardustSandbox.IO;
using StardustSandbox.Serialization.Settings;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace StardustSandbox.Serialization
{
    internal static class SettingsSerializer
    {
        private interface ISettingsDescriptor
        {
            Type SettingsType { get; }
            void LoadOrCreate();
        }

        private sealed class SettingsDescriptor<T> : ISettingsDescriptor where T : ISettingsModule, new()
        {
            public Type SettingsType => typeof(T);
            internal T Value => this.cache;

            private T cache;

            private readonly string fileName;
            private readonly XmlSerializer serializer;

            internal SettingsDescriptor(string fileName)
            {
                this.fileName = fileName;
                this.serializer = new(typeof(T));
            }

            public void LoadOrCreate()
            {
                string filePath = Path.Combine(SSDirectory.Settings, this.fileName);

                if (!File.Exists(filePath))
                {
                    CreateAndSaveDefault(filePath);
                    return;
                }

                try
                {
                    using FileStream stream = File.OpenRead(filePath);
                    this.cache = (T)this.serializer.Deserialize(stream);
                }
                catch
                {
                    File.Delete(filePath);
                    CreateAndSaveDefault(filePath);
                }
            }

            internal void Save(T value)
            {
                using FileStream stream = new(Path.Combine(SSDirectory.Settings, this.fileName), FileMode.Create, FileAccess.Write);

                this.cache = value;
                this.serializer.Serialize(stream, value);
            }

            private void CreateAndSaveDefault(string filePath)
            {
                this.cache = new T();

                using FileStream stream = new(filePath, FileMode.Create, FileAccess.Write);

                this.serializer.Serialize(stream, this.cache);
            }
        }

        private static readonly Dictionary<Type, ISettingsDescriptor> descriptors = new()
        {
            [typeof(ControlSettings)] = new SettingsDescriptor<ControlSettings>(IOConstants.CONTROL_SETTINGS_FILE),
            [typeof(CursorSettings)] = new SettingsDescriptor<CursorSettings>(IOConstants.CURSOR_SETTINGS_FILE),
            [typeof(GameplaySettings)] = new SettingsDescriptor<GameplaySettings>(IOConstants.GAMEPLAY_SETTINGS_FILE),
            [typeof(GeneralSettings)] = new SettingsDescriptor<GeneralSettings>(IOConstants.GENERAL_SETTINGS_FILE),
            [typeof(StatusSettings)] = new SettingsDescriptor<StatusSettings>(IOConstants.STATUS_SETTINGS_FILE),
            [typeof(VideoSettings)] = new SettingsDescriptor<VideoSettings>(IOConstants.VIDEO_SETTINGS_FILE),
            [typeof(VolumeSettings)] = new SettingsDescriptor<VolumeSettings>(IOConstants.VOLUME_SETTINGS_FILE),
        };

        internal static void Initialize()
        {
            _ = Directory.CreateDirectory(SSDirectory.Settings);

            foreach (ISettingsDescriptor descriptor in descriptors.Values)
            {
                descriptor.LoadOrCreate();
            }

            CreateWarningFile();
        }

        internal static T Load<T>() where T : ISettingsModule, new()
        {
            return GetDescriptor<T>().Value;
        }

        internal static void Save<T>(T value) where T : ISettingsModule, new()
        {
            GetDescriptor<T>().Save(value);
        }

        private static SettingsDescriptor<T> GetDescriptor<T>() where T : ISettingsModule, new()
        {
            return !descriptors.TryGetValue(typeof(T), out ISettingsDescriptor raw)
                ? throw new InvalidOperationException($"Settings type not registered: {typeof(T).FullName}")
                : (SettingsDescriptor<T>)raw;
        }

        private static void CreateWarningFile()
        {
            string filePath = Path.Combine(SSDirectory.Settings, IOConstants.WARNING);

            if (File.Exists(filePath))
            {
                return;
            }

            StringBuilder builder = new();

            _ = builder.AppendLine(new string('=', 64));
            _ = builder.AppendLine();
            _ = builder.AppendLine($"            (c) {GameConstants.YEAR} {GameConstants.AUTHOR}");
            _ = builder.AppendLine();
            _ = builder.AppendLine(new string('=', 64));
            _ = builder.AppendLine();
            _ = builder.AppendLine("WARNING: MODIFYING SETTINGS OUTSIDE THE GAME");
            _ = builder.AppendLine(new string('_', 64));
            _ = builder.AppendLine();
            _ = builder.AppendLine("Modifying configuration files outside the official game environment");
            _ = builder.AppendLine("can lead to unexpected behavior, including crashes, corrupted data,");
            _ = builder.AppendLine("or other failures.");
            _ = builder.AppendLine();
            _ = builder.AppendLine("These settings are designed to work seamlessly within the game's");
            _ = builder.AppendLine("framework. Any manual changes may bypass validation, causing");
            _ = builder.AppendLine("incompatibilities or errors that could severely impact gameplay.");
            _ = builder.AppendLine();
            _ = builder.AppendLine("We strongly recommend making adjustments only through the in-game");
            _ = builder.AppendLine("settings menu.");
            _ = builder.AppendLine();
            _ = builder.AppendLine("If you proceed to modify these files, you do so at your own risk.");
            _ = builder.AppendLine("Backup your settings regularly to avoid losing important data.");

            File.WriteAllText(filePath, builder.ToString());
        }
    }
}
