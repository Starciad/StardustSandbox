using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Worlding;
using PixelDust.Game.Constants;
using PixelDust.Game.Elements.Contexts;
using PixelDust.Game.Managers;
using PixelDust.Game.Objects;

using System.Collections.Generic;

namespace PixelDust.Game.Elements.Renders
{
    public sealed class PElementRender : PGameObject
    {
        public bool EnableAnimation { get; set; }
        public float AnimationDelay { get; set; }

        // Animation
        private float _currentAnimationDelay;
        private int _currentAnimationIndex;
        private readonly List<Rectangle> frames = [];

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

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
        //protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        //{
        //    base.OnDraw(gameTime, spriteBatch);

        //    Vector2 pos = new(context.Position.X * PWorldConstants.GRID_SCALE, context.Position.Y * PWorldConstants.GRID_SCALE);
        //    Rectangle rectangle = this.frames[this._currentAnimationIndex];

        //    PGraphicsManager.SpriteBatch.Draw(PTextures.Elements, pos, rectangle, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        //}

        public void AddFrame(Vector2 pos)
        {
            this.frames.Add(new(new((int)pos.X * PSpritesConstants.SPRITE_DEFAULT_WIDTH, (int)pos.Y * PSpritesConstants.SPRITE_DEFAULT_HEIGHT), new(PSpritesConstants.SPRITE_SCALE)));
        }
    }
}