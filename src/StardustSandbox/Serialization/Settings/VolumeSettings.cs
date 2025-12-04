using System.Xml.Serialization;

namespace StardustSandbox.Serialization.Settings
{
    [XmlRoot("VolumeSettings")]
    public sealed class VolumeSettings : SettingsModule
    {
        [XmlElement("MasterVolume", typeof(float))]
        public float MasterVolume { get; set; }

        [XmlElement("MusicVolume", typeof(float))]
        public float MusicVolume
        {
            get => this.musicVolume;
            set => this.musicVolume = value;
        }

        [XmlElement("SFXVolume", typeof(float))]
        public float SFXVolume
        {
            get => this.sfxVolume;
            set => this.sfxVolume = value;
        }

        private float musicVolume;
        private float sfxVolume;

        public VolumeSettings()
        {
            this.MasterVolume = 1f;
            this.musicVolume = 0.5f;
            this.sfxVolume = 0.5f;
        }
    }
}
