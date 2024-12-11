using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardustSandbox.Core.Interfaces.Managers
{
    public interface ISGraphicsManager
    {
        GraphicsDeviceManager GraphicsDeviceManager { get; }
        GraphicsDevice GraphicsDevice { get; }

        public Viewport Viewport { get; }

        Vector2 GetScreenScaleFactor();
    }
}
