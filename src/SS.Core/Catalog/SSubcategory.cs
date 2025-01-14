using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace StardustSandbox.Core.Catalog
{
    public sealed class SSubcategory(SCategory parent, string identifier, string name, string description, Texture2D iconTexture)
    {
        public SCategory Parent => parent;
        public string Identifier => identifier;
        public string Name => name;
        public string Description => description;
        public Texture2D IconTexture => iconTexture;
        public IEnumerable<SItem> Items => this.items.Values;

        private readonly Dictionary<string, SItem> items = [];

        public void AddItem(SItem value)
        {
            this.items.Add(value.Identifier, value);
        }
    }
}
