using Microsoft.Xna.Framework.Media;

using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;

namespace StardustSandbox.AudioSystem
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

        internal static void Play(SongIndex songIndex)
        {
            if (MediaPlayer.State != MediaState.Stopped)
            {
                Stop();
            }

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
    }
}
