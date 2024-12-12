using StardustSandbox.Core.Audio;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    public partial class SGUI_MainMenu
    {
        protected override void OnOpened()
        {
            this.SGameInstance.BackgroundManager.SetBackground(this.SGameInstance.BackgroundDatabase.GetBackgroundById("main_menu"));

            ResetElementPositions();

            this.SGameInstance.BackgroundManager.EnableClouds();
            this.SGameInstance.GameInputController.Disable();

            LoadAnimationValues();
            LoadMainMenuWorld();
            LoadMagicCursor();

            SSongEngine.Play(this.mainMenuSong);
        }

        protected override void OnClosed()
        {
            this.SGameInstance.BackgroundManager.DisableClouds();
            this.SGameInstance.EntityManager.RemoveAll();

            SSongEngine.Stop();
        }
    }
}
