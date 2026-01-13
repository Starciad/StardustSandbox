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

using StardustSandbox.Enums.UI.Tools;

namespace StardustSandbox.UI.States
{
    internal struct TextValidationState
    {
        internal ValidationStatus Status { get; private set; }
        internal string Message { get; private set; }

        public TextValidationState(ValidationStatus status)
        {
            this.Status = status;
            this.Message = string.Empty;
        }

        public TextValidationState(ValidationStatus status, string message)
        {
            this.Status = status;
            this.Message = message;
        }
    }
}

