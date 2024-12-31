using Microsoft.Xna.Framework;

using StardustSandbox.Core.Background;
using StardustSandbox.Core.Interfaces.Background.Handlers;

namespace StardustSandbox.Core.Interfaces.Managers
{
    public interface ISBackgroundManager : ISManager
    {
        Color SolidColor { get; set; }

        ISSkyHandler SkyHandler { get; }
        ISCloudHandler CloudHandler { get; }

        void SetBackground(SBackground background);
    }
}
