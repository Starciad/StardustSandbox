/*
 * Copyright (C) 2026  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
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

using StardustSandbox.Constants;
using StardustSandbox.Elements.Utilities;
using StardustSandbox.Enums.World;
using StardustSandbox.Randomness;

namespace StardustSandbox.Elements.Liquids
{
    internal sealed class LiquidCorruption : Liquid
    {
        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            if (CorruptionUtility.CheckIfNeighboringElementsAreCorrupted(Layer.Foreground, neighbors) &&
                CorruptionUtility.CheckIfNeighboringElementsAreCorrupted(Layer.Background, neighbors))
            {
                return;
            }

            context.NotifyChunk();

            if (SSRandom.Chance(ElementConstants.CHANCE_OF_CORRUPTION_TO_SPREAD))
            {
                context.InfectNeighboringElements(neighbors);
            }
        }
    }
}

