using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Actors.Collision;
using StardustSandbox.Enums.Actors;
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
        internal Rectangle SelfRectangle => new(this.positionX, this.positionY, this.width, this.height);

        internal bool CanUpdate { get => this.canUpdate; set => this.canUpdate = value; }
        internal bool CanDraw { get => this.canDraw; set => this.canDraw = value; }

        internal bool CollideWithActors { get => this.collideWithActors; set => this.collideWithActors = value; }
        internal bool CollideWithElements { get => this.collideWithElements; set => this.collideWithElements = value; }

        internal ActorIndex Index => this.index;

        internal int PositionX
        {
            get => this.positionX;
            set => this.positionX = value;
        }

        internal int PositionY
        {
            get => this.positionY;
            set => this.positionY = value;
        }

        internal int Width
        {
            get => this.width;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Width cannot be negative.");
                }

                this.width = value;
            }
        }

        internal int Height
        {
            get => this.height;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Height cannot be negative.");
                }

                this.height = value;
            }
        }

        internal bool IsSolid
        {
            get => this.isSolid;
            set => this.isSolid = value;
        }

        protected readonly ActorIndex index;
        protected int positionX;
        protected int positionY;
        protected int width;
        protected int height;

        protected bool canUpdate = true;
        protected bool canDraw = true;

        protected bool isSolid = false;

        protected bool collideWithActors = true;
        protected bool collideWithSolids = true;
        protected bool collideWithElements = true;

        private double remainderX;
        private double remainderY;

        protected readonly ActorManager actorManager;
        protected readonly World world;

        internal Actor(ActorIndex index, ActorManager actorManager, World world)
        {
            this.index = index;
            this.actorManager = actorManager;
            this.world = world;
        }

        public abstract void Reset();
        internal abstract void Initialize();
        internal abstract void Update(GameTime gameTime);
        internal abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        internal abstract ActorData Serialize();
        internal abstract void Deserialize(ActorData data);

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
                nextRect.X = this.positionX + sign;
                nextRect.Y = this.positionY;

                if (this.collideWithElements && this.world.IsCollideAt(nextRect, Layer.Foreground))
                {
                    OnTerrainCollisionOccurred(new(TerrainCollisionOrientation.Horizontally));
                    stopFlag = true;
                }

                if (stopFlag)
                {
                    break;
                }

                this.positionX += sign;
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
                nextRect.Y = this.positionY + sign;
                nextRect.X = this.positionX;

                if (this.collideWithElements && this.world.IsCollideAt(nextRect, Layer.Foreground))
                {
                    OnTerrainCollisionOccurred(new(TerrainCollisionOrientation.Vertically));
                    stopFlag = true;
                }

                if (stopFlag)
                {
                    break;
                }

                this.positionY += sign;
                motion -= sign;
            }
        }

        internal virtual void OnCreated() { return; }
        internal virtual void OnDestroyed() { return; }
        internal virtual void OnActorCollisionOccurred(Actor collider, in ActorCollisionContext context) { return; }
        internal virtual void OnTerrainCollisionOccurred(in TerrainCollisionContext context) { return; }
    }
}
