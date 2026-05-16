/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
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

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.Serialization.Settings;

#if SS_WINDOWS
using SharpDX.Multimedia;
#endif

namespace StardustSandbox.Core.Managers
{
    internal sealed class SoundEffectManager
    {
        private float masterVolume = 1.0f;

        private readonly SoundEffectInstance[] activeInstances = new SoundEffectInstance[SoundEffectConstants.MAX_CONCURRENT_INSTANCES];
        private readonly AssetDatabase assetDatabase;

        internal SoundEffectManager(AssetDatabase assetDatabase)
        {
            this.assetDatabase = assetDatabase;
            ApplyVolumeSettings(SettingsSerializer.Load<VolumeSettings>());

#if SS_WINDOWS
            SoundEffect.Speakers = Speakers.Stereo;
#endif
        }

        internal void ApplyVolumeSettings(VolumeSettings volumeSettings)
        {
            this.masterVolume = volumeSettings.MasterVolume;
            SoundEffect.MasterVolume = this.masterVolume;
        }

        private void RegisterInstance(SoundEffectInstance instance)
        {
            for (int i = 0; i < this.activeInstances.Length; i++)
            {
                if (this.activeInstances[i] == null || this.activeInstances[i].State == SoundState.Stopped)
                {
                    // Dispose old if present
                    this.activeInstances[i]?.Dispose();
                    this.activeInstances[i] = null;

                    this.activeInstances[i] = instance;
                    return;
                }
            }

            // If here, no free slot found => too many instances playing.
            // Immediately dispose to avoid leak.
            instance.Dispose();
        }

        private void Play(SoundEffectIndex index, float volume, float pitch, float pan)
        {
            SoundEffect effect = this.assetDatabase.GetSoundEffect(index);
            SoundEffectInstance instance = effect.CreateInstance();

            instance.Volume = MathHelper.Clamp(volume, 0.0f, 1.0f);
            instance.Pitch = MathHelper.Clamp(pitch, -1.0f, 1.0f);
            instance.Pan = MathHelper.Clamp(pan, -1.0f, 1.0f);
            instance.Play();

            RegisterInstance(instance);
        }

        internal void Play(SoundEffectIndex index)
        {
            Play(index, SettingsSerializer.Load<VolumeSettings>().SFXVolume, 0.0f, 0.0f);
        }
    }
}
