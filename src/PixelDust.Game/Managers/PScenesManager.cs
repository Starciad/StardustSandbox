using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Objects;
using PixelDust.Game.Scenes;

using System;
using System.Collections.Generic;

namespace PixelDust.Game.Managers
{
    public sealed class PScenesManager : PGameObject
    {
        public bool IsEmpty => this._sceneInstance == null;

        private readonly List<Type> _sceneTypes = [];
        private PScene[] _scenes = [];

        private PScene _sceneInstance;

        protected override void OnAwake()
        {
            int length = this._sceneTypes.Count;
            this._scenes = new PScene[length];

            for (int i = 0; i < length; i++)
            {
                Type type = this._sceneTypes[i];

                PScene tempScene = (PScene)Activator.CreateInstance(type);
                tempScene.Initialize(this.Game);

                this._scenes[i] = tempScene;
            }
        }
        protected override void OnUpdate(GameTime gameTime)
        {
            if (this.IsEmpty)
            {
                return;
            }

            this._sceneInstance.Update(gameTime);
        }
        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (this.IsEmpty)
            {
                return;
            }

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            this._sceneInstance.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }

        public void AddScene<T>() where T : PScene
        {
            AddScene(typeof(T));
        }
        public void AddScene(Type type)
        {
            this._sceneTypes.Add(type);
        }

        public void Load(int index)
        {
            Unload();

            this._sceneInstance = this._scenes[index];
            this._sceneInstance.Load();
        }
        public void Unload()
        {
            if (!this.IsEmpty)
            {
                this._sceneInstance.Unload();
                this._sceneInstance = null;
            }
        }

        public PScene GetCurrentScene()
        {
            return this._sceneInstance;
        }
    }
}