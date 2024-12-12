namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    public sealed partial class SGUI_OptionsMenu
    {
        protected override void OnOpened()
        {
            SelectSection(0);

            // Load Settings
            LoadVideoSettings();
            LoadLanguageSettings();
        }
    }
}
