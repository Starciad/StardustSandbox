/*
 * Copyright (C) 2026  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
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

using StardustSandbox.Backgrounds.Clouds;
using StardustSandbox.Camera;
using StardustSandbox.Collections;
using StardustSandbox.Constants;
using StardustSandbox.Enums.States;
using StardustSandbox.Extensions;
using StardustSandbox.Interfaces;
using StardustSandbox.Interfaces.Collections;
using StardustSandbox.Randomness;

using System.Collections.Generic;

namespace StardustSandbox.Scenario
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

        private readonly List<Cloud> activeClouds = new(BackgroundConstants.ACTIVE_CLOUDS_LIMIT);
        private readonly ObjectPool cloudPool = new();

        internal void Update(GameTime gameTime)
        {
            if (GameHandler.HasState(GameStates.IsSimulationPaused) ||
                GameHandler.HasState(GameStates.IsCriticalMenuOpen))
            {
                return;
            }

            for (int i = 0; i < this.activeClouds.Count; i++)
            {
                Cloud cloud = this.activeClouds[i];

                if (cloud == null)
                {
                    continue;
                }

                if (!SSCamera.InsideCameraBounds(cloud.Position, new(cloud.SourceRectangle.Width, cloud.SourceRectangle.Height), false, cloud.SourceRectangle.Width + (WorldConstants.GRID_SIZE * 2)))
                {
                    DestroyCloud(cloud);
                    continue;
                }

                cloud.Update(gameTime, GameHandler.SimulationSpeed);
            }

            if (SSRandom.Chance(BackgroundConstants.CHANCE_OF_CLOUD_SPAWNING, BackgroundConstants.CHANCE_OF_CLOUD_SPAWNING_TOTAL))
            {
                CreateCloud();
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.activeClouds.Count; i++)
            {
                Cloud cloud = this.activeClouds[i];

                if (cloud == null)
                {
                    continue;
                }

                cloud.Draw(spriteBatch);
            }
        }

        internal void Clear()
        {
            for (int i = 0; i < this.activeClouds.Count; i++)
            {
                Cloud cloud = this.activeClouds[i];

                if (cloud == null)
                {
                    continue;
                }

                DestroyCloud(cloud);
            }
        }

        public void Reset()
        {
            Clear();
        }

        private void CreateCloud()
        {
            if (this.activeClouds.Count > BackgroundConstants.ACTIVE_CLOUDS_LIMIT)
            {
                return;
            }

            Cloud target;
            Rectangle randomCloudRectangle = cloudRectangles.GetRandomItem();

            if (this.cloudPool.TryDequeue(out IPoolableObject value))
            {
                target = (Cloud)value;
                target.SetTextureRectangle(randomCloudRectangle);
                target.Reset();

                this.activeClouds.Add(target);
            }
            else
            {
                target = new();
                target.SetTextureRectangle(randomCloudRectangle);

                this.activeClouds.Add(target);
            }
        }

        private void DestroyCloud(Cloud cloud)
        {
            _ = this.activeClouds.Remove(cloud);
            this.cloudPool.Enqueue(cloud);
        }
    }
}

