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

using StardustSandbox.Core.Achievements;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.Elements;

namespace StardustSandbox.Core.Elements.Liquids
{
    internal sealed class Lava : Liquid
    {
        internal Lava(ElementIndex index, ElementCategory category, ElementCharacteristics characteristics, ElementRenderingType renderingType, Point textureOriginOffset, Color referenceColor, AchievementSystem achievementSystem) : base(index, category, characteristics, renderingType, textureOriginOffset, referenceColor, achievementSystem)
        {

        }

        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            for (int i = 0; i < ElementConstants.NEIGHBORS_ARRAY_LENGTH; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, context.CurrentLayer))
                {
                    continue;
                }

                switch (neighbors.GetSlotLayer(i, context.CurrentLayer).ElementIndex)
                {
                    case ElementIndex.Oil:
                    case ElementIndex.Wood:
                    case ElementIndex.TreeLeaf:
                    case ElementIndex.DrySponge:
                    case ElementIndex.Grass:
                    case ElementIndex.DryBlackWool:
                    case ElementIndex.DryWhiteWool:
                    case ElementIndex.DryRedWool:
                    case ElementIndex.DryOrangeWool:
                    case ElementIndex.DryYellowWool:
                    case ElementIndex.DryGreenWool:
                    case ElementIndex.DryGrayWool:
                    case ElementIndex.DryBlueWool:
                    case ElementIndex.DryVioletWool:
                    case ElementIndex.DryBrownWool:
                    case ElementIndex.LiquefiedPetroleumGas:
                    case ElementIndex.BlackPaint:
                    case ElementIndex.WhitePaint:
                    case ElementIndex.RedPaint:
                    case ElementIndex.OrangePaint:
                    case ElementIndex.YellowPaint:
                    case ElementIndex.GreenPaint:
                    case ElementIndex.CyanPaint:
                    case ElementIndex.GrayPaint:
                    case ElementIndex.VioletPaint:
                    case ElementIndex.BrownPaint:
                    case ElementIndex.Moss:
                    case ElementIndex.Seed:
                    case ElementIndex.Sapling:
                        context.ReplaceElementIndex(neighbors.GetNeighborPosition(i), context.CurrentLayer, ElementIndex.Fire);
                        break;

                    default:
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(ElementContext context, float currentValue)
        {
            if (currentValue <= 500.0f)
            {
                if (context.GetStoredElementIndex() is ElementIndex.None)
                {
                    context.ReplaceElementIndex(ElementIndex.Stone);
                }
                else
                {
                    context.ReplaceElementIndex(context.GetStoredElementIndex());
                }

                context.SetElementTemperature(500.0f);
            }
        }
    }
}
