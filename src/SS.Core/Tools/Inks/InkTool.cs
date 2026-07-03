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

using StardustSandbox.Core.Enums.Indexers;
using StardustSandbox.Core.WorldSystem.Slots;

namespace StardustSandbox.Core.Tools.Inks
{
    internal sealed class InkTool : Tool
    {
        private readonly Color inkColor;

        internal InkTool(ToolIndex index, Color inkColor, GameEvents gameEvents) : base(index, gameEvents)
        {
            this.inkColor = inkColor;
        }

        internal override void Execute(ToolContext context)
        {
            if (!context.TileMap.TryGetSlot(context.Position, out Slot slot) || !slot.HasElement(context.Layer))
            {
                return;
            }

            context.TileMap.SetColorModifier(context.Position, context.Layer, this.inkColor);
        }
    }
}

