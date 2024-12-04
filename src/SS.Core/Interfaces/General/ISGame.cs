using StardustSandbox.Core.Controllers.GameInput;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.World;

namespace StardustSandbox.Core.Interfaces.General
{
    public interface ISGame
    {
        // Databases
        SAssetDatabase AssetDatabase { get; }
        SElementDatabase ElementDatabase { get; }
        SGUIDatabase GUIDatabase { get; }
        SItemDatabase ItemDatabase { get; }
        SBackgroundDatabase BackgroundDatabase { get; }
        SEntityDatabase EntityDatabase { get; }

        // Managers
        SGameManager GameManager { get; }
        SInputManager InputManager { get; }
        SCameraManager CameraManager { get; }
        SGraphicsManager GraphicsManager { get; }
        SGUIManager GUIManager { get; }
        SEntityManager EntityManager { get; }

        // Core
        SWorld World { get; }
        SGameInputController GameInputController { get; }

        void Quit();
    }
}
