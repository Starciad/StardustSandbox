using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.Interfaces;

using System.Collections.Generic;

namespace StardustSandbox.ContentBundle.GUISystem.Elements
{
    internal class SGUIContainerElement : SGUIElement
    {
        public IEnumerable<SGUIElement> Elements => this.containerLayout.Elements;

        private readonly SGUILayout containerLayout;

        internal SGUIContainerElement(ISGame gameInstance) : base(gameInstance)
        {
            this.containerLayout = new(gameInstance);

            this.IsVisible = true;
            this.ShouldUpdate = true;
        }

        public override void Initialize()
        {
            this.containerLayout.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            this.containerLayout.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.containerLayout.Draw(gameTime, spriteBatch);
        }

        internal void Active()
        {
            this.containerLayout.IsActive = true;
        }

        internal void Disable()
        {
            this.containerLayout.IsActive = false;
        }

        internal void AddElement<T>(T value) where T : SGUIElement
        {
            this.containerLayout.AddElement(value);
        }
    }
}
