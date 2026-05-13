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

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.States;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.Serialization.Settings;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StardustSandbox.Core.Audio
{
    internal sealed class SongSystem
    {
        internal Song CurrentSong { get; private set; }
        internal SongIndex CurrentSongIndex { get; private set; }

        private float fadeFactor = 1f;
        private VolumeSettings currentVolumeSettings;

        private SongIndex lastPlayedGameplaySong;

        private CancellationTokenSource gameplayMusicToken;
        private Task gameplayMusicTask;

        private readonly AssetDatabase assetDatabase;
        private readonly GameLaunchOptions gameLaunchOptions;
        private readonly Queue<SongIndex> gameplaySongDeck = [];

        internal SongSystem(AssetDatabase assetDatabase, GameLaunchOptions gameLaunchOptions)
        {
            this.assetDatabase = assetDatabase;
            this.gameLaunchOptions = gameLaunchOptions;

            this.currentVolumeSettings = SettingsSerializer.Load<VolumeSettings>();
            ApplyFinalVolume();
        }

        internal void ApplyVolumeSettings(VolumeSettings volumeSettings)
        {
            this.currentVolumeSettings = volumeSettings;
            ApplyFinalVolume();
        }

        private void ApplyFinalVolume()
        {
            float baseVolume = this.currentVolumeSettings.MusicVolume * this.currentVolumeSettings.MasterVolume;
            MediaPlayer.Volume = baseVolume * this.fadeFactor;
        }

        internal void Play(SongIndex songIndex)
        {
            this.CurrentSong = this.assetDatabase.GetSong(songIndex);
            this.CurrentSongIndex = songIndex;

            this.fadeFactor = 1f;
            ApplyFinalVolume();

            MediaPlayer.Stop();
            MediaPlayer.Play(this.CurrentSong);
        }

        private SongIndex GetNextGameplaySong()
        {
            if (this.gameplaySongDeck.Count == 0)
            {
                List<SongIndex> shuffledSongs = [.. SongConstants.GAMEPLAY_SONGS.Shuffle()];

                // Prevents the first song in the new deck
                // from being the same as the last song in the previous deck
                if (this.lastPlayedGameplaySong != SongIndex.None && shuffledSongs.Count > 1 && shuffledSongs[0] == this.lastPlayedGameplaySong)
                {
                    int swapIndex = 1 + Randomness.Random.Range(0, shuffledSongs.Count - 1);

                    (shuffledSongs[0], shuffledSongs[swapIndex]) = (shuffledSongs[swapIndex], shuffledSongs[0]);
                }

                foreach (SongIndex songIndex in shuffledSongs)
                {
                    this.gameplaySongDeck.Enqueue(songIndex);
                }
            }

            SongIndex nextSong = this.gameplaySongDeck.Dequeue();
            this.lastPlayedGameplaySong = nextSong;

            return nextSong;
        }

        private async Task FadeOutAsync(CancellationToken token)
        {
            int steps = (int)(SongConstants.FADE_DURATION_MS / SongConstants.FADE_STEP_INTERVAL_MS);

            for (int i = 0; i < steps; i++)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }

                this.fadeFactor = MathHelper.Lerp(1f, 0f, i / (float)steps);
                ApplyFinalVolume();

                await Task.Delay((int)SongConstants.FADE_STEP_INTERVAL_MS, token);
            }

            this.fadeFactor = 0f;
            ApplyFinalVolume();
        }

        private async Task FadeInAsync(CancellationToken token)
        {
            int steps = (int)(SongConstants.FADE_DURATION_MS / SongConstants.FADE_STEP_INTERVAL_MS);

            for (int i = 0; i < steps; i++)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }

                this.fadeFactor = MathHelper.Lerp(0f, 1f, i / (float)steps);
                ApplyFinalVolume();

                await Task.Delay((int)SongConstants.FADE_STEP_INTERVAL_MS, token);
            }

            this.fadeFactor = 1f;
            ApplyFinalVolume();
        }

        internal void StartGameplayMusicCycle()
        {
            if (this.gameplayMusicTask != null && !this.gameplayMusicTask.IsCompleted)
            {
                return;
            }

            this.gameplayMusicToken = new CancellationTokenSource();
            CancellationToken token = this.gameplayMusicToken.Token;

            this.gameplayMusicTask = Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    if (!this.gameLaunchOptions.NoMusicDelay)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(Randomness.Random.Range(30, 60)), token);
                    }

                    if (token.IsCancellationRequested)
                    {
                        break;
                    }

                    while (!GameHandler.HasState(GameStates.IsFocused) && !token.IsCancellationRequested)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(1), token);
                    }

                    SongIndex songIndex = GetNextGameplaySong();
                    this.CurrentSong = this.assetDatabase.GetSong(songIndex);
                    this.CurrentSongIndex = songIndex;

                    this.fadeFactor = 0f;
                    ApplyFinalVolume();

                    MediaPlayer.Play(this.CurrentSong);
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

        internal void StopGameplayMusicCycle()
        {
            if (this.gameplayMusicToken == null)
            {
                return;
            }

            this.gameplayMusicToken.Cancel();
            this.gameplayMusicToken.Dispose();

            this.gameplayMusicToken = null;
            this.gameplayMusicTask = null;

            this.fadeFactor = 1f;
            ApplyFinalVolume();

            MediaPlayer.Stop();
        }
    }
}
