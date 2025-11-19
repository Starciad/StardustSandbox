using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Interfaces;

namespace StardustSandbox.AnimationSystem
{
    internal sealed class Animation(AnimationFrame[] frames) : IResettable
    {
        internal Texture2D Texture { get; set; }
        internal AnimationFrame CurrentFrame => this.frames[this.currentFrameIndex];

        internal bool Paused { get; set; } = false;
        internal bool Loop { get; set; } = true;
        internal bool RunOnlyOnce { get; set; } = false;

        private readonly AnimationFrame[] frames = frames;
        private uint currentFrameIndex = 0;
        private float currentDuration = 0.0f;

        public void Reset()
        {
            this.currentFrameIndex = 0;
            this.currentDuration = 0f;
        }

        internal void Update(GameTime gameTime)
        {
            if (this.Paused && this.frames.Length <= 1)
            {
                return;
            }

            this.currentDuration += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (this.currentDuration >= this.CurrentFrame.Duration)
            {
                this.currentDuration = 0f;
                NextFrame();
            }
        }

        internal void NextFrame()
        {
            if (this.currentFrameIndex < this.frames.Length - 1)
            {
                this.currentFrameIndex++;
                return;
            }

            if (this.Loop)
            {
                this.currentFrameIndex = 0;
            }

            if (this.RunOnlyOnce)
            {
                this.Paused = true;
            }
        }
    }
}
