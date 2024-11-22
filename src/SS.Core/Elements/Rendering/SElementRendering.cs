using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Objects;

using System;

namespace StardustSandbox.Core.Elements.Rendering
{
    public sealed class SElementRendering(ISGame gameInstance, SElement element) : SGameObject(gameInstance)
    {
        private SElementRenderingMechanism renderingMechanism;

        private readonly SElement _element = element;
        private ISElementContext _context;

        public override void Update(GameTime gameTime)
        {
            if (this.renderingMechanism == null)
            {
                throw new InvalidOperationException($"No rendering mechanism has been implemented for the {this._element.GetType().Name} element. Please ensure a valid rendering mechanism is set using the {nameof(SetRenderingMechanism)} method.");
            }

            this.renderingMechanism.Update(gameTime);
        }

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