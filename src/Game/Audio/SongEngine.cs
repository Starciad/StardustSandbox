using Microsoft.Xna.Framework.Media;

using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Extensions;
using StardustSandbox.Randomness;
using StardustSandbox.Serialization;
using StardustSandbox.Serialization.Settings;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace StardustSandbox.Audio
{
    internal static class SongEngine
    {
        internal static Song CurrentSong { get; private set; }
        internal static SongIndex CurrentSongIndex { get; private set; }

        internal static float Volume
        {
            get => MediaPlayer.Volume;
            set => MediaPlayer.Volume = value;
        }

        internal static bool IsMuted
        {
            get => MediaPlayer.IsMuted;
            set => MediaPlayer.IsMuted = value;
        }

        internal static bool IsRepeating
        {
            get => MediaPlayer.IsRepeating;
            set => MediaPlayer.IsRepeating = value;
        }

        internal static bool IsShuffled
        {
            get => MediaPlayer.IsShuffled;
            set => MediaPlayer.IsShuffled = value;
        }

        internal static MediaState State => MediaPlayer.State;

        private static bool isInitialized = false;

        private static readonly SongIndex[] gameplaySongs =
        {
            SongIndex.V01_CanvasOfSilence,
        };

        private static CancellationTokenSource gameplayMusicToken;
        private static Task gameplayMusicTask;

        internal static void Initialize()
        {
            if (isInitialized)
            {
                throw new InvalidOperationException($"{nameof(SongEngine)} is already initialized.");
            }

            VolumeSettings volumeSettings = SettingsSerializer.Load<VolumeSettings>();
            Volume = volumeSettings.MusicVolume * volumeSettings.MasterVolume;

            isInitialized = true;
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
                    int silenceTimeMs = SSRandom.Range(10_000, 25_000);
                    await Task.Delay(silenceTimeMs, token);

                    if (token.IsCancellationRequested)
                    {
                        break;
                    }

                    SongIndex songIndex = gameplaySongs.GetRandomItem();
                    Play(songIndex);

                    while (MediaPlayer.State == MediaState.Playing && !token.IsCancellationRequested)
                    {
                        await Task.Delay(500, token);
                    }
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
