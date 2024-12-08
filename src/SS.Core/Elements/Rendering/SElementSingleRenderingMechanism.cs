using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Animations;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Interfaces.Elements;

namespace StardustSandbox.Core.Elements.Rendering
{
    public sealed class SElementSingleRenderingMechanism(SAnimation animation) : SElementRenderingMechanism
    {
        private readonly SAnimation animation = animation;

        public override void Initialize(SElement element)
        {
            this.animation.Texture = element.Texture;
        }

        public override void Update(GameTime gameTime)
        {
            this.animation.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, ISElementContext context)
        {
            spriteBatch.Draw(this.animation.Texture, new Vector2(context.Position.X, context.Position.Y) * SWorldConstants.GRID_SCALE, this.animation.CurrentFrame.TextureClipArea, context.Slot.Color, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }
    }
}
