using StardustSandbox.ContentBundle.GUISystem.Elements.Informational;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Hud;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Menus;
using StardustSandbox.Core.Constants.GUI;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Interfaces.General;

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
            // Build

            SGUI_MainMenu mainMenu = new(game, SGUIConstants.MAIN_MENU_IDENTIFIER, game.GUIManager.GUIEvents);
            SGUI_PlayMenu playMenu = new(game, SGUIConstants.PLAY_MENU_IDENTIFIER, game.GUIManager.GUIEvents);
            SGUI_OptionsMenu optionsMenu = new(game, SGUIConstants.OPTIONS_MENU_IDENTIFIER, game.GUIManager.GUIEvents);
            SGUI_CreditsMenu creditsMenu = new(game, SGUIConstants.CREDITS_MENU_IDENTIFIER, game.GUIManager.GUIEvents);

            SGUI_HUD hud = new(game, SGUIConstants.HUD_IDENTIFIER, game.GUIManager.GUIEvents, tooltipBoxElementElement);
            SGUI_ItemExplorer itemExplorer = new(game, SGUIConstants.HUD_ELEMENT_EXPLORER_IDENTIFIER, game.GUIManager.GUIEvents, hud, tooltipBoxElementElement);

            // =================================== //
            // Register

            guiDatabase.RegisterGUISystem(mainMenu.Identifier, mainMenu);
            guiDatabase.RegisterGUISystem(playMenu.Identifier, playMenu);
            guiDatabase.RegisterGUISystem(optionsMenu.Identifier, optionsMenu);
            guiDatabase.RegisterGUISystem(creditsMenu.Identifier, creditsMenu);

            guiDatabase.RegisterGUISystem(hud.Identifier, hud);
            guiDatabase.RegisterGUISystem(itemExplorer.Identifier, itemExplorer);
        }
    }
}
