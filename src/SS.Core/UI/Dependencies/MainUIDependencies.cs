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
using StardustSandbox.Core.WorldSystem;

namespace StardustSandbox.Core.UI.Dependencies
{
    internal sealed class MainUIDependencies(
        AmbientManager ambientManager,
        AssetDatabase assetDatabase,
        GameHandler gameHandler,
        SongManager songManager,
        SoundEffectManager soundEffectManager,
        UIManager uiManager,
        World world
    ) : IUIDependencies
    {
        internal AmbientManager AmbientManager => ambientManager;
        internal AssetDatabase AssetDatabase => assetDatabase;
        internal GameHandler GameHandler => gameHandler;
        internal SongManager SongManager => songManager;
        internal SoundEffectManager SoundEffectManager => soundEffectManager;
        internal UIManager UIManager => uiManager;
        internal World World => world;
    }
}
