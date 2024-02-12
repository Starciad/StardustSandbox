using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.GUI.Elements;
using PixelDust.Game.GUI.Interfaces;
using PixelDust.Game.Objects;

using System;
using System.Collections.Generic;

namespace PixelDust.Game.GUI
{
    public sealed class PGUILayout : PGameObject, IPGUILayoutBuilder
    {
        public PGUIElement[] Elements => [.. this.elements];

        private readonly List<PGUIElement> elements = [];

        protected override void OnUpdate(GameTime gameTime)
        {
            foreach (PGUIElement element in this.Elements)
            {
                element.Update(gameTime);
            }
        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (PGUIElement element in this.Elements)
            {
                element.Draw(gameTime, spriteBatch);
            }
        }

        public T CreateElement<T>() where T : PGUIElement
        {
            T element = Activator.CreateInstance<T>();
            element.Initialize(this.Game);

            this.elements.Add(element);
            return element;
        }
    }
}
