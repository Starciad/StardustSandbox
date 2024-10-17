using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace StardustSandbox.Game.Items
{
    public sealed class SItemCategory
    {
        public string Identifier { get; private set; }
        public string DisplayName { get; private set; }
        public string Description { get; private set; }
        public Texture2D IconTexture { get; private set; }
        public SItem[] Items => [.. this.items];

        private readonly List<SItem> items = [];

        public SItemCategory(string identifier, string displayName, string description, Texture2D iconTexture)
        {
            this.Identifier = identifier;
            this.DisplayName = displayName;
            this.Description = description;
            this.IconTexture = iconTexture;
        }

        internal void AddItem(SItem item)
        {
            this.items.Add(item);
        }
    }
}
