using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.IO;
using StardustSandbox.Core.IO.Files.Settings;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace StardustSandbox.Core.IO.Handlers
{
    public static class SSettingsHandler
    {
        private static readonly ConcurrentDictionary<Type, object> settingsCache = [];

        private static readonly Dictionary<Type, Func<object>> defaultSettingsFactory = new()
        {
            { typeof(SVideoSettings), () => new SVideoSettings() },
            { typeof(SVolumeSettings), () => new SVolumeSettings() },
            { typeof(SCursorSettings), () => new SCursorSettings() },
            { typeof(SLanguageSettings), () => new SLanguageSettings() }
        };

        private static readonly Dictionary<Type, string> settingsFileMap = new()
        {
            { typeof(SVideoSettings), SFileConstants.SETTINGS_VIDEO },
            { typeof(SVolumeSettings), SFileConstants.SETTINGS_VOLUME },
            { typeof(SCursorSettings), SFileConstants.SETTINGS_CURSOR },
            { typeof(SLanguageSettings), SFileConstants.SETTINGS_LANGUAGE }
        };

        private static readonly Dictionary<Type, XmlSerializer> serializers = new()
        {
            { typeof(SVideoSettings), new(typeof(SVideoSettings))},
            { typeof(SVolumeSettings), new(typeof(SVolumeSettings))},
            { typeof(SCursorSettings), new(typeof(SCursorSettings))},
            { typeof(SLanguageSettings), new(typeof(SLanguageSettings))}
        };

        public static void Initialize()
        {
            foreach (Type type in settingsFileMap.Keys)
            {
                EnsureSettingsFileExists(type);
            }

            CreateWarningFile();
        }

        public static void SaveSettings<T>(T settings) where T : SSettings
        {
            if (!settingsFileMap.TryGetValue(typeof(T), out string fileName))
            {
                throw new InvalidOperationException($"Unknown settings type: {typeof(T).Name}");
            }

            settingsCache[typeof(T)] = settings;
            Serialize(settings, fileName);
        }

        public static T LoadSettings<T>() where T : SSettings, new()
        {
            if (settingsCache.TryGetValue(typeof(T), out object cachedValue))
            {
                return (T)cachedValue;
            }

            if (!settingsFileMap.TryGetValue(typeof(T), out string fileName))
            {
                throw new InvalidOperationException($"Unknown settings type: {typeof(T).Name}");
            }

            T settings = Deserialize<T>(fileName) ?? new T();
            settingsCache[typeof(T)] = settings;

            return settings;
        }

        public static void UpdateSettings<T>(Func<T, T> updateCallback) where T : SSettings, new()
        {
            T settings = LoadSettings<T>();
            T updatedSettings = updateCallback(settings);
            SaveSettings(updatedSettings);
        }

        public static void ResetSettings<T>() where T : SSettings
        {
            if (!settingsFileMap.TryGetValue(typeof(T), out string fileName) || !defaultSettingsFactory.TryGetValue(typeof(T), out Func<object> createDefault))
            {
                throw new InvalidOperationException($"Unknown settings type: {typeof(T).Name}");
            }

            T defaultSettings = (T)createDefault();
            settingsCache[typeof(T)] = defaultSettings;

            Serialize(defaultSettings, fileName);
        }

        // ======================================================= //

        private static void EnsureSettingsFileExists(Type type)
        {
            if (!settingsFileMap.TryGetValue(type, out string fileName))
            {
                return;
            }

            string filePath = Path.Combine(SDirectory.Settings, fileName);

            if (File.Exists(filePath))
            {
                HandleExistingFile(type, filePath);
            }
            else
            {
                CreateDefaultSettings(type, fileName);
            }
        }

        private static void HandleExistingFile(Type type, string filePath)
        {
            try
            {
                _ = Deserialize(type, filePath);
            }
            catch (Exception)
            {
                File.Delete(filePath);
                CreateDefaultSettings(type, Path.GetFileName(filePath));
            }
        }

        private static void CreateDefaultSettings(Type type, string fileName)
        {
            if (defaultSettingsFactory.TryGetValue(type, out Func<object> createDefault))
            {
                object defaultSettings = createDefault();
                settingsCache[type] = defaultSettings;
                Serialize(defaultSettings, fileName);
            }
        }

        private static void CreateWarningFile()
        {
            string filePath = Path.Combine(SDirectory.Settings, SFileConstants.WARNING);

            if (File.Exists(filePath))
            {
                return;
            }

            StringBuilder builder = new();

            _ = builder.AppendLine(new string('=', 64));
            _ = builder.AppendLine();
            _ = builder.AppendLine(string.Concat(new string(' ', 12), "(c) ", SGameConstants.YEAR, ' ', SGameConstants.AUTHOR));
            _ = builder.AppendLine();
            _ = builder.AppendLine(new string('=', 64));
            _ = builder.AppendLine();
            _ = builder.AppendLine("WARNING: MODIFYING SETTINGS OUTSIDE THE GAME");
            _ = builder.AppendLine(string.Concat(new string('_', 64)));
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

        private static void Serialize<T>(T value, string fileName)
        {
            using FileStream fileStream = new(Path.Combine(SDirectory.Settings, fileName), FileMode.Create, FileAccess.Write);
            serializers[value.GetType()].Serialize(fileStream, value);
        }

        private static T Deserialize<T>(string fileName) where T : SSettings
        {
            return (T)Deserialize(typeof(T), fileName);
        }

        private static SSettings Deserialize(Type type, string fileName)
        {
            string filePath = Path.Combine(SDirectory.Settings, fileName);

            if (File.Exists(filePath))
            {
                using FileStream fileStream = File.OpenRead(filePath);
                return (SSettings)serializers[type].Deserialize(fileStream);
            }

            return default;
        }
    }
}
