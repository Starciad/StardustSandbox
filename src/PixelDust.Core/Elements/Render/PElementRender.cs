using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Elements.Context;
using PixelDust.Core.Engine.Assets;
using PixelDust.Core.Engine.Components;
using PixelDust.Core.Worlding;

using System.Collections.Generic;

namespace PixelDust.Core.Elements.Render
{
    public sealed class PElementRender
    {
        internal const int SpriteSize = PWorld.Scale;

        public bool EnableAnimation { get; set; }
        public float AnimationDelay { get; set; }

        // Animation
        private float _currentAnimationDelay;
        private int _currentAnimationIndex;
        private readonly List<Rectangle> frames = new();

        public void AddFrame(Vector2 pos)
        {
            this.frames.Add(new(new((int)pos.X * SpriteSize, (int)pos.Y * SpriteSize), new(SpriteSize)));
        }

        internal void Update()
        {
            if (this.EnableAnimation)
            { UpdateAnimation(); }
        }
        private void UpdateAnimation()
        {
            if (this._currentAnimationDelay < this.AnimationDelay)
            {
                this._currentAnimationDelay++;
            }
            else
            {
                this._currentAnimationDelay = 0;
                this._currentAnimationIndex = this._currentAnimationIndex < this.frames.Count ? this._currentAnimationIndex + 1 : 0;
            }
        }

        internal void Draw(PElementContext context)
        {
            Vector2 pos = new(context.Position.X * PWorld.Scale, context.Position.Y * PWorld.Scale);
            Rectangle rectangle = this.frames[this._currentAnimationIndex];

            PGraphics.SpriteBatch.Draw(PTextures.Elements, pos, rectangle, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }
    }
}