using Microsoft.Xna.Framework.Media;

namespace StardustSandbox.Core.Audio
{
    public static class SSongEngine
    {
        public static float Volume
        {
            get
            {
                return MediaPlayer.Volume;
            }

            set
            {
                MediaPlayer.Volume = value;
            }
        }

        public static bool IsMuted
        {
            get
            {
                return MediaPlayer.IsMuted;
            }

            set
            {
                MediaPlayer.IsMuted = value;
            }
        }

        public static bool IsRepeating
        {
            get
            {
                return MediaPlayer.IsRepeating;
            }

            set
            {
                MediaPlayer.IsRepeating = value;
            }
        }

        public static bool IsShuffled
        {
            get
            {
                return MediaPlayer.IsShuffled;
            }

            set
            {
                MediaPlayer.IsShuffled = value;
            }
        }

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
