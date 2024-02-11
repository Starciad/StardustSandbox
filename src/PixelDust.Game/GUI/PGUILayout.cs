using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.GUI.Elements;
using PixelDust.Game.GUI.Elements.Common;
using PixelDust.Game.GUI.Interfaces;
using PixelDust.Game.Objects;

using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelDust.Game.GUI
{
    public sealed class PGUILayout : PGameObject, IPGUILayoutBuilder
    {
        public PGUIRootElement Root => this.root;
        public PGUIElement[] Elements => [.. this.elements];

        private readonly List<PGUIElement> elements = [];
        private readonly List<PGUIElement> lastOpenElements = [];

        private PGUIRootElement root;

        protected override void OnAwake()
        {
            this.root = OpenElement<PGUIRootElement>();
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            foreach (PGUIElement element in Elements)
            {
                element.Update(gameTime);
            }
        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (this.root != null)
            {
                DrawElementsRecursively(this.root, gameTime, spriteBatch);
            }
        }

        private static void DrawElementsRecursively(PGUIElement element, GameTime gameTime, SpriteBatch spriteBatch)
        {
            element.Draw(gameTime, spriteBatch);

            if (element.HasChildren)
            {
                foreach (PGUIElement child in element.Children)
                {
                    DrawElementsRecursively(child, gameTime, spriteBatch);
                }
            }
        }

        public T OpenElement<T>() where T : PGUIElement
        {
            T element = CreateElement<T>();
            element.Open();
            this.lastOpenElements.Add(element);
            return element;
        }

        public T CreateElement<T>() where T : PGUIElement
        {
            T element = Activator.CreateInstance<T>();

            this.elements.Add(element);

            if (this.lastOpenElements.Count > 0)
            {
                this.lastOpenElements.Last().AppendChild(element);
            }

            element.SetRootElement(this.root);
            element.Close();
            element.Initialize(this.Game);

            return element;
        }

        public void CloseElement()
        {
            PGUIElement element = this.lastOpenElements.Last();
            element.Close();
            this.lastOpenElements.Remove(element);
        }
    }
}
