using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Constants;
using StardustSandbox.Game.Elements;
using StardustSandbox.Game.Elements.Contexts;
using StardustSandbox.Game.Elements.Rendering;

namespace StardustSandbox.Game.GameContent.Elements.Rendering
{
    public sealed class SElementSingleRenderingMechanism : SElementRenderingMechanism
    {
        private Rectangle? clipAreaRectangle;
        private Texture2D elementTexture;

        public SElementSingleRenderingMechanism()
        {
            this.clipAreaRectangle = null;
        }
        public SElementSingleRenderingMechanism(Rectangle? clipAreaRectangle)
        {
            this.clipAreaRectangle = clipAreaRectangle;
        }

        protected override void OnInitialize(SElement element)
        {
            this.elementTexture = element.Texture;
        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch, SElementContext context)
        {
            spriteBatch.Draw(this.elementTexture, new Vector2(context.Position.X, context.Position.Y) * SWorldConstants.GRID_SCALE, this.clipAreaRectangle, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }
    }
}
