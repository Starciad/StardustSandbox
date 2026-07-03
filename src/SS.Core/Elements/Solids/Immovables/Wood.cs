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
using StardustSandbox.Core.Randomness;

namespace StardustSandbox.Core.Elements.Solids.Immovables
{
    internal sealed class Wood : ImmovableSolid
    {
        internal Wood(ElementIndex index, ElementCategory category, ElementRenderingType renderingType, Point textureOriginOffset, Color referenceColor, GameEvents gameEvents) : base(index, category, renderingType, textureOriginOffset, referenceColor, gameEvents)
        {
            this.InitialTemperature = 20.0f;
            this.BaseFlammabilityResistance = 35.0f;
            this.BaseDensity = 0.7f;
            this.BaseExplosionResistance = 1.5f;

            this.HasTemperature = true;
            this.IsFlammable = true;
            this.IsCorruptible = true;
            this.IsPushable = true;
        }

        protected override void OnTemperatureChanged(ElementContext context, float currentValue)
        {
            if (currentValue >= 300.0f)
            {
                if (Random.Chance(65))
                {
                    context.Replace(ElementIndex.Fire);
                }
                else
                {
                    context.Replace(ElementIndex.Ash);
                }
            }
        }
    }
}
