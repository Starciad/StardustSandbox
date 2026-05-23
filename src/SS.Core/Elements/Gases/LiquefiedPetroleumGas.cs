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

namespace StardustSandbox.Core.Elements.Gases
{
    internal sealed class LiquefiedPetroleumGas : Gas
    {
        internal LiquefiedPetroleumGas(ElementIndex index, ElementCategory category, ElementRenderingType renderingType, Point textureOriginOffset, Color referenceColor, GameEvents gameEvents) : base(index, category, renderingType, textureOriginOffset, referenceColor, gameEvents)
        {
            this.InitialTemperature = -42.0f;
            this.BaseFlammabilityResistance = 1.0f;
            this.BaseDensity = 0.25f;
            this.BaseExplosionResistance = 0.2f;

            this.HasNeighborInteractions = true;
            this.HasTemperature = true;
            this.IsFlammable = true;
            this.IsCorruptible = true;
            this.IsPushable = true;
        }

        protected override void OnTemperatureChanged(ElementContext context, float currentValue)
        {
            if (currentValue >= 400.0f)
            {
                context.ReplaceElementIndex(ElementIndex.Fire);
            }
        }
    }
}
