using StardustSandbox.ContentBundle.GUISystem.Experimental;
using StardustSandbox.ContentBundle.GUISystem.Hud;
using StardustSandbox.ContentBundle.GUISystem.Menus;
using StardustSandbox.Core.Constants.GUI;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle
{
    public sealed partial class SContentBundleBuilder
    {
        protected override void OnRegisterGUIs(ISGame game, SGUIDatabase guiDatabase)
        {
            SGUI_PivotDebug textDebug = new(game, SGUIConstants.MAIN_MENU_IDENTIFIER, game.GUIManager.GUIEvents);

            SGUI_MainMenu mainMenu = new(game, SGUIConstants.MAIN_MENU_IDENTIFIER, game.GUIManager.GUIEvents);
            SGUI_HUD hud = new(game, SGUIConstants.HUD_IDENTIFIER, game.GUIManager.GUIEvents);
            SGUI_ItemExplorer itemExplorer = new(game, SGUIConstants.HUD_ELEMENT_EXPLORER_IDENTIFIER, game.GUIManager.GUIEvents, hud);

            // guiDatabase.RegisterGUISystem(SGUIConstants.MAIN_MENU_IDENTIFIER, textDebug);
            guiDatabase.RegisterGUISystem(SGUIConstants.MAIN_MENU_IDENTIFIER, mainMenu);
            guiDatabase.RegisterGUISystem(SGUIConstants.HUD_IDENTIFIER, hud);
            guiDatabase.RegisterGUISystem(SGUIConstants.HUD_ELEMENT_EXPLORER_IDENTIFIER, itemExplorer);
        }
    }
}
