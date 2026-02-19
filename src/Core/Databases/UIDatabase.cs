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
    internal static class UIDatabase
    {
        private static UIBase[] uis;
        private static bool isLoaded;

        internal static void Load(
            ActorManager actorManager,
            AmbientManager ambientManager,
            CursorManager cursorManager,
            GameWindow gameWindow,
            GraphicsDevice graphicsDevice,
            PlayerInputController playerInputController,
            StardustSandboxGame stardustSandboxGame,
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
            TooltipBox tooltipBox = new(cursorManager)
            {
                MinimumSize = new(500f, 0f),
            };

            ColorPickerUI colorPickerUI = new(
                tooltipBox,
                uiManager
            );

            ConfirmUI confirmUI = new(
                uiManager
            );

            CreditsUI creditsUI = new(
                ambientManager,
                uiManager,
                world
            );

            EnvironmentSettingsUI environmentSettingsUI = new(
                tooltipBox,
                uiManager,
                world
            );

            GeneratorSettingsUI generatorSettingsUI = new(
                actorManager,
                confirmUI,
                tooltipBox,
                uiManager,
                world
            );

            HudUI hudUI = new(
                actorManager,
                confirmUI,
                notificationBox,
                playerInputController,
                tooltipBox,
                uiManager,
                world
            );

            InformationUI informationUI = new(
                actorManager,
                tooltipBox,
                uiManager,
                world
            );

            ItemExplorerUI itemExplorerUI = new(
                hudUI,
                tooltipBox,
                uiManager
            );

            ItemSearchUI itemSearchUI = new(
                gameWindow,
                hudUI,
                playerInputController,
                tooltipBox,
                uiManager
            );

            KeySelectorUI keySelectorUI = new(
                gameWindow,
                playerInputController,
                uiManager
            );

            MainUI mainUI = new(
                actorManager,
                ambientManager,
                playerInputController,
                stardustSandboxGame,
                uiManager,
                world
            );

            MessageUI messageUI = new(
                uiManager
            );

            SliderUI sliderUI = new(
                uiManager
            );

            OptionsUI optionsUI = new(
                colorPickerUI,
                cursorManager,
                keySelectorUI,
                messageUI,
                sliderUI,
                tooltipBox,
                uiManager,
                videoManager
            );

            PauseUI pauseUI = new(
                confirmUI,
                uiManager
            );

            PenSettingsUI penSettingsUI = new(
                hudUI,
                playerInputController,
                tooltipBox,
                uiManager,
                world
            );

            PlayUI playUI = new(
                uiManager
            );

            TextInputUI textInputUI = new(
                gameWindow,
                messageUI,
                playerInputController,
                uiManager
            );

            SaveUI saveSettingsUI = new(
                actorManager,
                graphicsDevice,
                textInputUI,
                tooltipBox,
                uiManager,
                world
            );

            TemperatureSettingsUI temperatureSettingsUI = new(
                tooltipBox,
                uiManager,
                world
            );

            WorldDetailsUI worldDetailsUI = new(
                actorManager,
                ambientManager,
                playerInputController,
                uiManager,
                world
            );

            WorldExplorerUI worldExplorerUI = new(
                graphicsDevice,
                uiManager,
                worldDetailsUI
            );

            WorldSettingsUI worldSettingsUI = new(
                actorManager,
                confirmUI,
                messageUI,
                tooltipBox,
                uiManager,
                world
            );

            AchievementsUI achievementsUI = new(
                ambientManager,
                tooltipBox,
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
                sliderUI,
                temperatureSettingsUI,
                textInputUI,
                worldDetailsUI,
                worldExplorerUI,
                worldSettingsUI,
            ];

            for (int i = 0; i < uis.Length; i++)
            {
                uis[i].Initialize();
            }

            isLoaded = true;
        }

        internal static UIBase GetUI(UIIndex index)
        {
            return uis[(int)index];
        }

        internal static void ResizeUIs(Vector2 newSize)
        {
            foreach (UIBase ui in uis)
            {
                ui.Resize(newSize);
            }
        }
    }
}
