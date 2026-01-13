/*
 * Copyright (C) 2026  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
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

