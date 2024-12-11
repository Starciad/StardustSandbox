using StardustSandbox.Core.Controllers.GameInput;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Interfaces.Managers;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Managers.IO;
using StardustSandbox.Core.World;

namespace StardustSandbox.Core.Interfaces.General
{
    public interface ISGame
    {
        // Databases
        ISAssetDatabase AssetDatabase { get; }
        ISElementDatabase ElementDatabase { get; }
        ISGUIDatabase GUIDatabase { get; }
        ISItemDatabase ItemDatabase { get; }
        ISBackgroundDatabase BackgroundDatabase { get; }
        ISEntityDatabase EntityDatabase { get; }

        // Managers
        ISGameManager GameManager { get; }
        ISInputManager InputManager { get; }
        ISCameraManager CameraManager { get; }
        ISGraphicsManager GraphicsManager { get; }
        ISGUIManager GUIManager { get; }
        ISEntityManager EntityManager { get; }
        ISBackgroundManager BackgroundManager { get; }
        ISCursorManager CursorManager { get; }

        // Core
        SWorld World { get; }
        SGameInputController GameInputController { get; }

        void Quit();
    }
}
