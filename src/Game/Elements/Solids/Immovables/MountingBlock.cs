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

using StardustSandbox.Constants;
using StardustSandbox.Core;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Extensions;

namespace StardustSandbox.Elements.Solids.Immovables
{
    internal sealed class MountingBlock : ImmovableSolid
    {
        protected override void OnInstantiated(ElementContext context)
        {
            context.SetElementColorModifier(ElementConstants.COLORS_OF_MOUNTING_BLOCKS.GetRandomItem());
        }

        protected override void OnTemperatureChanged(ElementContext context, in float currentValue)
        {
            if (currentValue > 300.0f)
            {
                if (Random.Chance(75))
                {
                    context.ReplaceElement(ElementIndex.Fire);
                }
                else
                {
                    context.ReplaceElement(ElementIndex.Ash);
                }
            }
        }
    }
}

