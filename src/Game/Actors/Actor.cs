using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Actors.Collision;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Actors;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.World;
using StardustSandbox.Interfaces.Collections;
using StardustSandbox.Managers;
using StardustSandbox.Serialization.Saving.Data;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox.Actors
{
    internal abstract class Actor : IPoolableObject
    {
        internal ActorIndex Index { get; }
        internal Rectangle SelfRectangle => new(this.PositionX, this.PositionY, this.Width, this.Height);

        internal bool CanUpdate { get; set; }
        internal bool CanDraw { get; set; }
        internal bool Destroyed { get; set; }

        internal bool CollideWithActors { get; set; }
        internal bool CollideWithElements { get; set; }

        internal int PositionX { get; set; }
        internal int PositionY { get; set; }

        internal int Width { get; set; }
        internal int Height { get; set; }

        internal bool IsSolid { get; set; }

        private double remainderX;
        private double remainderY;

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

        internal abstract ActorData Serialize();
        internal abstract void Deserialize(ActorData data);

        private static bool IsCollidingWithElements(Rectangle rectangle, World world)
        {
            if (rectangle.IsEmpty)
            {
                return false;
            }

            int startX = (int)Math.Floor(rectangle.Left / (float)SpriteConstants.SPRITE_SCALE);
            int endX = (int)Math.Floor((rectangle.Right - 1) / (float)SpriteConstants.SPRITE_SCALE);
            int startY = (int)Math.Floor(rectangle.Top / (float)SpriteConstants.SPRITE_SCALE);
            int endY = (int)Math.Floor((rectangle.Bottom - 1) / (float)SpriteConstants.SPRITE_SCALE);

            if (endX < 0 ||
                endY < 0 ||
                startX > world.Information.Size.X - 1 ||
                startY > world.Information.Size.Y - 1)
            {
                return false;
            }

            startX = Math.Max(0, startX);
            startY = Math.Max(0, startY);
            endX = Convert.ToInt32(Math.Min(world.Information.Size.X - 1, endX));
            endY = Convert.ToInt32(Math.Min(world.Information.Size.Y - 1, endY));

            for (int y = startY; y <= endY; y++)
            {
                for (int x = startX; x <= endX; x++)
                {
                    if (!world.TryGetElement(new(x, y), Layer.Foreground, out ElementIndex index))
                    {
                        continue;
                    }

                    if (ElementDatabase.GetElement(index).Category is ElementCategory.MovableSolid or ElementCategory.ImmovableSolid)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        internal void MoveHorizontally(double amount)
        {
            this.remainderX += amount;
            int motion = (int)Math.Round(this.remainderX);

            if (motion == 0)
            {
                return;
            }

            this.remainderX -= motion;
            sbyte sign = (sbyte)Math.Sign(motion);

            Rectangle nextRect = this.SelfRectangle;
            bool stopFlag = false;

            while (motion != 0)
            {
                nextRect.X = this.PositionX + sign;
                nextRect.Y = this.PositionY;

                if (this.CollideWithElements && IsCollidingWithElements(nextRect, this.world))
                {
                    OnElementCollisionOccurred(new(ElementCollisionOrientation.Horizontally));
                    stopFlag = true;
                }

                if (stopFlag)
                {
                    break;
                }

                this.PositionX += sign;
                motion -= sign;
            }
        }

        internal void MoveVertically(double amount)
        {
            this.remainderY += amount;
            int motion = (int)Math.Round(this.remainderY);

            if (motion == 0)
            {
                return;
            }

            this.remainderY -= motion;
            sbyte sign = (sbyte)Math.Sign(motion);

            Rectangle nextRect = this.SelfRectangle;
            bool stopFlag = false;

            while (motion != 0)
            {
                nextRect.Y = this.PositionY + sign;
                nextRect.X = this.PositionX;

                if (this.CollideWithElements && IsCollidingWithElements(nextRect, this.world))
                {
                    OnElementCollisionOccurred(new(ElementCollisionOrientation.Vertically));
                    stopFlag = true;
                }

                if (stopFlag)
                {
                    break;
                }

                this.PositionY += sign;
                motion -= sign;
            }
        }

        internal virtual void OnCreated() { return; }
        internal virtual void OnDestroyed() { return; }
        internal virtual void OnActorCollisionOccurred(Actor collider, in ActorCollisionContext context) { return; }
        internal virtual void OnElementCollisionOccurred(in ElementCollisionContext context) { return; }
    }
}
