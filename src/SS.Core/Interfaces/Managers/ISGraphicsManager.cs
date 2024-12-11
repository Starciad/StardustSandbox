using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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
