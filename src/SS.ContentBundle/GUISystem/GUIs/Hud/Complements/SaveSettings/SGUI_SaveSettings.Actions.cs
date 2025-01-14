using StardustSandbox.Core.IO.Handlers;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements.SaveSettings
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
            this.nameInputBuilder.Content = this.world.Infos.Name;

            this.guiInput.Configure(this.nameInputBuilder);
            this.SGameInstance.GUIManager.OpenGUI(this.guiInput.Identifier);
        }

        private void DescriptionFieldButtonAction()
        {
            this.descriptionInputBuilder.Content = this.world.Infos.Description;

            this.guiInput.Configure(this.descriptionInputBuilder);
            this.SGameInstance.GUIManager.OpenGUI(this.guiInput.Identifier);
        }

        // Footer
        private void SaveButtonAction()
        {
            SWorldSavingHandler.Serialize(this.world, this.SGameInstance.GraphicsManager.GraphicsDevice);
            this.SGameInstance.GUIManager.CloseGUI();
        }
    }
}
