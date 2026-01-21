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
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.WorldSystem;

using System;

namespace StardustSandbox.Core.Generators
{
    internal static class LightningGenerator
    {
        internal static void Start(ElementContext context, Point origin)
        {
            CreateBranchedThunder(context, origin);
        }

        private static void CreateBranchedThunder(ElementContext context, Point origin)
        {
            int length = Core.Random.Range(3, 6);

            CreateBranchedThunderBranch(context, origin, length, Core.Random.Range(-30, -10));
            CreateBranchedThunderBranch(context, origin, length, Core.Random.Range(10, 30));
        }

        private static void CreateBranchedThunderBranch(ElementContext context, Point origin, int length, int angle)
        {
            float rad = MathF.PI * angle / 180.0f;
            int dx = (int)MathF.Round((MathF.Sin(rad) * length) + Core.Random.Range(-3, 3));
            int dy = (int)MathF.Round((MathF.Cos(rad) * length) + Core.Random.Range(2, 4));

            Point endPoint = new(
                origin.X + dx,
                origin.Y + dy
            );

            if (!TryCreateBodyLine(context, origin, endPoint))
            {
                return;
            }

            CreateBranchedThunder(context, endPoint);
        }

        private static bool TryCreateBodyLine(ElementContext context, Point start, Point end)
        {
            bool skipFirst = true;

            foreach (Point position in ShapePointGenerator.EnumerateLinePoints(start, end))
            {
                if (skipFirst)
                {
                    skipFirst = false;
                    continue;
                }

                if (context.TryGetSlot(position, out Slot slot) && !slot.GetLayer(context.Layer).IsEmpty)
                {
                    if (slot.GetLayer(context.Layer).Element.Category is ElementCategory.Gas)
                    {
                        continue;
                    }

                    switch (slot.GetLayer(context.Layer).ElementIndex)
                    {
                        case ElementIndex.Water:
                        case ElementIndex.Ice:
                        case ElementIndex.Snow:
                            if (slot.HasState(context.Layer, ElementStates.IsFalling))
                            {
                                continue;
                            }

                            break;
                    }
                }

                if (!context.TryInstantiateElement(position, ElementIndex.LightningBody))
                {
                    return false;
                }
            }

            return true;
        }
    }
}

