using StardustSandbox.Core.Audio;
using StardustSandbox.Core.GUISystem.Elements;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    public sealed partial class SGUI_CreditsMenu
    {
        protected override void OnOpened()
        {
            this.SGameInstance.BackgroundManager.SetBackground(this.SGameInstance.BackgroundDatabase.GetBackgroundById("credits"));

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
