using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Actors;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Actors;
using StardustSandbox.Serialization;

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

        private static bool Intersects(Rectangle rect1, Rectangle rect2, out ActorCollisionDirection collisionDirection, out Point intersectionPoint)
        {
            collisionDirection = ActorCollisionDirection.None;
            intersectionPoint = Point.Zero;

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
                intersectionPoint.X = intersectLeft + (intersectWidth / 2);
                intersectionPoint.Y = intersectTop + (intersectHeight / 2);

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

                actor.OnCreated();
            }

            // Remove queued actors
            while (this.actorsToRemove.TryDequeue(out Actor actor))
            {
                this.instantiatedActorCount--;
                _ = this.instantiatedActors.Remove(actor);

                ActorDatabase.GetDescriptor(actor.Index).Recycle(actor);

                actor.OnDestroyed();
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

                // Check for collisions with other actors
                if (currentActor.CollideWithActors)
                {
                    foreach (Actor otherActor in this.instantiatedActors)
                    {
                        if (currentActor == otherActor || !otherActor.CollideWithActors)
                        {
                            continue;
                        }

                        if (Intersects(currentActor.SelfRectangle, otherActor.SelfRectangle, out ActorCollisionDirection collisionDirection, out Point intersectionPoint))
                        {
                            currentActor.OnActorCollisionOccurred(otherActor, new(collisionDirection, intersectionPoint));
                        }
                    }
                }

                // Update the actor
                currentActor.Update(gameTime);
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            foreach (Actor actor in this.instantiatedActors)
            {
                // Skip non-drawable actors
                if (!actor.CanDraw)
                {
                    continue;
                }

                // Draw the actor
                actor.Draw(spriteBatch);
            }
        }

        public byte[][] SerializeAll()
        {
            byte[][] result = new byte[this.instantiatedActorCount][];

            for (int i = 0; i < this.instantiatedActorCount; i++)
            {
                result[i] = ActorSerializer.Serialize(this.instantiatedActors[i]);
            }

            return result;
        }

        public void DeserializeAll(byte[][] data)
        {
            DestroyAll();

            for (int i = 0; i < data.Length; i++)
            {
                this.actorsToAdd.Enqueue(ActorSerializer.Deserialize(data[i]));
            }
        }
    }
}
