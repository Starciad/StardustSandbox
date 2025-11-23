using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Enums.Items;

using System;

namespace StardustSandbox.Catalog
{
    internal sealed class Item(Type associatedType, string name, string description, ItemContentType contentType, Texture2D iconTexture, Rectangle iconTextureRectangle)
    {
        internal Type AssociatedType => associatedType;
        internal string Name => name;
        internal string Description => description;
        internal ItemContentType ContentType => contentType;
        internal Texture2D IconTexture => iconTexture;
        internal Rectangle IconTextureRectangle => iconTextureRectangle;
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
