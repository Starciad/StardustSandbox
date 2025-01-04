using StardustSandbox.ContentBundle.GUISystem.Elements.Informational;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Hud;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Menus;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.Complements;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Specials;
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

            SGUITooltipBoxElement tooltipBoxElementElement = new(game)
            {
                MinimumSize = new(500f, 0f),
            };

            // =================================== //
            // Tools

            SGUI_Input input = new(game, SGUIConstants.INPUT_TOOL_IDENTIFIER, game.GUIManager.GUIEvents);

            // =================================== //
            // Build

            SGUI_MainMenu mainMenu = new(game, SGUIConstants.MAIN_MENU_IDENTIFIER, game.GUIManager.GUIEvents);
            SGUI_PlayMenu playMenu = new(game, SGUIConstants.PLAY_MENU_IDENTIFIER, game.GUIManager.GUIEvents);
            SGUI_OptionsMenu optionsMenu = new(game, SGUIConstants.OPTIONS_MENU_IDENTIFIER, game.GUIManager.GUIEvents);
            SGUI_CreditsMenu creditsMenu = new(game, SGUIConstants.CREDITS_MENU_IDENTIFIER, game.GUIManager.GUIEvents);

            SGUI_HUD hud = new(game, SGUIConstants.HUD_IDENTIFIER, game.GUIManager.GUIEvents, tooltipBoxElementElement);
            SGUI_ItemExplorer itemExplorer = new(game, SGUIConstants.HUD_ITEM_EXPLORER_IDENTIFIER, game.GUIManager.GUIEvents, hud, tooltipBoxElementElement);
            SGUI_PenSettings penSettings = new(game, SGUIConstants.HUD_PEN_SETTINGS_IDENTIFIER, game.GUIManager.GUIEvents, hud, tooltipBoxElementElement);
            SGUI_EnvironmentSettings environmentSettings = new(game, SGUIConstants.HUD_ENVIRONMENT_SETTINGS_IDENTIFIER, game.GUIManager.GUIEvents);
            SGUI_SaveSettings saveSettings = new(game, SGUIConstants.HUD_SAVE_SETTINGS_IDENTIFIER, game.GUIManager.GUIEvents, input, tooltipBoxElementElement);
            SGUI_WorldSettings worldSettings = new(game, SGUIConstants.HUD_WORLD_SETTINGS_IDENTIFIER, game.GUIManager.GUIEvents);
            SGUI_Information information = new(game, SGUIConstants.HUD_INFORMATION_IDENTIFIER, game.GUIManager.GUIEvents);

            SGUI_WorldDetailsMenu detailsMenu = new(game, SGUIConstants.WORLD_DETAILS_IDENTIFIER, game.GUIManager.GUIEvents);
            SGUI_WorldsExplorerMenu worldsExplorer = new(game, SGUIConstants.WORLDS_EXPLORER_IDENTIFIER, game.GUIManager.GUIEvents, detailsMenu);

            // =================================== //
            // Register

            guiDatabase.RegisterGUISystem(input.Identifier, input);

            guiDatabase.RegisterGUISystem(mainMenu.Identifier, mainMenu);
            guiDatabase.RegisterGUISystem(playMenu.Identifier, playMenu);
            guiDatabase.RegisterGUISystem(optionsMenu.Identifier, optionsMenu);
            guiDatabase.RegisterGUISystem(creditsMenu.Identifier, creditsMenu);

            guiDatabase.RegisterGUISystem(hud.Identifier, hud);
            guiDatabase.RegisterGUISystem(itemExplorer.Identifier, itemExplorer);
            guiDatabase.RegisterGUISystem(penSettings.Identifier, penSettings);
            guiDatabase.RegisterGUISystem(environmentSettings.Identifier, environmentSettings);
            guiDatabase.RegisterGUISystem(saveSettings.Identifier, saveSettings);
            guiDatabase.RegisterGUISystem(worldSettings.Identifier, worldSettings);
            guiDatabase.RegisterGUISystem(information.Identifier, information);

            guiDatabase.RegisterGUISystem(worldsExplorer.Identifier, worldsExplorer);
            guiDatabase.RegisterGUISystem(detailsMenu.Identifier, detailsMenu);
        }
    }
}
