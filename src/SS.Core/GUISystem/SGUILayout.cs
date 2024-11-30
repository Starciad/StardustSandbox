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
        public int ElementCount => this.elements.Count;

        private readonly List<SGUIElement> elements = [];

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < this.elements.Count; i++)
            {
                SGUIElement element = this.elements[i];

                if (element.ShouldUpdate)
                {
                    element.Update(gameTime);
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.elements.Count; i++)
            {
                SGUIElement element = this.elements[i];

                if (element.IsVisible)
                {
                    element.Draw(gameTime, spriteBatch);
                }
            }
        }

        public void AddElement<T>(T value) where T : SGUIElement
        {
            this.elements.Add(value);
        }
    }
}