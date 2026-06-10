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

using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Interfaces.UI;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.UI.Common;

namespace StardustSandbox.Core.UI.Dependencies
{
    internal sealed class PauseUIDependencies : IUIDependencies
    {
        internal PauseUIDependencies(
            AssetDatabase assetDatabase,
            ConfirmUI confirmUI,
            GameHandler gameHandler,
            GameScreen gameScreen,
            OptionsUI optionsUI,
            SoundEffectManager soundEffectManager,
            UIManager uiManager
        )
        {
        }
    }
}
