using StardustSandbox.Core.Audio;
using StardustSandbox.Core.GUISystem.Elements;

namespace StardustSandbox.Core.GUISystem.GUIs.Menus.Credits
{
    internal sealed partial class SGUI_CreditsMenu
    {
        protected override void OnOpened()
        {
            this.SGameInstance.AmbientManager.BackgroundHandler.SetBackground(this.SGameInstance.BackgroundDatabase.GetBackgroundById("credits"));
            this.SGameInstance.AmbientManager.CloudHandler.IsActive = false;
            this.SGameInstance.AmbientManager.CelestialBodyHandler.IsActive = false;
            this.SGameInstance.AmbientManager.SkyHandler.IsActive = false;

            this.world.IsActive = false;
            this.world.IsVisible = false;

            SSongEngine.Play(this.creditsMenuSong);

            foreach (SGUIElement element in this.creditElements)
            {
                element.PositionRelativeToScreen();
            }
        }
    }
}
