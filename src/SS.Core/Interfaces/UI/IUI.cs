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

namespace StardustSandbox.Core.Interfaces.UI
{
    internal interface IUI
    {
        bool IsActive { get; }

        void Open(IUIModel model);
        void Close();
        void Refresh();

        // Show and Hide are helper methods similar to
        // Open and Close; the difference lies in the fact
        // that the model is preserved regardless of the
        // situation, allowing for an identical reconstruction
        // should the UI be reloaded.

        void Show();
        void Hide();
    }
}
