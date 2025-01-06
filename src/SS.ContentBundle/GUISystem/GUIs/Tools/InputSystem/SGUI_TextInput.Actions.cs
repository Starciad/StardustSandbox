using StardustSandbox.ContentBundle.Enums.GUISystem.Tools.InputSystem;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Tools.InputSystem.Settings;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Specials
{
    internal sealed partial class SGUI_TextInput
    {
        private void CancelButtonAction()
        {
            this.inputSettings?.OnCancelCallback?.Invoke();
            this.SGameInstance.GUIManager.CloseGUI();
        }

        private void SendButtonAction()
        {
            this.inputSettings?.OnSendCallback?.Invoke(new(this.userInputStringBuilder.ToString()));
            if (this.inputSettings != null)
            {
                STextValidationState validationState = new();
                STextArgumentResult argumentResult = new(this.userInputStringBuilder.ToString());

                this.inputSettings.OnValidationCallback?.Invoke(validationState, argumentResult);

                if (validationState.Status == SValidationStatus.Failure)
                {
                    this.guiMessage.SetMessage(validationState.Message);
                    this.SGameInstance.GUIManager.OpenGUI(this.guiMessage.Identifier);
                    return;
                }

                this.inputSettings.OnSendCallback?.Invoke(argumentResult);
            }

            this.SGameInstance.GUIManager.CloseGUI();
        }
    }
}
