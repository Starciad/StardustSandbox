using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Interfaces.Elements;
using StardustSandbox.Game.Objects;

using System;

namespace StardustSandbox.Game.Elements.Rendering
{
    public sealed class SElementRendering(SGame gameInstance, SElement element) : SGameObject(gameInstance)
    {
        private SElementRenderingMechanism renderingMechanism;

        private readonly SElement _element = element;
        private ISElementContext _context;

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (this.renderingMechanism == null)
            {
                throw new InvalidOperationException($"No rendering mechanism has been implemented for the {this._element.GetType().Name} element. Please ensure a valid rendering mechanism is set using the {nameof(SetRenderingMechanism)} method.");
            }

            if (this._element.Texture == null)
            {
                return;
            }

            this.renderingMechanism.Draw(gameTime, spriteBatch, this._context);
        }

        public void SetRenderingMechanism(SElementRenderingMechanism renderingMechanism)
        {
            this.renderingMechanism = renderingMechanism;
            this.renderingMechanism.Initialize(this._element);
        }

        internal void SetContext(ISElementContext context)
        {
            this._context = context;
        }
    }
}