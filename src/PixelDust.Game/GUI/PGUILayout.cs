using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.GUI.Elements;
using PixelDust.Game.GUI.Elements.Common;
using PixelDust.Game.GUI.Interfaces;
using PixelDust.Game.Objects;
using PixelDust.Game.Enums.GUI;
using PixelDust.Game.Mathematics;

using System;
using System.Collections.Generic;
using System.Linq;
using PixelDust.Game.Constants;

namespace PixelDust.Game.GUI
{
    public sealed class PGUILayout : PGameObject, IPGUILayoutBuilder
    {
        public PGUIRootElement Root => this.root;
        public PGUIElement[] Elements => [.. this.elements];

        private readonly List<PGUIElement> elements = [];

        private PGUIRootElement root = null;

        protected override void OnAwake()
        {
            this.root = CreateElement<PGUIRootElement>();
            this.root.Style.PositioningType = PPositioningType.Fixed;
            this.root.Style.Size = new Size2(PScreenConstants.DEFAULT_SCREEN_WIDTH, PScreenConstants.DEFAULT_SCREEN_HEIGHT);
            this.root.Style.Color = Color.Transparent;
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

        public T CreateElement<T>() where T : PGUIElement
        {
            T element = Activator.CreateInstance<T>();
            element.SetRootElement(this.root);
            element.Initialize(this.Game);

            this.elements.Add(element);

            return element;
        }
    }
}
