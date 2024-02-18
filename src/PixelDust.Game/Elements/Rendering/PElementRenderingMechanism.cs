using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Elements.Contexts;

namespace PixelDust.Game.Elements.Rendering
{
    public abstract class PElementRenderingMechanism
    {
        public void Initialize(PElement element)
        {
            OnInitialize(element);
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, PElementContext context)
        {
            OnDraw(gameTime, spriteBatch, context);
        }

        protected virtual void OnInitialize(PElement element) { return; }
        protected virtual void OnDraw(GameTime gameTime, SpriteBatch spriteBatch, PElementContext context) { return; }
    }
}
