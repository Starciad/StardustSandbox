/*
 * Copyright (C) 2026  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Serialization;
using StardustSandbox.Serialization.Settings;

using System;

#if SS_WINDOWS
using SharpDX.Multimedia;
#endif

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

            ApplyVolumeSettings(SettingsSerializer.Load<VolumeSettings>());

#if SS_WINDOWS
            SoundEffect.Speakers = Speakers.Stereo;
#endif

            isInitialized = true;
        }

        internal static void ApplyVolumeSettings(in VolumeSettings volumeSettings)
        {
            SoundEffect.MasterVolume = volumeSettings.MasterVolume;
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
            Play(index, SettingsSerializer.Load<VolumeSettings>().SFXVolume, 0.0f, 0.0f);
        }
    }
}

