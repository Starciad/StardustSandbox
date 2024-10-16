using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardustSandbox.Game.Objects
{
    public abstract class SGameObject
    {
        protected SGame SGameInstance { get; private set; }

        public SGameObject(SGame gameInstance)
        {
            this.SGameInstance = gameInstance;
        }

        public void Initialize()
        {
            OnInitialize();
        }

        public void Update(GameTime gameTime)
        {
            OnUpdate(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            OnDraw(gameTime, spriteBatch);
        }

        protected virtual void OnInitialize() { return; }
        protected virtual void OnUpdate(GameTime gameTime) { return; }
        protected virtual void OnDraw(GameTime gameTime, SpriteBatch spriteBatch) { return; }
    }
}
