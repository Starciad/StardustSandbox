using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Constants;
using PixelDust.Game.Enums.GUI;
using PixelDust.Game.GUI.Elements;
using PixelDust.Game.GUI.Elements.Common;
using PixelDust.Game.GUI.Interfaces;
using PixelDust.Game.Mathematics;
using PixelDust.Game.Objects;

using System;
using System.Collections.Generic;

namespace PixelDust.Game.GUI
{
    public sealed class PGUILayout : PGameObject, IPGUILayoutBuilder
    {
        public PGUIRootElement RootElement => this.rootElement;
        public PGUIElement[] Elements => [.. this.elements];

        private readonly List<PGUIElement> elements = [];
        private PGUIRootElement rootElement = null;

        protected override void OnAwake()
        {
            this.rootElement = CreateElement<PGUIRootElement>();
            this.rootElement.Style.SetPositioningType(PPositioningType.Fixed);
            this.rootElement.Style.SetSize(new Size2(PScreenConstants.DEFAULT_SCREEN_WIDTH, PScreenConstants.DEFAULT_SCREEN_HEIGHT));
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            foreach (PGUIElement element in this.Elements)
            {
                element.Update(gameTime);
            }
        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (this.RootElement != null)
            {
                DrawElementsRecursively(this.RootElement, gameTime, spriteBatch);
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

        public T CreateElement<T>() where T : PGUIElement
        {
            T element = Activator.CreateInstance<T>();

            element.SetGUILayout(this);
            element.Initialize(this.Game);

            this.elements.Add(element);
            return element;
        }
    }
}
