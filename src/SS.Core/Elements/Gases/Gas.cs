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

using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.Indexers;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.WorldSystem.Slots;

using System.Collections.Generic;

namespace StardustSandbox.Core.Elements.Gases
{
    internal abstract class Gas : Element
    {
        private static readonly List<Point> availablePositions = [];

        internal Gas(ElementIndex index, ElementCategory category, ElementRenderingType renderingType, Point textureOriginOffset, Color referenceColor, GameEvents gameEvents) : base(index, category, renderingType, textureOriginOffset, referenceColor, gameEvents)
        {

        }

        private void EvaluateNeighboringPosition(ElementContext context, Point position)
        {
            if (!context.HasElement(position))
            {
                availablePositions.Add(position);
                return;
            }

            // If the neighboring position is not empty, check if the element in that position
            // is a gas or liquid and if it has a lower temperature or density than the current
            // gas element. If so, add that position to the available positions list.

            if ((context.TryGetSlot(position, out Slot value) &&
                value.GetElement(context.Layer).Category is ElementCategory.Gas or ElementCategory.Liquid &&
                value.GetElementIndex(context.Layer) == this.Index &&
                value.GetTemperature(context.Layer) > context.GetTemperature()) ||
                this.BaseDensity > value.GetElement(context.Layer).BaseDensity)
            {
                availablePositions.Add(position);
            }
        }

        protected override void OnStep(ElementContext context)
        {
            availablePositions.Clear();

            int centerX = context.Slot.Position.X;
            int centerY = context.Slot.Position.Y;

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0)
                    {
                        continue;
                    }

                    EvaluateNeighboringPosition(context, new(centerX + dx, centerY + dy));
                }
            }

            if (availablePositions.Count == 0)
            {
                return;
            }

            Point targetPosition = availablePositions.GetRandomItem();

            if (!context.HasElement(targetPosition))
            {
                context.SetPosition(targetPosition);
                return;
            }

            context.Swap(targetPosition);
        }
    }
}
