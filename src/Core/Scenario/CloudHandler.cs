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

using StardustSandbox.Core.Backgrounds.Clouds;
using StardustSandbox.Core.Cameras;
using StardustSandbox.Core.Collections;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.States;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Collections;
using StardustSandbox.Core.Randomness;

using System.Collections.Generic;

namespace StardustSandbox.Core.Scenario
{
    internal sealed class CloudHandler : IResettable
    {
        private static readonly Rectangle[] cloudRectangles = [
            new(0, 0, 160, 64),
            new(160, 0, 96, 32),
            new(256, 0, 160, 96),
            new(0, 64, 128, 64),
            new(128, 64, 128, 64),
        ];

        private int totalCloudCount;

        private readonly List<Cloud> instantiatedClouds = [];
        private readonly Queue<Cloud> cloudsToAdd = [];
        private readonly Queue<Cloud> cloudsToRemove = [];

        private readonly ObjectPool cloudPool = new();

        internal IEnumerable<Cloud> GetClouds()
        {
            foreach (Cloud cloud in this.cloudsToAdd)
            {
                yield return cloud;
            }

            foreach (Cloud cloud in this.instantiatedClouds)
            {
                yield return cloud;
            }

            foreach (Cloud cloud in this.cloudsToRemove)
            {
                yield return cloud;
            }
        }

        private bool TryCreate()
        {
            if (this.totalCloudCount >= BackgroundConstants.MAX_SIMULTANEOUS_CLOUDS)
            {
                return false;
            }

            Cloud cloud = this.cloudPool.TryDequeue(out IPoolableObject value) ? (Cloud)value : new();
            cloud.SourceRectangle = cloudRectangles.GetRandomItem();

            this.cloudsToAdd.Enqueue(cloud);
            this.totalCloudCount++;

            return true;
        }

        private void Destroy(Cloud cloud)
        {
            if (cloud.IsDestroyed)
            {
                return;
            }

            cloud.IsDestroyed = true;

            this.cloudPool.Enqueue(cloud);
            this.cloudsToRemove.Enqueue(cloud);

            this.totalCloudCount--;
        }

        private void FlushPendingChanges()
        {
            FlushAdditions();
            FlushRemovals();
        }

        private void FlushAdditions()
        {
            while (this.cloudsToAdd.TryDequeue(out Cloud cloud))
            {
                this.instantiatedClouds.Add(cloud);
                cloud.IsDestroyed = false;
            }
        }

        private void FlushRemovals()
        {
            while (this.cloudsToRemove.TryDequeue(out Cloud cloud))
            {
                _ = this.instantiatedClouds.Remove(cloud);
            }
        }

        internal void Update(GameTime gameTime)
        {
            if (GameHandler.HasState(GameStates.IsSimulationPaused) ||
                GameHandler.HasState(GameStates.IsCriticalMenuOpen))
            {
                return;
            }

            FlushPendingChanges();

            foreach (Cloud cloud in GetClouds())
            {
                if (cloud.IsDestroyed)
                {
                    continue;
                }

                cloud.Update(gameTime, GameHandler.SimulationSpeed);

                if (!Camera.IsWithinBounds(cloud.SelfRectangle, false, cloud.SourceRectangle.Width * 2.0f))
                {
                    Destroy(cloud);
                }
            }

            if (Random.Chance(1, 350))
            {
                _ = TryCreate();
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.instantiatedClouds.Count; i++)
            {
                Cloud cloud = this.instantiatedClouds[i];

                if (cloud == null)
                {
                    continue;
                }

                cloud.Draw(spriteBatch);
            }
        }

        public void Reset()
        {
            HashSet<Cloud> visited = [];

            foreach (Cloud cloud in GetClouds())
            {
                if (!visited.Add(cloud))
                {
                    continue;
                }

                if (!cloud.IsDestroyed)
                {
                    this.cloudPool.Enqueue(cloud);
                }
            }

            this.cloudsToAdd.Clear();
            this.cloudsToRemove.Clear();
            this.instantiatedClouds.Clear();

            this.totalCloudCount = 0;
        }
    }
}

