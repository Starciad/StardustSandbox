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
using System.Diagnostics;

namespace StardustSandbox.Managers
{
    internal sealed class ActorManager : IResettable
    {
        internal IEnumerable<Actor> InstantiatedActors => this.instantiatedActors;
        internal int TotalActorCount => this.instantiatedActors.Count + this.actorsToAdd.Count - this.actorsToRemove.Count;

        private string currentlySelectedSaveFile;

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

        internal bool TryCreate(ActorIndex index, out Actor actor)
        {
            if (this.TotalActorCount >= ActorConstants.MAX_SIMULTANEOUS_ACTORS)
            {
                actor = null;
                return false;
            }

            actor = ActorDatabase.GetDescriptor(index).Create();
            this.actorsToAdd.Enqueue(actor);

            return true;
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

        private bool IsActorWithinWorldBounds(Actor actor)
        {
            int left = actor.Position.X;
            int right = actor.Position.X + actor.Size.X;
            int top = actor.Position.Y;
            int bottom = actor.Position.Y + actor.Size.Y;

            if (right < -ActorConstants.WORLD_BOUNDS_TOLERANCE)
            {
                return false; // West
            }

            if (left > this.world.Information.Size.X + ActorConstants.WORLD_BOUNDS_TOLERANCE)
            {
                return false; // East
            }

            if (bottom < -ActorConstants.WORLD_BOUNDS_TOLERANCE)
            {
                return false; // North
            }

            if (top > this.world.Information.Size.Y + ActorConstants.WORLD_BOUNDS_TOLERANCE)
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

            this.accumulatedTimeSeconds += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

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
            this.delayThresholdSeconds = speed switch
            {
                SimulationSpeed.Normal => SimulationConstants.NORMAL_SPEED_DELAY_SECONDS,
                SimulationSpeed.Fast => SimulationConstants.FAST_SPEED_DELAY_SECONDS,
                SimulationSpeed.VeryFast => SimulationConstants.VERY_FAST_SPEED_DELAY_SECONDS,
                _ => SimulationConstants.NORMAL_SPEED_DELAY_SECONDS,
            };
        }
    }
}
