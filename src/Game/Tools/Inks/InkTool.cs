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

using Microsoft.Xna.Framework;

using StardustSandbox.WorldSystem;

namespace StardustSandbox.Tools.Inks
{
    internal abstract class InkTool : Tool
    {
        internal required Color InkColor { get; init; }

        internal override void Execute(ToolContext context)
        {
            if (!context.World.TryGetSlot(context.Position, out Slot slot) ||
                slot.GetLayer(context.Layer).IsEmpty)
            {
                return;
            }

            context.World.SetElementColorModifier(context.Position, context.Layer, this.InkColor);
        }
    }
}

