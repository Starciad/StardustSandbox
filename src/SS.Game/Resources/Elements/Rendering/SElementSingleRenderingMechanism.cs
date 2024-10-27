using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Constants;
using StardustSandbox.Game.Elements;
using StardustSandbox.Game.Elements.Rendering;
using StardustSandbox.Game.Interfaces.Elements;

namespace StardustSandbox.Game.Resources.Elements.Rendering
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

        public override void Initialize(SElement element)
        {
            this.elementTexture = element.Texture;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, ISElementContext context)
        {
            spriteBatch.Draw(this.elementTexture, new Vector2(context.Position.X, context.Position.Y) * SWorldConstants.GRID_SCALE, this.clipAreaRectangle, context.Slot.Color, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }
    }
}
