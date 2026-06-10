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
using StardustSandbox.Core.UI.Elements.Specials;
using StardustSandbox.Core.WorldSystem;

using System;

namespace StardustSandbox.Core.Databases
{
    internal sealed class UIDatabase
    {
        private IUI[] uis;

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
            ProgressSerializer progressSerializer,
            SettingsSerializer settingsSerializer,
            SongManager songManager,
            SoundEffectManager soundEffectManager,
            UIManager uiManager,
            VideoManager videoManager,
            World world,
            WorldSerializer worldSerializer
        )
        {
            AchievementProgress achievementProgress = progressSerializer.Load<AchievementProgress>();

            ControlSettings controlSettings = settingsSerializer.Load<ControlSettings>();
            InterfaceSettings interfaceSettings = settingsSerializer.Load<InterfaceSettings>();

            NotificationBox notificationBox = new(assetDatabase, gameScreen);
            TooltipBox tooltipBox = new(assetDatabase, cursorManager, gameScreen, interfaceSettings)
            {
                MinimumSize = new(500f, 0f),
            };

            ColorPickerUI colorPickerUI = new(
                assetDatabase,
                gameHandler,
                gameScreen,
                soundEffectManager,
                tooltipBox,
                uiManager
            );

            ConfirmUI confirmUI = new(
                assetDatabase,
                gameHandler,
                gameScreen,
                soundEffectManager,
                uiManager
            );

            CreditsUI creditsUI = new(
                ambientManager,
                assetDatabase,
                gameScreen,
                songManager,
                uiManager,
                world
            );

            EnvironmentSettingsUI environmentSettingsUI = new(
                assetDatabase,
                gameHandler,
                gameScreen,
                soundEffectManager,
                tooltipBox,
                uiManager,
                world
            );

            GeneratorSettingsUI generatorSettingsUI = new(
                assetDatabase,
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
                assetDatabase,
                catalogDatabase,
                confirmUI,
                gameHandler,
                gameScreen,
                notificationBox,
                playerInputController,
                soundEffectManager,
                tooltipBox,
                uiManager
            );

            InformationUI informationUI = new(
                actorManager,
                assetDatabase,
                gameHandler,
                gameScreen,
                soundEffectManager,
                tooltipBox,
                uiManager,
                world
            );

            ItemSearchUI itemSearchUI = new(
                assetDatabase,
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
                assetDatabase,
                gameHandler,
                gameScreen,
                gameWindow,
                playerInputController,
                soundEffectManager,
                uiManager
            );

            MessageUI messageUI = new(
                assetDatabase,
                gameHandler,
                gameScreen,
                uiManager
            );

            SliderUI sliderUI = new(
                assetDatabase,
                gameHandler,
                gameScreen,
                soundEffectManager,
                uiManager
            );

            SelectorUI selectorUI = new(
                assetDatabase,
                gameHandler,
                gameScreen,
                soundEffectManager,
                uiManager
            );

            OptionsUI optionsUI = new(
                assetDatabase,
                colorPickerUI,
                cursorManager,
                gameHandler,
                gameScreen,
                keySelectorUI,
                playerInputController,
                selectorUI,
                settingsSerializer,
                sliderUI,
                songManager,
                soundEffectManager,
                tooltipBox,
                uiManager,
                videoManager
            );

            MainUI mainUI = new(
                ambientManager,
                assetDatabase,
                gameHandler,
                gameScreen,
                optionsUI,
                songManager,
                soundEffectManager,
                uiManager,
                world
            );

            PauseUI pauseUI = new(
                assetDatabase,
                confirmUI,
                gameHandler,
                gameScreen,
                optionsUI,
                soundEffectManager,
                uiManager
            );

            PenSettingsUI penSettingsUI = new(
                assetDatabase,
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
                assetDatabase,
                gameHandler,
                gameScreen,
                soundEffectManager,
                uiManager,
                worldSerializer
            );

            WorldExplorerUI worldExplorerUI = new(
                assetDatabase,
                gameScreen,
                graphicsDevice,
                soundEffectManager,
                uiManager,
                worldDetailsUI,
                worldSerializer
            );

            PlayUI playUI = new(
                assetDatabase,
                gameScreen,
                soundEffectManager,
                uiManager,
                worldExplorerUI
            );

            TextInputUI textInputUI = new(
                assetDatabase,
                gameHandler,
                gameScreen,
                gameWindow,
                messageUI,
                playerInputController,
                soundEffectManager,
                uiManager
            );

            SaveUI saveSettingsUI = new(
                assetDatabase,
                gameHandler,
                gameScreen,
                graphicsDevice,
                soundEffectManager,
                textInputUI,
                tooltipBox,
                uiManager,
                world,
                worldSerializer
            );

            TemperatureSettingsUI temperatureSettingsUI = new(
                assetDatabase,
                gameHandler,
                gameScreen,
                soundEffectManager,
                tooltipBox,
                uiManager,
                world
            );

            WorldSettingsUI worldSettingsUI = new(
                actorManager,
                assetDatabase,
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
                achievementProgress,
                assetDatabase,
                ambientManager,
                gameScreen,
                soundEffectManager,
                tooltipBox,
                uiManager
            );

            TutorialUI tutorialUI = new(
                assetDatabase,
                controlSettings,
                gameScreen,
                uiManager
            );

            this.uis = [
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

            for (int i = 0; i < this.uis.Length; i++)
            {
                this.uis[i].Initialize();
            }
        }

        internal UIBase GetUI(UIIndex index)
        {
            return index is UIIndex.None ? null : this.uis[((byte)index) - 1];
        }

        internal void ResizeUIs()
        {
            Array.ForEach(this.uis, x => x.Resize());
        }
    }
}
