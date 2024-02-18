using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Constants;
using PixelDust.Game.Elements.Contexts;
using PixelDust.Game.Managers;
using PixelDust.Game.Mathematics;

namespace PixelDust.Game.Elements.Rendering
{
    public abstract class PElementRenderingMechanism
    {
        public void Initialize(PElement element)
        {
            OnInitialize(element);
        }
        public void Draw(PCameraManager camera, GameTime gameTime, SpriteBatch spriteBatch, PElementContext context)
        {
            if (camera.InsideCameraBounds((Vector2)context.Position, new Size2(PWorldConstants.GRID_SCALE)))
            {
                OnDraw(gameTime, spriteBatch, context);
            }
        }

        protected virtual void OnInitialize(PElement element) { return; }
        protected virtual void OnDraw(GameTime gameTime, SpriteBatch spriteBatch, PElementContext context) { return; }
    }
}
