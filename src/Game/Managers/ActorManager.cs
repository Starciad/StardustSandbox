using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Actors;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Actors;
using StardustSandbox.Interfaces.Actors;
using StardustSandbox.WorldSystem;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Managers
{
    internal sealed class ActorManager
    {
        internal IEnumerable<Actor> InstantiatedActors => this.instantiatedActors;
        internal int InstantiatedActorCount => this.instantiatedActorCount;

        private int instantiatedActorCount;

        private readonly List<Actor> instantiatedActors = [];
        private readonly Queue<Actor> actorsToAdd = [];
        private readonly Queue<Actor> actorsToRemove = [];

        private readonly World world;

        internal ActorManager(World world)
        {
            this.world = world;
        }

        internal void Create(ActorIndex index)
        {
            this.actorsToAdd.Enqueue(ActorDatabase.GetDescriptor(index).Create());
        }

        internal void Destroy(Actor actor)
        {
            this.actorsToRemove.Enqueue(actor);
        }

        internal void DestroyAll()
        {
            foreach (Actor actor in this.instantiatedActors)
            {
                this.actorsToRemove.Enqueue(actor);
            }
        }

        internal bool IsCollideAt(Rectangle collisionRect)
        {
            foreach (Actor actor in this.instantiatedActors)
            {
                if (collisionRect.Intersects(actor.SelfRectangle))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool Intersects(Rectangle rect1, Rectangle rect2, out ActorCollisionDirection collisionDirection, out int xIntersectionPosition, out int yIntersectionPosition)
        {
            collisionDirection = ActorCollisionDirection.None;
            xIntersectionPosition = 0;
            yIntersectionPosition = 0;

            // Check for intersection
            if (rect1.Intersects(rect2))
            {
                // Calculates intersection areas
                int intersectLeft = Math.Max(rect1.Left, rect2.Left);
                int intersectRight = Math.Min(rect1.Right, rect2.Right);
                int intersectTop = Math.Max(rect1.Top, rect2.Top);
                int intersectBottom = Math.Min(rect1.Bottom, rect2.Bottom);

                int intersectWidth = intersectRight - intersectLeft;
                int intersectHeight = intersectBottom - intersectTop;

                // Central position of the intersection
                xIntersectionPosition = intersectLeft + (intersectWidth / 2);
                yIntersectionPosition = intersectTop + (intersectHeight / 2);

                // Calculates the distances to each side
                int fromTop = Math.Abs(rect1.Bottom - rect2.Top);
                int fromBottom = Math.Abs(rect1.Top - rect2.Bottom);
                int fromLeft = Math.Abs(rect1.Right - rect2.Left);
                int fromRight = Math.Abs(rect1.Left - rect2.Right);

                // Determines the direction of collision (least penetration)
                int minDist = Math.Min(Math.Min(fromTop, fromBottom), Math.Min(fromLeft, fromRight));
                collisionDirection = minDist == fromTop
                    ? ActorCollisionDirection.Bottom
                    : minDist == fromBottom ? ActorCollisionDirection.Top : minDist == fromLeft ? ActorCollisionDirection.Right : ActorCollisionDirection.Left;

                return true;
            }

            return false;
        }

        private void FlushPendingChanges()
        {
            // Add queued actors
            while (this.actorsToAdd.TryDequeue(out Actor actor))
            {
                this.instantiatedActorCount++;
                this.instantiatedActors.Add(actor);
            }

            // Remove queued actors
            while (this.actorsToRemove.TryDequeue(out Actor actor))
            {
                this.instantiatedActorCount--;
                _ = this.instantiatedActors.Remove(actor);

                ActorDatabase.GetDescriptor(actor.Index).Recycle(actor);
            }
        }

        internal void Update(GameTime gameTime)
        {
            FlushPendingChanges();

            // Update each instantiated actor
            foreach (Actor currentActor in this.instantiatedActors)
            {
                // Skip non-updatable actors
                if (!currentActor.CanUpdate)
                {
                    continue;
                }

                // Check if the actor is off-screen and trigger the appropriate event
                if (!this.world.IsActive)
                {
                    if (!this.world.IsWithinHorizontalBounds(currentActor.SelfRectangle, out WorldExitDirection direction, out int distance))
                    {
                        currentActor.OnExitedWorldBounds(direction, distance);
                    }

                    if (!this.world.IsWithinVerticalBounds(currentActor.SelfRectangle, out direction, out distance))
                    {
                        currentActor.OnExitedWorldBounds(direction, distance);
                    }
                }

                // Check for collisions with other actors
                if (currentActor.CollideWithActors)
                {
                    foreach (Actor otherActor in this.instantiatedActors)
                    {
                        if (currentActor == otherActor || !otherActor.CollideWithActors)
                        {
                            continue;
                        }

                        if (Intersects(currentActor.SelfRectangle, otherActor.SelfRectangle, out ActorCollisionDirection collisionDirection, out int xIntersectionPosition, out int yIntersectionPosition))
                        {
                            currentActor.OnActorCollisionOccurred(otherActor, new ActorCollisionContext(collisionDirection, xIntersectionPosition, yIntersectionPosition));
                        }
                    }
                }

                // Update the actor
                currentActor.Update(gameTime);
            }
        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Actor actor in this.instantiatedActors)
            {
                // Skip non-drawable actors
                if (!actor.CanDraw)
                {
                    continue;
                }

                // Draw the actor
                actor.Draw(gameTime, spriteBatch);
            }
        }

        internal byte[][] Serialize()
        {
            byte[][] data = new byte[this.instantiatedActorCount][];

            for (int i = 0; i < this.instantiatedActorCount; i++)
            {
                Actor actor = this.instantiatedActors[i];

                IActorDescriptor descriptor = ActorDatabase.GetDescriptor(actor.Index);
                
                byte[] actorData = descriptor.Serialize(actor);
                byte[] buffer = new byte[4 + actorData.Length];

                BitConverter.GetBytes((int)actor.Index).CopyTo(buffer, 0);
                actorData.CopyTo(buffer, 4);

                data[i] = buffer;
            }

            return data;
        }

        internal void Deserialize(byte[][] data)
        {
            DestroyAll();

            foreach (byte[] entry in data)
            {
                ActorIndex index = (ActorIndex)BitConverter.ToInt32(entry, 0);

                byte[] payload = new byte[entry.Length - 4];
                Buffer.BlockCopy(entry, 4, payload, 0, payload.Length);

                IActorDescriptor descriptor = ActorDatabase.GetDescriptor(index);
                Actor actor = descriptor.Deserialize(payload);

                this.actorsToAdd.Enqueue(actor);
            }
        }
    }
}
