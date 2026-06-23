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

using StardustSandbox.Core.Databases;
using StardustSandbox.Core.InputSystem;
using StardustSandbox.Core.Interfaces.UI;
using StardustSandbox.Core.Managers;

namespace StardustSandbox.Core.UI.Dependencies
{
    internal sealed class ItemSearchUIDependencies(
        AssetDatabase assetDatabase,
        CatalogDatabase catalogDatabase,
        GameHandler gameHandler,
        GameWindow gameWindow,
        PlayerInputController playerInputController,
        SoundEffectManager soundEffectManager,
        UIManager uiManager
    ) : IUIDependencies
    {
        internal AssetDatabase AssetDatabase => assetDatabase;
        internal CatalogDatabase CatalogDatabase => catalogDatabase;
        internal GameHandler GameHandler => gameHandler;
        internal GameWindow GameWindow => gameWindow;
        internal PlayerInputController PlayerInputController => playerInputController;
        internal SoundEffectManager SoundEffectManager => soundEffectManager;
        internal UIManager UIManager => uiManager;
    }
}
