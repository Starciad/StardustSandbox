using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Colors;

using System;

namespace StardustSandbox.Core.Interfaces.Background.Handlers
{
    public interface ISSkyHandler
    {
        bool IsActive { get; set; }
        Texture2D Texture { get; }
        Effect Effect { get; }

        SGradientColorMap GetBackgroundGradientByTime(TimeSpan currentTime);
        SGradientColorMap GetSkyGradientByTime(TimeSpan currentTime);
    }
}
