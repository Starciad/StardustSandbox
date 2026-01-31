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

namespace StardustSandbox.Core.Enums.Assets
{
    internal enum SoundEffectIndex : sbyte
    {
        None = -1,

        #region GUIs

        GUI_Accepted,
        GUI_Click,
        GUI_Error,
        GUI_Hover,
        GUI_Message,
        GUI_Pause_Ended,
        GUI_Pause_Started,
        GUI_Rejected,
        GUI_Returning,
        GUI_Typing_1,
        GUI_Typing_2,
        GUI_Typing_3,
        GUI_Typing_4,
        GUI_Typing_5,
        GUI_World_Loaded,
        GUI_World_Saved,

        #endregion
    }
}
