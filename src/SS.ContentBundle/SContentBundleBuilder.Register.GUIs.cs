using StardustSandbox.ContentBundle.GUISystem.Elements.Informational;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Hud;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements.EnvironmentSettings;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements.Information;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements.ItemExplorer;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements.Pause;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements.PenSettings;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements.SaveSettings;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements.WorldSettings;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.Credits;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.Main;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.Options;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.Play;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.WorldsExplorer;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.WorldsExplorer.Complements;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Tools;
using StardustSandbox.Core.Constants.GUISystem;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;

namespace StardustSandbox.ContentBundle
{
    public sealed partial class SContentBundleBuilder
    {
        protected override void OnRegisterGUIs(ISGame game, ISGUIDatabase guiDatabase)
        {
            // =================================== //
            // Elements

            SGUITooltipBoxElement tooltipBoxElement = new(game)
            {
                MinimumSize = new(500f, 0f),
            };

            // =================================== //
            // Tools

            SGUI_Message message = new(game, SGUIConstants.MESSAGE_TOOL_IDENTIFIER, game.GUIManager.GUIEvents);
            SGUI_Confirm confirm = new(game, SGUIConstants.CONFIRM_TOOL_IDENTIFIER, game.GUIManager.GUIEvents);
            SGUI_ColorPicker colorPicker = new(game, SGUIConstants.COLOR_PICKER_TOOL_IDENTIFIER, game.GUIManager.GUIEvents, tooltipBoxElement);
            SGUI_TextInput input = new(game, SGUIConstants.INPUT_TOOL_IDENTIFIER, game.GUIManager.GUIEvents, message);

            // =================================== //
            // Build

            SGUI_MainMenu mainMenu = new(game, SGUIConstants.MAIN_MENU_IDENTIFIER, game.GUIManager.GUIEvents);
            SGUI_PlayMenu playMenu = new(game, SGUIConstants.PLAY_MENU_IDENTIFIER, game.GUIManager.GUIEvents);
            SGUI_OptionsMenu optionsMenu = new(game, SGUIConstants.OPTIONS_MENU_IDENTIFIER, game.GUIManager.GUIEvents, colorPicker, message);
            SGUI_CreditsMenu creditsMenu = new(game, SGUIConstants.CREDITS_MENU_IDENTIFIER, game.GUIManager.GUIEvents);

            SGUI_HUD hud = new(game, SGUIConstants.HUD_IDENTIFIER, game.GUIManager.GUIEvents, confirm, tooltipBoxElement);
            SGUI_Pause pause = new(game, SGUIConstants.HUD_PAUSE_IDENTIFIER, game.GUIManager.GUIEvents, confirm);
            SGUI_ItemExplorer itemExplorer = new(game, SGUIConstants.HUD_ITEM_EXPLORER_IDENTIFIER, game.GUIManager.GUIEvents, hud, tooltipBoxElement);
            SGUI_PenSettings penSettings = new(game, SGUIConstants.HUD_PEN_SETTINGS_IDENTIFIER, game.GUIManager.GUIEvents, hud, tooltipBoxElement);
            SGUI_EnvironmentSettings environmentSettings = new(game, SGUIConstants.HUD_ENVIRONMENT_SETTINGS_IDENTIFIER, game.GUIManager.GUIEvents, tooltipBoxElement);
            SGUI_SaveSettings saveSettings = new(game, SGUIConstants.HUD_SAVE_SETTINGS_IDENTIFIER, game.GUIManager.GUIEvents, input, tooltipBoxElement);
            SGUI_WorldSettings worldSettings = new(game, SGUIConstants.HUD_WORLD_SETTINGS_IDENTIFIER, game.GUIManager.GUIEvents, confirm, tooltipBoxElement);
            SGUI_Information information = new(game, SGUIConstants.HUD_INFORMATION_IDENTIFIER, game.GUIManager.GUIEvents);

            SGUI_WorldDetailsMenu detailsMenu = new(game, SGUIConstants.WORLD_DETAILS_IDENTIFIER, game.GUIManager.GUIEvents);
            SGUI_WorldsExplorerMenu worldsExplorer = new(game, SGUIConstants.WORLDS_EXPLORER_IDENTIFIER, game.GUIManager.GUIEvents, detailsMenu);

            // =================================== //
            // Register

            guiDatabase.RegisterGUISystem(message);
            guiDatabase.RegisterGUISystem(confirm);
            guiDatabase.RegisterGUISystem(colorPicker);
            guiDatabase.RegisterGUISystem(input);

            guiDatabase.RegisterGUISystem(mainMenu);
            guiDatabase.RegisterGUISystem(playMenu);
            guiDatabase.RegisterGUISystem(optionsMenu);
            guiDatabase.RegisterGUISystem(creditsMenu);

            guiDatabase.RegisterGUISystem(hud);
            guiDatabase.RegisterGUISystem(pause);
            guiDatabase.RegisterGUISystem(itemExplorer);
            guiDatabase.RegisterGUISystem(penSettings);
            guiDatabase.RegisterGUISystem(environmentSettings);
            guiDatabase.RegisterGUISystem(saveSettings);
            guiDatabase.RegisterGUISystem(worldSettings);
            guiDatabase.RegisterGUISystem(information);

            guiDatabase.RegisterGUISystem(worldsExplorer);
            guiDatabase.RegisterGUISystem(detailsMenu);
        }
    }
}
