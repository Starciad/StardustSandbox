using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace StardustSandbox.Core.Interfaces.Managers
{
    public interface ISInputManager
    {
        MouseState MouseState { get; }
        MouseState PreviousMouseState { get; }
        KeyboardState KeyboardState { get; }
        KeyboardState PreviousKeyboardState { get; }

        Vector2 GetScaledMousePosition();
        Vector2 GetScaledPreviousMousePosition();

        int GetDeltaScrollWheel();
    }
}
