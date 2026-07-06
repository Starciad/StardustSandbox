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

namespace StardustSandbox.Core.Elements.Liquids
{
    internal sealed class Lava : Liquid
    {
        internal Lava(ElementIndex index, ElementCategory category, ElementRenderingType renderingType, Point textureOriginOffset, Color referenceColor, GameEvents gameEvents) : base(index, category, renderingType, textureOriginOffset, referenceColor, gameEvents)
        {
            this.InitialTemperature = 1500.0f;
            this.BaseDensity = 2.7f;
            this.BaseDispersionRate = 1;
            this.BaseExplosionResistance = 0.4f;

            this.HasNeighborInteractions = true;
            this.HasTemperature = true;
            this.IsCorruptible = true;
            this.IsPushable = true;
        }

        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, context.Layer))
                {
                    continue;
                }

                switch (neighbors.GetSlot(i).GetElementIndex(context.Layer))
                {
                    case ElementIndex.Oil:
                    case ElementIndex.Wood:
                    case ElementIndex.Leaf:
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
                        context.Replace(neighbors.GetNeighborPosition(i), context.Layer, ElementIndex.Fire);
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
                    context.Replace(ElementIndex.Stone);
                }
                else
                {
                    context.Replace(context.GetStoredElementIndex());
                }

                context.SetTemperature(500.0f);
            }
        }
    }
}
