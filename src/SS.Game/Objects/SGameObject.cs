using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardustSandbox.Game.Objects
{
    public abstract class SGameObject(SGame gameInstance)
    {
        protected SGame SGameInstance { get; private set; } = gameInstance;

        public virtual void Initialize()
        {
            return;
        }

        public virtual void Update(GameTime gameTime)
        {
            return;
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            return;
        }
    }
}
