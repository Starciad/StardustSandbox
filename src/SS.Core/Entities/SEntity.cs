using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Components;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Collections;
using StardustSandbox.Core.Objects;

namespace StardustSandbox.Core.Entities
{
    public abstract class SEntity(ISGame gameInstance) : SGameObject(gameInstance), ISPoolableObject
    {
        public SComponentContainer ComponentContainer => this.componentContainer;

        private readonly SComponentContainer componentContainer = new(gameInstance);

        public override void Initialize()
        {
            this.componentContainer.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            this.componentContainer.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.componentContainer.Draw(gameTime, spriteBatch);
        }

        public virtual void Destroy()
        {
            return;
        }

        public virtual void Reset()
        {
            this.componentContainer.Reset();
        }
    }
}