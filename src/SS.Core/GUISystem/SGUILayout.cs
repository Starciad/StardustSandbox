using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.GUI;
using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Mathematics.Primitives;
using StardustSandbox.Core.Objects;

using System.Collections.Generic;

namespace StardustSandbox.Core.GUISystem
{
    public sealed class SGUILayout(ISGame gameInstance) : SGameObject(gameInstance), ISGUILayoutBuilder
    {
        public SGUIRootElement RootElement => this.rootElement;
        public int ElementCount => this.elements.Count;

        private readonly List<SGUIElement> elements = [];

        private SGUIRootElement rootElement = null;

        public override void Initialize()
        {
            this.rootElement = new(this.SGameInstance);
            this.rootElement.SetPositioningType(SPositioningType.Fixed);
            this.rootElement.SetSize(new SSize2(SScreenConstants.DEFAULT_SCREEN_WIDTH, SScreenConstants.DEFAULT_SCREEN_HEIGHT));

            AddElement(this.rootElement);
        }

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