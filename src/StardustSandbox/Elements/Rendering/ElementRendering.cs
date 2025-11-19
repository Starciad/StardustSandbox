using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace StardustSandbox.Elements.Rendering
{
    internal sealed class ElementRendering(Element element)
    {
        private ElementRenderingMechanism renderingMechanism;

        private readonly Element element = element;
        private ElementContext context;

        internal void Update(GameTime gameTime)
        {
            if (this.renderingMechanism == null)
            {
                throw new InvalidOperationException($"No rendering mechanism has been implemented for the {this.element.GetType().Name} element. Please ensure a valid rendering mechanism is set using the {nameof(SetRenderingMechanism)} method.");
            }

            this.renderingMechanism.Update(gameTime);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            if (this.renderingMechanism == null)
            {
                throw new InvalidOperationException($"No rendering mechanism has been implemented for the {this.element.GetType().Name} element. Please ensure a valid rendering mechanism is set using the {nameof(SetRenderingMechanism)} method.");
            }

            if (this.element.Texture == null)
            {
                return;
            }

            this.renderingMechanism.Draw(spriteBatch, this.context);
        }

        internal void SetRenderingMechanism(ElementRenderingMechanism renderingMechanism)
        {
            this.renderingMechanism = renderingMechanism;
            this.renderingMechanism.Initialize(this.element);
        }

        internal void SetContext(ElementContext context)
        {
            this.context = context;
        }
    }
}