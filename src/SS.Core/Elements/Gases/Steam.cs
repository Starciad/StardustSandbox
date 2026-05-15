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
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.Randomness;

namespace StardustSandbox.Core.Elements.Gases
{
    internal sealed class Steam : Gas
    {
        internal Steam(ElementIndex index, ElementCategory category, ElementCharacteristics characteristics, ElementRenderingType renderingType, Point textureOriginOffset, Color referenceColor, AchievementManager achievementManager, StatisticsManager statisticsManager) : base(index, category, characteristics, renderingType, textureOriginOffset, referenceColor, achievementManager, statisticsManager)
        {
            this.InitialTemperature = 200.0f;
            this.BaseDensity = 0.0006f;
        }

        protected override void OnStep(ElementContext context)
        {
            if (Random.Chance(40))
            {
                context.UpdateElementPosition(new(context.CurrentSlot.Position.X, context.CurrentSlot.Position.Y - 1));
            }
            else
            {
                base.OnStep(context);
            }
        }

        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            if (context.CurrentPosition.Y <= PercentageMath.PercentageOfValue(context.GetWorldSize().Y, 15.0f) && Random.Chance(5))
            {
                context.ReplaceElementIndex(ElementIndex.Cloud);
            }
        }

        protected override void OnTemperatureChanged(ElementContext context, float currentValue)
        {
            if (currentValue < 35.0f)
            {
                context.ReplaceElementIndex(ElementIndex.Water);
            }
        }
    }
}
