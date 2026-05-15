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

using StardustSandbox.Core.Cameras;
using StardustSandbox.Core.Enums.UI;
using StardustSandbox.Core.InputSystem;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.UI;
using StardustSandbox.Core.UI.Common;
using StardustSandbox.Core.UI.Elements;
using StardustSandbox.Core.WorldSystem;

using System;

namespace StardustSandbox.Core.Databases
{
    internal sealed class UIDatabase
    {
        private UIBase[] uis;
        private bool isLoaded;

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
            GraphicsDevice graphicsDevice,
            PlayerInputController playerInputController,
            SongManager songManager,
            SoundEffectManager soundEffectManager,
            UIManager uiManager,
            VideoManager videoManager,
            World world
        )
        {
            if (isLoaded)
            {
                throw new InvalidOperationException($"{nameof(UIDatabase)} has already been loaded.");
            }

            NotificationBox notificationBox = new();
            TooltipBox tooltipBox = new(assetDatabase, cursorManager, gameScreen)
            {
                MinimumSize = new(500f, 0f),
            };

            ColorPickerUI colorPickerUI = new(
                gameHandler,
                gameScreen,
                soundEffectManager,
                tooltipBox,
                uiManager
            );

            ConfirmUI confirmUI = new(
                gameHandler,
                gameScreen,
                soundEffectManager,
                uiManager
            );

            CreditsUI creditsUI = new(
                assetDatabase,
                ambientManager,
                gameScreen,
                songManager,
                uiManager,
                world
            );

            EnvironmentSettingsUI environmentSettingsUI = new(
                gameHandler,
                gameScreen,
                soundEffectManager,
                tooltipBox,
                uiManager,
                world
            );

            GeneratorSettingsUI generatorSettingsUI = new(
                confirmUI,
                gameHandler,
                gameScreen,
                soundEffectManager,
                tooltipBox,
                uiManager,
                world
            );

            HudUI hudUI = new(
                achievementManager,
                catalogDatabase,
                confirmUI,
                gameHandler,
                notificationBox,
                playerInputController,
                soundEffectManager,
                tooltipBox,
                uiManager
            );

            InformationUI informationUI = new(
                actorManager,
                gameHandler,
                gameScreen,
                soundEffectManager,
                tooltipBox,
                uiManager,
                world
            );

            ItemSearchUI itemSearchUI = new(
                catalogDatabase,
                gameHandler,
                gameScreen,
                gameWindow,
                playerInputController,
                soundEffectManager,
                tooltipBox,
                uiManager
            );

            ItemExplorerUI itemExplorerUI = new(
                assetDatabase,
                catalogDatabase,
                gameHandler,
                gameScreen,
                hudUI,
                itemSearchUI,
                soundEffectManager,
                tooltipBox,
                uiManager
            );

            KeySelectorUI keySelectorUI = new(
                gameHandler,
                gameScreen,
                gameWindow,
                playerInputController,
                soundEffectManager,
                uiManager
            );

            MessageUI messageUI = new(
                gameHandler,
                gameScreen,
                uiManager
            );

            SliderUI sliderUI = new(
                gameHandler,
                gameScreen,
                soundEffectManager,
                uiManager
            );

            SelectorUI selectorUI = new(
                gameHandler,
                gameScreen,
                soundEffectManager,
                uiManager
            );

            OptionsUI optionsUI = new(
                colorPickerUI,
                cursorManager,
                gameHandler,
                gameScreen,
                keySelectorUI,
                playerInputController,
                selectorUI,
                sliderUI,
                songManager,
                soundEffectManager,
                tooltipBox,
                uiManager,
                videoManager
            );

            MainUI mainUI = new(
                ambientManager,
                gameHandler,
                gameScreen,
                optionsUI,
                songManager,
                soundEffectManager,
                uiManager,
                world
            );

            PauseUI pauseUI = new(
                confirmUI,
                gameHandler,
                gameScreen,
                optionsUI,
                soundEffectManager,
                uiManager
            );

            PenSettingsUI penSettingsUI = new(
                gameHandler,
                gameScreen,
                hudUI,
                playerInputController,
                soundEffectManager,
                tooltipBox,
                uiManager,
                world
            );

            WorldDetailsUI worldDetailsUI = new(
                gameHandler,
                gameScreen,
                soundEffectManager,
                uiManager
            );

            WorldExplorerUI worldExplorerUI = new(
                graphicsDevice,
                soundEffectManager,
                uiManager,
                worldDetailsUI
            );

            PlayUI playUI = new(
                gameScreen,
                soundEffectManager,
                uiManager,
                worldExplorerUI
            );

            TextInputUI textInputUI = new(
                gameHandler,
                gameScreen,
                gameWindow,
                messageUI,
                playerInputController,
                soundEffectManager,
                uiManager
            );

            SaveUI saveSettingsUI = new(
                actorManager,
                gameHandler,
                gameScreen,
                graphicsDevice,
                soundEffectManager,
                textInputUI,
                tooltipBox,
                uiManager,
                world
            );

            TemperatureSettingsUI temperatureSettingsUI = new(
                gameHandler,
                gameScreen,
                soundEffectManager,
                tooltipBox,
                uiManager,
                world
            );

            WorldSettingsUI worldSettingsUI = new(
                actorManager,
                confirmUI,
                gameHandler,
                gameScreen,
                messageUI,
                soundEffectManager,
                tooltipBox,
                uiManager,
                world
            );

            AchievementsUI achievementsUI = new(
                achievementDatabase,
                ambientManager,
                soundEffectManager,
                tooltipBox,
                uiManager
            );

            TutorialUI tutorialUI = new(
                gameScreen,
                uiManager
            );

            uis = [
                achievementsUI,
                colorPickerUI,
                confirmUI,
                creditsUI,
                environmentSettingsUI,
                generatorSettingsUI,
                hudUI,
                informationUI,
                itemExplorerUI,
                itemSearchUI,
                keySelectorUI,
                mainUI,
                messageUI,
                optionsUI,
                pauseUI,
                penSettingsUI,
                playUI,
                saveSettingsUI,
                selectorUI,
                sliderUI,
                temperatureSettingsUI,
                textInputUI,
                tutorialUI,
                worldDetailsUI,
                worldExplorerUI,
                worldSettingsUI,
            ];

            for (int i = 0; i < uis.Length; i++)
            {
                uis[i].Initialize(gameScreen);
            }

            isLoaded = true;
        }

        internal UIBase GetUI(UIIndex index)
        {
            return uis[(int)index];
        }

        internal void ResizeUIs(Vector2 newSize)
        {
            foreach (UIBase ui in uis)
            {
                ui.Resize(newSize);
            }
        }
    }
}
