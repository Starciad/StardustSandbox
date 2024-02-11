using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PixelDust.Game.Objects
{
    public abstract class PGameObject
    {
        protected PGame Game { get; private set; }

        public void Initialize(PGame game)
        {
            this.Game = game;

            OnAwake();
            OnStart();
        }
        public void Update(GameTime gameTime)
        {
            OnUpdate(gameTime);
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            OnDraw(gameTime, spriteBatch);
        }

        protected virtual void OnAwake() { return; }
        protected virtual void OnStart() { return; }
        protected virtual void OnUpdate(GameTime gameTime) { return; }
        protected virtual void OnDraw(GameTime gameTime, SpriteBatch spriteBatch) { return; }
    }
}
