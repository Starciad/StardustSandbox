using StardustSandbox.Enums.UI.Tools;

namespace StardustSandbox.UI.States
{
    internal struct TextValidationState
    {
        internal ValidationStatus Status { get; private set; }
        internal string Message { get; private set; }

        public TextValidationState(ValidationStatus status)
        {
            this.Status = status;
            this.Message = string.Empty;
        }

        public TextValidationState(ValidationStatus status, string message)
        {
            this.Status = status;
            this.Message = message;
        }
    }
}
