using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.GUI;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    public sealed partial class SGUI_MainMenu
    {
        private void CreateMenuButton()
        {
            this.SGameInstance.GUIManager.CloseGUI(this.Identifier);
            this.SGameInstance.GUIManager.ShowGUI(SGUIConstants.HUD_IDENTIFIER);

            this.SGameInstance.BackgroundManager.SetBackground(this.SGameInstance.BackgroundDatabase.GetBackgroundById("ocean_1"));

            this.world.StartNew(SWorldConstants.WORLD_SIZES_TEMPLATE[2]);

            this.SGameInstance.CameraManager.Position = new(0f, -(this.world.Infos.Size.Height * SWorldConstants.GRID_SCALE));
            this.SGameInstance.GameInputController.Activate();
        }

        private void OptionsMenuButton()
        {
            this.SGameInstance.GUIManager.CloseGUI(this.Identifier);
            this.SGameInstance.GUIManager.ShowGUI(SGUIConstants.OPTIONS_MENU_IDENTIFIER);
        }

        private void CreditsMenuButton()
        {
            this.SGameInstance.GUIManager.CloseGUI(this.Identifier);
            this.SGameInstance.GUIManager.ShowGUI(SGUIConstants.CREDITS_MENU_IDENTIFIER);
        }

        private void QuitMenuButton()
        {
            this.SGameInstance.Quit();
        }
    }
}
