using StardustSandbox.Core.Interfaces.System;

namespace StardustSandbox.Core.Interfaces.Ambient.Handlers
{
    public interface ISCloudHandler : ISResettable
    {
        bool IsActive { get; set; }
    }
}
