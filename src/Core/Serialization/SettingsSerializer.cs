/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Serialization.Settings;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace StardustSandbox.Core.Serialization
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
                string filePath = Path.Combine(Core.Directory.Settings, this.fileName);

                if (!System.IO.File.Exists(filePath))
                {
                    CreateAndSaveDefault(filePath);
                    return;
                }

                try
                {
                    using FileStream stream = System.IO.File.OpenRead(filePath);
                    this.cache = (T)this.serializer.Deserialize(stream);
                }
                catch
                {
                    System.IO.File.Delete(filePath);
                    CreateAndSaveDefault(filePath);
                }
            }

            internal void Save(T value)
            {
                using FileStream stream = new(Path.Combine(Core.Directory.Settings, this.fileName), FileMode.Create, FileAccess.Write);

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
            _ = System.IO.Directory.CreateDirectory(Core.Directory.Settings);

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
            string filePath = Path.Combine(Core.Directory.Settings, IOConstants.WARNING);

            if (System.IO.File.Exists(filePath))
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

            System.IO.File.WriteAllText(filePath, builder.ToString());
        }
    }
}

