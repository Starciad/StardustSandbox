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
using StardustSandbox.Core.InputSystem;
using StardustSandbox.Core.Interfaces.UI;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.UI.Common;
using StardustSandbox.Core.UI.Elements.Compounds;

namespace StardustSandbox.Core.UI.Dependencies
{
    internal sealed class OptionsUIDependencies(
        AssetDatabase assetDatabase,
        ColorPickerUI colorPickerUI,
        CursorManager cursorManager,
        GameHandler gameHandler,
        GameScreen gameScreen,
        KeySelectorUI keySelectorUI,
        PlayerInputController playerInputController,
        SelectorUI selectorUI,
        SettingsSerializer settingsSerializer,
        SliderUI sliderUI,
        SongManager songManager,
        SoundEffectManager soundEffectManager,
        UIManager uiManager,
        VideoManager videoManager
    ) : IUIDependencies
    {
        internal AssetDatabase AssetDatabase => assetDatabase;
        internal ColorPickerUI ColorPickerUI => colorPickerUI;
        internal CursorManager CursorManager => cursorManager;
        internal GameHandler GameHandler => gameHandler;
        internal GameScreen GameScreen => gameScreen;
        internal KeySelectorUI KeySelectorUI => keySelectorUI;
        internal PlayerInputController PlayerInputController => playerInputController;
        internal SelectorUI SelectorUI => selectorUI;
        internal SettingsSerializer SettingsSerializer => settingsSerializer;
        internal SliderUI SliderUI => sliderUI;
        internal SongManager SongManager => songManager;
        internal SoundEffectManager SoundEffectManager => soundEffectManager;
        internal UIManager UIManager => uiManager;
        internal VideoManager VideoManager => videoManager;
    }
}
