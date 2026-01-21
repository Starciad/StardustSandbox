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

using StardustSandbox.Core.Enums.Elements;

namespace StardustSandbox.Core.Elements.Liquids
{
    internal sealed class Lava : Liquid
    {
        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, context.Layer))
                {
                    continue;
                }

                switch (neighbors.GetSlotLayer(i, context.Layer).ElementIndex)
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
                    case ElementIndex.BluePaint:
                    case ElementIndex.GrayPaint:
                    case ElementIndex.VioletPaint:
                    case ElementIndex.BrownPaint:
                    case ElementIndex.Moss:
                    case ElementIndex.Seed:
                    case ElementIndex.Sapling:
                        context.ReplaceElement(neighbors.GetSlot(i).Position, context.Layer, ElementIndex.Fire);
                        break;

                    default:
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(ElementContext context, in float currentValue)
        {
            if (currentValue <= 500.0f)
            {
                if (context.GetStoredElement() is ElementIndex.None)
                {
                    context.ReplaceElement(ElementIndex.Stone);
                }
                else
                {
                    context.ReplaceElement(context.GetStoredElement());
                }

                context.SetElementTemperature(500.0f);
            }
        }
    }
}
