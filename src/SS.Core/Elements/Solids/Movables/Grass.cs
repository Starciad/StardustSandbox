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
using StardustSandbox.Core.Randomness;

namespace StardustSandbox.Core.Elements.Solids.Movables
{
    internal sealed class Grass : MovableSolid
    {
        internal Grass(ElementIndex index, ElementCategory category, ElementCharacteristics characteristics, ElementRenderingType renderingType, Point textureOriginOffset, Color referenceColor, AchievementManager achievementManager) : base(index, category, characteristics, renderingType, textureOriginOffset, referenceColor, achievementManager)
        {
            this.BaseDensity = 0.1f;
            this.BaseExplosionResistance = 0.5f;
            this.BaseFlammabilityResistance = 10.0f;
            this.InitialTemperature = 22.0f;
        }

        protected override void OnTemperatureChanged(ElementContext context, float currentValue)
        {
            if (currentValue > 200.0f)
            {
                if (Random.Chance(85))
                {
                    context.ReplaceElementIndex(ElementIndex.Fire);
                }
                else
                {
                    context.ReplaceElementIndex(ElementIndex.Ash);
                }
            }
        }
    }
}
