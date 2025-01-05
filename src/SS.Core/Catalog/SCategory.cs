using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace StardustSandbox.Core.Catalog
{
    public sealed class SCategory(string identifier, string displayName, string description, Texture2D iconTexture)
    {
        public string Identifier => identifier;
        public string DisplayName => displayName;
        public string Description => description;
        public Texture2D IconTexture => iconTexture;
        public IEnumerable<SSubcategory> Subcategories => this.subcategories.Values;
        public int SubcategoriesCount => this.subcategories.Count;

        private readonly Dictionary<string, SSubcategory> subcategories = [];

        public void AddSubcategory(SSubcategory subcategory)
        {
            this.subcategories.Add(subcategory.Identifier, subcategory);
        }

        public SSubcategory GetSubcategory(string identifier)
        {
            return this.subcategories[identifier];
        }
    }
}
