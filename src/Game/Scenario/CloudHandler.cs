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
using StardustSandbox.Managers;
using StardustSandbox.Randomness;
using StardustSandbox.WorldSystem.Status;

using System.Collections.Generic;

namespace StardustSandbox.Scenario
{
    internal sealed class CloudHandler(GameManager gameManager, Simulation simulation) : IResettable
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

        private readonly GameManager gameManager = gameManager;
        private readonly Simulation simulation = simulation;

        internal void Update(GameTime gameTime)
        {
            if (this.gameManager.HasState(GameStates.IsSimulationPaused) ||
                this.gameManager.HasState(GameStates.IsCriticalMenuOpen))
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

                if (!SSCamera.InsideCameraBounds(cloud.Position, new Point(cloud.SourceRectangle.Width, cloud.SourceRectangle.Height), false, cloud.SourceRectangle.Width + (WorldConstants.GRID_SIZE * 2)))
                {
                    DestroyCloud(cloud);
                    continue;
                }

                cloud.Update(gameTime, this.simulation.CurrentSpeed);
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
