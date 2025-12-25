using Microsoft.Xna.Framework.Media;

using StardustSandbox.Audio;
using StardustSandbox.Camera;
using StardustSandbox.Constants;
using StardustSandbox.Enums.Backgrounds;
using StardustSandbox.Enums.Inputs.Game;
using StardustSandbox.Enums.Simulation;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UI;
using StardustSandbox.InputSystem.Game;
using StardustSandbox.Managers;
using StardustSandbox.WorldSystem;

namespace StardustSandbox
{
    internal static class GameHandler
    {
        private static GameStates states;

        internal static void StartGame(AmbientManager ambientManager, InputController inputController, UIManager uiManager, World world)
        {
            MediaPlayer.Stop();
            SongEngine.StartGameplayMusicCycle();

            uiManager.OpenGUI(UIIndex.Hud);

            ambientManager.BackgroundHandler.SetBackground(BackgroundIndex.Ocean);

            world.Time.Reset();
            world.StartNew(WorldConstants.WORLD_SIZES_TEMPLATE[0]);

            world.CanUpdate = true;
            world.CanDraw = true;

            SSCamera.Position = new(0f, -(world.Information.Size.Y * WorldConstants.GRID_SIZE));

            inputController.Pen.Tool = PenTool.Pencil;
            inputController.Enable();
        }

        internal static void StopGame(InputController inputController, World world)
        {
            SongEngine.StopGameplayMusicCycle();

            inputController.Pen.Tool = PenTool.Visualization;
            inputController.Disable();

            world.CanDraw = false;
            world.CanUpdate = false;
            world.SetSpeed(SimulationSpeed.Normal);

            RemoveState(GameStates.IsPaused);
            RemoveState(GameStates.IsSimulationPaused);
            RemoveState(GameStates.IsCriticalMenuOpen);

            world.Time.InGameSecondsPerRealSecond = TimeConstants.DEFAULT_VERY_FAST_SECONDS_PER_FRAMES;
            world.Time.IsFrozen = false;
        }

        internal static bool HasState(GameStates value)
        {
            return states.HasFlag(value);
        }

        internal static void SetState(GameStates value)
        {
            states |= value;
        }

        internal static void RemoveState(GameStates value)
        {
            states &= ~value;
        }

        internal static void ToggleState(GameStates value)
        {
            states ^= value;
        }
    }
}
