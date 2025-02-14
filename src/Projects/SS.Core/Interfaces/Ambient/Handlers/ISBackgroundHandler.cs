using Microsoft.Xna.Framework;

using StardustSandbox.Core.Ambient.Background;

namespace StardustSandbox.Core.Interfaces.Ambient.Handlers
{
    public interface ISBackgroundHandler
    {
        Color SolidColor { get; set; }
        SBackground SelectedBackground { get; }

        void SetBackground(SBackground background);
    }
}
