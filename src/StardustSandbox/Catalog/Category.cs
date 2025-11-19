using Microsoft.Xna.Framework.Graphics;

namespace StardustSandbox.Catalog
{
    internal sealed class Category(string name, string description, Texture2D iconTexture, Subcategory[] subcategories)
    {
        internal string Name => name;
        internal string Description => description;
        internal Texture2D IconTexture => iconTexture;
        internal Subcategory[] Subcategories => this.subcategories;
        internal int SubcategoriesLength => this.subcategories.Length;

        private readonly Subcategory[] subcategories = subcategories;

        internal Subcategory GetSubcategory(byte index)
        {
            return this.subcategories[index];
        }
    }
}
