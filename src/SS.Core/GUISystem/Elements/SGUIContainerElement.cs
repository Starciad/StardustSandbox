using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Interfaces.General;

using System.Collections.Generic;

namespace StardustSandbox.Core.GUISystem.Elements
{
    public class SGUIContainerElement : SGUIElement
    {
        private readonly List<SGUIElement> elements = [];

        public SGUIContainerElement(ISGame gameInstance) : base(gameInstance)
        {
            this.IsVisible = true;
            this.ShouldUpdate = true;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (SGUIElement element in this.elements)
            {
                if (!element.ShouldUpdate)
                {
                    continue;
                }

                element.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            foreach (SGUIElement element in this.elements)
            {
                if (!element.IsVisible)
                {
                    continue;
                }

                element.Draw(gameTime, spriteBatch);
            }
        }

        public void AddElement(SGUIElement value)
        {
            this.elements.Add(value);
        }
    }
}
