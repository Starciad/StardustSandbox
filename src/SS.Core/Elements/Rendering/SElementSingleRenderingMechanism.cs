using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Animations;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Elements.Contexts;

namespace StardustSandbox.Core.Elements.Rendering
{
    public sealed class SElementSingleRenderingMechanism : SElementRenderingMechanism
    {
        private readonly SAnimation animation;

        public SElementSingleRenderingMechanism(ISGame gameInstance)
        {
            this.animation = new(gameInstance, [new(new(new(0), new(SSpritesConstants.SPRITE_SCALE)), 0)]);
        }

        public SElementSingleRenderingMechanism(SAnimation animation)
        {
            this.animation = animation;
        }

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
            Color colorModifier = context.Slot.GetLayer(context.Layer).ColorModifier;

            if (context.Layer == SWorldLayer.Background)
            {
                colorModifier = colorModifier.Darken(SWorldConstants.BACKGROUND_COLOR_DARKENING_FACTOR);
            }

            spriteBatch.Draw(this.animation.Texture, new Vector2(context.Slot.Position.X, context.Slot.Position.Y) * SWorldConstants.GRID_SIZE, this.animation.CurrentFrame.TextureClipArea, colorModifier, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }
    }
}
