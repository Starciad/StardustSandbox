using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;

namespace StardustSandbox.Catalog
{
    internal sealed class Category(string name, string description, TextureIndex textureIndex, Rectangle? sourceRectangle, Subcategory[] subcategories)
    {
        internal string Name => name;
        internal string Description => description;
        internal Texture2D Texture => AssetDatabase.GetTexture(textureIndex);
        internal Rectangle? SourceRectangle => sourceRectangle;
        internal Subcategory[] Subcategories => this.subcategories;
        internal int SubcategoriesLength => this.subcategories.Length;

        private readonly Subcategory[] subcategories = subcategories;

        internal Subcategory GetSubcategory(byte index)
        {
            return this.subcategories[index];
        }
    }
}
