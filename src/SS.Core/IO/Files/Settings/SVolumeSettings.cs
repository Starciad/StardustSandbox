using System.Xml.Serialization;

namespace StardustSandbox.Core.IO.Files.Settings
{
    [XmlRoot("VolumeSettings")]
    public sealed class SVolumeSettings : SSettings
    {
        [XmlElement("MasterVolume", typeof(float))]
        public float MasterVolume { get; set; }

        [XmlElement("MusicVolume", typeof(float))]
        public float MusicVolume
        {
            get => this.musicVolume * this.MasterVolume;
            set => this.musicVolume = value;
        }

        [XmlElement("SFXVolume", typeof(float))]
        public float SFXVolume
        {
            get => this.sfxVolume * this.MasterVolume;
            set => this.sfxVolume = value;
        }

        private float musicVolume;
        private float sfxVolume;

        public SVolumeSettings()
        {
            this.MasterVolume = 1f;
            this.musicVolume = 0.5f;
            this.sfxVolume = 0.5f;
        }
    }
}
