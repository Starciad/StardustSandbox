using StardustSandbox.Core.Audio;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    internal partial class SGUI_MainMenu
    {
        protected override void OnOpened()
        {
            this.SGameInstance.BackgroundManager.SetBackground(this.SGameInstance.BackgroundDatabase.GetBackgroundById("main_menu"));
            this.SGameInstance.BackgroundManager.Reset();
            this.SGameInstance.BackgroundManager.CloudHandler.IsActive = true;

            ResetElementPositions();

            this.SGameInstance.GameInputController.Disable();

            LoadAnimationValues();
            LoadMainMenuWorld();
            LoadMagicCursor();

            SSongEngine.Play(this.mainMenuSong);
        }

        protected override void OnClosed()
        {
            this.SGameInstance.BackgroundManager.Reset();
            this.SGameInstance.BackgroundManager.CloudHandler.IsActive = false;

            this.SGameInstance.World.Clear();
            this.SGameInstance.World.IsActive = false;
            this.SGameInstance.World.IsVisible = false;

            SSongEngine.Stop();
        }
    }
}
