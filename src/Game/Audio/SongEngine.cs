using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Extensions;
using StardustSandbox.Randomness;
using StardustSandbox.Serialization;
using StardustSandbox.Serialization.Settings;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StardustSandbox.Audio
{
    internal static class SongEngine
    {
        internal static Song CurrentSong { get; private set; }
        internal static SongIndex CurrentSongIndex { get; private set; }

        internal static MediaState State => MediaPlayer.State;

        private static bool isInitialized = false;

        private static readonly float FadeStepIntervalMs = 50f;
        private static readonly float FadeDurationMs = 1500f;

        private static float targetVolume;

        private static CancellationTokenSource gameplayMusicToken;
        private static Task gameplayMusicTask;

        private static readonly Queue<SongIndex> gameplaySongDeck = [];

        internal static void Initialize()
        {
            if (isInitialized)
            {
                throw new InvalidOperationException($"{nameof(SongEngine)} is already initialized.");
            }

            ApplyVolumeSettings(SettingsSerializer.Load<VolumeSettings>());
            isInitialized = true;
        }

        internal static void ApplyVolumeSettings(in VolumeSettings volumeSettings)
        {
            MediaPlayer.Volume = volumeSettings.MusicVolume * volumeSettings.MasterVolume;
        }

        internal static void Play(SongIndex songIndex)
        {
            Stop();

            CurrentSong = AssetDatabase.GetSong(songIndex);
            CurrentSongIndex = songIndex;

            MediaPlayer.Play(CurrentSong);
        }

        internal static void Stop()
        {
            MediaPlayer.Stop();
        }

        internal static void Pause()
        {
            MediaPlayer.Pause();
        }

        internal static void Resume()
        {
            MediaPlayer.Resume();
        }

        private static async Task FadeOutAsync(CancellationToken token)
        {
            float startVolume = MediaPlayer.Volume;
            int steps = (int)(FadeDurationMs / FadeStepIntervalMs);

            for (int i = 0; i < steps; i++)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }

                MediaPlayer.Volume = MathHelper.Lerp(startVolume, 0f, i / (float)steps);
                await Task.Delay((int)FadeStepIntervalMs, token);
            }

            MediaPlayer.Volume = 0f;
        }

        private static async Task FadeInAsync(CancellationToken token)
        {
            int steps = (int)(FadeDurationMs / FadeStepIntervalMs);

            for (int i = 0; i < steps; i++)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }

                MediaPlayer.Volume = MathHelper.Lerp(0f, targetVolume, i / (float)steps);
                await Task.Delay((int)FadeStepIntervalMs, token);
            }

            MediaPlayer.Volume = targetVolume;
        }

        private static SongIndex GetNextGameplaySong()
        {
            if (gameplaySongDeck.Count == 0)
            {
                foreach (SongIndex songIndex in SongConstants.GAMEPLAY_SONGS.Shuffle())
                {
                    gameplaySongDeck.Enqueue(songIndex);
                }
            }

            return gameplaySongDeck.Dequeue();
        }

        internal static void StartGameplayMusicCycle()
        {
            if (gameplayMusicTask != null && !gameplayMusicTask.IsCompleted)
            {
                return;
            }

            gameplayMusicToken = new CancellationTokenSource();
            CancellationToken token = gameplayMusicToken.Token;

            targetVolume = MediaPlayer.Volume;

            gameplayMusicTask = Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    int silenceTimeMs = SSRandom.Range(10_000, 25_000);
                    await Task.Delay(silenceTimeMs, token);

                    if (token.IsCancellationRequested)
                    {
                        break;
                    }

                    SongIndex songIndex = GetNextGameplaySong();
                    CurrentSong = AssetDatabase.GetSong(songIndex);
                    CurrentSongIndex = songIndex;

                    MediaPlayer.Volume = 0f;
                    MediaPlayer.Play(CurrentSong);

                    await FadeInAsync(token);

                    while (MediaPlayer.State is MediaState.Playing or MediaState.Paused && !token.IsCancellationRequested)
                    {
                        await Task.Delay(500, token);
                    }

                    await FadeOutAsync(token);
                    MediaPlayer.Stop();
                }
            }, token);
        }

        internal static void StopGameplayMusicCycle()
        {
            if (gameplayMusicToken == null)
            {
                return;
            }

            gameplayMusicToken.Cancel();
            gameplayMusicToken.Dispose();

            gameplayMusicToken = null;
            gameplayMusicTask = null;

            Stop();
        }
    }
}
