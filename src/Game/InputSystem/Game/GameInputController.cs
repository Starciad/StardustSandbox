using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using StardustSandbox.Camera;
using StardustSandbox.Enums.Inputs;
using StardustSandbox.Enums.Inputs.Game;
using StardustSandbox.InputSystem.Game.Handlers;
using StardustSandbox.InputSystem.Game.Simulation;
using StardustSandbox.Managers;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.InputSystem.Game
{
    internal sealed class InputController
    {
        internal Pen Pen => this.pen;
        internal Player Player => this.player;

        private SimulationHandler simulationHandler;
        private WorldHandler worldHandler;
        private InputActionMapHandler actionHandler;

        private readonly Pen pen;
        private readonly Player player;

        internal InputController()
        {
            this.pen = new();
            this.player = new();
        }

        private void BuildKeyboardInputs()
        {
            InputActionMap cameraKeyboardActionMap = this.actionHandler.AddActionMap("Camera_Keyboard", true);
            InputActionMap simulationKeyboardActionMap = this.actionHandler.AddActionMap("Simulation_Keyboard", true);
            InputActionMap worldKeyboardActionMap = this.actionHandler.AddActionMap("World_Keyboard", true);

            cameraKeyboardActionMap.AddAction("Move_Camera_Up", new InputAction(Keys.W, Keys.Up)).OnPerformed += _ => SSCamera.Move(new Vector2(0, this.player.MovementSpeed));
            cameraKeyboardActionMap.AddAction("Move_Camera_Right", new InputAction(Keys.D, Keys.Right)).OnPerformed += _ => SSCamera.Move(new Vector2(this.player.MovementSpeed, 0));
            cameraKeyboardActionMap.AddAction("Move_Camera_Down", new InputAction(Keys.S, Keys.Down)).OnPerformed += _ => SSCamera.Move(new Vector2(0, -this.player.MovementSpeed));
            cameraKeyboardActionMap.AddAction("Move_Camera_Left", new InputAction(Keys.A, Keys.Left)).OnPerformed += _ => SSCamera.Move(new Vector2(-this.player.MovementSpeed, 0));

            simulationKeyboardActionMap.AddAction("Toggle_Pause", new(Keys.Space)).OnStarted += _ => this.simulationHandler.TogglePause();

            worldKeyboardActionMap.AddAction("Clear_World", new(Keys.R)).OnStarted += _ => this.worldHandler.Clear();
        }

        private void BuildMouseInputs()
        {
            InputActionMap worldMouseActionMap = this.actionHandler.AddActionMap("World_Mouse", true);
            worldMouseActionMap.AddAction("Place_Item", new(MouseButton.Left)).OnPerformed += _ => this.worldHandler.Modify(WorldModificationType.Adding);
            worldMouseActionMap.AddAction("Erase_Item", new(MouseButton.Right)).OnPerformed += _ => this.worldHandler.Modify(WorldModificationType.Removing);
        }

        internal void Initialize(GameManager gameManager, World world)
        {
            this.simulationHandler = new(gameManager);
            this.worldHandler = new(this.player, this.pen, world);

            this.actionHandler = new();

            BuildKeyboardInputs();
            BuildMouseInputs();
        }

        internal void Update()
        {
            UpdatePlaceAreaSize();

            this.actionHandler.Update();
        }

        private void UpdatePlaceAreaSize()
        {
            if (Input.GetDeltaScrollWheel() > 0)
            {
                this.pen.Size--;
            }
            else if (Input.GetDeltaScrollWheel() < 0)
            {
                this.pen.Size++;
            }
        }

        internal void Activate()
        {
            this.actionHandler.ActivateAll();
        }

        internal void Disable()
        {
            this.actionHandler.DisableAll();
        }
    }
}
