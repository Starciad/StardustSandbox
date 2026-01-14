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
using Microsoft.Xna.Framework.Media;

using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.States;
using StardustSandbox.Extensions;
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

        private static bool isInitialized;

        private static float fadeFactor = 1f;
        private static VolumeSettings currentVolumeSettings;

        private static SongIndex lastPlayedGameplaySong;

        private static CancellationTokenSource gameplayMusicToken;
        private static Task gameplayMusicTask;

        private static readonly Queue<SongIndex> gameplaySongDeck = [];

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

        internal static void ApplyVolumeSettings(in VolumeSettings volumeSettings)
        {
            currentVolumeSettings = volumeSettings;
            ApplyFinalVolume();
        }

        private static void ApplyFinalVolume()
        {
            float baseVolume = currentVolumeSettings.MusicVolume * currentVolumeSettings.MasterVolume;
            MediaPlayer.Volume = baseVolume * fadeFactor;
        }

        internal static void Play(SongIndex songIndex)
        {
            CurrentSong = AssetDatabase.GetSong(songIndex);
            CurrentSongIndex = songIndex;

            fadeFactor = 1f;
            ApplyFinalVolume();

            MediaPlayer.Stop();
            MediaPlayer.Play(CurrentSong);
        }

        private static SongIndex GetNextGameplaySong()
        {
            if (gameplaySongDeck.Count == 0)
            {
                List<SongIndex> shuffledSongs = [.. SongConstants.GAMEPLAY_SONGS.Shuffle()];

                // Prevents the first song in the new deck
                // from being the same as the last song in the previous deck
                if (lastPlayedGameplaySong != SongIndex.None && shuffledSongs.Count > 1 && shuffledSongs[0] == lastPlayedGameplaySong)
                {
                    int swapIndex = 1 + Core.Random.Range(0, shuffledSongs.Count - 1);

                    (shuffledSongs[0], shuffledSongs[swapIndex]) = (shuffledSongs[swapIndex], shuffledSongs[0]);
                }

                foreach (SongIndex songIndex in shuffledSongs)
                {
                    gameplaySongDeck.Enqueue(songIndex);
                }
            }

            SongIndex nextSong = gameplaySongDeck.Dequeue();
            lastPlayedGameplaySong = nextSong;

            return nextSong;
        }

        private static async Task FadeOutAsync(CancellationToken token)
        {
            int steps = (int)(SongConstants.FADE_DURATION_MS / SongConstants.FADE_STEP_INTERVAL_MS);

            for (int i = 0; i < steps; i++)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }

                fadeFactor = MathHelper.Lerp(1f, 0f, i / (float)steps);
                ApplyFinalVolume();

                await Task.Delay((int)SongConstants.FADE_STEP_INTERVAL_MS, token);
            }

            fadeFactor = 0f;
            ApplyFinalVolume();
        }

        private static async Task FadeInAsync(CancellationToken token)
        {
            int steps = (int)(SongConstants.FADE_DURATION_MS / SongConstants.FADE_STEP_INTERVAL_MS);

            for (int i = 0; i < steps; i++)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }

                fadeFactor = MathHelper.Lerp(0f, 1f, i / (float)steps);
                ApplyFinalVolume();

                await Task.Delay((int)SongConstants.FADE_STEP_INTERVAL_MS, token);
            }

            fadeFactor = 1f;
            ApplyFinalVolume();
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
                    await Task.Delay(TimeSpan.FromSeconds(Core.Random.Range(30, 60)), token);

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

