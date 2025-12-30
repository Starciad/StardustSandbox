using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Actors;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Actors;
using StardustSandbox.Enums.Serialization;
using StardustSandbox.Enums.Simulation;
using StardustSandbox.Interfaces;
using StardustSandbox.Serialization;
using StardustSandbox.WorldSystem;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Managers
{
    internal sealed class ActorManager : IResettable
    {
        internal IEnumerable<Actor> InstantiatedActors => this.instantiatedActors;
        internal int TotalActorCount => this.instantiatedActors.Count + this.actorsToAdd.Count - this.actorsToRemove.Count;

        private string currentlySelectedSaveFile;

        private SimulationSpeed currentSpeed;
        private float accumulatedTimeSeconds;
        private float delayThresholdSeconds;

        private readonly List<Actor> instantiatedActors = [];
        private readonly Queue<Actor> actorsToAdd = [];
        private readonly Queue<Actor> actorsToRemove = [];

        private readonly World world;

        internal ActorManager(World world)
        {
            this.world = world;
        }

        internal Actor Create(ActorIndex index)
        {
            if (this.TotalActorCount >= ActorConstants.MAX_SIMULTANEOUS_ACTORS)
            {
                return null;
            }

            Actor actor = ActorDatabase.GetDescriptor(index).Create();
            this.actorsToAdd.Enqueue(actor);
            return actor;
        }

        internal void Destroy(Actor actor)
        {
            if (actor.Destroyed)
            {
                return;
            }

            ActorDatabase.GetDescriptor(actor.Index).Destroy(actor);
            this.actorsToRemove.Enqueue(actor);
        }

        internal void DestroyAll()
        {
            foreach (Actor actor in this.instantiatedActors)
            {
                Destroy(actor);
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

        private bool IsActorWithinWorldBounds(Actor actor)
        {
            Rectangle rect = actor.SelfRectangle;

            int worldWidth = this.world.Information.Size.X * SpriteConstants.SPRITE_SCALE;
            int worldHeight = this.world.Information.Size.Y * SpriteConstants.SPRITE_SCALE;

            if (rect.Right < -ActorConstants.WORLD_BOUNDS_TOLERANCE)
            {
                return false; // West
            }

            if (rect.Left > worldWidth + ActorConstants.WORLD_BOUNDS_TOLERANCE)
            {
                return false; // East
            }

            if (rect.Bottom < -ActorConstants.WORLD_BOUNDS_TOLERANCE)
            {
                return false; // North
            }

            if (rect.Top > worldHeight + ActorConstants.WORLD_BOUNDS_TOLERANCE)
            {
                return false; // South
            }

            return true;
        }

        private void FlushPendingChanges()
        {
            // Add queued actors
            while (this.actorsToAdd.TryDequeue(out Actor actor))
            {
                this.instantiatedActors.Add(actor);

                actor.OnCreated();
            }

            // Remove queued actors
            while (this.actorsToRemove.TryDequeue(out Actor actor))
            {
                _ = this.instantiatedActors.Remove(actor);

                actor.OnDestroyed();
            }
        }

        private void UpdateActors(GameTime gameTime)
        {
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

                // Destroy actor if it leaves world bounds
                if (!IsActorWithinWorldBounds(currentActor))
                {
                    Destroy(currentActor);
                }
            }
        }

        internal void Update(GameTime gameTime)
        {
            FlushPendingChanges();

            float deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            this.accumulatedTimeSeconds += deltaTime;

            if (this.accumulatedTimeSeconds >= this.delayThresholdSeconds)
            {
                this.accumulatedTimeSeconds = 0.0f;
                UpdateActors(gameTime);
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            foreach (Actor actor in this.actorsToAdd)
            {
                actor.Draw(spriteBatch);
            }

            foreach (Actor actor in this.instantiatedActors)
            {
                // Skip non-drawable and destroyed actors
                if (!actor.CanDraw || actor.Destroyed)
                {
                    continue;
                }

                // Draw the actor
                actor.Draw(spriteBatch);
            }
        }

        private IEnumerable<Actor> GetSerializableActors()
        {
            HashSet<Actor> removingActors = [.. this.actorsToRemove];

            foreach (Actor actor in this.instantiatedActors)
            {
                if (!removingActors.Contains(actor))
                {
                    yield return actor;
                }
            }

            foreach (Actor actor in this.actorsToAdd)
            {
                yield return actor;
            }
        }

        public byte[][] Serialize()
        {
            List<byte[]> result = [];

            foreach (Actor actor in GetSerializableActors())
            {
                result.Add(ActorSerializer.Serialize(actor));
            }

            return [.. result];
        }

        public void Deserialize(byte[][] data)
        {
            DestroyAll();

            this.actorsToAdd.Clear();
            this.actorsToRemove.Clear();

            for (int i = 0; i < data.Length; i++)
            {
                Actor actor = ActorSerializer.Deserialize(data[i]);
                this.actorsToAdd.Enqueue(actor);
            }
        }

        internal void LoadFromSaveFile(string name)
        {
            this.currentlySelectedSaveFile = name;
            Deserialize(SavingSerializer.Load(name, LoadFlags.Content).Content.Actors);
        }

        internal void Reload()
        {
            if (!string.IsNullOrEmpty(this.currentlySelectedSaveFile))
            {
                LoadFromSaveFile(this.currentlySelectedSaveFile);
            }
            else
            {
                DestroyAll();
            }
        }

        public void Reset()
        {
            this.currentlySelectedSaveFile = null;
            DestroyAll();
        }

        internal void SetSpeed(SimulationSpeed speed)
        {
            this.currentSpeed = speed;
            this.delayThresholdSeconds = speed switch
            {
                SimulationSpeed.Normal => SimulationConstants.NORMAL_SPEED_DELAY_SECONDS / 2.0f,
                SimulationSpeed.Fast => SimulationConstants.FAST_SPEED_DELAY_SECONDS / 2.0f,
                SimulationSpeed.VeryFast => SimulationConstants.VERY_FAST_SPEED_DELAY_SECONDS / 2.0f,
                _ => SimulationConstants.NORMAL_SPEED_DELAY_SECONDS / 2.0f,
            };
        }
    }
}
