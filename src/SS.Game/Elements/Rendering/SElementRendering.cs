using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Elements.Contexts;
using StardustSandbox.Game.Objects;

using System;

namespace StardustSandbox.Game.Elements.Rendering
{
    public sealed class SElementRendering : SGameObject
    {
        private SElementRenderingMechanism renderingMechanism;

        private readonly SElement _element;
        private SElementContext _context;

        public SElementRendering(SGame gameInstance, SElement element) : base(gameInstance)
        {
            this._element = element;
        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
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

        internal void SetContext(SElementContext context)
        {
            this._context = context;
        }
    }
}