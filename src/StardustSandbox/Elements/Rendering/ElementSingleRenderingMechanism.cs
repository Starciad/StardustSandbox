using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.AnimationSystem;
using StardustSandbox.Constants;
using StardustSandbox.Enums.World;
using StardustSandbox.Extensions;

namespace StardustSandbox.Elements.Rendering
{
    internal sealed class ElementSingleRenderingMechanism : ElementRenderingMechanism
    {
        private readonly Animation animation;

        internal ElementSingleRenderingMechanism()
        {
            this.animation = new([new(new(new(0), new(SpriteConstants.SPRITE_SCALE)), 0)]);
        }

        internal ElementSingleRenderingMechanism(Animation animation)
        {
            this.animation = animation;
        }

        internal override void Initialize(Element element)
        {
            this.animation.Texture = element.Texture;
        }

        internal override void Update(GameTime gameTime)
        {
            this.animation.Update(gameTime);
        }

        internal override void Draw(SpriteBatch spriteBatch, ElementContext context)
        {
            Color colorModifier = context.Slot.GetLayer(context.Layer).ColorModifier;

            if (context.Layer == LayerType.Background)
            {
                colorModifier = colorModifier.Darken(WorldConstants.BACKGROUND_COLOR_DARKENING_FACTOR);
            }

            spriteBatch.Draw(this.animation.Texture, new Vector2(context.Slot.Position.X, context.Slot.Position.Y) * WorldConstants.GRID_SIZE, this.animation.CurrentFrame.TextureClipArea, colorModifier, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }
    }
}
