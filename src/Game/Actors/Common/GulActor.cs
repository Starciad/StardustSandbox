using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Actors;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.World;
using StardustSandbox.Managers;
using StardustSandbox.Serialization.Saving.Data;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Actors.Common
{
    internal sealed class GulActor : Actor
    {
        private bool isGrounded;

        internal GulActor(ActorIndex index, ActorManager actorManager, World world) : base(index, actorManager, world)
        {

        }

        public override void Reset()
        {

        }

        internal override void OnCreated()
        {

        }

        private void UpdateGroundedState()
        {
            this.isGrounded = !this.world.IsEmptySlotLayer(new(this.Position.X, this.Position.Y + 1), Layer.Foreground);
        }

        private void UpdateGravity()
        {
            if (!this.isGrounded)
            {
                this.Position = new(this.Position.X, this.Position.Y + 1);
            }
        }

        internal override void Update(GameTime gameTime)
        {
            UpdateGroundedState();
            UpdateGravity();
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.ActorGul), new(this.Position.X * SpriteConstants.SPRITE_SCALE, this.Position.Y * SpriteConstants.SPRITE_SCALE), new(0, 0, 32, 32), Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
        }

        internal override ActorData Serialize()
        {
            return new()
            {
                Index = this.Index,
                Content = new Dictionary<string, object>()
                {
                    ["PositionX"] = this.Position.X,
                    ["PositionY"] = this.Position.Y,
                },
            };
        }

        internal override void Deserialize(ActorData data)
        {
            if (data.Content.TryGetValue("PositionX", out object positionXObj) && positionXObj is int positionX)
            {
                this.Position = new(positionX, this.Position.Y);
            }

            if (data.Content.TryGetValue("PositionY", out object positionYObj) && positionYObj is int positionY)
            {
                this.Position = new(this.Position.X, positionY);
            }
        }
    }
}
