using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Managers;
using StardustSandbox.Game.World;

namespace StardustSandbox.Game.Interfaces
{
    public interface ISGame
    {
        // Databases
        SAssetDatabase AssetDatabase { get; }
        SElementDatabase ElementDatabase { get; }
        SGUIDatabase GUIDatabase { get; }
        SItemDatabase ItemDatabase { get; }
        SBackgroundDatabase BackgroundDatabase { get; }

        // Managers
        SInputManager InputManager { get; }
        SGameInputManager GameInputManager { get; }
        SCameraManager CameraManager { get; }
        SGUIManager GUIManager { get; }

        // Core
        SWorld World { get; }
    }
}
