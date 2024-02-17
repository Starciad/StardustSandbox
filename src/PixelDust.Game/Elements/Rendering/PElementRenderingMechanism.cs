using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Camera;
using PixelDust.Game.Elements.Contexts;

namespace PixelDust.Game.Elements.Rendering
{
    public abstract class PElementRenderingMechanism
    {
        public abstract void Initialize(PElement element);
        public abstract void Draw(POrthographicCamera camera, GameTime gameTime, SpriteBatch spriteBatch, PElementContext context);
    }
}
