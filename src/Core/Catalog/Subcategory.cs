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
    internal sealed class Subcategory(string name, string description, TextureIndex textureIndex, Rectangle? sourceRectangle, params Item[] items)
    {
        internal string Name => name;
        internal string Description => description;
        internal Texture2D Texture => AssetDatabase.GetTexture(textureIndex);
        internal Rectangle? SourceRectangle => sourceRectangle;
        internal int Length => this.items.Length;
        internal Category Parent { get; private set; }

        private readonly Item[] items = items;

        internal Item this[int i]
        {
            get => this.items[i];
        }

        internal void SetParentCategory(Category category)
        {
            if (this.Parent != null)
            {
                throw new InvalidOperationException("Parent category has already been set.");
            }

            this.Parent = category;
        }
    }
}
