using StardustSandbox.ContentBundle.GUISystem.Hud;
using StardustSandbox.Core.Constants.GUI;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle
{
    public sealed partial class SContentBundleBuilder
    {
        protected override void OnRegisterGUIs(ISGame game, SGUIDatabase guiDatabase)
        {
            SGUI_HUD hud = new(game, SGUIConstants.HUD_IDENTIFIER, game.GUIManager.GUIEvents);
            SGUI_ItemExplorer itemExplorer = new(game, SGUIConstants.HUD_ELEMENT_EXPLORER_IDENTIFIER, game.GUIManager.GUIEvents, hud);

            guiDatabase.RegisterGUISystem(SGUIConstants.HUD_IDENTIFIER, hud);
            guiDatabase.RegisterGUISystem(SGUIConstants.HUD_ELEMENT_EXPLORER_IDENTIFIER, itemExplorer);
        }
    }
}
