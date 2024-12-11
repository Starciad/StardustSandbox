using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Backgrounds.Details;
using StardustSandbox.Core.Collections;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.Mathematics.Primitives;
using StardustSandbox.Core.Objects;

using System.Collections.Generic;

namespace StardustSandbox.Core.Controllers.Background
{
    internal sealed class SCloudController(ISGame gameInstance) : SGameObject(gameInstance)
    {
        private bool isActive;

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
            if (!this.isActive)
            {
                return;
            }

            for (int i = 0; i < this.activeClouds.Count; i++)
            {
                SCloud cloud = this.activeClouds[i];

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
            if (!this.isActive)
            {
                return;
            }

            for (int i = 0; i < this.activeClouds.Count; i++)
            {
                this.activeClouds[i].Draw(gameTime, spriteBatch);
            }
        }

        public void Clear()
        {
            foreach (SCloud cloud in this.activeClouds.ToArray())
            {
                DestroyCloud(cloud);
            }
        }

        public void Enable()
        {
            this.isActive = true;
        }

        public void Disable()
        {
            this.isActive = false;
        }

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
