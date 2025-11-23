using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace StardustSandbox.Catalog
{
    internal sealed class Subcategory(string name, string description, Texture2D iconTexture, Rectangle iconTextureRectangle, Item[] items)
    {
        internal string Name => name;
        internal string Description => description;
        internal Texture2D IconTexture => iconTexture;
        internal Rectangle IconTextureRectangle => iconTextureRectangle;
        internal Item[] Items => this.items;
        internal int ItemLength => this.items.Length;
        internal Category ParentCategory { get; private set; }

        private readonly Item[] items = items;

        internal Item GetItem(byte index)
        {
            return this.items[index];
        }

        internal Item[] GetItems(int startIndex, int endIndex)
        {
            int length = endIndex - startIndex;

            Item[] result = new Item[length];
            Array.Copy(this.items, startIndex, result, 0, length);

            return result;
        }

        internal void SetParentCategory(Category category)
        {
            if (this.ParentCategory != null)
            {
                throw new InvalidOperationException("Parent category has already been set.");
            }

            this.ParentCategory = category;
        }
    }
}
