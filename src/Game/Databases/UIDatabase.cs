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

using StardustSandbox.Enums.UI;
using StardustSandbox.InputSystem.Game;
using StardustSandbox.Managers;
using StardustSandbox.UI;
using StardustSandbox.UI.Common.HUD;
using StardustSandbox.UI.Common.Menus;
using StardustSandbox.UI.Common.Tools;
using StardustSandbox.UI.Elements;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox.Databases
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
            InputController inputController,
            UIManager uiManager,
            VideoManager videoManager,
            World world
        )
        {
            if (isLoaded)
            {
                throw new InvalidOperationException($"{nameof(UIDatabase)} has already been loaded.");
            }

            #region Elements

            TooltipBox tooltipBox = new(cursorManager)
            {
                MinimumSize = new(500f, 0f),
            };

            #endregion

            #region Tools

            ColorPickerUI colorPickerUI = new(
                tooltipBox,
                uiManager
            );

            ConfirmUI confirmUI = new(
                uiManager
            );

            MessageUI messageUI = new(
                uiManager
            );

            TextInputUI textInputUI = new(
                gameWindow,
                inputController,
                messageUI,
                uiManager
            );

            SliderUI sliderUI = new(
                uiManager
            );

            KeySelectorUI keySelectorUI = new(
                gameWindow,
                inputController,
                uiManager
            );

            #endregion

            #region UIs

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

            HudUI hudUI = new(
                actorManager,
                inputController,
                confirmUI,
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

            TemperatureSettingsUI temperatureSettingsUI = new(
                tooltipBox,
                uiManager,
                world
            );

            ItemExplorerUI itemExplorerUI = new(
                hudUI,
                tooltipBox,
                uiManager
            );

            MainUI mainUI = new(
                actorManager,
                ambientManager,
                inputController,
                uiManager,
                world
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
                inputController,
                hudUI,
                tooltipBox,
                uiManager,
                world
            );

            PlayUI playUI = new(
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

            WorldDetailsUI worldDetailsUI = new(
                actorManager,
                ambientManager,
                inputController,
                uiManager,
                world
            );

            WorldExplorerUI worldsExplorerUI = new(
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

            GeneratorSettingsUI generatorSettingsUI = new(
                tooltipBox,
                uiManager,
                world
            );

            uis = [
                messageUI,
                confirmUI,
                colorPickerUI,
                textInputUI,
                sliderUI,
                keySelectorUI,

                mainUI,
                playUI,
                optionsUI,
                creditsUI,

                hudUI,
                pauseUI,
                itemExplorerUI,
                penSettingsUI,
                environmentSettingsUI,
                saveSettingsUI,
                worldSettingsUI,
                informationUI,
                temperatureSettingsUI,
                generatorSettingsUI,

                worldDetailsUI,
                worldsExplorerUI,
            ];

            for (int i = 0; i < uis.Length; i++)
            {
                uis[i].Initialize();
            }

            #endregion

            isLoaded = true;
        }

        internal static UIBase GetUI(UIIndex index)
        {
            return uis[(int)index];
        }
    }
}

