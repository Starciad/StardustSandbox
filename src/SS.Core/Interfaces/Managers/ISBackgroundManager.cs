using Microsoft.Xna.Framework;

using StardustSandbox.Core.Backgrounds;

namespace StardustSandbox.Core.Interfaces.Managers
{
    public interface ISBackgroundManager : ISManager
    {
        Color SolidColor { get; set; }

        void SetBackground(SBackground background);
        void EnableClouds();
        void DisableClouds();
    }
}
