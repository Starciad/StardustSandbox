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

using StardustSandbox.UI.Elements;

namespace StardustSandbox.UI.Information
{
    internal sealed class SlotInfo
    {
        internal Image Background => this.background;
        internal Image Icon => this.icon;
        internal Label Label => this.label;

        private readonly Image background;
        private readonly Image icon;
        private readonly Label label;

        internal SlotInfo(Image background, Image icon)
        {
            this.background = background;
            this.icon = icon;
            this.label = null;
        }

        internal SlotInfo(Image background, Image icon, Label label)
        {
            this.background = background;
            this.icon = icon;
            this.label = label;
        }
    }
}

