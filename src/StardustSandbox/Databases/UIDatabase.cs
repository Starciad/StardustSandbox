using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Enums.UI;
using StardustSandbox.InputSystem.GameInput;
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
            InputManager inputManager,
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

            TooltipBox tooltipBox = new(cursorManager, inputManager)
            {
                MinimumSize = new(500f, 0f),
            };

            #endregion

            #region Tools

            ColorPickerUI colorPickerUI = new(
                gameManager,
                UIIndex.ColorPicker,
                inputController,
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
                gameManager,
                gameWindow,
                UIIndex.TextInput,
                inputController,
                messageUI,
                uiManager
            );

            #endregion

            #region UIs

            CreditsMenuUI creditsMenuUI = new(
                ambientManager,
                UIIndex.CreditsMenu,
                inputManager,
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

            ItemExplorerUI itemExplorerUI = new(
                gameManager,
                UIIndex.ItemExplorer,
                hudUI,
                tooltipBox,
                uiManager
            );

            MainMenuUI mainMenuUI = new(
                ambientManager,
                inputController,
                gameManager,
                UIIndex.MainMenu,
                uiManager,
                world
            );

            OptionsMenuUI optionsMenuUI = new(
                colorPickerUI,
                cursorManager,
                UIIndex.OptionsMenu,
                messageUI,
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
                uiManager
            );

            PlayMenuUI playMenuUI = new(
                UIIndex.PlayMenu,
                uiManager
            );

            SaveSettingsUI saveSettingsUI = new(
                gameManager,
                graphicsDevice,
                UIIndex.SaveSettings,
                textInputUI,
                tooltipBox,
                uiManager,
                world
            );

            WorldDetailsMenuUI worldDetailsMenuUI = new(
                gameManager,
                UIIndex.WorldDetailsMenu,
                uiManager,
                world
            );

            WorldExplorerMenuUI worldsExplorerMenuUI = new(
                graphicsDevice,
                UIIndex.WorldExplorerMenu,
                uiManager,
                worldDetailsMenuUI
            );

            WorldSettingsUI worldSettingsUI = new(
                confirmUI,
                gameManager,
                UIIndex.WorldSettings,
                tooltipBox,
                uiManager,
                world
            );

            uis = [
                messageUI,
                confirmUI,
                colorPickerUI,
                textInputUI,

                mainMenuUI,
                playMenuUI,
                optionsMenuUI,
                creditsMenuUI,

                hudUI,
                pauseUI,
                itemExplorerUI,
                penSettingsUI,
                environmentSettingsUI,
                saveSettingsUI,
                worldSettingsUI,
                informationUI,

                worldDetailsMenuUI,
                worldsExplorerMenuUI,
            ];

            // DirectoryInfo dic = Directory.CreateDirectory(Path.Combine(SSDirectory.Local, "UIs"));

            for (int i = 0; i < uis.Length; i++)
            {
                uis[i].Initialize();
                // File.WriteAllText(Path.Combine(dic.FullName, string.Concat(uis[i].GetType().FullName, ".txt")), uis[i].ToString());
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
