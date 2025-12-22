using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Items;

using System;

namespace StardustSandbox.Catalog
{
    internal sealed class Item(int contentIndex, string name, string description, ItemContentType contentType, TextureIndex textureIndex, Rectangle? sourceRectangle)
    {
        internal int ContentIndex => contentIndex;
        internal string Name => name;
        internal string Description => description;
        internal ItemContentType ContentType => contentType;
        internal Texture2D Texture => AssetDatabase.GetTexture(textureIndex);
        internal Rectangle? SourceRectangle => sourceRectangle;
        internal Subcategory ParentSubcategory { get; private set; }

        internal void SetParentSubcategory(Subcategory subcategory)
        {
            if (this.ParentSubcategory != null)
            {
                throw new InvalidOperationException("Parent subcategory has already been set for this item.");
            }

            this.ParentSubcategory = subcategory;
        }
    }
}
