/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Actors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.Actors;
using StardustSandbox.Core.Enums.Serialization;
using StardustSandbox.Core.Enums.Simulation;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.Serialization.Saving.Data;
using StardustSandbox.Core.WorldSystem;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.Managers
{
    internal sealed class ActorManager : IResettable
    {
        internal int TotalActorCount => this.totalActorCount;

        internal bool CanUpdate { get; set; }
        internal bool CanDraw { get; set; }

        private int totalActorCount;

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

        internal IEnumerable<Actor> GetActors()
        {
            foreach (Actor actor in this.actorsToAdd)
            {
                yield return actor;
            }

            foreach (Actor actor in this.instantiatedActors)
            {
                yield return actor;
            }

            foreach (Actor actor in this.actorsToRemove)
            {
                yield return actor;
            }
        }

        internal bool TryCreate(ActorIndex index, out Actor actor)
        {
            if (this.totalActorCount >= ActorConstants.MAX_SIMULTANEOUS_ACTORS)
            {
                actor = null;
                return false;
            }

            actor = ActorDatabase.GetDescriptor(index).Dequeue();
            this.actorsToAdd.Enqueue(actor);
            this.totalActorCount++;

            return true;
        }

        internal void Destroy(Actor actor)
        {
            if (actor.State is ActorState.Destroyed)
            {
                return;
            }

            ActorDatabase.GetDescriptor(actor.Index).Enqueue(actor);
            this.actorsToRemove.Enqueue(actor);
            this.totalActorCount--;
        }

        private void FlushPendingChanges()
        {
            FlushAdditions();
            FlushRemovals();
        }

        private void FlushAdditions()
        {
            while (this.actorsToAdd.TryDequeue(out Actor actor))
            {
                this.instantiatedActors.Add(actor);
                actor.State = ActorState.Active;
                actor.OnCreated();
            }
        }

        private void FlushRemovals()
        {
            while (this.actorsToRemove.TryDequeue(out Actor actor))
            {
                _ = this.instantiatedActors.Remove(actor);
                actor.OnDestroyed();
            }
        }

        private void Clear()
        {
            HashSet<Actor> visited = [];

            foreach (Actor actor in GetActors())
            {
                if (!visited.Add(actor))
                {
                    continue;
                }

                if (actor.State is not ActorState.Destroyed)
                {
                    ActorDatabase.GetDescriptor(actor.Index).Enqueue(actor);
                    actor.OnDestroyed();
                }
            }

            this.actorsToAdd.Clear();
            this.actorsToRemove.Clear();
            this.instantiatedActors.Clear();

            this.totalActorCount = 0;
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

        internal bool HasEntityAtPosition(Point position)
        {
            Rectangle queryRect = new(position, new(1, 1));

            foreach (Actor actor in this.instantiatedActors)
            {
                if (actor.State is ActorState.Destroyed)
                {
                    continue;
                }

                Rectangle actorRect = new(actor.Position, actor.Size);

                if (actorRect.Intersects(queryRect))
                {
                    return true;
                }
            }

            return false;
        }

        private void UpdateActors(GameTime gameTime)
        {
            // Update each instantiated actor
            foreach (Actor currentActor in GetActors())
            {
                // Skip non-updatable actors
                if (!currentActor.CanUpdate || currentActor.State is not ActorState.Active)
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

            if (!this.CanUpdate)
            {
                return;
            }

            this.accumulatedTimeSeconds += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            if (this.accumulatedTimeSeconds >= this.delayThresholdSeconds)
            {
                this.accumulatedTimeSeconds = 0.0f;
                UpdateActors(gameTime);
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            if (!this.CanDraw)
            {
                return;
            }

            foreach (Actor actor in GetActors())
            {
                // Skip non-drawable and destroyed actors
                if (!actor.CanDraw || actor.State is ActorState.Destroyed)
                {
                    continue;
                }

                // Draw the actor
                actor.Draw(spriteBatch);
            }
        }

        public ActorData[] Serialize()
        {
            List<ActorData> datas = [];

            foreach (Actor actor in GetActors())
            {
                if (actor.State is ActorState.Destroyed)
                {
                    continue;
                }

                datas.Add(actor.Serialize());
            }

            return [.. datas];
        }

        public void Deserialize(ActorData[] datas)
        {
            if (datas == null)
            {
                return;
            }

            Clear();

            for (int i = 0; i < datas.Length; i++)
            {
                Actor actor = ActorDatabase.GetDescriptor(datas[i].Index).Dequeue();

                this.actorsToAdd.Enqueue(actor);
                this.totalActorCount++;
            }
        }

        internal void LoadFromSaveFile(string name)
        {
            Deserialize(SavingSerializer.Load(name, LoadFlags.Content).Content.Actors);
        }

        internal void Reload()
        {
            if (GameHandler.HasSaveFileLoaded)
            {
                LoadFromSaveFile(GameHandler.LoadedSaveFileName);
                return;
            }

            Clear();
        }

        public void Reset()
        {
            GameStatistics.ResetActorsStatistics();
            Clear();
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

