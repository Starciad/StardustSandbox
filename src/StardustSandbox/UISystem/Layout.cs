using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.UISystem.Elements;

using System.Collections.Generic;

namespace StardustSandbox.UISystem
{
    internal sealed class Layout
    {
        internal IEnumerable<UIElement> Elements => this.elements;
        internal bool IsActive { get; set; } = true;

        private readonly List<UIElement> elements = [];

        internal void Initialize()
        {
            foreach (UIElement element in this.elements)
            {
                element.Initialize();
            }
        }

        internal void Update(GameTime gameTime)
        {
            if (!this.IsActive)
            {
                return;
            }

            foreach (UIElement element in this.elements)
            {
                if (!element.ShouldUpdate)
                {
                    continue;
                }

                element.Update(gameTime);
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            if (!this.IsActive)
            {
                return;
            }

            foreach (UIElement element in this.elements)
            {
                if (!element.IsVisible)
                {
                    continue;
                }

                element.Draw(spriteBatch);
            }
        }

        internal void AddElement<T>(T value) where T : UIElement
        {
            this.elements.Add(value);
        }

        internal void RemoveElement<T>(T value) where T : UIElement
        {
            _ = this.elements.Remove(value);
        }
    }
}