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

using StardustSandbox.Core.Enums.Assets;

namespace StardustSandbox.Core.UI.Common
{
    internal sealed partial class OptionsUI
    {
        private enum OptionType
        {
            ColorSelector,
            KeySelector,
            Selector,
            Slider,
            Toggle
        }

        private sealed class Category(string name, string description, TextureIndex textureIndex, Rectangle textureSourceRectangle, params Option[] options)
        {
            internal string Name => name;
            internal string Description => description;
            internal TextureIndex TextureIndex => textureIndex;
            internal Rectangle TextureSourceRectangle => textureSourceRectangle;
            internal Option[] Options => options;
        }

        private sealed class Option(string name, string description, OptionType type)
        {
            internal string Name => name;
            internal string Description => description;
            internal OptionType Type => type;
        }
    }
}
