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
using StardustSandbox.Core.UI.Information;

using System;

namespace StardustSandbox.Core.UI.Common
{
    internal sealed partial class OptionsUI
    {
        private interface IOption
        {
            string Name { get; }
            string Description { get; }
            object[] Options { get; }
            bool RequiresApplicationRestart { get; }

            void SetValue(OptionSlotInfo optionSlotInfo);

            object GetValue();
            string GetValueString();
        }

        private sealed class Option<T>(string name, string description, Func<T> getValueFunc, Func<T, string> getValueStringFunc, Action<IOption, OptionSlotInfo> setValueAction) : IOption
        {
            public string Name => name;
            public string Description => description;
            public object[] Options { get; init; }
            public bool RequiresApplicationRestart { get; init; }

            public void SetValue(OptionSlotInfo optionSlotInfo)
            {
                setValueAction(this, optionSlotInfo);
            }

            public object GetValue()
            {
                return getValueFunc();
            }

            public string GetValueString()
            {
                return getValueStringFunc(getValueFunc());
            }
        }

        private sealed class Category(string name, string description, TextureIndex textureIndex, Rectangle textureSourceRectangle, params IOption[] options)
        {
            internal string Name => name;
            internal string Description => description;
            internal TextureIndex TextureIndex => textureIndex;
            internal Rectangle TextureSourceRectangle => textureSourceRectangle;
            internal int Length => options.Length;

            internal IOption this[int i]
            {
                get => options[i];
            }
        }
    }
}
