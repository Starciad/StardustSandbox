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

using StardustSandbox.Core.Achievements;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.Achievements;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Randomness;

namespace StardustSandbox.Core.Elements.Liquids
{
    internal sealed class Saltwater : Liquid
    {
        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            for (int i = 0; i < ElementConstants.NEIGHBORS_ARRAY_LENGTH; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, context.Layer))
                {
                    continue;
                }

                switch (neighbors.GetSlotLayer(i, context.Layer).ElementIndex)
                {
                    case ElementIndex.FertileSoil:
                    case ElementIndex.Dirt:
                        context.ReplaceElement(neighbors.GetNeighborPosition(i), context.Layer, ElementIndex.Mud);
                        context.DestroyElement();
                        return;

                    case ElementIndex.Stone:
                        if (Random.Range(0, 150) == 0)
                        {
                            context.ReplaceElement(neighbors.GetNeighborPosition(i), context.Layer, ElementIndex.Sand);
                            context.DestroyElement();
                        }

                        return;

                    case ElementIndex.Fire:
                        context.DestroyElement(neighbors.GetNeighborPosition(i), context.Layer);
                        break;

                    default:
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(ElementContext context, in float currentValue)
        {
            if (currentValue <= 21.0f)
            {
                context.ReplaceElement(ElementIndex.Ice);
                context.SetStoredElement(ElementIndex.Saltwater);
                AchievementEngine.Unlock(AchievementIndex.ACH_021);
            }
            else if (currentValue >= 110.0f)
            {
                if (Random.GetBool())
                {
                    context.ReplaceElement(ElementIndex.Steam);
                    AchievementEngine.Unlock(AchievementIndex.ACH_005);
                }
                else
                {
                    context.ReplaceElement(ElementIndex.Saltwater);
                }
            }
        }
    }
}
