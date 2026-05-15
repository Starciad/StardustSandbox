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
using StardustSandbox.Core.Enums.Achievements;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Randomness;
using StardustSandbox.Core.WorldSystem;

namespace StardustSandbox.Core.Elements.Energies
{
    internal sealed class Fire : Energy
    {
        internal Fire(ElementIndex index, ElementCategory category, ElementCharacteristics characteristics, ElementRenderingType renderingType, Point textureOriginOffset, Color referenceColor, AchievementManager achievementManager, StatisticsManager statisticsManager) : base(index, category, characteristics, renderingType, textureOriginOffset, referenceColor, achievementManager, statisticsManager)
        {
            this.InitialTemperature = 500.0f;
            this.BaseDensity = 0.0f;
        }

        private static bool TryIgniteElement(ElementContext context, Slot slot, SlotLayer slotLayer, Layer layer)
        {
            // Increase neighboring temperature by fire's heat value
            context.SetElementTemperature(slotLayer.Temperature + ElementConstants.FIRE_HEAT_VALUE);

            // Check if the element is flammable
            if (slotLayer.Element.HasCharacteristic(ElementCharacteristics.IsFlammable))
            {
                // Adjust combustion chance based on the element's flammability resistance
                int combustionChance = ElementConstants.CHANCE_OF_COMBUSTION;
                bool isAbove = slot.Position.Y < context.CurrentSlot.Position.Y;

                // Increase chance of combustion if the element is directly above
                if (isAbove)
                {
                    combustionChance += 10;
                }

                // Attempt combustion based on flammabilityResistance
                if (Random.Chance(combustionChance, 100.0f + slotLayer.Element.BaseFlammabilityResistance))
                {
                    context.ReplaceElementIndex(slot.Position, layer, ElementIndex.Fire);
                    return true;
                }
            }

            return false;
        }

        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            int burnedElements = 0;
            int aroundElements = 0;

            for (int i = 0; i < ElementConstants.NEIGHBORS_ARRAY_LENGTH; i++)
            {
                if (!neighbors.HasNeighbor(i))
                {
                    continue;
                }

                if (!neighbors.GetSlotLayer(i, Layer.Foreground).IsEmpty)
                {
                    if (TryIgniteElement(context, neighbors.GetSlot(i), neighbors.GetSlotLayer(i, Layer.Foreground), Layer.Foreground))
                    {
                        burnedElements++;
                    }

                    aroundElements++;
                }

                if (!neighbors.GetSlotLayer(i, Layer.Background).IsEmpty)
                {
                    if (TryIgniteElement(context, neighbors.GetSlot(i), neighbors.GetSlotLayer(i, Layer.Background), Layer.Background))
                    {
                        burnedElements++;
                    }

                    aroundElements++;
                }
            }

            // Unlock achievement if 4 or more elements were burned and there were at least 4 elements around
            if (burnedElements >= 4 && aroundElements >= 4)
            {
                this.AchievementManager.Unlock(AchievementIndex.ACH_023);
            }
        }

        protected override void OnStep(ElementContext context)
        {
            if (Random.Chance(ElementConstants.CHANCE_OF_FIRE_TO_DISAPPEAR))
            {
                context.DestroyElement();

                if (Random.Chance(ElementConstants.CHANCE_FOR_FIRE_TO_LEAVE_SMOKE))
                {
                    context.InstantiateElementIndex(ElementIndex.Smoke);
                }

                return;
            }

            Point targetPosition = new(context.CurrentSlot.Position.X + Random.Range(-1, 1), context.CurrentSlot.Position.Y - 1);

            if (context.IsEmptySlot(targetPosition))
            {
                if (context.TrySetPosition(targetPosition, context.CurrentLayer))
                {
                    return;
                }
            }
            else
            {
                if (!context.TryGetElement(targetPosition, context.CurrentLayer, out Element element))
                {
                    return;
                }

                if (element is not null && (element.Category is ElementCategory.MovableSolid or ElementCategory.Liquid or ElementCategory.Gas))
                {
                    context.SwappingElements(targetPosition);
                }
            }
        }
    }
}
