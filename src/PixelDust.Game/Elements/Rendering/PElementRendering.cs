using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Elements.Contexts;
using PixelDust.Game.Objects;

using System;

namespace PixelDust.Game.Elements.Rendering
{
    public sealed class PElementRendering(PElement element) : PGameObject
    {
        private PElementRenderingMechanism renderingMechanism;

        // Dependencies
        private readonly PElement _element = element;
        private PElementContext _context;

        protected override void OnUpdate(GameTime gameTime)
        {
            this.renderingMechanism.Update(gameTime, this._context);
        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (this.renderingMechanism == null)
            {
                throw new InvalidOperationException($"No rendering mechanism has been implemented for the {_element.Name} element. Please ensure a valid rendering mechanism is set using the {nameof(SetRenderingMechanism)} method.");
            }

            if (this._element.Texture == null)
            {
                return;
            }

            this.renderingMechanism.Draw(gameTime, spriteBatch, this._context);
        }

        public void SetRenderingMechanism(PElementRenderingMechanism renderingMechanism)
        {
            this.renderingMechanism = renderingMechanism;
            this.renderingMechanism.Initialize(this._element);
        }

        public void UpdateContext(PElementContext context)
        {
            this._context = context;
        }
    }
}