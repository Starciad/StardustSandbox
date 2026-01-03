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
                UIIndex.ColorPicker,
                tooltipBox,
                uiManager
            );

            ConfirmUI confirmUI = new(
                UIIndex.Confirm,
                uiManager
            );

            MessageUI messageUI = new(
                UIIndex.Message,
                uiManager
            );

            TextInputUI textInputUI = new(
                gameWindow,
                UIIndex.TextInput,
                inputController,
                messageUI,
                uiManager
            );

            SliderUI sliderUI = new(
                UIIndex.Slider,
                uiManager
            );

            KeySelectorUI keySelectorUI = new(
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
                UIIndex.EnvironmentSettings,
                tooltipBox,
                uiManager,
                world
            );

            HudUI hudUI = new(
                actorManager,
                inputController,
                confirmUI,
                UIIndex.Hud,
                tooltipBox,
                uiManager,
                world
            );

            InformationUI informationUI = new(
                actorManager,
                UIIndex.Information,
                tooltipBox,
                uiManager,
                world
            );

            TemperatureSettingsUI temperatureSettingsUI = new(
                UIIndex.TemperatureSettings,
                tooltipBox,
                uiManager,
                world
            );

            ItemExplorerUI itemExplorerUI = new(
                UIIndex.ItemExplorer,
                hudUI,
                tooltipBox,
                uiManager
            );

            MainUI mainUI = new(
                actorManager,
                ambientManager,
                inputController,
                UIIndex.MainMenu,
                uiManager,
                world
            );

            OptionsUI optionsUI = new(
                colorPickerUI,
                cursorManager,
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
                UIIndex.Pause,
                uiManager
            );

            PenSettingsUI penSettingsUI = new(
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
                actorManager,
                graphicsDevice,
                UIIndex.Save,
                textInputUI,
                tooltipBox,
                uiManager,
                world
            );

            WorldDetailsUI worldDetailsUI = new(
                actorManager,
                ambientManager,
                UIIndex.WorldDetailsMenu,
                inputController,
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
                actorManager,
                confirmUI,
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
