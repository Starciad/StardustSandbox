using StardustSandbox.ContentBundle.Enums.GUISystem;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Tools.InputSystem.Settings;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Specials
{
    internal sealed partial class SGUI_Input
    {
        private void CancelButtonAction()
        {
            this.inputSettings?.OnCancelCallback?.Invoke();
            this.SGameInstance.GUIManager.CloseGUI();
        }

        private void SendButtonAction()
        {
            if (this.inputSettings != null)
            {
                SValidationState validationState = new();
                SArgumentResult argumentResult = new(this.userInputStringBuilder.ToString());

                this.inputSettings.OnValidationCallback?.Invoke(validationState, argumentResult);

                if (validationState.Status == SValidationStatus.Failure)
                {
                    goto FINALIZATION_LABEL;
                }

                this.inputSettings.OnSendCallback?.Invoke(argumentResult);
            }

        FINALIZATION_LABEL:;
            this.SGameInstance.GUIManager.CloseGUI();
        }
    }
}
