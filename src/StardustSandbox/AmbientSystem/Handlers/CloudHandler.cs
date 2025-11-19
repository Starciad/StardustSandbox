using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.AmbientSystem.Clouds;
using StardustSandbox.Collections;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.States;
using StardustSandbox.Extensions;
using StardustSandbox.Interfaces;
using StardustSandbox.Interfaces.Collections;
using StardustSandbox.Managers;
using StardustSandbox.Randomness;

using System.Collections.Generic;

namespace StardustSandbox.AmbientSystem.Handlers
{
    internal sealed class CloudHandler : IResettable
    {
        internal bool IsActive { get; set; } = true;

        private readonly Texture2D[] cloudTextures = new Texture2D[AssetConstants.TEXTURES_BGOS_CLOUDS_LENGTH];
        private readonly List<Cloud> activeClouds = new(BackgroundConstants.ACTIVE_CLOUDS_LIMIT);
        private readonly ObjectPool cloudPool = new();

        private readonly CameraManager cameraManager;
        private readonly GameManager gameManager;

        public CloudHandler(CameraManager cameraManager, GameManager gameManager)
        {
            this.cameraManager = cameraManager;
            this.gameManager = gameManager;

            for (byte i = 0; i < AssetConstants.TEXTURES_BGOS_CLOUDS_LENGTH; i++)
            {
                this.cloudTextures[i] = AssetDatabase.GetTexture($"texture_bgo_cloud_{i + 1}");
            }
        }

        internal void Update(GameTime gameTime)
        {
            if (!this.IsActive ||
                this.gameManager.HasState(GameStates.IsSimulationPaused) ||
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

                if (!this.cameraManager.InsideCameraBounds(cloud.Position, new Point(cloud.Texture.Width, cloud.Texture.Height), false, cloud.Texture.Width + (WorldConstants.GRID_SIZE * 2)))
                {
                    DestroyCloud(cloud);
                    continue;
                }

                cloud.Update(gameTime);
            }

            if (SSRandom.Chance(BackgroundConstants.CHANCE_OF_CLOUD_SPAWNING, BackgroundConstants.CHANCE_OF_CLOUD_SPAWNING_TOTAL))
            {
                CreateCloud();
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            if (!this.IsActive)
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
            Texture2D randomBGOCloudTexture = GetRandomBGOCloudTexture();

            if (this.cloudPool.TryDequeue(out IPoolableObject value))
            {
                target = (Cloud)value;
                target.SetTexture(randomBGOCloudTexture);
                target.Reset();

                this.activeClouds.Add(target);
            }
            else
            {
                target = new();
                target.SetTexture(randomBGOCloudTexture);

                this.activeClouds.Add(target);
            }
        }

        private void DestroyCloud(Cloud cloud)
        {
            _ = this.activeClouds.Remove(cloud);
            this.cloudPool.Enqueue(cloud);
        }

        private Texture2D GetRandomBGOCloudTexture()
        {
            return this.cloudTextures.GetRandomItem();
        }
    }
}
