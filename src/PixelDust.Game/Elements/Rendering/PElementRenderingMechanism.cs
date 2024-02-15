using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Elements.Contexts;

namespace PixelDust.Game.Elements.Rendering
{
    public abstract class PElementRenderingMechanism
    {
        protected PElement Element { get; private set; }

        public virtual void Initialize(PElement element)
        {
            this.Element = element;
        }
        public virtual void Update(GameTime gameTime, PElementContext context) { return; }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, PElementContext context) { return; }
    }
}
