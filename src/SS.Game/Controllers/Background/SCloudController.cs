﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Background.Details;
using StardustSandbox.Game.Collections;
using StardustSandbox.Game.Constants;
using StardustSandbox.Game.Interfaces.General;
using StardustSandbox.Game.Mathematics;
using StardustSandbox.Game.Mathematics.Primitives;
using StardustSandbox.Game.Objects;

using System.Collections.Generic;

namespace StardustSandbox.Game.Controllers.Background
{
    internal sealed class SCloudController(SGame gameInstance) : SGameObject(gameInstance)
    {
        private readonly List<SCloud> activeClouds = new(SBackgroundConstants.ACTIVE_CLOUDS_LIMIT);
        private readonly SObjectPool cloudPool = new();

        public override void Update(GameTime gameTime)
        {
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
            for (int i = 0; i < this.activeClouds.Count; i++)
            {
                this.activeClouds[i].Draw(gameTime, spriteBatch);
            }
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
            return this.SGameInstance.AssetDatabase.GetTexture($"bgo_cloud_{SRandomMath.Range(1, SAssetConstants.GRAPHICS_BGOS_CLOUDS_LENGTH + 1)}");
        }
    }
}
