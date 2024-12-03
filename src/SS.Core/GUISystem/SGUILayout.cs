using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Objects;

using System.Collections.Generic;

namespace StardustSandbox.Core.GUISystem
{
    public sealed class SGUILayout(ISGame gameInstance) : SGameObject(gameInstance), ISGUILayoutBuilder
    {
        private readonly List<SGUIElement> elements = [];

        public override void Update(GameTime gameTime)
        {
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
            foreach (SGUIElement element in this.elements)
            {
                if (!element.IsVisible)
                {
                    continue;
                }

                element.Draw(gameTime, spriteBatch);
            }
        }

        public void AddElement<T>(T value) where T : SGUIElement
        {
            this.elements.Add(value);
        }
    }
}