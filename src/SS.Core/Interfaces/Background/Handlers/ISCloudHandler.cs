using StardustSandbox.Core.Interfaces.System;

namespace StardustSandbox.Core.Interfaces.Background.Handlers
{
    public interface ISCloudHandler : ISReset
    {
        bool IsActive { get; set; }
    }
}
