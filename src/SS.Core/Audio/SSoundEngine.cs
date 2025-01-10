using Microsoft.Xna.Framework.Audio;

namespace StardustSandbox.Core.Audio
{
    public static class SSoundEngine
    {
        public static float Volume
        {
            get
            {
                return SoundEffect.MasterVolume;
            }

            set
            {
                SoundEffect.MasterVolume = value;
            }
        }

        public static void Play(SoundEffect soundEffect)
        {
            _ = soundEffect.Play();
        }

        public static void Play(SoundEffect soundEffect, float volume, float pitch, float pan)
        {
            _ = soundEffect.Play(volume, pitch, pan);
        }
    }
}
