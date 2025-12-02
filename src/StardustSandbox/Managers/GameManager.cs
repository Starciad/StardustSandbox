using Microsoft.Xna.Framework;

using StardustSandbox.AudioSystem;
using StardustSandbox.Camera;
using StardustSandbox.Constants;
using StardustSandbox.Enums.BackgroundSystem;
using StardustSandbox.Enums.InputSystem.GameInput;
using StardustSandbox.Enums.Simulation;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UI;
using StardustSandbox.InputSystem.GameInput;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.Managers
{
    internal sealed class GameManager
    {
        private GameStates states;

        private AmbientManager ambientManager;
        private InputController inputController;
        private UIManager uiManager;
        private World world;

        internal void Initialize(AmbientManager ambientManager, InputController inputController, UIManager uiManager, World world)
        {
            this.ambientManager = ambientManager;
            this.inputController = inputController;
            this.uiManager = uiManager;
            this.world = world;
        }

        internal void Update()
        {
            ClampCameraInTheWorld();
        }

        internal void StartGame()
        {
            SongEngine.Stop();

            this.uiManager.OpenGUI(UIIndex.Hud);

            this.ambientManager.BackgroundHandler.SetBackground(BackgroundIndex.Ocean);
            this.ambientManager.SkyHandler.IsActive = true;
            this.ambientManager.CelestialBodyHandler.IsActive = true;
            this.ambientManager.CloudHandler.IsActive = true;

            this.world.Time.Reset();
            this.world.StartNew(WorldConstants.WORLD_SIZES_TEMPLATE[0]);

            SSCamera.Position = new(0f, -(this.world.Information.Size.Y * WorldConstants.GRID_SIZE));

            this.inputController.Pen.Tool = PenTool.Pencil;
            this.inputController.Activate();
        }

        internal void SetSimulationSpeed(SimulationSpeed speed)
        {
            switch (speed)
            {
                case SimulationSpeed.Normal:
                    this.world.SetSpeed(SimulationSpeed.Normal);
                    break;

                case SimulationSpeed.Fast:
                    this.world.SetSpeed(SimulationSpeed.Fast);
                    break;

                case SimulationSpeed.VeryFast:
                    this.world.SetSpeed(SimulationSpeed.VeryFast);
                    break;

                default:
                    this.world.SetSpeed(SimulationSpeed.Normal);
                    break;
            }
        }

        private void ClampCameraInTheWorld()
        {
            int totalWorldWidth = this.world.Information.Size.X * WorldConstants.GRID_SIZE;
            int totalWorldHeight = this.world.Information.Size.Y * WorldConstants.GRID_SIZE;

            float visibleWidth = ScreenConstants.SCREEN_WIDTH;
            float visibleHeight = ScreenConstants.SCREEN_HEIGHT;

            float worldLeftLimit = 0f;
            float worldRightLimit = totalWorldWidth - visibleWidth;

            float worldBottomLimit = (totalWorldHeight - visibleHeight) * -1;
            float worldTopLimit = 0f;

            Vector2 cameraPosition = SSCamera.Position;

            cameraPosition.X = MathHelper.Clamp(cameraPosition.X, worldLeftLimit, worldRightLimit);
            cameraPosition.Y = MathHelper.Clamp(cameraPosition.Y, worldBottomLimit, worldTopLimit);

            SSCamera.Position = cameraPosition;
        }

        internal bool HasState(GameStates value)
        {
            return this.states.HasFlag(value);
        }

        internal void SetState(GameStates value)
        {
            this.states |= value;
        }

        internal void RemoveState(GameStates value)
        {
            this.states &= ~value;
        }

        internal void ToggleState(GameStates value)
        {
            this.states ^= value;
        }
    }
}
