using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.Core.Objects
{
    public abstract class SGameObject(ISGame gameInstance)
    {
        protected ISGame SGameInstance { get; private set; } = gameInstance;

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
