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

using System;

namespace StardustSandbox.UI.Options
{
    internal sealed class SelectorOption(string name, string description, object[] values) : Option(name, description)
    {
        internal object[] Values => values;

        private int selectedValueIndex;

        internal override object GetValue()
        {
            return this.Values[this.selectedValueIndex];
        }

        internal override void SetValue(object value)
        {
            this.selectedValueIndex = Array.IndexOf(this.Values, value);
        }

        internal void Next()
        {
            this.selectedValueIndex = (this.selectedValueIndex + 1) % this.Values.Length;
        }

        internal void Previous()
        {
            this.selectedValueIndex = this.selectedValueIndex == 0
                ? this.Values.Length - 1
                : this.selectedValueIndex - 1;
        }
    }
}

