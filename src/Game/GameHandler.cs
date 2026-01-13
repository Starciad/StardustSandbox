using Microsoft.Xna.Framework;
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

using System;

namespace StardustSandbox
{
    internal static class GameHandler
    {
        internal static SimulationSpeed SimulationSpeed { get; private set; }
        internal static bool HasSaveFileLoaded => !string.IsNullOrWhiteSpace(loadedSaveFileName);
        internal static string LoadedSaveFileName => loadedSaveFileName;

        private static GameStates states;
        private static string loadedSaveFileName;
        private static GameWindow gameWindow;

        internal static void Initialize(GameWindow gameWindow)
        {
            GameHandler.gameWindow = gameWindow;
        }

        internal static void StartGame(ActorManager actorManager, AmbientManager ambientManager, InputController inputController, UIManager uiManager, World world)
        {
            MediaPlayer.Stop();
            SongEngine.StartGameplayMusicCycle();

            uiManager.OpenUI(UIIndex.Hud);

            ambientManager.BackgroundHandler.SetBackground(BackgroundIndex.Ocean);

            Reset(actorManager, world);
            world.StartNew(WorldConstants.WORLD_SIZES_TEMPLATE[0]);

            world.CanUpdate = true;
            world.CanDraw = true;

            actorManager.CanDraw = true;
            actorManager.CanUpdate = true;

            SetSpeed(SimulationSpeed.Normal, actorManager, world);

            SSCamera.Position = new(0f, -(world.Information.Size.Y * WorldConstants.GRID_SIZE));

            inputController.Pen.Tool = PenTool.Pencil;
            inputController.Enable();
        }

        internal static void StopGame(ActorManager actorManager, InputController inputController, World world)
        {
            UnloadSaveFile();
            SongEngine.StopGameplayMusicCycle();

            inputController.Pen.Tool = PenTool.Visualization;
            inputController.Disable();

            world.CanDraw = false;
            world.CanUpdate = false;

            actorManager.CanDraw = false;
            actorManager.CanUpdate = false;

            SetSpeed(SimulationSpeed.Normal, actorManager, world);

            RemoveState(GameStates.IsPaused);
            RemoveState(GameStates.IsSimulationPaused);
            RemoveState(GameStates.IsCriticalMenuOpen);

            world.Time.InGameSecondsPerRealSecond = TimeConstants.DEFAULT_VERY_FAST_SECONDS_PER_FRAMES;
            world.Time.IsFrozen = false;
        }

        internal static void SetSpeed(in SimulationSpeed speed, ActorManager actorManager, World world)
        {
            SimulationSpeed = speed;

            world.SetSpeed(speed);
            actorManager.SetSpeed(speed);
        }

        internal static void DefineLoadedSaveFile(string saveFileName)
        {
            loadedSaveFileName = saveFileName;

            gameWindow.Title = string.IsNullOrWhiteSpace(saveFileName)
                ? GameConstants.GetTitleAndVersionString()
                : string.Concat(saveFileName, " — ", GameConstants.GetTitleAndVersionString());
        }

        internal static void LoadSaveFile(ActorManager actorManager, World world, string saveFileName)
        {
            if (string.IsNullOrWhiteSpace(saveFileName))
            {
                throw new ArgumentException("Save file name cannot be null or whitespace.", nameof(saveFileName));
            }

            actorManager.LoadFromSaveFile(saveFileName);
            world.LoadFromSaveFile(saveFileName);

            DefineLoadedSaveFile(saveFileName);
        }

        internal static void UnloadSaveFile()
        {
            DefineLoadedSaveFile(string.Empty);
        }

        internal static void ReloadSaveFile(ActorManager actorManager, World world)
        {
            actorManager.Reload();
            world.Reload();
        }

        internal static void Reset(ActorManager actorManager, World world)
        {
            UnloadSaveFile();

            actorManager.Reset();
            world.Reset();
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
