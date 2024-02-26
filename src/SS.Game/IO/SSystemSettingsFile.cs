using MessagePack;

using StardustSandbox.Game.Constants;
using StardustSandbox.Game.Models.Settings;

using System;
using System.IO;

namespace StardustSandbox.Game.IO
{
    public static class SSystemSettingsFile
    {
        private static readonly MessagePackSerializerOptions serializerOptions = MessagePackSerializerOptions.Standard.WithSecurity(MessagePackSecurity.UntrustedData).WithCompression(MessagePackCompression.Lz4Block);

        public static void Initialize()
        {
            EnsureSettingsFileExists(SFileConstants.SETTINGS_GRAPHICS, () => new SGraphicsSettings());
            EnsureSettingsFileExists(SFileConstants.SETTINGS_CURSOR, () => new SCursorSettings());
        }

        #region CORE
        public static void SetGraphicsSettings(SGraphicsSettings value)
        {
            Serialize(value, SFileConstants.SETTINGS_GRAPHICS);
        }
        public static void SetCursorSettings(SCursorSettings value)
        {
            Serialize(value, SFileConstants.SETTINGS_CURSOR);
        }

        public static SGraphicsSettings GetGraphicsSettings()
        {
            return Deserialize<SGraphicsSettings>(SFileConstants.SETTINGS_GRAPHICS);
        }
        public static SCursorSettings GetCursorSettings()
        {
            return Deserialize<SCursorSettings>(SFileConstants.SETTINGS_CURSOR);
        }
        #endregion

        #region UTILITIES
        private static void EnsureSettingsFileExists<T>(string fileName, Func<T> createDefaultSettings)
        {
            if (!File.Exists(Path.Combine(SDirectory.Settings, fileName)))
            {
                Serialize(createDefaultSettings(), fileName);
            }
        }
        private static void Serialize<T>(T value, string fileName)
        {
            string filePath = Path.Combine(SDirectory.Settings, fileName);
            File.WriteAllBytes(filePath, MessagePackSerializer.Serialize(value, serializerOptions));
        }
        private static T Deserialize<T>(string fileName)
        {
            string path = Path.Combine(SDirectory.Settings, fileName);

            if (File.Exists(path))
            {
                using FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read);
                return MessagePackSerializer.Deserialize<T>(fs);
            }
            else
            {
                return default(T);
            }
        }
        #endregion
    }
}