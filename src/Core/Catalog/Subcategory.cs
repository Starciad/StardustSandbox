/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.Assets;

using System;

namespace StardustSandbox.Core.Catalog
{
    internal sealed class Subcategory(string name, string description, TextureIndex textureIndex, Rectangle? sourceRectangle, Item[] items)
    {
        internal string Name => name;
        internal string Description => description;
        internal Texture2D Texture => AssetDatabase.GetTexture(textureIndex);
        internal Rectangle? SourceRectangle => sourceRectangle;
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

