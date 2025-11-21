using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Enums.UISystem;
using StardustSandbox.InputSystem.GameInput;
using StardustSandbox.Managers;
using StardustSandbox.UISystem;
using StardustSandbox.UISystem.Elements;
using StardustSandbox.UISystem.UIs.HUD;
using StardustSandbox.UISystem.UIs.Menus;
using StardustSandbox.UISystem.UIs.Tools;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.Databases
{
    internal static class UIDatabase
    {
        private static UI[] uis;

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
            #region Elements

            TooltipBoxUIElement tooltipBoxElement = new(cursorManager, inputManager)
            {
                MinimumSize = new(500f, 0f),
            };

            #endregion

            #region Tools

            ColorPickerUI colorPickerUI = new(
                gameManager,
                UIIndex.ColorPicker,
                inputController,
                tooltipBoxElement,
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
                tooltipBoxElement,
                uiManager,
                world
            );

            HudUI hudUI = new(
                gameManager,
                inputController,
                confirmUI,
                UIIndex.Hud,
                tooltipBoxElement,
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
                tooltipBoxElement,
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
                tooltipBoxElement,
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
                tooltipBoxElement,
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
                tooltipBoxElement,
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
                tooltipBoxElement,
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

            for (int i = 0; i < uis.Length; i++)
            {
                uis[i].Initialize();
            }

            #endregion
        }

        internal static UI GetUIByIndex(UIIndex index)
        {
            return uis[(int)index];
        }
    }
}
