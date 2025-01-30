﻿using Microsoft.Xna.Framework.Audio;

using StardustSandbox.Core.Collections;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Interfaces.Collections;

using System.Collections.Generic;

namespace StardustSandbox.Core.Audio
{
    public static class SSoundEngine
    {
        private sealed class SPoolableSoundEffect(SoundEffect soundEffect, SoundEffectInstance instance) : ISPoolableObject
        {
            public SoundEffect SoundEffect => soundEffect;
            public SoundEffectInstance Instance => instance;

            public void Reset()
            {
                this.Instance.Stop();
                this.Instance.Volume = 1.0f;
                this.Instance.Pitch = 0.0f;
                this.Instance.Pan = 0.0f;
            }
        }

        private static readonly List<SPoolableSoundEffect> activeInstances = [];
        private static readonly Dictionary<SoundEffect, SObjectPool> soundEffectPools = [];

        public static float Volume
        {
            get => SoundEffect.MasterVolume;
            set => SoundEffect.MasterVolume = value;
        }

        public static void Play(SoundEffect soundEffect)
        {
            Play(soundEffect, 1.0f, 0.0f, 0.0f);
        }

        public static void Play(SoundEffect soundEffect, float volume, float pitch, float pan)
        {
            ReleaseInstances();

            if (activeInstances.Count >= SAudioConstants.MAX_CONCURRENT_SOUNDS)
            {
                return;
            }

            SPoolableSoundEffect poolableSoundEffect = GetOrCreateInstance(soundEffect);

            if (poolableSoundEffect == null)
            {
                return;
            }

            poolableSoundEffect.Instance.Volume = volume;
            poolableSoundEffect.Instance.Pitch = pitch;
            poolableSoundEffect.Instance.Pan = pan;
            poolableSoundEffect.Instance.Play();

            activeInstances.Add(poolableSoundEffect);
        }

        private static SPoolableSoundEffect GetOrCreateInstance(SoundEffect soundEffect)
        {
            if (!soundEffectPools.TryGetValue(soundEffect, out SObjectPool pool))
            {
                pool = new();
                soundEffectPools[soundEffect] = pool;
            }

            SPoolableSoundEffect result = pool.TryGet(out ISPoolableObject poolableObject)
                ? (SPoolableSoundEffect)poolableObject
                : new(soundEffect, soundEffect.CreateInstance());

            return result;
        }

        private static void ReleaseInstances()
        {
            for (int i = 0; i < activeInstances.Count; i++)
            {
                SPoolableSoundEffect poolableSoundEffect = activeInstances[i];

                if (poolableSoundEffect.Instance == null || poolableSoundEffect.Instance.State != SoundState.Stopped)
                {
                    continue;
                }

                ReleaseInstance(poolableSoundEffect);
            }
        }

        private static void ReleaseInstance(SPoolableSoundEffect poolableSoundEffect)
        {
            if (!activeInstances.Remove(poolableSoundEffect))
            {
                return;
            }

            poolableSoundEffect.Instance.Stop();

            soundEffectPools[poolableSoundEffect.SoundEffect].Add(poolableSoundEffect);
        }
    }
}
