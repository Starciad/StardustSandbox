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
using StardustSandbox.Core.Enums.Achievements;
using StardustSandbox.Core.Enums.Elements;

namespace StardustSandbox.Core.Elements.Solids.Immovables
{
    internal sealed class WetSponge : ImmovableSolid
    {
        protected override void OnStep(ElementContext context)
        {
            Point belowPosition = new(context.Slot.Position.X, context.Slot.Position.Y + 1);

            if (context.TryGetElement(belowPosition, context.Layer, out ElementIndex index))
            {
                switch (index)
                {
                    case ElementIndex.DrySponge:
                        context.SwappingElements(context.Position, belowPosition);
                        break;

                    default:
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(ElementContext context, in float currentValue)
        {
            if (currentValue >= 60.0f)
            {
                context.ReplaceElement(ElementIndex.DrySponge);
                AchievementEngine.Unlock(AchievementIndex.ACH_019);
            }
        }
    }
}
