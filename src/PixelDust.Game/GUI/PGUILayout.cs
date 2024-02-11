using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.GUI.Elements;
using PixelDust.Game.GUI.Interfaces;
using PixelDust.Game.Objects;

using System.Collections.Generic;

namespace PixelDust.Game.GUI
{
    public sealed class PGUILayout : PGameObject, IPGUILayoutBuilder
    {
        public PGUIElement Root => this.root;
        public PGUIElement[] Elements => [.. this.elements];

        private readonly PGUIElement root;
        private readonly List<PGUIElement> elements = [];

        protected override void OnAwake()
        {
            base.OnAwake();
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
    }
}
