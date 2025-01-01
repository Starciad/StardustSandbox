using StardustSandbox.Core.Interfaces.System;

namespace StardustSandbox.Core.Interfaces.Ambient.Handlers
{
    public interface ISCloudHandler : ISReset
    {
        bool IsActive { get; set; }
    }
}
