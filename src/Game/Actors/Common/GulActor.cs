using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Actors.Collision;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Actors;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Managers;
using StardustSandbox.Randomness;
using StardustSandbox.Serialization.Saving.Data;
using StardustSandbox.WorldSystem;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Actors.Common
{
    internal sealed class GulActor : Actor
    {
        private Vector2 direction;
        private float speed;

        internal GulActor(ActorIndex index, ActorManager actorManager, World world) : base(index, actorManager, world)
        {

        }

        public override void Reset()
        {

        }

        internal override bool OnElementCollisionOccurred(in ElementCollisionContext context)
        {
            switch (context.Orientation)
            {
                case ElementCollisionOrientation.None:
                    return false;

                case ElementCollisionOrientation.Horizontally:
                    this.direction.X *= -1.0f;
                    return true;

                case ElementCollisionOrientation.Vertically:
                    this.direction.Y *= -1.0f;
                    return true;

                default:
                    return false;
            }
        }

        internal override void OnCreated()
        {
            this.speed = SSRandom.Range(40, 80);
            this.direction = new(SSRandom.Range(-1.0f, 1.0f), SSRandom.Range(-1.0f, 1.0f));
        }

        internal override void Update(GameTime gameTime)
        {
            float deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            MoveHorizontally(this.direction.X * this.speed * deltaTime);
            MoveVertically(this.direction.Y * this.speed * deltaTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.ActorGul), new(this.PositionX, this.PositionY), new(0, 0, 32, 32), Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
        }

        internal override ActorData Serialize()
        {
            return new()
            {
                Index = this.Index,
                Content = new Dictionary<string, object>()
                {
                    ["PositionX"] = this.PositionX,
                    ["PositionY"] = this.PositionY,
                },
            };
        }

        internal override void Deserialize(ActorData data)
        {
            if (data.Content.TryGetValue("PositionX", out object positionXObj) && positionXObj is int positionX)
            {
                this.PositionX = positionX;
            }

            if (data.Content.TryGetValue("PositionY", out object positionYObj) && positionYObj is int positionY)
            {
                this.PositionY = positionY;
            }
        }
    }
}
