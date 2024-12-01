using Microsoft.Xna.Framework.Audio;

namespace StardustSandbox.Core.Audio
{
    public static class SSoundEngine
    {
        public static void Play(SoundEffect soundEffect)
        {
            soundEffect.Play();
        }

        public static void Play(SoundEffect soundEffect, float volume, float pitch, float pan)
        {
            soundEffect.Play(volume, pitch, pan);
        }
    }
}
