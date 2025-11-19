using Microsoft.Xna.Framework.Media;

namespace StardustSandbox.AudioSystem
{
    internal static class SongEngine
    {
        internal static Song CurrentSong { get; private set; }

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

        internal static void Play(Song song)
        {
            if (MediaPlayer.State != MediaState.Stopped)
            {
                Stop();
            }

            CurrentSong = song;
            MediaPlayer.Play(song);
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
