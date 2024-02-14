using MessagePack;

using PixelDust.Game.Constants;
using PixelDust.Game.Models.Settings;

using System;
using System.IO;

namespace PixelDust.Game.IO
{
    public static class PSystemSettingsFile
    {
        private static readonly MessagePackSerializerOptions serializerOptions = MessagePackSerializerOptions.Standard.WithSecurity(MessagePackSecurity.UntrustedData).WithCompression(MessagePackCompression.Lz4Block);

        public static void Initialize()
        {
            EnsureSettingsFileExists(PFileConstants.SETTINGS_GRAPHICS, () => new PGraphicsSettings());
            EnsureSettingsFileExists(PFileConstants.SETTINGS_CURSOR, () => new PCursorSettings());
        }

        #region CORE
        public static void SetGraphicsSettings(PGraphicsSettings value)
        {
            Serialize(value, PFileConstants.SETTINGS_GRAPHICS);
        }
        public static void SetCursorSettings(PCursorSettings value)
        {
            Serialize(value, PFileConstants.SETTINGS_CURSOR);
        }

        public static PGraphicsSettings GetGraphicsSettings()
        {
            return Deserialize<PGraphicsSettings>(PFileConstants.SETTINGS_GRAPHICS);
        }
        public static PCursorSettings GetCursorSettings()
        {
            return Deserialize<PCursorSettings>(PFileConstants.SETTINGS_CURSOR);
        }
        #endregion

        #region UTILITIES
        private static void EnsureSettingsFileExists<T>(string fileName, Func<T> createDefaultSettings)
        {
            if (!File.Exists(Path.Combine(PDirectory.Settings, fileName)))
            {
                Serialize(createDefaultSettings(), fileName);
            }
        }
        private static void Serialize<T>(T value, string fileName)
        {
            string filePath = Path.Combine(PDirectory.Settings, fileName);
            File.WriteAllBytes(filePath, MessagePackSerializer.Serialize(value, serializerOptions));
        }
        private static T Deserialize<T>(string fileName)
        {
            string path = Path.Combine(PDirectory.Settings, fileName);

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