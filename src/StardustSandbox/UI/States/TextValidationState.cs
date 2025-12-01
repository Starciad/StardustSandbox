using StardustSandbox.Enums.UI.Tools;

namespace StardustSandbox.UI.States
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
