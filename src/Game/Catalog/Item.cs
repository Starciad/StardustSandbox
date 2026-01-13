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

