using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Objects;

namespace StardustSandbox.Core.Animations
{
    public sealed class SAnimation(ISGame gameInstance, Texture2D texture, SAnimationFrame[] frames) : SGameObject(gameInstance), ISReset
    {
        public Texture2D Texture => texture;
        public SAnimationFrame CurrentFrame => this.frames[this.currentFrameIndex];

        public bool Paused { get; set; } = false;
        public bool Loop { get; set; } = true;
        public bool RunOnlyOnce { get; set; } = false;

        private readonly SAnimationFrame[] frames = frames;
        private uint currentFrameIndex = 0;
        private float currentDuration = 0f;

        public override void Update(GameTime gameTime)
        {
            if (this.Paused)
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
            if (this.currentFrameIndex < this.frames.Length)
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
