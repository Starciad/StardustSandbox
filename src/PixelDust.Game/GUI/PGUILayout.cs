using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Constants;
using PixelDust.Game.Enums.GUI;
using PixelDust.Game.GUI.Elements;
using PixelDust.Game.GUI.Elements.Common;
using PixelDust.Game.GUI.Interfaces;
using PixelDust.Game.Mathematics;
using PixelDust.Game.Objects;

using System.Collections.Generic;

namespace PixelDust.Game.GUI
{
    public sealed class PGUILayout(PGUILayoutPool layoutPool) : PGameObject, IPGUILayoutBuilder
    {
        public PGUIRootElement RootElement => this.rootElement;
        public int ElementCount => this.elements.Count;

        private readonly List<PGUIElement> elements = [];

        private PGUIRootElement rootElement = null;

        protected override void OnUpdate(GameTime gameTime)
        {
            foreach (PGUIElement element in this.elements)
            {
                element.Update(gameTime);
            }
        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (PGUIElement element in this.elements)
            {
                element.Draw(gameTime, spriteBatch);
            }
        }

        public T CreateElement<T>() where T : PGUIElement
        {
            T element = layoutPool.GetElement<T>(this.Game);
            this.elements.Add(element);

            return element;
        }

        public void Load()
        {
            this.rootElement = CreateElement<PGUIRootElement>();
            this.rootElement.SetPositioningType(PPositioningType.Fixed);
            this.rootElement.SetSize(new Size2(PScreenConstants.DEFAULT_SCREEN_WIDTH, PScreenConstants.DEFAULT_SCREEN_HEIGHT));
        }

        public void Unload()
        {
            this.elements.ForEach(x => layoutPool.AddElement(x));
            this.elements.Clear();
        }
    }
}