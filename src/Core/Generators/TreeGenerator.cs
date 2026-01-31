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

using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Enums.Elements;

namespace StardustSandbox.Core.Generators
{
    internal static class TreeGenerator
    {
        internal static void Start(ElementContext context, in int height, in int trunkWidth, in int leavesRadius)
        {
            // Generate trunk
            for (int y = 0; y < height; y++)
            {
                for (int x = -trunkWidth / 2; x <= trunkWidth / 2; x++)
                {
                    Point position = new(context.Position.X + x, context.Position.Y - y);

                    if (context.IsEmptySlotLayer(position, context.Layer))
                    {
                        context.InstantiateElement(position, context.Layer, ElementIndex.Wood);
                    }
                    else
                    {
                        context.ReplaceElement(position, context.Layer, ElementIndex.Wood);
                    }
                }
            }

            // Generate leaves
            int leavesStartY = context.Position.Y - height;

            for (int y = -leavesRadius; y <= leavesRadius; y++)
            {
                for (int x = -leavesRadius; x <= leavesRadius; x++)
                {
                    if ((x * x) + (y * y) <= leavesRadius * leavesRadius)
                    {
                        Point position = new(context.Position.X + x, leavesStartY + y);

                        if (context.IsEmptySlotLayer(position, context.Layer))
                        {
                            context.InstantiateElement(position, context.Layer, ElementIndex.TreeLeaf);
                        }
                        else
                        {
                            context.ReplaceElement(position, context.Layer, ElementIndex.TreeLeaf);
                        }
                    }
                }
            }
        }
    }
}
