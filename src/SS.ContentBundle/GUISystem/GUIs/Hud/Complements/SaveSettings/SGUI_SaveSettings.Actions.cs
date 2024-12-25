using StardustSandbox.Core.Managers.IO;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements
{
    internal sealed partial class SGUI_SaveSettings
    {
        // Menu
        private void ExitButtonAction()
        {
            this.SGameInstance.GUIManager.CloseGUI();
        }

        // Fields
        private void NameFieldButtonAction()
        {
            this.guiInput.Configure(this.nameInputBuilder);
            this.SGameInstance.GUIManager.OpenGUI(this.guiInput.Identifier);
        }

        private void DescriptionFieldButtonAction()
        {
            this.guiInput.Configure(this.descriptionInputBuilder);
            this.SGameInstance.GUIManager.OpenGUI(this.guiInput.Identifier);
        }

        // Footer
        private void SaveButtonAction()
        {
            SWorldSavingManager.Serialize(this.world, this.SGameInstance.GraphicsManager.GraphicsDevice);
        }
    }
}
