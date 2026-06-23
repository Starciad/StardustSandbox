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

using StardustSandbox.Core.InputSystem;
using StardustSandbox.Core.Interfaces.UI;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.UI.Common;
using StardustSandbox.Core.UI.Dependencies;
using StardustSandbox.Core.UI.Handlers;
using StardustSandbox.Core.WorldSystem;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.Databases
{
    internal sealed class UIDatabase
    {
        private readonly Dictionary<Type, IUI> uis = [];

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
            UIElementHandler uiElementHandler,
            UIManager uiManager,
            VideoManager videoManager,
            World world,
            WorldSerializer worldSerializer
        )
        {
            AchievementsUIDependencies achievementsUIDependencies = new(achievementDatabase, assetDatabase, ambientManager, progressSerializer, soundEffectManager, uiManager);
            ColorPickerUIDependencies colorPickerUIDependencies = new(assetDatabase, gameHandler, soundEffectManager, uiManager);
            ConfirmUIDependencies confirmUIDependencies = new(assetDatabase, gameHandler, soundEffectManager, uiManager);
            CreditsUIDependencies creditsUIDependencies = new(ambientManager, assetDatabase, songManager, uiManager, world);
            EnvironmentSettingsUIDependencies environmentSettingsUIDependencies = new(assetDatabase, gameHandler, soundEffectManager, uiManager, world);
            ExperimentalUIDependencies experimentalUIDependencies = new();
            GeneratorSettingsUIDependencies generatorSettingsUIDependencies = new(assetDatabase, gameHandler, soundEffectManager, uiManager, world);
            HudUIDependencies hudUIDependencies = new(achievementManager, assetDatabase, catalogDatabase, gameHandler, playerInputController, soundEffectManager, uiManager);
            InformationUIDependencies informationUIDependencies = new(actorManager, assetDatabase, gameHandler, soundEffectManager, uiManager, world);
            ItemExplorerUIDependencies itemExplorerUIDependencies = new(assetDatabase, catalogDatabase, gameHandler, soundEffectManager, uiManager);
            ItemSearchUIDependencies itemSearchUIDependencies = new(assetDatabase, catalogDatabase, gameHandler, gameWindow, playerInputController, soundEffectManager, uiManager);
            KeySelectorUIDependencies keySelectorUIDependencies = new(assetDatabase, gameHandler, gameWindow, playerInputController, soundEffectManager, uiManager);
            MainUIDependencies mainUIDependencies = new(ambientManager, assetDatabase, gameHandler, songManager, soundEffectManager, uiManager, world);
            MessageUIDependencies messageUIDependencies = new(assetDatabase, gameHandler, uiManager);
            OptionsUIDependencies optionsUIDependencies = new(assetDatabase, cursorManager, gameHandler, playerInputController, settingsSerializer, songManager, soundEffectManager, uiManager, videoManager);
            PauseUIDependencies pauseUIDependencies = new(assetDatabase, gameHandler, soundEffectManager, uiManager);
            PenSettingsUIDependencies penSettingsUIDependencies = new(assetDatabase, gameHandler, playerInputController, soundEffectManager, uiManager, world);
            PlayUIDependencies playUIDependencies = new(assetDatabase, soundEffectManager, uiManager);
            SaveUIDependencies saveUIDependencies = new(assetDatabase, gameHandler, graphicsDeviceManager, soundEffectManager, uiManager, world, worldSerializer);
            SelectorUIDependencies selectorUIDependencies = new(assetDatabase, gameHandler, soundEffectManager, uiManager);
            SliderUIDependencies sliderUIDependencies = new(assetDatabase, gameHandler, soundEffectManager, uiManager);
            TemperatureSettingsUIDependencies temperatureSettingsUIDependencies = new(assetDatabase, gameHandler, soundEffectManager, uiManager, world);
            TextInputUIDependencies textInputUIDependencies = new(assetDatabase, gameHandler, gameWindow, playerInputController, soundEffectManager, uiManager);
            TutorialUIDependencies tutorialUIDependencies = new(assetDatabase, settingsSerializer, uiManager);
            WorldDetailsUIDependencies worldDetailsUIDependencies = new(assetDatabase, gameHandler, soundEffectManager, uiManager, worldSerializer);
            WorldExplorerUIDependencies worldExplorerUIDependencies = new(assetDatabase, graphicsDeviceManager, soundEffectManager, uiManager, worldSerializer);
            WorldSettingsUIDependencies worldSettingsUIDependencies = new(actorManager, assetDatabase, gameHandler, soundEffectManager, uiManager, world);

            AchievementsUI achievementsUI = new(achievementsUIDependencies, gameScreen, uiElementHandler);
            ColorPickerUI colorPickerUI = new(colorPickerUIDependencies, gameScreen, uiElementHandler);
            ConfirmUI confirmUI = new(confirmUIDependencies, gameScreen, uiElementHandler);
            CreditsUI creditsUI = new(creditsUIDependencies, gameScreen, uiElementHandler);
            EnvironmentSettingsUI environmentSettingsUI = new(environmentSettingsUIDependencies, gameScreen, uiElementHandler);
            ExperimentalUI experimentalUI = new(experimentalUIDependencies, gameScreen, uiElementHandler);
            GeneratorSettingsUI generatorSettingsUI = new(generatorSettingsUIDependencies, gameScreen, uiElementHandler);
            HudUI hudUI = new(hudUIDependencies, gameScreen, uiElementHandler);
            InformationUI informationUI = new(informationUIDependencies, gameScreen, uiElementHandler);
            ItemExplorerUI itemExplorerUI = new(itemExplorerUIDependencies, gameScreen, uiElementHandler);
            ItemSearchUI itemSearchUI = new(itemSearchUIDependencies, gameScreen, uiElementHandler);
            KeySelectorUI keySelectorUI = new(keySelectorUIDependencies, gameScreen, uiElementHandler);
            MainUI mainUI = new(mainUIDependencies, gameScreen, uiElementHandler);
            MessageUI messageUI = new(messageUIDependencies, gameScreen, uiElementHandler);
            OptionsUI optionsUI = new(optionsUIDependencies, gameScreen, uiElementHandler);
            PauseUI pauseUI = new(pauseUIDependencies, gameScreen, uiElementHandler);
            PenSettingsUI penSettingsUI = new(penSettingsUIDependencies, gameScreen, uiElementHandler);
            PlayUI playUI = new(playUIDependencies, gameScreen, uiElementHandler);
            SaveUI saveUI = new(saveUIDependencies, gameScreen, uiElementHandler);
            SelectorUI selectorUI = new(selectorUIDependencies, gameScreen, uiElementHandler);
            SliderUI sliderUI = new(sliderUIDependencies, gameScreen, uiElementHandler);
            TemperatureSettingsUI temperatureSettingsUI = new(temperatureSettingsUIDependencies, gameScreen, uiElementHandler);
            TextInputUI textInputUI = new(textInputUIDependencies, gameScreen, uiElementHandler);
            TutorialUI tutorialUI = new(tutorialUIDependencies, gameScreen, uiElementHandler);
            WorldDetailsUI worldDetailsUI = new(worldDetailsUIDependencies, gameScreen, uiElementHandler);
            WorldExplorerUI worldExplorerUI = new(worldExplorerUIDependencies, gameScreen, uiElementHandler);
            WorldSettingsUI worldSettingsUI = new(worldSettingsUIDependencies, gameScreen, uiElementHandler);

            RegisterUI(achievementsUI);
            RegisterUI(colorPickerUI);
            RegisterUI(confirmUI);
            RegisterUI(creditsUI);
            RegisterUI(environmentSettingsUI);
            RegisterUI(experimentalUI);
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
