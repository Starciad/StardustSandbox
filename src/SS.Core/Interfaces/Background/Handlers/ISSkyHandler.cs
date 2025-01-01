using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Interfaces.System;

using System;

namespace StardustSandbox.Core.Interfaces.Background.Handlers
{
    public interface ISSkyHandler
    {
        bool IsActive { get; set; }
        Texture2D Texture { get; }
        Effect Effect { get; }
        SSkyGradientColorMap[] GradientColorMap { get; }

        SSkyGradientColorMap GetGradientByTime(TimeSpan currentTime);
    }
}
