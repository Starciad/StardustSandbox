using MessagePack;

using StardustSandbox.Core.Constants.IO;
using StardustSandbox.Core.IO.Files.Settings;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace StardustSandbox.Core.IO.Handlers
{
    public static class SSettingsHandler
    {
        private static readonly MessagePackSerializerOptions serializerOptions =
            MessagePackSerializerOptions.Standard
                .WithSecurity(MessagePackSecurity.UntrustedData)
                .WithCompression(MessagePackCompression.Lz4Block);

        private static readonly ConcurrentDictionary<Type, object> settingsCache = [];

        private static readonly Dictionary<Type, Func<object>> defaultSettingsFactory = new()
        {
            { typeof(SGeneralSettings), () => new SGeneralSettings() },
            { typeof(SVideoSettings), () => new SVideoSettings() },
            { typeof(SVolumeSettings), () => new SVolumeSettings() },
            { typeof(SCursorSettings), () => new SCursorSettings() },
            { typeof(SLanguageSettings), () => new SLanguageSettings() }
        };

        private static readonly Dictionary<Type, string> settingsFileMap = new()
        {
            { typeof(SGeneralSettings), SFileConstants.SETTINGS_GENERAL },
            { typeof(SVideoSettings), SFileConstants.SETTINGS_VIDEO },
            { typeof(SVolumeSettings), SFileConstants.SETTINGS_VOLUME },
            { typeof(SCursorSettings), SFileConstants.SETTINGS_CURSOR },
            { typeof(SLanguageSettings), SFileConstants.SETTINGS_LANGUAGE }
        };

        public static void Initialize()
        {
            foreach (Type type in settingsFileMap.Keys)
            {
                EnsureSettingsFileExists(type);
            }
        }

        public static void SaveSettings<T>(T settings) where T : SSettings
        {
            if (!settingsFileMap.TryGetValue(typeof(T), out string fileName))
            {
                throw new InvalidOperationException($"Unknown settings type: {typeof(T).Name}");
            }

            settingsCache[typeof(T)] = settings;
            Task.Run(() => SerializeAsync(settings, fileName)).Wait();
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

            Task<T> task = Task.Run(() => DeserializeAsync<T>(fileName));
            task.Wait();

            T settings = task.Result ?? new T();
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

            Task.Run(() => SerializeAsync(defaultSettings, fileName)).Wait();
        }

        private static void EnsureSettingsFileExists(Type type)
        {
            if (settingsFileMap.TryGetValue(type, out string fileName))
            {
                string filePath = Path.Combine(SDirectory.Settings, fileName);
                if (!File.Exists(filePath) && defaultSettingsFactory.TryGetValue(type, out Func<object> createDefault))
                {
                    object defaultSettings = createDefault();
                    settingsCache[type] = defaultSettings;
                    Task.Run(() => SerializeAsync(defaultSettings, fileName)).Wait();
                }
            }
        }

        private static async Task SerializeAsync<T>(T value, string fileName)
        {
            string filePath = Path.Combine(SDirectory.Settings, fileName);
            byte[] data = MessagePackSerializer.Serialize(value, serializerOptions);
            await File.WriteAllBytesAsync(filePath, data);
        }

        private static async Task<T> DeserializeAsync<T>(string fileName)
        {
            string filePath = Path.Combine(SDirectory.Settings, fileName);

            if (File.Exists(filePath))
            {
                byte[] data = await File.ReadAllBytesAsync(filePath);
                return MessagePackSerializer.Deserialize<T>(data, serializerOptions);
            }

            return default;
        }
    }
}
