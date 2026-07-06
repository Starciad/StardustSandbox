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
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Events.Elements;
using StardustSandbox.Core.Randomness;
using StardustSandbox.Core.WorldSystem.Slots;

namespace StardustSandbox.Core.Elements.Energies
{
    internal sealed class Fire : Energy
    {
        internal Fire(ElementIndex index, ElementCategory category, ElementRenderingType renderingType, Point textureOriginOffset, Color referenceColor, GameEvents gameEvents) : base(index, category, renderingType, textureOriginOffset, referenceColor, gameEvents)
        {
            this.InitialTemperature = 500.0f;
            this.BaseDensity = 0.0f;

            this.HasNeighborInteractions = true;
            this.HasTemperature = true;
            this.IsExplosionImmune = true;
            this.IsCorruptible = true;
            this.IsPushable = true;
        }

        private static bool TryIgniteElement(ElementContext context, Slot slot, Layer layer)
        {
            // Increase neighboring temperature by fire's heat value
            context.SetTemperature(slot.GetTemperature(layer) + ElementConstants.FIRE_HEAT_VALUE);

            // Check if the element is flammable
            Element element = slot.GetElement(layer);

            if (element.IsFlammable)
            {
                // Adjust combustion chance based on the element's flammability resistance
                int combustionChance = ElementConstants.CHANCE_OF_COMBUSTION;
                bool isAbove = slot.Position.Y < context.Slot.Position.Y;

                // Increase chance of combustion if the element is directly above
                if (isAbove)
                {
                    combustionChance += 10;
                }

                // Attempt combustion based on flammabilityResistance
                if (Random.Chance(combustionChance, 100.0f + element.BaseFlammabilityResistance))
                {
                    context.Replace(slot.Position, layer, ElementIndex.Fire);
                    return true;
                }
            }

            return false;
        }

        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            int aroundElements = 0, burnedElements = 0;

            void ProcessLayer(Slot slot, Layer layer)
            {
                if (!slot.HasElement(layer))
                {
                    return;
                }

                if (TryIgniteElement(context, slot, layer))
                {
                    burnedElements++;
                }

                aroundElements++;
            }

            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.HasNeighbor(i))
                {
                    continue;
                }

                ProcessLayer(neighbors.GetSlot(i), Layer.Foreground);
                ProcessLayer(neighbors.GetSlot(i), Layer.Background);
            }

            this.GameEvents.Publish(new FireSpreadEvent(aroundElements, burnedElements));
        }

        protected override void OnStep(ElementContext context)
        {
            if (Random.Chance(ElementConstants.CHANCE_OF_FIRE_TO_DISAPPEAR))
            {
                context.Destroy();

                if (Random.Chance(ElementConstants.CHANCE_FOR_FIRE_TO_LEAVE_SMOKE))
                {
                    context.Instantiate(ElementIndex.Smoke);
                }

                return;
            }

            Point targetPosition = new(context.Slot.Position.X + Random.Range(-1, 1), context.Slot.Position.Y - 1);

            // If the target position is empty or can be moved into, do nothing.
            if (!context.HasElement(targetPosition) ||
                context.TrySetPosition(targetPosition, context.Layer) ||
                !context.TryGetElement(targetPosition, context.Layer, out Element element))
            {
                return;
            }

            // If the element is not null and is movable, liquid, or gas, swap positions with it.
            if (element is not null && (element.Category is ElementCategory.MovableSolid or ElementCategory.Liquid or ElementCategory.Gas))
            {
                context.Swap(targetPosition);
            }
        }
    }
}
