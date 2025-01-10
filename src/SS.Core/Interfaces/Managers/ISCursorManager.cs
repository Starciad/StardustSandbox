using Microsoft.Xna.Framework;

namespace StardustSandbox.Core.Interfaces.Managers
{
    public interface ISCursorManager : ISManager
    {
        Vector2 Scale { get; }

        void ApplySettings();
    }
}
