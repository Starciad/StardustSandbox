﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Constants;
using PixelDust.Game.Elements.Contexts;

namespace PixelDust.Game.Elements.Rendering.Common
{
    public sealed class PElementSingleRenderingMechanism : PElementRenderingMechanism
    {
        private Rectangle? clipAreaRectangle;

        public PElementSingleRenderingMechanism()
        {
            this.clipAreaRectangle = null;
        }
        public PElementSingleRenderingMechanism(Rectangle? clipAreaRectangle)
        {
            this.clipAreaRectangle = clipAreaRectangle;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, PElementContext context)
        {
            spriteBatch.Draw(this.Element.Texture, new Vector2(context.Position.X, context.Position.Y) * PWorldConstants.GRID_SCALE, this.clipAreaRectangle, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }
    }
}
