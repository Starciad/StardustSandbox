using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace StardustSandbox.Core.Items
{
    public sealed class SCategory(string identifier, string displayName, string description, Texture2D iconTexture)
    {
        public string Identifier => identifier;
        public string DisplayName => displayName;
        public string Description => description;
        public Texture2D IconTexture => iconTexture;
        public SItem[] Items => [.. this.items];

        private readonly List<SItem> items = [];

        internal void AddItem(SItem item)
        {
            this.items.Add(item);
        }
    }
}
