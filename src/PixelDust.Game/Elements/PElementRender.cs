using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Constants;
using PixelDust.Game.Elements.Contexts;
using PixelDust.Game.Objects;

namespace PixelDust.Game.Elements
{
    public sealed class PElementRender(Texture2D elementTexture) : PGameObject
    {
        private readonly Texture2D _elementTexture = elementTexture;
        private PElementContext _context;

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (this._elementTexture == null)
            {
                return;
            }

            Vector2 pos = new(this._context.Position.X * PWorldConstants.GRID_SCALE, this._context.Position.Y * PWorldConstants.GRID_SCALE);
            spriteBatch.Draw(this._elementTexture, pos, new Rectangle(new Point(0, 0), new Point(32, 32)), Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }

        public void UpdateContext(PElementContext context)
        {
            this._context = context;
        }
    }
}