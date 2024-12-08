using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace StardustSandbox.Core.Items
{
    public sealed class SItemCategory(string identifier, string displayName, string description, Texture2D iconTexture)
    {
        public string Identifier { get; private set; } = identifier;
        public string DisplayName { get; private set; } = displayName;
        public string Description { get; private set; } = description;
        public Texture2D IconTexture { get; private set; } = iconTexture;
        public SItem[] Items => [.. this.items];

        private readonly List<SItem> items = [];

        internal void AddItem(SItem item)
        {
            this.items.Add(item);
        }
    }
}
