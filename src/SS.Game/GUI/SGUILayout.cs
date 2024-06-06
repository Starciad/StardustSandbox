using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Constants;
using StardustSandbox.Game.Enums.GUI;
using StardustSandbox.Game.GUI.Elements;
using StardustSandbox.Game.GUI.Elements.Common;
using StardustSandbox.Game.Interfaces.GUI;
using StardustSandbox.Game.Mathematics;
using StardustSandbox.Game.Objects;

using System.Collections.Generic;
using System.Linq;

namespace StardustSandbox.Game.GUI
{
    public sealed class SGUILayout(SGUILayoutPool layoutPool) : SGameObject, ISGUILayoutBuilder
    {
        public SGUIRootElement RootElement => this.rootElement;
        public int ElementCount => this.elements.Count;

        private readonly List<SGUIElement> elements = [];
        private SGUIElement[] elementsThatShouldUpdate;
        private SGUIElement[] elementsThatShouldDraw;

        private SGUIRootElement rootElement = null;

        public void Load()
        {
            this.rootElement = CreateElement<SGUIRootElement>();
            this.rootElement.SetPositioningType(SPositioningType.Fixed);
            this.rootElement.SetSize(new SSize2(SScreenConstants.DEFAULT_SCREEN_WIDTH, SScreenConstants.DEFAULT_SCREEN_HEIGHT));
        }

        public void Configure()
        {
            this.elementsThatShouldUpdate = this.elements.Where(x => x.ShouldUpdate).ToArray();
            this.elementsThatShouldDraw = this.elements.Where(x => x.IsVisible).ToArray();
        }

        public void Unload()
        {
            this.elements.ForEach(x => layoutPool.AddElement(x));
            this.elements.Clear();
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            for (int i = 0; i < this.elementsThatShouldUpdate.Length; i++)
            {
                this.elementsThatShouldUpdate[i].Update(gameTime);
            }
        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.elementsThatShouldDraw.Length; i++)
            {
                this.elementsThatShouldDraw[i].Draw(gameTime, spriteBatch);
            }
        }

        public T CreateElement<T>() where T : SGUIElement
        {
            T element = layoutPool.GetElement<T>(this.Game);
            this.elements.Add(element);

            return element;
        }
    }
}