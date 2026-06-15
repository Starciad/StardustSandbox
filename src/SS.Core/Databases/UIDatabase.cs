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
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Enums.Indexers;
using StardustSandbox.Core.InputSystem;
using StardustSandbox.Core.Interfaces.UI;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.Serialization.Progress;
using StardustSandbox.Core.Serialization.Settings;
using StardustSandbox.Core.UI;
using StardustSandbox.Core.UI.Common;
using StardustSandbox.Core.UI.Dependencies;
using StardustSandbox.Core.UI.Elements.Compounds;
using StardustSandbox.Core.UI.Handlers;
using StardustSandbox.Core.WorldSystem;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.Databases
{
    internal sealed class UIDatabase
    {
        private readonly Dictionary<Type, IUI> uis;

        internal void Load(
            AchievementDatabase achievementDatabase,
            AchievementManager achievementManager,
            ActorManager actorManager,
            AmbientManager ambientManager,
            AssetDatabase assetDatabase,
            CatalogDatabase catalogDatabase,
            CursorManager cursorManager,
            GameHandler gameHandler,
            GameScreen gameScreen,
            GameWindow gameWindow,
            GraphicsDeviceManager graphicsDeviceManager,
            PlayerInputController playerInputController,
            ProgressSerializer progressSerializer,
            SettingsSerializer settingsSerializer,
            SongManager songManager,
            SoundEffectManager soundEffectManager,
            UIElementHandler elementHandler,
            UIManager uiManager,
            VideoManager videoManager,
            World world,
            WorldSerializer worldSerializer
        )
        {
            AchievementsUIDependencies achievementsUIDependencies = new(achievementDatabase, assetDatabase, ambientManager, gameScreen, progressSerializer, soundEffectManager, uiManager);
            ColorPickerUIDependencies colorPickerUIDependencies = new(assetDatabase, gameHandler, gameScreen, soundEffectManager, uiManager);
            ConfirmUIDependencies confirmUIDependencies = new(assetDatabase, gameHandler, gameScreen, soundEffectManager, uiManager);
            CreditsUIDependencies creditsUIDependencies = new(ambientManager, assetDatabase, gameScreen, songManager, uiManager, world);
            EnvironmentSettingsUIDependencies environmentSettingsUIDependencies = new(assetDatabase, gameHandler, gameScreen, soundEffectManager, uiManager, world);
            GeneratorSettingsUIDependencies generatorSettingsUIDependencies = new(assetDatabase, gameHandler, gameScreen, soundEffectManager, uiManager, world);
            HudUIDependencies hudUIDependencies = new(achievementManager, assetDatabase, catalogDatabase, gameHandler, gameScreen, playerInputController, soundEffectManager, uiManager);
            InformationUIDependencies informationUIDependencies = new(actorManager, assetDatabase, gameHandler, gameScreen, soundEffectManager, uiManager, world);
            ItemExplorerUIDependencies itemExplorerUIDependencies = new(assetDatabase, catalogDatabase, gameHandler, gameScreen, soundEffectManager, uiManager);
            ItemSearchUIDependencies itemSearchUIDependencies = new(assetDatabase, catalogDatabase, gameHandler, gameScreen, gameWindow, playerInputController, soundEffectManager, uiManager);
            KeySelectorUIDependencies keySelectorUIDependencies = new(assetDatabase, gameHandler, gameScreen, gameWindow, playerInputController, soundEffectManager, uiManager);
            MainUIDependencies mainUIDependencies = new(ambientManager, assetDatabase, gameHandler, gameScreen, songManager, soundEffectManager, uiManager, world);
            MessageUIDependencies messageUIDependencies = new(assetDatabase, gameHandler, gameScreen, uiManager);
            OptionsUIDependencies optionsUIDependencies = new(assetDatabase, cursorManager, gameHandler, gameScreen, playerInputController, settingsSerializer, songManager, soundEffectManager, uiManager, videoManager);
            PauseUIDependencies pauseUIDependencies = new(assetDatabase, gameHandler, gameScreen, soundEffectManager, uiManager);
            PenSettingsUIDependencies penSettingsUIDependencies = new(assetDatabase, gameHandler, gameScreen, playerInputController, soundEffectManager, uiManager, world);
            PlayUIDependencies playUIDependencies = new(assetDatabase, gameScreen, soundEffectManager, uiManager);
            SaveUIDependencies saveUIDependencies = new(assetDatabase, gameHandler, gameScreen, graphicsDeviceManager, soundEffectManager, uiManager, world, worldSerializer);
            SelectorUIDependencies selectorUIDependencies = new(assetDatabase, gameHandler, gameScreen, soundEffectManager, uiManager);
            SliderUIDependencies sliderUIDependencies = new(assetDatabase, gameHandler, gameScreen, soundEffectManager, uiManager);
            TemperatureSettingsUIDependencies temperatureSettingsUIDependencies = new(assetDatabase, gameHandler, gameScreen, soundEffectManager, uiManager, world);
            TextInputUIDependencies textInputUIDependencies = new(assetDatabase, gameHandler, gameScreen, gameWindow, playerInputController, soundEffectManager, uiManager);
            TutorialUIDependencies tutorialUIDependencies = new(assetDatabase, gameScreen, settingsSerializer, uiManager);
            WorldDetailsUIDependencies worldDetailsUIDependencies = new(assetDatabase, gameHandler, gameScreen, soundEffectManager, uiManager, worldSerializer);
            WorldExplorerUIDependencies worldExplorerUIDependencies = new(assetDatabase, gameScreen, graphicsDeviceManager, soundEffectManager, uiManager, worldSerializer);
            WorldSettingsUIDependencies worldSettingsUIDependencies = new(actorManager, assetDatabase, gameHandler, gameScreen, soundEffectManager, uiManager, world);

            AchievementsUI achievementsUI = new(achievementsUIDependencies, elementHandler);
            ColorPickerUI colorPickerUI = new(colorPickerUIDependencies, elementHandler);
            ConfirmUI confirmUI = new(confirmUIDependencies, elementHandler);
            CreditsUI creditsUI = new(creditsUIDependencies, elementHandler);
            EnvironmentSettingsUI environmentSettingsUI = new(environmentSettingsUIDependencies, elementHandler);
            GeneratorSettingsUI generatorSettingsUI = new(generatorSettingsUIDependencies, elementHandler);
            HudUI hudUI = new(hudUIDependencies, elementHandler);
            InformationUI informationUI = new(informationUIDependencies, elementHandler);
            ItemExplorerUI itemExplorerUI = new(itemExplorerUIDependencies, elementHandler);
            ItemSearchUI itemSearchUI = new(itemSearchUIDependencies, elementHandler);
            KeySelectorUI keySelectorUI = new(keySelectorUIDependencies, elementHandler);
            MainUI mainUI = new(mainUIDependencies, elementHandler);
            MessageUI messageUI = new(messageUIDependencies, elementHandler);
            OptionsUI optionsUI = new(optionsUIDependencies, elementHandler);
            PauseUI pauseUI = new(pauseUIDependencies, elementHandler);
            PenSettingsUI penSettingsUI = new(penSettingsUIDependencies, elementHandler);
            PlayUI playUI = new(playUIDependencies, elementHandler);
            SaveUI saveUI = new(saveUIDependencies, elementHandler);
            SelectorUI selectorUI = new(selectorUIDependencies, elementHandler);
            SliderUI sliderUI = new(sliderUIDependencies, elementHandler);
            TemperatureSettingsUI temperatureSettingsUI = new(temperatureSettingsUIDependencies, elementHandler);
            TextInputUI textInputUI = new(textInputUIDependencies, elementHandler);
            TutorialUI tutorialUI = new(tutorialUIDependencies, elementHandler);
            WorldDetailsUI worldDetailsUI = new(worldDetailsUIDependencies, elementHandler);
            WorldExplorerUI worldExplorerUI = new(worldExplorerUIDependencies, elementHandler);
            WorldSettingsUI worldSettingsUI = new(worldSettingsUIDependencies, elementHandler);

            RegisterUI(achievementsUI);
            RegisterUI(colorPickerUI);
            RegisterUI(confirmUI);
            RegisterUI(creditsUI);
            RegisterUI(environmentSettingsUI);
            RegisterUI(generatorSettingsUI);
            RegisterUI(hudUI);
            RegisterUI(informationUI);
            RegisterUI(itemExplorerUI);
            RegisterUI(itemSearchUI);
            RegisterUI(keySelectorUI);
            RegisterUI(mainUI);
            RegisterUI(messageUI);
            RegisterUI(optionsUI);
            RegisterUI(pauseUI);
            RegisterUI(penSettingsUI);
            RegisterUI(playUI);
            RegisterUI(saveUI);
            RegisterUI(selectorUI);
            RegisterUI(sliderUI);
            RegisterUI(temperatureSettingsUI);
            RegisterUI(textInputUI);
            RegisterUI(tutorialUI);
            RegisterUI(worldDetailsUI);
            RegisterUI(worldExplorerUI);
            RegisterUI(worldSettingsUI);
        }

        private void RegisterUI<T>(T ui) where T : IUI
        {
            this.uis.Add(typeof(T), ui);
        }

        internal T GetUI<T>() where T : IUI
        {
            return (T)this.uis[typeof(T)];
        }
    }
}
