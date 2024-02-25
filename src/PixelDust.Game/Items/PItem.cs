using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Databases;
using PixelDust.Game.Objects;

using System;

namespace PixelDust.Game.Items
{
    public abstract class PItem : PGameObject
    {
        public string Identifier { get; protected set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public string Category { get; protected set; }
        public Texture2D IconTexture { get; protected set; }
        public bool IsHidden { get; protected set; }
        public int LevelRequired { get; protected set; }
        public Type ReferencedType { get; protected set; }

        protected PAssetDatabase AssetDatabase { get; private set; }

        internal void Build(PAssetDatabase assetDatabase)
        {
            this.AssetDatabase = assetDatabase;

            OnBuild();
        }

        protected abstract void OnBuild();
    }
}
