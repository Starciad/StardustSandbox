using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Databases;
using StardustSandbox.Enums.Actors;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Managers;
using StardustSandbox.Serialization.Saving.Data;
using StardustSandbox.WorldSystem;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Actors.Common
{
    internal sealed class GulActor : Actor
    {
        internal GulActor(ActorIndex index, ActorManager actorManager, World world) : base(index, actorManager, world)
        {

        }

        public override void Reset()
        {

        }

        internal override void Initialize()
        {

        }

        internal override void Update(GameTime gameTime)
        {
            float deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            MoveVertically(150.0f * deltaTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.ActorGul), new(this.positionX, this.positionY), null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
        }

        internal override ActorData Serialize()
        {
            return new()
            {
                Index = Index,
                Content = new Dictionary<string, object>()
                {

                },
            };
        }

        internal override void Deserialize(ActorData data)
        {

        }
    }
}
