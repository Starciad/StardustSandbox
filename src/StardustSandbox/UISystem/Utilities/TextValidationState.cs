using StardustSandbox.Enums.UISystem.Tools;

namespace StardustSandbox.UISystem.Utilities
{
    internal sealed class TextValidationState
    {
        internal ValidationStatus Status { get; set; }
        internal string Message { get; set; }

        internal TextValidationState()
        {
            this.Status = ValidationStatus.Success;
            this.Message = string.Empty;
        }
    }
}
