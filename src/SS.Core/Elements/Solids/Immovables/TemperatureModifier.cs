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

namespace StardustSandbox.Core.Elements.Solids.Immovables
{
    internal sealed class TemperatureModifier : ImmovableSolid
    {
        private readonly TemperatureModifierMode temperatureModifierMode;

        internal TemperatureModifier(ElementIndex index, ElementCategory category, TemperatureModifierMode temperatureModifierMode, ElementRenderingType renderingType, Point textureOriginOffset, Color referenceColor, GameEvents gameEvents) : base(index, category, renderingType, textureOriginOffset, referenceColor, gameEvents)
        {
            this.temperatureModifierMode = temperatureModifierMode;

            this.InitialTemperature = 0.0f;
            this.BaseDensity = 1.5f;
            this.BaseExplosionResistance = 2.5f;

            this.HasNeighborInteractions = true;
            this.HasTemperature = true;
            this.IsCorruptible = true;
            this.IsPushable = true;
        }

        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            for (int i = 0; i < ElementConstants.NEIGHBORS_ARRAY_LENGTH; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, context.Layer) ||
                    !neighbors.GetSlotLayer(i, context.Layer).Element.HasTemperature)
                {
                    continue;
                }

                float result = neighbors.GetSlotLayer(i, context.Layer).Temperature;

                switch (this.temperatureModifierMode)
                {
                    case TemperatureModifierMode.Warming:
                        result += ToolConstants.DEFAULT_HEAT_VALUE;
                        break;

                    case TemperatureModifierMode.Cooling:
                        result += ToolConstants.DEFAULT_FREEZE_VALUE;
                        break;

                    default:
                        break;
                }

                context.SetElementTemperature(neighbors.GetNeighborPosition(i), context.Layer, result);
            }
        }
    }
}
