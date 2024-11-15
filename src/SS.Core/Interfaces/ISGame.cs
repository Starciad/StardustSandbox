using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Managers;
using StardustSandbox.Game.Managers;
using StardustSandbox.Game.World;

namespace StardustSandbox.Core.Interfaces
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
