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

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.Indexers;

namespace StardustSandbox.Core.Elements.Common.Liquids
{
    internal sealed class Paint : Liquid
    {
        private readonly Color dyeingColor;

        internal Paint(ElementIndex index, ElementCategory category, ElementRenderingProfile renderingProfile, GameEvents gameEvents) : base(index, category, renderingProfile, gameEvents)
        {
            this.dyeingColor = renderingProfile.ReferenceColor;
            this.InitialTemperature = 20.0f;
            this.BaseDensity = 1.2f;
            this.BaseExplosionResistance = 0.3f;

            this.HasNeighborInteractions = true;
            this.HasTemperature = true;
            this.IsCorruptible = true;
            this.IsPushable = true;
            this.IsFlammable = true;
        }

        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            for (int i = 0; i < ElementConstants.NEIGHBORS_ARRAY_LENGTH; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, context.CurrentLayer) ||
                    neighbors.GetSlotLayer(i, context.CurrentLayer).ElementIndex == this.Index)
                {
                    continue;
                }

                context.SetElementColorModifier(neighbors.GetNeighborPosition(i), this.dyeingColor);
            }
        }

        protected override void OnTemperatureChanged(ElementContext context, float currentValue)
        {
            if (currentValue >= 200.0f)
            {
                context.ReplaceElementIndex(ElementIndex.Fire);
            }
        }
    }
}
