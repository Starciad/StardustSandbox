using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Animations;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Extensions;
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
            if (!context.Slot.ForegroundLayer.IsEmpty)
            {
                return;
            }

            Color colorModifier = context.Slot.GetLayer(context.Layer).ColorModifier;

            if (context.Layer == SWorldLayer.Background)
            {
                colorModifier = colorModifier.Darken(SWorldConstants.BACKGROUND_COLOR_DARKENING_FACTOR);
            }

            spriteBatch.Draw(this.animation.Texture, new Vector2(context.Slot.Position.X, context.Slot.Position.Y) * SWorldConstants.GRID_SCALE, this.animation.CurrentFrame.TextureClipArea, colorModifier, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }
    }
}
