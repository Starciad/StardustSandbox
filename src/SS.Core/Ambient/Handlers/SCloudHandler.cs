using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Ambient.Clouds;
using StardustSandbox.Core.Collections;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Ambient.Handlers;
using StardustSandbox.Core.Interfaces.Collections;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.Mathematics.Primitives;
using StardustSandbox.Core.Objects;

using System.Collections.Generic;

namespace StardustSandbox.Core.Ambient.Handlers
{
    internal sealed class SCloudHandler(ISGame gameInstance) : SGameObject(gameInstance), ISCloudHandler
    {
        public bool IsActive { get; set; } = true;

        private readonly Texture2D[] cloudTextures = new Texture2D[SAssetConstants.GRAPHICS_BGOS_CLOUDS_LENGTH];
        private readonly List<SCloud> activeClouds = new(SBackgroundConstants.ACTIVE_CLOUDS_LIMIT);
        private readonly SObjectPool cloudPool = new();

        public override void Initialize()
        {
            for (int i = 0; i < SAssetConstants.GRAPHICS_BGOS_CLOUDS_LENGTH; i++)
            {
                this.cloudTextures[i] = this.SGameInstance.AssetDatabase.GetTexture($"bgo_cloud_{i + 1}");
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!this.IsActive || this.SGameInstance.GameManager.GameState.IsSimulationPaused || this.SGameInstance.GameManager.GameState.IsCriticalMenuOpen)
            {
                return;
            }

            for (int i = 0; i < this.activeClouds.Count; i++)
            {
                SCloud cloud = this.activeClouds[i];

                if (cloud == null)
                {
                    continue;
                }

                if (!this.SGameInstance.CameraManager.InsideCameraBounds(cloud.Position, new SSize2(cloud.Texture.Width, cloud.Texture.Height), false, cloud.Texture.Width + (SWorldConstants.GRID_SCALE * 2)))
                {
                    DestroyCloud(cloud);
                    continue;
                }

                cloud.Update(gameTime);
            }

            if (SRandomMath.Chance(SBackgroundConstants.CHANCE_OF_CLOUD_SPAWNING, SBackgroundConstants.CHANCE_OF_CLOUD_SPAWNING_TOTAL))
            {
                CreateCloud();
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!this.IsActive)
            {
                return;
            }

            for (int i = 0; i < this.activeClouds.Count; i++)
            {
                SCloud cloud = this.activeClouds[i];

                if (cloud == null)
                {
                    continue;
                }

                cloud.Draw(gameTime, spriteBatch);
            }
        }

        public void Clear()
        {
            for (int i = 0; i < this.activeClouds.Count; i++)
            {
                SCloud cloud = this.activeClouds[i];

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

        // ============================================ //

        private void CreateCloud()
        {
            if (this.activeClouds.Count > SBackgroundConstants.ACTIVE_CLOUDS_LIMIT)
            {
                return;
            }

            SCloud target;
            Texture2D randomBGOCloudTexture = GetRandomBGOCloudTexture();

            if (this.cloudPool.TryGet(out ISPoolableObject value))
            {
                target = (SCloud)value;
                target.SetTexture(randomBGOCloudTexture);
                target.Initialize();

                this.activeClouds.Add(target);
            }
            else
            {
                target = new(this.SGameInstance);
                target.SetTexture(randomBGOCloudTexture);
                target.Initialize();

                this.activeClouds.Add(target);
            }
        }

        private void DestroyCloud(SCloud cloud)
        {
            _ = this.activeClouds.Remove(cloud);
            this.cloudPool.Add(cloud);
        }

        private Texture2D GetRandomBGOCloudTexture()
        {
            return this.cloudTextures.GetRandomItem();
        }
    }
}
