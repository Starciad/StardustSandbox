using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using SharpDX.Multimedia;

using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Serialization;
using StardustSandbox.Serialization.Settings;

using System;

namespace StardustSandbox.Audio
{
    internal static class SoundEngine
    {
        private static readonly SoundEffectInstance[] activeInstances = new SoundEffectInstance[SoundEffectConstants.MAX_CONCURRENT_INSTANCES];

        private static bool isInitialized = false;

        internal static void Initialize()
        {
            if (isInitialized)
            {
                throw new InvalidOperationException($"{nameof(SoundEngine)} is already initialized.");
            }

            VolumeSettings volumeSettings = SettingsSerializer.LoadSettings<VolumeSettings>();

            SoundEffect.MasterVolume = volumeSettings.MasterVolume;
            SoundEffect.Speakers = Speakers.Stereo;

            isInitialized = true;
        }

        private static void RegisterInstance(SoundEffectInstance instance)
        {
            for (int i = 0; i < activeInstances.Length; i++)
            {
                if (activeInstances[i] == null || activeInstances[i].State == SoundState.Stopped)
                {
                    // Dispose old if present
                    activeInstances[i]?.Dispose();
                    activeInstances[i] = null;

                    activeInstances[i] = instance;
                    return;
                }
            }

            // If here, no free slot found => too many instances playing.
            // Immediately dispose to avoid leak.
            instance.Dispose();
        }

        private static void Play(SoundEffectIndex index, float volume, float pitch, float pan)
        {
            SoundEffect effect = AssetDatabase.GetSoundEffect(index);
            SoundEffectInstance instance = effect.CreateInstance();

            instance.Volume = MathHelper.Clamp(volume, 0.0f, 1.0f);
            instance.Pitch = MathHelper.Clamp(pitch, -1.0f, 1.0f);
            instance.Pan = MathHelper.Clamp(pan, -1.0f, 1.0f);
            instance.Play();

            RegisterInstance(instance);
        }

        internal static void Play(SoundEffectIndex index)
        {
            Play(index, 1.0f, 0.0f, 0.0f);
        }
    }
}
