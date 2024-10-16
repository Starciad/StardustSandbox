using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Objects;

using System;

namespace StardustSandbox.Game.Items
{
    public abstract class SItem : SGameObject
    {
        public string Identifier { get; protected set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public string Category { get; protected set; }
        public Texture2D IconTexture { get; protected set; }
        public bool IsVisible { get; protected set; }
        public int UnlockProgress { get; protected set; }
        public Type ReferencedType { get; protected set; }

        protected SAssetDatabase AssetDatabase { get; private set; }

        public SItem(SGame gameInstance, SAssetDatabase assetDatabase) : base(gameInstance)
        {
            this.AssetDatabase = assetDatabase;
        }
    }
}
