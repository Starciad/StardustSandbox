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

using StardustSandbox.Core.Enums.Inputs.Game;

namespace StardustSandbox.Core.Extensions
{
    internal static class PenToolExtension
    {
        internal static PenTool Next(this PenTool penTool)
        {
            return penTool switch
            {
                PenTool.Visualization => PenTool.Pencil,
                PenTool.Pencil => PenTool.Eraser,
                PenTool.Eraser => PenTool.Fill,
                PenTool.Fill => PenTool.Replace,
                PenTool.Replace => PenTool.Visualization,
                _ => penTool
            };
        }

        internal static PenTool Previous(this PenTool penTool)
        {
            return penTool switch
            {
                PenTool.Visualization => PenTool.Replace,
                PenTool.Pencil => PenTool.Visualization,
                PenTool.Eraser => PenTool.Pencil,
                PenTool.Fill => PenTool.Eraser,
                PenTool.Replace => PenTool.Fill,
                _ => penTool
            };
        }
    }
}
