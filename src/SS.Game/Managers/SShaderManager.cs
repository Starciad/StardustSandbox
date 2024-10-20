﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Objects;

namespace StardustSandbox.Game.Managers
{
    public sealed class SShaderManager : SGameObject
    {
        private Effect[] shaders;
        private int shadersLength;

        private readonly SAssetDatabase _assetDatabase;

        public SShaderManager(SGame gameInstance, SAssetDatabase assetDatabase) : base(gameInstance)
        {
            this._assetDatabase = assetDatabase;
        }

        public override void Initialize()
        {
            this.shaders = this._assetDatabase.Shaders;
            this.shadersLength = this.shaders.Length;
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < this.shadersLength; i++)
            {
                this.shaders[i].Parameters["Time"]?.SetValue((float)gameTime.TotalGameTime.TotalSeconds);
            }
        }
    }
}
