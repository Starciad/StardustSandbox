using System;
using System.Xml.Serialization;

namespace StardustSandbox.Serialization.Settings
{
    [Serializable]
    [XmlRoot("VolumeSettings")]
    public readonly struct VolumeSettings : ISettingsModule
    {
        [XmlElement("MasterVolume", typeof(float))]
        public readonly float MasterVolume { get; init; }

        [XmlElement("MusicVolume", typeof(float))]
        public readonly float MusicVolume { get; init; }

        [XmlElement("SFXVolume", typeof(float))]
        public readonly float SFXVolume { get; init; }

        public VolumeSettings()
        {
            this.MasterVolume = 1f;
            this.MusicVolume = 0.5f;
            this.SFXVolume = 0.5f;
        }
    }
}
