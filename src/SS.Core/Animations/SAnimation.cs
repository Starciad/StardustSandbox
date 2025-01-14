using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.System;
using StardustSandbox.Core.Objects;

namespace StardustSandbox.Core.Animations
{
    public sealed class SAnimation(ISGame gameInstance, SAnimationFrame[] frames) : SGameObject(gameInstance), ISResettable
    {
        public Texture2D Texture { get; set; }
        public SAnimationFrame CurrentFrame => this.frames[this.currentFrameIndex];

        public bool Paused { get; set; } = false;
        public bool Loop { get; set; } = true;
        public bool RunOnlyOnce { get; set; } = false;

        private readonly SAnimationFrame[] frames = frames;
        private uint currentFrameIndex = 0;
        private float currentDuration = 0f;

        public override void Update(GameTime gameTime)
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

        public void NextFrame()
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

        public void Reset()
        {
            this.currentFrameIndex = 0;
            this.currentDuration = 0f;
        }
    }
}
