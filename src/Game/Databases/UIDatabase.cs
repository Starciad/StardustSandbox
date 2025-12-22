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
            AmbientManager ambientManager,
            CursorManager cursorManager,
            GameManager gameManager,
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
                gameManager,
                UIIndex.ColorPicker,
                tooltipBox,
                uiManager
            );

            ConfirmUI confirmUI = new(
                gameManager,
                UIIndex.Confirm,
                uiManager
            );

            MessageUI messageUI = new(
                gameManager,
                UIIndex.Message,
                uiManager
            );

            TextInputUI textInputUI = new(
                gameManager,
                gameWindow,
                UIIndex.TextInput,
                inputController,
                messageUI,
                uiManager
            );

            SliderUI sliderUI = new(
                gameManager,
                UIIndex.Slider,
                uiManager
            );

            KeySelectorUI keySelectorUI = new(
                gameManager,
                gameWindow,
                UIIndex.KeySelector,
                inputController,
                uiManager
            );

            #endregion

            #region UIs

            CreditsUI creditsUI = new(
                ambientManager,
                UIIndex.CreditsMenu,
                uiManager,
                world
            );

            EnvironmentSettingsUI environmentSettingsUI = new(
                gameManager,
                UIIndex.EnvironmentSettings,
                tooltipBox,
                uiManager,
                world
            );

            HudUI hudUI = new(
                gameManager,
                inputController,
                confirmUI,
                UIIndex.Hud,
                tooltipBox,
                uiManager,
                world
            );

            InformationUI informationUI = new(
                gameManager,
                UIIndex.Information,
                uiManager,
                world
            );

            TemperatureSettingsUI temperatureSettingsUI = new(
                gameManager,
                UIIndex.TemperatureSettings,
                tooltipBox,
                uiManager,
                world
            );

            ItemExplorerUI itemExplorerUI = new(
                gameManager,
                UIIndex.ItemExplorer,
                hudUI,
                tooltipBox,
                uiManager
            );

            MainUI mainUI = new(
                ambientManager,
                inputController,
                gameManager,
                UIIndex.MainMenu,
                uiManager,
                world
            );

            OptionsUI optionsUI = new(
                colorPickerUI,
                cursorManager,
                gameManager,
                UIIndex.OptionsMenu,
                keySelectorUI,
                messageUI,
                sliderUI,
                tooltipBox,
                uiManager,
                videoManager
            );

            PauseUI pauseUI = new(
                confirmUI,
                gameManager,
                UIIndex.Pause,
                uiManager
            );

            PenSettingsUI penSettingsUI = new(
                gameManager,
                UIIndex.PenSettings,
                inputController,
                hudUI,
                tooltipBox,
                uiManager,
                world
            );

            PlayUI playUI = new(
                UIIndex.PlayMenu,
                uiManager
            );

            SaveUI saveSettingsUI = new(
                gameManager,
                graphicsDevice,
                UIIndex.Save,
                textInputUI,
                tooltipBox,
                uiManager,
                world
            );

            WorldDetailsUI worldDetailsUI = new(
                gameManager,
                UIIndex.WorldDetailsMenu,
                uiManager,
                world
            );

            WorldExplorerUI worldsExplorerUI = new(
                graphicsDevice,
                UIIndex.WorldExplorerMenu,
                uiManager,
                worldDetailsUI
            );

            WorldSettingsUI worldSettingsUI = new(
                confirmUI,
                gameManager,
                UIIndex.WorldSettings,
                messageUI,
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
