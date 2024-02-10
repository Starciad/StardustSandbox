using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Objects;
using PixelDust.Game.Scenes;

using System;

namespace PixelDust.Game.Managers
{
    public sealed class PScenesManager : PGameObject
    {
        public bool IsEmpty => this._sceneInstance == null;

        private PScene _sceneInstance;

        public void Load<T>() where T : PScene
        {
            if (!IsEmpty)
            {
                _sceneInstance.Unload();
            }

            _sceneInstance = Activator.CreateInstance<T>();
            _sceneInstance.Initialize(this.Game);
        }
        public PScene GetCurrentScene()
        {
            return _sceneInstance;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (IsEmpty)
            {
                return;
            }

            _sceneInstance.Update(gameTime);
        }
        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsEmpty)
            {
                return;
            }

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            _sceneInstance.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }
}