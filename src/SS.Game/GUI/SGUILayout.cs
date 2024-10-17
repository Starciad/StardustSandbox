﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Constants;
using StardustSandbox.Game.Enums.GUI;
using StardustSandbox.Game.GameContent.GUI.Elements;
using StardustSandbox.Game.GUI.Elements;
using StardustSandbox.Game.Interfaces.GUI;
using StardustSandbox.Game.Mathematics;
using StardustSandbox.Game.Objects;

using System.Collections.Generic;

namespace StardustSandbox.Game.GUI
{
    public sealed class SGUILayout(SGame gameInstance) : SGameObject(gameInstance), ISGUILayoutBuilder
    {
        public SGUIRootElement RootElement => this.rootElement;
        public int ElementCount => this.elements.Count;

        private readonly List<SGUIElement> elements = [];

        private SGUIRootElement rootElement = null;

        public void Load()
        {
            this.rootElement = new(this.SGameInstance);
            this.rootElement.SetPositioningType(SPositioningType.Fixed);
            this.rootElement.SetSize(new SSize2(SScreenConstants.DEFAULT_SCREEN_WIDTH, SScreenConstants.DEFAULT_SCREEN_HEIGHT));

            AddElement(this.rootElement);
        }

        public void Unload()
        {
            this.elements.Clear();
        }

        protected override void OnUpdate(GameTime gameTime)
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

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
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