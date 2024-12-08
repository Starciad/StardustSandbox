using MessagePack;

namespace StardustSandbox.Core.IO.Files.Settings
{
    [MessagePackObject]
    public sealed class SVolumeSettings : SSettings
    {
        [Key(0)]
        public float MasterVolume { get; set; }

        [Key(1)]
        public float MusicVolume
        {
            get => this.musicVolume * this.MasterVolume;
            set => this.musicVolume = value;
        }

        [Key(2)]
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
