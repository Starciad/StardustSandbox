using StardustSandbox.ContentBundle.GUISystem.Hud;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle
{
    public sealed partial class SContentBundleBuilder
    {
        protected override void OnRegisterGUIs(ISGame game, SGUIDatabase guiDatabase)
        {
            SGUI_HUD hud = new(game, game.GUIManager.GUIEvents);
            SGUI_ItemExplorer itemExplorer = new(game, game.GUIManager.GUIEvents, hud);

            guiDatabase.RegisterGUISystem(hud);
            guiDatabase.RegisterGUISystem(itemExplorer);
        }
    }
}
