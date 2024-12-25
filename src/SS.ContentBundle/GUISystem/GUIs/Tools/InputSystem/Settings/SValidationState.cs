using StardustSandbox.ContentBundle.Enums.GUISystem;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Tools.InputSystem.Settings
{
    internal sealed class SValidationState
    {
        internal SValidationStatus Status { get; set;  }
        internal string Message { get; set; }

        internal SValidationState()
        {
            this.Status = SValidationStatus.Success;
            this.Message = string.Empty;
        }
    }
}
