using Microsoft.Xna.Framework.Media;

namespace StardustSandbox.Core.Audio
{
    public static class SSongEngine
    {
        public static float Volume
        {
            get => MediaPlayer.Volume;

            set => MediaPlayer.Volume = value;
        }

        public static bool IsMuted
        {
            get => MediaPlayer.IsMuted;

            set => MediaPlayer.IsMuted = value;
        }

        public static bool IsRepeating
        {
            get => MediaPlayer.IsRepeating;

            set => MediaPlayer.IsRepeating = value;
        }

        public static bool IsShuffled
        {
            get => MediaPlayer.IsShuffled;

            set => MediaPlayer.IsShuffled = value;
        }

        public static MediaState State => MediaPlayer.State;

        public static void Play(Song song)
        {
            if (MediaPlayer.State != MediaState.Stopped)
            {
                Stop();
            }

            MediaPlayer.Play(song);
        }

        public static void Stop()
        {
            MediaPlayer.Stop();
        }

        public static void Pause()
        {
            MediaPlayer.Pause();
        }

        public static void Resume()
        {
            MediaPlayer.Resume();
        }
    }
}
