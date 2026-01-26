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
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.WorldSystem;

namespace StardustSandbox.Core.Tools.Environment
{
    internal sealed class FreezeTool : Tool
    {
        internal override void Execute(ToolContext context)
        {
            if (!context.World.TryGetSlot(context.Position, out Slot slot))
            {
                return;
            }

            SlotLayer slotLayer = slot.GetLayer(context.Layer);

            if (slotLayer.IsEmpty)
            {
                return;
            }

            context.World.SetElementTemperature(context.Position, context.Layer, TemperatureMath.Clamp(slotLayer.Temperature + ToolConstants.DEFAULT_FREEZE_VALUE));
            AchievementEngine.Unlock(AchievementIndex.ACH_011);
        }
    }
}

