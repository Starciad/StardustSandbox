using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Background.Details;
using StardustSandbox.Game.Mathematics;
using StardustSandbox.Game.Objects;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Game.Controllers.Background
{
    internal sealed class SCloudController : SGameObject
    {
        private readonly List<SCloud> _clouds = [];
        private readonly Texture2D[] _cloudTextures;

        public SCloudController(SGame gameInstance, Texture2D[] cloudTextures) : base(gameInstance)
        {
            this._cloudTextures = cloudTextures;
            SpawnClouds();
        }

        private void SpawnClouds()
        {
            for (int i = 0; i < 5; i++)
            {
                AddCloud();
            }
        }

        private void AddCloud()
        {
            Texture2D cloudTexture = _cloudTextures[Random.Shared.Next(_cloudTextures.Length)];
            SCloud newCloud = new(this.SGameInstance, cloudTexture, SRandomMath.Range(10, 50), (float)Random.Shared.NextDouble());
            _clouds.Add(newCloud);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (SCloud cloud in _clouds)
            {
                cloud.Update(gameTime);
            }

            if (SRandomMath.Range(0, 500) == 1)
            {
                AddCloud();
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (SCloud cloud in _clouds)
            {
                cloud.Draw(gameTime, spriteBatch);
            }
        }
    }
}
