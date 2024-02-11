using PixelDust.Game.Objects;

using System.Collections.Generic;

using System;
using PixelDust.Game.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PixelDust.Game.Managers
{
    public sealed class PGUIManager : PGameObject
    {
        private readonly List<Type> _guiTypes = [];
        private PGUISystem[] _guiSystems = [];

        protected override void OnAwake()
        {
            this._guiSystems = new PGUISystem[this._guiTypes.Count];

            for (int i = 0; i < this._guiSystems.Length; i++)
            {
                Type type = this._guiTypes[i];

                PGUISystem tempGUISystem = (PGUISystem)Activator.CreateInstance(type);
                tempGUISystem.Initialize(this.Game);

                this._guiSystems[i] = tempGUISystem;
            }
        }
        protected override void OnUpdate(GameTime gameTime)
        {
            for (int i = 0; i < this._guiSystems.Length; i++)
            {
                this._guiSystems[i].Update(gameTime);
            }
        }
        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this._guiSystems.Length; i++)
            {
                this._guiSystems[i].Draw(gameTime, spriteBatch);
            }
        }

        public void RegisterGUISystem<T>() where T : PGUISystem
        {
            RegisterGUISystem(typeof(T));
        }
        public void RegisterGUISystem(Type type)
        {
            this._guiTypes.Add(type);
        }
    }
}
