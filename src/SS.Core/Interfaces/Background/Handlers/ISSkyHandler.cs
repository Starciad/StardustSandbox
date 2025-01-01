using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Interfaces.System;

namespace StardustSandbox.Core.Interfaces.Background.Handlers
{
    public interface ISSkyHandler : ISReset
    {
        bool IsActive { get; set; }
        Texture2D Texture { get; }
        Effect Effect { get; }
        SSkyGradientColorMap[] GradientColorMap { get; }
    }
}
