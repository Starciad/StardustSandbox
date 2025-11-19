using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace StardustSandbox.UISystem.Elements
{
    internal class ContainerUIElement : UIElement
    {
        internal IEnumerable<UIElement> Elements => this.containerLayout.Elements;

        private readonly Layout containerLayout;

        internal ContainerUIElement()
        {
            this.containerLayout = new();

            this.IsVisible = true;
            this.ShouldUpdate = true;
        }

        internal override void Initialize()
        {
            this.containerLayout.Initialize();
        }

        internal override void Update(GameTime gameTime)
        {
            this.containerLayout.Update(gameTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            this.containerLayout.Draw(spriteBatch);
        }

        internal void Active()
        {
            this.containerLayout.IsActive = true;
        }

        internal void Disable()
        {
            this.containerLayout.IsActive = false;
        }

        internal void AddElement<T>(T value) where T : UIElement
        {
            this.containerLayout.AddElement(value);
        }
    }
}
