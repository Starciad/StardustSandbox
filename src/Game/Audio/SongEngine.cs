using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.States;
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

        private static bool isInitialized;

        /* ===========================
           FADE / VOLUME STATE
           =========================== */

        private const float FadeStepIntervalMs = 50f;
        private const float FadeDurationMs = 1500f;

        private static float fadeFactor = 1f;
        private static VolumeSettings currentVolumeSettings;

        /* ===========================
           GAMEPLAY MUSIC STATE
           =========================== */

        private static CancellationTokenSource gameplayMusicToken;
        private static Task gameplayMusicTask;

        private static readonly Queue<SongIndex> gameplaySongDeck = [];

        /* ===========================
           INITIALIZATION
           =========================== */

        internal static void Initialize()
        {
            if (isInitialized)
            {
                throw new InvalidOperationException($"{nameof(SongEngine)} is already initialized.");
            }

            currentVolumeSettings = SettingsSerializer.Load<VolumeSettings>();
            ApplyFinalVolume();

            isInitialized = true;
        }

        /* ===========================
           VOLUME CONTROL
           =========================== */

        internal static void ApplyVolumeSettings(in VolumeSettings volumeSettings)
        {
            currentVolumeSettings = volumeSettings;
            ApplyFinalVolume();
        }

        private static void ApplyFinalVolume()
        {
            float baseVolume =
                currentVolumeSettings.MusicVolume *
                currentVolumeSettings.MasterVolume;

            MediaPlayer.Volume = baseVolume * fadeFactor;
        }

        /* ===========================
           DIRECT PLAYBACK
           =========================== */

        internal static void Play(SongIndex songIndex)
        {
            CurrentSong = AssetDatabase.GetSong(songIndex);
            CurrentSongIndex = songIndex;

            fadeFactor = 1f;
            ApplyFinalVolume();

            MediaPlayer.Stop();
            MediaPlayer.Play(CurrentSong);
        }

        /* ===========================
           GAMEPLAY SONG DECK
           =========================== */

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

        /* ===========================
           FADE LOGIC
           =========================== */

        private static async Task FadeOutAsync(CancellationToken token)
        {
            int steps = (int)(FadeDurationMs / FadeStepIntervalMs);

            for (int i = 0; i < steps; i++)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }

                fadeFactor = MathHelper.Lerp(1f, 0f, i / (float)steps);
                ApplyFinalVolume();

                await Task.Delay((int)FadeStepIntervalMs, token);
            }

            fadeFactor = 0f;
            ApplyFinalVolume();
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

                fadeFactor = MathHelper.Lerp(0f, 1f, i / (float)steps);
                ApplyFinalVolume();

                await Task.Delay((int)FadeStepIntervalMs, token);
            }

            fadeFactor = 1f;
            ApplyFinalVolume();
        }

        /* ===========================
           GAMEPLAY MUSIC CYCLE
           =========================== */

        internal static void StartGameplayMusicCycle()
        {
            if (gameplayMusicTask != null && !gameplayMusicTask.IsCompleted)
            {
                return;
            }

            gameplayMusicToken = new CancellationTokenSource();
            CancellationToken token = gameplayMusicToken.Token;

            gameplayMusicTask = Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromSeconds(SSRandom.Range(30, 60)), token);

                    if (token.IsCancellationRequested)
                    {
                        break;
                    }

                    while (!GameHandler.HasState(GameStates.IsFocused) && !token.IsCancellationRequested)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(1), token);
                    }

                    SongIndex songIndex = GetNextGameplaySong();
                    CurrentSong = AssetDatabase.GetSong(songIndex);
                    CurrentSongIndex = songIndex;

                    fadeFactor = 0f;
                    ApplyFinalVolume();

                    MediaPlayer.Play(CurrentSong);
                    await FadeInAsync(token);

                    while (MediaPlayer.State is MediaState.Playing or MediaState.Paused && !token.IsCancellationRequested)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(1), token);
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

            fadeFactor = 1f;
            ApplyFinalVolume();

            MediaPlayer.Stop();
        }
    }
}
