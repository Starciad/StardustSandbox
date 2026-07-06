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
using StardustSandbox.Core.Randomness;
using StardustSandbox.Core.WorldSystem.Slots;

namespace StardustSandbox.Core.Elements.Gases
{
    internal sealed class AntiCorruption : Gas
    {
        internal AntiCorruption(ElementIndex index, ElementCategory category, ElementRenderingType renderingType, Point textureOriginOffset, Color referenceColor, GameEvents gameEvents) : base(index, category, renderingType, textureOriginOffset, referenceColor, gameEvents)
        {
            this.BaseDensity = 0.5f;

            this.HasNeighborInteractions = true;
            this.IsPushable = true;
        }

        private static void ProcessDissipation(ElementContext context)
        {
            // If the AntiCorruption element has a stored element, it will check if
            // it is in a dissipating state. If it is, it will replace itself with
            // the stored element. If not, it will set itself to a dissipating state.
            // If there is no stored element, there is a 5% chance that the AntiCorruption
            // element will be destroyed. Otherwise, it will notify its chunk to update.

            if (context.HasStoredElement())
            {
                if (context.GetDissipatingState())
                {
                    context.Replace(context.GetStoredElementIndex());
                    return;
                }

                context.SetDissipatingState(true);
                return;
            }

            if (Random.Chance(5))
            {
                context.Destroy();
                return;
            }

            context.NotifyChunk();
        }

        private static void PurifyNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.HasNeighbor(i))
                {
                    continue;
                }

                Slot slot = neighbors.GetSlot(i);

                if (slot.HasElement(context.Layer) &&
                    slot.GetElementIndex(context.Layer) is not ElementIndex.AntiCorruption &&
                    slot.GetElement(context.Layer).IsCorruption)
                {
                    ElementIndex originalElementIndex = slot.GetStoredElementIndex(context.Layer);

                    context.Replace(slot.Position, ElementIndex.AntiCorruption);
                    context.SetStoredElementIndex(slot.Position, context.Layer, originalElementIndex);
                }
            }
        }

        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            ProcessDissipation(context);
            PurifyNeighbors(context, neighbors);
        }

        protected override void OnStep(ElementContext context)
        {
            if (!context.HasStoredElement())
            {
                base.OnStep(context);
            }
        }
    }
}
