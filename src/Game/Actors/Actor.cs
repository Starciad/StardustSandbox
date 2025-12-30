using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Enums.Actors;
using StardustSandbox.Interfaces.Collections;
using StardustSandbox.Managers;
using StardustSandbox.Serialization.Saving.Data;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.Actors
{
    internal abstract class Actor : IPoolableObject
    {
        internal ActorIndex Index { get; }

        internal Point Position { get; set; }
        internal Point Size { get; set; }

        internal bool CanUpdate { get; set; }
        internal bool CanDraw { get; set; }
        internal bool Destroyed { get; set; }

        protected readonly ActorManager actorManager;
        protected readonly World world;

        internal Actor(ActorIndex index, ActorManager actorManager, World world)
        {
            this.Index = index;
            this.actorManager = actorManager;
            this.world = world;
        }

        public abstract void Reset();
        internal abstract void Update(GameTime gameTime);
        internal abstract void Draw(SpriteBatch spriteBatch);

        internal virtual void OnCreated() { return; }
        internal virtual void OnDestroyed() { return; }

        internal abstract ActorData Serialize();
        internal abstract void Deserialize(ActorData data);
    }
}
