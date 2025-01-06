using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardustSandbox.Core.Interfaces.Managers
{
    public interface ISGraphicsManager : ISManager
    {
        GraphicsDeviceManager GraphicsDeviceManager { get; }
        GraphicsDevice GraphicsDevice { get; }
        GameWindow GameWindow { get; }

        public Viewport Viewport { get; }

        void UpdateSettings();
        Vector2 GetScreenScaleFactor();
    }
}
