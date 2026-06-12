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

using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Interfaces.UI;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.UI.Common;

namespace StardustSandbox.Core.UI.Dependencies
{
    internal sealed class WorldExplorerUIDependencies(
        AssetDatabase assetDatabase,
        GameScreen gameScreen,
        GraphicsDevice graphicsDevice,
        SoundEffectManager soundEffectManager,
        UIManager uiManager,
        WorldDetailsUI worldDetailsUI,
        WorldSerializer worldSerializer
    ) : IUIDependencies
    {
        internal AssetDatabase AssetDatabase => assetDatabase;
        internal GameScreen GameScreen => gameScreen;
        internal GraphicsDevice GraphicsDevice => graphicsDevice;
        internal SoundEffectManager SoundEffectManager => soundEffectManager;
        internal UIManager UIManager => uiManager;
        internal WorldDetailsUI WorldDetailsUI => worldDetailsUI;
        internal WorldSerializer WorldSerializer => worldSerializer;
    }
}
