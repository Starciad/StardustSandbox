using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Elements.Contexts;

namespace StardustSandbox.Game.Elements.Rendering
{
    public abstract class SElementRenderingMechanism
    {
        public void Initialize(SElement element)
        {
            OnInitialize(element);
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, SElementContext context)
        {
            OnDraw(gameTime, spriteBatch, context);
        }

        protected virtual void OnInitialize(SElement element) { return; }
        protected virtual void OnDraw(GameTime gameTime, SpriteBatch spriteBatch, SElementContext context) { return; }
    }
}
