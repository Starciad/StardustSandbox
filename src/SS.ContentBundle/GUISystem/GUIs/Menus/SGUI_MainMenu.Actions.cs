using StardustSandbox.Core.Audio;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.GUI;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    public sealed partial class SGUI_MainMenu
    {
        private void CreateMenuButton()
        {
            SSongEngine.Stop();

            this.SGameInstance.GUIManager.CloseGUI(this.Identifier);
            this.SGameInstance.GUIManager.ShowGUI(SGUIConstants.HUD_IDENTIFIER);

            this.world.Reset();

            this.SGameInstance.CameraManager.Position = new(0f, -(this.world.Infos.Size.Height * SWorldConstants.GRID_SCALE));
            this.SGameInstance.GameInputManager.CanModifyEnvironment = true;
        }

        private static void PlayMenuButton()
        {
            return;
        }

        private static void OptionsMenuButton()
        {
            return;
        }

        private static void CreditsMenuButton()
        {
            return;
        }

        private void QuitMenuButton()
        {
            this.SGameInstance.Quit();
        }
    }
}
