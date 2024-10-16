using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Constants;
using StardustSandbox.Game.Enums.GUI;
using StardustSandbox.Game.GameContent.GUI.Elements;
using StardustSandbox.Game.GUI.Elements;
using StardustSandbox.Game.Interfaces.GUI;
using StardustSandbox.Game.Mathematics;
using StardustSandbox.Game.Objects;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace StardustSandbox.Game.GUI
{
    public sealed class SGUILayout : SGameObject, ISGUILayoutBuilder
    {
        public SGUIRootElement RootElement => this.rootElement;
        public int ElementCount => this.elements.Count;

        private readonly List<SGUIElement> elements = [];

        private SGUIElement[] elementsThatShouldUpdate;
        private SGUIElement[] elementsThatShouldDraw;

        private SGUIRootElement rootElement = null;

        public SGUILayout(SGame gameInstance) : base(gameInstance)
        {

        }

        public void Load()
        {
            this.rootElement = new(this.SGameInstance);
            this.rootElement.SetPositioningType(SPositioningType.Fixed);
            this.rootElement.SetSize(new SSize2(SScreenConstants.DEFAULT_SCREEN_WIDTH, SScreenConstants.DEFAULT_SCREEN_HEIGHT));

            AddElement(this.rootElement);
        }

        public void Configure()
        {
            this.elementsThatShouldUpdate = this.elements.Where(x => x.ShouldUpdate).ToArray();
            this.elementsThatShouldDraw = this.elements.Where(x => x.IsVisible).ToArray();
        }

        public void Unload()
        {
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

        public void AddElement<T>(T value) where T : SGUIElement
        {
            this.elements.Add(value);
        }
    }
}