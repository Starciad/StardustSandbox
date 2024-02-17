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
            if (ShouldRender(camera, context))
            {
                OnDraw(gameTime, spriteBatch, context);
            }
        }

        private static bool ShouldRender(PCameraManager camera, PElementContext context)
        {
            Vector2Int elementPosition = context.Position;
            Vector2Int cameraPosition = camera.Position * PWorldConstants.GRID_SCALE;

            float zoomValue = 2 * camera.Zoom;

            bool isInCamera = elementPosition.X > cameraPosition.X - PScreenConstants.DEFAULT_SCREEN_WIDTH / zoomValue &&
                              elementPosition.X < cameraPosition.X + PScreenConstants.DEFAULT_SCREEN_WIDTH / zoomValue &&
                              elementPosition.Y < cameraPosition.Y - PScreenConstants.DEFAULT_SCREEN_HEIGHT / zoomValue &&
                              elementPosition.Y > cameraPosition.Y + PScreenConstants.DEFAULT_SCREEN_HEIGHT / zoomValue;

            return isInCamera;
        }

        protected virtual void OnInitialize(PElement element) { return; }
        protected virtual void OnDraw(GameTime gameTime, SpriteBatch spriteBatch, PElementContext context) { return; }
    }
}
